using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Backbone.Graphics;
using Backbone.IO;
using Backbone.Networking;
using SharpDX.Direct3D9;
using Steamworks;

namespace Backbone.Integration.Steamworks
{
    public class SteamNetworkingWrapper : INetworkingLobby
    {
        public uint LobbyMatchList { get; set; }
        public Action<string> OnLobbyChatMessage { get; set; }

        private CSteamID CurrentLobbyId;

        private const string HostAddressKey = "HostAddress";

        private CallResult<LobbyMatchList_t> mCallResultLobbyMatchList;
        private Callback<LobbyDataUpdate_t> mCallbackLobbyDataUpdate;
        private Callback<LobbyChatMsg_t> mCallbackLobbyChatMsg;
        private Callback<LobbyCreated_t> mCallbackLobbyCreated;
        private Callback<GameLobbyJoinRequested_t> mCallbackGameLobbyJoinRequested;
        private Callback<LobbyEnter_t> mCallbackLobbyEntered;

        private int maxConnections = 4;

        public void Initialize(int maxConnections)
        {
            this.maxConnections = maxConnections;
            mCallResultLobbyMatchList = CallResult<LobbyMatchList_t>.Create(OnLobbyMatchList);
            mCallbackLobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(OnLobbyDataUpdate);
            mCallbackLobbyChatMsg = Callback<LobbyChatMsg_t>.Create(OnLobbyChatMsg);
            mCallbackLobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            mCallbackGameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
            mCallbackLobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        }

        public void HostLobby()
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, maxConnections);
        }

        private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
        {
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);

        }

        public void JoinLobby(ulong lobbyId)
        {
            SteamMatchmaking.JoinLobby(new CSteamID(lobbyId));
        }

        public void FindLobbies()
        {
            string message = "FindLobbies called!";
            VariableMonitor.Report(x => message);

            var apiCall = SteamMatchmaking.RequestLobbyList();
            mCallResultLobbyMatchList.Set(apiCall, OnLobbyMatchList);
        }

        private void OnLobbyCreated(LobbyCreated_t pCallback)
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

        private void OnLobbyMatchList(LobbyMatchList_t pCallback, bool bIoFailure)
        {
            string message = "OnLobbyMatchList called!";
            VariableMonitor.Report(x => message);

            string lobbiesMatching = string.Format("{0} lobbies", pCallback.m_nLobbiesMatching);
            VariableMonitor.Report(x => lobbiesMatching);

            LobbyMatchList = pCallback.m_nLobbiesMatching;
        }

        private void OnLobbyChatMsg(LobbyChatMsg_t pCallback)
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

        private void OnLobbyDataUpdate(LobbyDataUpdate_t pCallback)
        {
        }


        public bool SendLobbyChatMessage(string message)
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
        public bool SetLobbyOwnerData(string key, string value)
        {
            return SteamMatchmaking.SetLobbyData(CurrentLobbyId, key, value);
        }

        /// <summary>
        /// Sets metadata for the member (not owner) of the lobby
        /// </summary>
        /// <param name="key">Name of the metadata key</param>
        /// <param name="value">Value of the metadata for that key</param>
        public void SetLobbyMemberData(string key, string value)
        {
            SteamMatchmaking.SetLobbyMemberData(CurrentLobbyId, key, value);
        }


        /// <summary>
        /// Refreshes all of the metadata for a lobby that you're not in right now.
        ///
        /// You will never do this for lobbies you're a member of, that data will always be up to date.
        /// You can use this to refresh lobbies that you have obtained from RequestLobbyList or that are available via friends.
        ///
        /// Triggers a LobbyDataUpdate_t callback.
        /// </summary>
        /// <param name="steamIDLobby">id of the lobby</param>
        /// <returns>true if the request was successfully sent to the server. false if no connection to Steam could be made, or steamIDLobby is invalid.</returns>
        public bool RequestLobbyData(ulong lobbyId)
        {
            return SteamMatchmaking.RequestLobbyData(new CSteamID(lobbyId));
        }
    }
}
