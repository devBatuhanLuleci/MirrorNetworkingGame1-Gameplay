using System;
using System.Drawing;
using System.Reflection;
using Mirror;

namespace ACGAuthentication {
    public static class Writers {
        #region Spawn Server Writers
        public static void WriteCloseRoomEvent (this NetworkWriter writer, CloseRoomEvent req) {
            // write MyType data here
        }

        public static CloseRoomEvent ReadCloseRoomEvent (this NetworkReader reader) {
            // read MyType data here
            return new CloseRoomEvent ();
        }
        public static void WriteConnectToGameServerEvent (this NetworkWriter writer, ConnectToGameServerEvent req) {
            // write MyType data here
            writer.WriteUShort (req.Port);
            writer.WriteString (req.Host);
        }

        public static ConnectToGameServerEvent ReadConnectToGameServerEvent (this NetworkReader reader) {
            // read MyType data here
            return new ConnectToGameServerEvent (reader.ReadUShort (), reader.ReadString ());
        }
        public static void WriteOnReadyEventEvent (this NetworkWriter writer, OnReadyEvent req) {
            // write MyType data here
            writer.WriteInt (req.Port);
        }

        public static OnReadyEvent ReadOnReadyEventEvent (this NetworkReader reader) {
            // read MyType data here
            return new OnReadyEvent (reader.ReadInt ());
        }
        #endregion
        #region Lobby Writers
        public static void WriteStartLobbyRoomEvent (this NetworkWriter writer, StartLobbyRoom req) {
            // write MyType data here
        }

        public static StartLobbyRoom ReadStartLobbyRoomEvent (this NetworkReader reader) {
            // read MyType data here
            return new StartLobbyRoom ();
        }
        public static void WriteThereIsNoRoomEvent (this NetworkWriter writer, ThereIsNoRoom req) {
            // write MyType data here
            writer.WriteInt (req.RoomId);
        }

        public static ThereIsNoRoom ReadThereIsNoRoomEvent (this NetworkReader reader) {
            // read MyType data here
            return new ThereIsNoRoom (reader.ReadInt ());
        }
        public static void WriteLeaveRoomEvent (this NetworkWriter writer, LeaveRoom req) {
            // write MyType data here
        }

        public static LeaveRoom ReadLeaveRoomEvent (this NetworkReader reader) {
            // read MyType data here
            return new LeaveRoom ();
        }

        public static void WriteReadyStateChangedEvent (this NetworkWriter writer, ReadyStateChanged req) {
            // write MyType data here
            writer.WriteLobbyPlayer (req.LobbyPlayer);
        }

        public static ReadyStateChanged ReadReadyStateChangedEvent (this NetworkReader reader) {
            // read MyType data here
            return new ReadyStateChanged (reader.ReadLobbyPlayer ());
        }
        public static void WriteReadyEvent (this NetworkWriter writer, ReadyStateChange req) {
            // write MyType data here
        }

        public static ReadyStateChange ReadReadyEvent (this NetworkReader reader) {
            // read MyType data here
            return new ReadyStateChange ();
        }
        public static void WriteOnDisconnectedLobbyRoomEvent (this NetworkWriter writer, OnLeaveLobbyRoom req) {
            // write MyType data here
            writer.WriteLobbyPlayer (req.LobbyPlayer);
        }

        public static OnLeaveLobbyRoom ReadOnDisconnectedLobbyRoomEvent (this NetworkReader reader) {
            // read MyType data here
            return new OnLeaveLobbyRoom (reader.ReadLobbyPlayer ());
        }
        public static void WriteMaxPlayerErrorEvent (this NetworkWriter writer, MaxPlayerError req) {
            // write MyType data here
        }

        public static MaxPlayerError ReadMaxPlayerErrorEvent (this NetworkReader reader) {
            // read MyType data here
            return new MaxPlayerError ();
        }

        public static void WritePlayerJoinedToLobbyRoomEvent (this NetworkWriter writer, PlayerJoinedToLobbyRoom req) {
            // write MyType data here
            writer.WriteInt (req.RoomCode);
            writer.WriteArray<LobbyPlayer> (req.LobbyPlayers);
            writer.WriteLobbyPlayer (req.LobbyPlayer);
        }

        public static PlayerJoinedToLobbyRoom ReadPlayerJoinedToLobbyRoomEvent (this NetworkReader reader) {
            // read MyType data here
            return new PlayerJoinedToLobbyRoom (reader.ReadInt (), reader.ReadArray<LobbyPlayer> (), reader.ReadLobbyPlayer ());
        }
        public static void WriteLobbyPlayer (this NetworkWriter writer, LobbyPlayer req) {
            // write MyType data here
            writer.WriteString (req.UserName);
            writer.WriteBool (req.IsLeader);
            writer.WriteBool (req.IsReady);
            writer.WriteInt (req.RoomId);

        }

        public static LobbyPlayer ReadLobbyPlayer (this NetworkReader reader) {
            // read MyType data here
            var player = new LobbyPlayer (reader.ReadString (), reader.ReadBool (), reader.ReadBool ());
            player.RoomId = reader.ReadInt ();
            return player;
        }
        public static void WriteJoinedToLobbyRoomEvent (this NetworkWriter writer, NewPlayerJoinedToLobbyRoom req) {
            // write MyType data here
            writer.Write<LobbyPlayer> (req.Player);
        }

        public static NewPlayerJoinedToLobbyRoom ReadJoinedToLobbyRoomEvent (this NetworkReader reader) {
            // read MyType data here
            return new NewPlayerJoinedToLobbyRoom (reader.Read<LobbyPlayer> ());
        }
        public static void WriteJoinLobbyRoomEvent (this NetworkWriter writer, JoinLobbyRoom req) {
            writer.WriteInt (req.RoomCode);
            // write MyType data here
        }

        public static JoinLobbyRoom ReadJoinLobbyRoomEvent (this NetworkReader reader) {
            // read MyType data here
            return new JoinLobbyRoom (reader.ReadInt ());

        }
        public static void WriteCreateLobbyRoomEvent (this NetworkWriter writer, LobbyRoomCreated req) {
            writer.WriteInt (req.RoomCode);
            writer.WriteLobbyPlayer (req.LobbyPlayer);
            // write MyType data here
        }

        public static LobbyRoomCreated ReadCreateLobbyRoomEvent (this NetworkReader reader) {
            // read MyType data here
            return new LobbyRoomCreated (reader.ReadInt (), reader.ReadLobbyPlayer ());

        }
        public static void WriteGetFriendsEvent (this NetworkWriter writer, GetPlayersEvent req) {
            // write MyType data here
        }

        public static GetPlayersEvent ReadGetFriendsEvent (this NetworkReader reader) {
            // read MyType data here
            return new GetPlayersEvent ();

        }
        public static void WriteStartMatchEvent (this NetworkWriter writer, CreateLobbyRoom req) {
            // write MyType data here
        }

        public static CreateLobbyRoom ReadStartMatchEvent (this NetworkReader reader) {
            // read MyType data here
            return new CreateLobbyRoom ();

        }
        #endregion

        #region Panel Writers and Read
        /// <summary>
        /// Get ClanNames
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="req"></param>
        public static void WriteOnGetClanNamesEvent (this NetworkWriter writer, OnGetClanNames req) {
            // write MyType data here
            writer.WriteArray<String> (req.ClanNames);
            writer.WriteBool (req.IsNewClanNameCreate);
        }

        public static OnGetClanNames ReadOnGetClanNamesEvent (this NetworkReader reader) {
            // read MyType data here
            return new OnGetClanNames (reader.ReadArray<string> (), reader.ReadBool ());
        }
        public static void WriteGetClanNamesEvent (this NetworkWriter writer, GetClanNames req) {
            // write MyType data here
        }

        public static GetClanNames ReadGetClanNamesEvent (this NetworkReader reader) {
            // read MyType data here
            return new GetClanNames ();
        }
        /// <summary>
        /// Send ClanName 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="req"></param>
        public static void WriteOnSendClanNameEvent (this NetworkWriter writer, OnSendClanName req) {
            // write MyType data here
            // writer.WriteString(req.ClanName);
        }

        public static OnSendClanName ReadOnSendClanNameEvent (this NetworkReader reader) {
            // read MyType data here
            return new OnSendClanName (reader.ReadString ());
        }
        public static void WriteSendClanNameEvent (this NetworkWriter writer, SendClanName req) {
            // write MyType data here
            writer.WriteString (req.ClanName);
        }

        public static SendClanName ReadSendClanNamesEvent (this NetworkReader reader) {
            // read MyType data here
            return new SendClanName (reader.ReadString ());
        }

        /// <summary>
        /// Get FriendNames
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="req"></param>
        public static void WriteOnGetFriendNamesEvent (this NetworkWriter writer, OnGetFriendNames req) {
            // write MyType data here
            writer.WriteArray<String> (req.FriendNames);
            writer.WriteBool (req.IsNewFriendNameAdd);
        }

        public static OnGetFriendNames ReadOnGetFriendNamesEvent (this NetworkReader reader) {
            // read MyType data here
            return new OnGetFriendNames (reader.ReadArray<string> (), reader.ReadBool ());
        }
        public static void WriteGetFriendNamesEvent (this NetworkWriter writer, GetFriendNames req) {
            // write MyType data here
        }

        public static GetFriendNames ReadGetFriendNamesEvent (this NetworkReader reader) {
            // read MyType data here
            return new GetFriendNames ();
        }
        /// <summary>
        /// Send FriendName 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="req"></param>
        public static void WriteOnSendFriendNameEvent (this NetworkWriter writer, OnSendFriendName req) {
            // write MyType data here
            // writer.WriteString(req.ClanName);
        }

        public static OnSendFriendName ReadOnSendFriendNameEvent (this NetworkReader reader) {
            // read MyType data here
            return new OnSendFriendName (reader.ReadString ());
        }
        public static void WriteSendFriendNameEvent (this NetworkWriter writer, SendFriendName req) {
            // write MyType data here
            writer.WriteString (req.FriendName);
        }

        public static SendFriendName ReadSendFriendNamesEvent (this NetworkReader reader) {
            // read MyType data here
            return new SendFriendName (reader.ReadString ());
        }
        #endregion

        #region NotificationInfo
        /// <summary>
        /// NotificationInfo
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="req"></param>
        public static void WriteOnSendNotificationInfoEvent (this NetworkWriter writer, OnSendNotificationInfo req) {
            // write MyType data here
            writer.WriteString (req.NotificationInfo);
            writer.WriteInt ((int) req.InfoType);
        }

        public static OnSendNotificationInfo ReadOnSendNotificationInfoEvent (this NetworkReader reader) {
            // read MyType data here
            return new OnSendNotificationInfo (reader.ReadString (), (NotificationManager.InfoType) reader.ReadInt ());
        }
        public static void WriteSendNotificationInfoEvent (this NetworkWriter writer, SendNotificationInfo req) {
            // write MyType data here
            writer.WriteString (req.NotificationInfo);
            writer.WriteInt ((int) req.InfoType);
        }

        public static SendNotificationInfo ReadSendNotificationInfoEvent (this NetworkReader reader) {
            // read MyType data here
            return new SendNotificationInfo (reader.ReadString (), (NotificationManager.InfoType) reader.ReadInt ());
        }
        #endregion
        #region Authentication Writers

        #region RegisterRequest
        public static void WriteRegisterRequest (this NetworkWriter writer, RegisterRequest req) {
            // write MyType data here
            writer.WriteString (req.Email);
            writer.WriteString (req.MoralisId);
            writer.WriteString (req.WalletId);
            writer.WriteString (req.UserName);
        }

        public static RegisterRequest ReadRegisterRequest (this NetworkReader reader) {
            // read MyType data here
            return new RegisterRequest (reader.ReadString (), reader.ReadString (), reader.ReadString (), reader.ReadString ());

        }
        #endregion

        #region LoginRequest
        public static void WriteLoginRequest (this NetworkWriter writer, LoginEvent req) {
            // write MyType data here
            writer.WriteString (req.MoralisId);
            writer.WriteString (req.UserName);
        }

        public static LoginEvent ReadLoginRequest (this NetworkReader reader) {
            // read MyType data here
            return new LoginEvent (reader.ReadString (), reader.ReadString ());

        }
        #endregion
        #endregion
        #region IEvent
        public static IResponseEvent ReadIEvent (this NetworkReader reader, Type type) {
            // Get the generic type definition
            MethodInfo method = typeof (NetworkReader).GetMethod ("Read", BindingFlags.Instance | BindingFlags.Public);
            // Build a method with the specific type argument you're interested in
            method = method.MakeGenericMethod (type);
            return method.Invoke (reader, null) as IResponseEvent;
        }
        public static void WriteIEvent (this NetworkWriter writer, Type type, IEvent data) {
            // Get the generic type definition
            MethodInfo method = writer.GetType ().GetMethod ("Write", BindingFlags.Instance | BindingFlags.Public);
            // Build a method with the specific type argument you're interested in
            method = method.MakeGenericMethod (type);
            method.Invoke (writer, new object[] { data });
        }

        #endregion

        #region AccessTokenRequest
        /// <summary>
        /// Send AccessToken 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="req"></param>
        public static void WriteOnSendAccessTokenEvent (this NetworkWriter writer, OnSendAccessTokenKey req) {
            // write MyType data here
            // writer.WriteString(req.AccessTokenKey);
        }

        public static OnSendAccessTokenKey ReadOnSendAccessTokenKeyEvent (this NetworkReader reader) {
            // read MyType data here
            return new OnSendAccessTokenKey (reader.ReadBool());
        }
        public static void WriteSendAccessTokenKeyEvent (this NetworkWriter writer, SendAccessTokenKey req) {
            // write MyType data here
            writer.WriteString (req.AccessTokenKey);
        }

        public static SendAccessTokenKey ReadSendAccessTokenKeyEvent (this NetworkReader reader) {
            // read MyType data here
            return new SendAccessTokenKey (reader.ReadString ());
        }
        #endregion
        
    }
}