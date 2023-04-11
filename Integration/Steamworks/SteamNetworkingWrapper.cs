using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Backbone.Graphics;
using Steamworks;

namespace Backbone.Integration.Steamworks
{
    public static class SteamNetworkingWrapper
    {
        public static uint LobbyMatchList { get; set; }
        public static Action<string> OnLobbyChatMessage { get; set; }

        private static CSteamID CurrentLobbyId;

        private const string HostAddressKey = "HostAddress";

        private static CallResult<LobbyMatchList_t> mCallResultLobbyMatchList;
        private static Callback<LobbyDataUpdate_t> mCallbackLobbyDataUpdate;
        private static Callback<LobbyChatMsg_t> mCallbackLobbyChatMsg;
        private static Callback<LobbyCreated_t> mCallbackLobbyCreated;
        private static Callback<GameLobbyJoinRequested_t> mCallbackGameLobbyJoinRequested;
        private static Callback<LobbyEnter_t> mCallbackLobbyEntered;

        private static int maxConnections = 4;

        public static void Initialize(int maxConnections)
        {
            SteamNetworkingWrapper.maxConnections = maxConnections;
            mCallResultLobbyMatchList = CallResult<LobbyMatchList_t>.Create(OnLobbyMatchList);
            mCallbackLobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(OnLobbyDataUpdate);
            mCallbackLobbyChatMsg = Callback<LobbyChatMsg_t>.Create(OnLobbyChatMsg);
            mCallbackLobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            mCallbackGameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
            mCallbackLobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        }

        public static void HostLobby()
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, maxConnections);
        }

        private static void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
        {
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        private static void OnLobbyEntered(LobbyEnter_t callback)
        {
            string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);

        }

        public static void FindLobbies()
        {
            string message = "FindLobbies called!";
            VariableMonitor.Report(x => message);

            var apiCall = SteamMatchmaking.RequestLobbyList();
            mCallResultLobbyMatchList.Set(apiCall, OnLobbyMatchList);
        }

        private static void OnLobbyCreated(LobbyCreated_t pCallback)
        {
            if(pCallback.m_eResult != EResult.k_EResultOK)
            {
                return;
            }

            SteamMatchmaking.SetLobbyData(
                new CSteamID(pCallback.m_ulSteamIDLobby),
                HostAddressKey,
                SteamUser.GetSteamID().ToString()
            );
        }

        private static void OnLobbyMatchList(LobbyMatchList_t pCallback, bool bIoFailure)
        {
            string message = "OnLobbyMatchList called!";
            VariableMonitor.Report(x => message);

            string lobbiesMatching = string.Format("{0} lobbies", pCallback.m_nLobbiesMatching);
            VariableMonitor.Report(x => lobbiesMatching);

            LobbyMatchList = pCallback.m_nLobbiesMatching;
        }

        private static void OnLobbyChatMsg(LobbyChatMsg_t pCallback)
        {
            var byteSize = 4096;
            Byte[] pData = new Byte[byteSize];
            EChatEntryType eChatEntryType;
            CSteamID steamIdUser = new CSteamID(pCallback.m_ulSteamIDUser);
            
            int bytesCopied = SteamMatchmaking.GetLobbyChatEntry(steamIdUser, (int)pCallback.m_iChatID, out steamIdUser, pData, byteSize, out eChatEntryType);
            if (bytesCopied > 0 && OnLobbyChatMessage != null)
            {
                OnLobbyChatMessage.Invoke(pData.ToString());
            }
        }

        private static void OnLobbyDataUpdate(LobbyDataUpdate_t pCallback)
        {
        }


        public static bool SendLobbyChatMessage(string message)
        {
            var pvMsgBody = Encoding.ASCII.GetBytes(message);
            var cubMsgBody = pvMsgBody.Length;
            return SteamMatchmaking.SendLobbyChatMsg(CurrentLobbyId, pvMsgBody, cubMsgBody);
        }


        /// <summary>
        /// Sets a key/value pair in the lobby metadata. This can be used to set the the lobby name, current map, game mode, etc.
        /// This can only be set by the owner of the lobby.Lobby members should use SetLobbyMemberData instead.
        /// 
        /// Each user in the lobby will be receive notification of the lobby data change via a LobbyDataUpdate_t callback, and any
        /// new users joining will receive any existing data.
        /// 
        /// This will only send the data if it has changed.There is a slight delay before sending the data so you can call this repeatedly
        /// to set all the data you need to and it will automatically be batched up and sent after the last sequential call.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetLobbyOwnerData(string key, string value)
        {
            return SteamMatchmaking.SetLobbyData(CurrentLobbyId, key, value);
        }

        /// <summary>
        /// Sets metadata for the member (not owner) of the lobby
        /// </summary>
        /// <param name="key">Name of the metadata key</param>
        /// <param name="value">Value of the metadata for that key</param>
        public static void SetLobbyMemberData(string key, string value)
        {
            SteamMatchmaking.SetLobbyMemberData(CurrentLobbyId, key, value);
        }
    }
}
