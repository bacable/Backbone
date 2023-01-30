using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;


namespace Backbone.Integration.Steamworks
{
    public static class SteamManager
    {
        public static bool IsSteamRunning { get; private set; } = false;
        public static string SteamUserName { get; set; } = "";
        public static string CurrentLanguage { get; set; } = "";
        public static string AvailableLanguages { get; set; } = "";
        public static string InstallDir { get; set; } = "";
        public static Texture2D UserAvatar { get; set; }
        public static bool SteamOverlayActive { get; set; }
        public static string UserStats { get; set; } = "";
        public static string PersonaState { get; set; } = "";
        public static string LeaderboardData { get; set; } = "";
        public static string NumberOfCurrentPlayers { get; set; } = "";

        public static uint PlayTimeInSeconds() => SteamUtils.GetSecondsSinceAppActive();
        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            if (!SteamAPI.Init())
            {
                Console.WriteLine("SteamAPI.Init() failed!");
            }
            else
            {
                IsSteamRunning = true;

                // It's important that the next call happens AFTER the call to SteamAPI.Init().
                InitializeCallbacks();

                SteamUtils.SetOverlayNotificationPosition(ENotificationPosition.k_EPositionBottomRight);
                // Uncomment the next line to adjust the OverlayNotificationPosition.
                //SteamUtils.SetOverlayNotificationInset(400, 0);

                // Set collactible data.
                CurrentLanguage = $"CurrentGameLanguage: {SteamApps.GetCurrentGameLanguage()}";
                AvailableLanguages = $"Languages: {SteamApps.GetAvailableGameLanguages()}";
                UserStats = $"Reqesting Current Stats - {SteamUserStats.RequestCurrentStats()}";
                //mNumberOfCurrentPlayers.Set(SteamUserStats.GetNumberOfCurrentPlayers());
                //var hSteamApiCall = SteamUserStats.FindLeaderboard("Quickest Win");
                //mCallResultFindLeaderboard.Set(hSteamApiCall);

                string folder;
                var length = SteamApps.GetAppInstallDir(SteamUtils.GetAppID(), out folder, 260);
                InstallDir = $"AppInstallDir: {length} {folder}";

                // Get your Steam Avatar (Image) as a Texture2D.
                UserAvatar = GetSteamUserAvatar(graphicsDevice);

                // Get your trimmed Steam User Name.
                var untrimmedUserName = SteamFriends.GetPersonaName();
                // Remove unsupported chars like emojis or other stuff our font cannot handle.
                //untrimmedUserName = ReplaceUnsupportedChars(Font, untrimmedUserName);
                SteamUserName = untrimmedUserName.Trim();
            }
        }

        /// <summary>
        ///     Get your steam avatar.
        ///     Important:
        ///     The returned Texture2D object is NOT loaded using a ContentManager.
        ///     So it's your responsibility to dispose it at the end by calling <see cref="Texture2D.Dispose()" />.
        /// </summary>
        /// <param name="device">The GraphicsDevice</param>
        /// <returns>Your Steam Avatar Image as a Texture2D object</returns>
        private static Texture2D GetSteamUserAvatar(GraphicsDevice device)
        {
            // Get the icon type as a integer.
            var icon = SteamFriends.GetMediumFriendAvatar(SteamUser.GetSteamID());

            // Check if we got an icon type.
            if (icon != 0)
            {
                uint width;
                uint height;
                var ret = SteamUtils.GetImageSize(icon, out width, out height);

                if (ret && width > 0 && height > 0)
                {
                    var rgba = new byte[width * height * 4];
                    ret = SteamUtils.GetImageRGBA(icon, rgba, rgba.Length);
                    if (ret)
                    {
                        var texture = new Texture2D(device, (int)width, (int)height, false, SurfaceFormat.Color);
                        texture.SetData(rgba, 0, rgba.Length);
                        return texture;
                    }
                }
            }
            return null;
        }

        private static Callback<GameOverlayActivated_t> mGameOverlayActivated;
        private static CallResult<NumberOfCurrentPlayers_t> mNumberOfCurrentPlayers;
        private static CallResult<LeaderboardFindResult_t> mCallResultFindLeaderboard;
        private static Callback<PersonaStateChange_t> mPersonaStateChange;
        private static Callback<UserStatsReceived_t> mUserStatsReceived;

        /// <summary>
        ///     Initialize some Steam Callbacks.
        /// </summary>
        private static void InitializeCallbacks()
        {
            mGameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
            mNumberOfCurrentPlayers = CallResult<NumberOfCurrentPlayers_t>.Create(OnNumberOfCurrentPlayers);
            mCallResultFindLeaderboard = CallResult<LeaderboardFindResult_t>.Create(OnFindLeaderboard);
            mPersonaStateChange = Callback<PersonaStateChange_t>.Create(OnPersonaStateChange);
            mUserStatsReceived =
                Callback<UserStatsReceived_t>.Create(
                    pCallback =>
                    {
                        UserStats =
                            $"[{UserStatsReceived_t.k_iCallback} - UserStatsReceived] - {pCallback.m_eResult} -- {pCallback.m_nGameID} -- {pCallback.m_steamIDUser}";
                    });
        }

        private static void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
        {
            if (pCallback.m_bActive == 0)
            {
                // GameOverlay is not active.
                SteamOverlayActive = false;
            }
            else
            {
                // GameOverlay is active.
                SteamOverlayActive = true;
            }
        }

        private static void OnNumberOfCurrentPlayers(NumberOfCurrentPlayers_t pCallback, bool bIoFailure)
        {
            NumberOfCurrentPlayers =
                $"[{NumberOfCurrentPlayers_t.k_iCallback} - NumberOfCurrentPlayers] - {pCallback.m_bSuccess} -- {pCallback.m_cPlayers}";
        }

        private static void OnFindLeaderboard(LeaderboardFindResult_t pCallback, bool bIoFailure)
        {
            LeaderboardData =
                $"[{LeaderboardFindResult_t.k_iCallback} - LeaderboardFindResult] - {pCallback.m_bLeaderboardFound} -- {pCallback.m_hSteamLeaderboard}";
        }

        private static void OnPersonaStateChange(PersonaStateChange_t pCallback)
        {
            PersonaState =
                $"[{PersonaStateChange_t.k_iCallback} - PersonaStateChange] - {pCallback.m_ulSteamID} -- {pCallback.m_nChangeFlags}";
        }
        public static void Update()
        {
            if (!IsSteamRunning)
            {
                return;
            }

            // Run Steam client callbacks
            SteamAPI.RunCallbacks();
        }

    }
}
