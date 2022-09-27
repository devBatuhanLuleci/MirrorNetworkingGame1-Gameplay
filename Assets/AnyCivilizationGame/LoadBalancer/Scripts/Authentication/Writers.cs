


using Mirror;
using System;
using System.Drawing;
using System.Reflection;

namespace ACGAuthentication
{
    public static class Writers
    {
        #region Spawn Server Writers

        public static void WriteConnectToGameServerEvent(this NetworkWriter writer, ConnectToGameServerEvent req)
        {
            // write MyType data here
            writer.WriteUShort(req.Port);
            writer.WriteString(req.Host);
        }

        public static ConnectToGameServerEvent ReadConnectToGameServerEvent(this NetworkReader reader)
        {
            // read MyType data here
            return new ConnectToGameServerEvent(reader.ReadUShort(), reader.ReadString());
        }
        public static void WriteOnReadyEventEvent(this NetworkWriter writer, OnReadyEvent req)
        {
            // write MyType data here
            writer.WriteInt(req.Port);
        }

        public static OnReadyEvent ReadOnReadyEventEvent(this NetworkReader reader)
        {
            // read MyType data here
            return new OnReadyEvent(reader.ReadInt());
        }
        #endregion
        #region Lobby Writers

        public static void WriteOnDisconnectedLobbyRoomEvent(this NetworkWriter writer, OnDisconnectedLobbyRoom req)
        {
            // write MyType data here
            writer.WriteLobbyPlayer(req.LobbyPlayer);
        }

        public static OnDisconnectedLobbyRoom ReadOnDisconnectedLobbyRoomEvent(this NetworkReader reader)
        {
            // read MyType data here
            return new OnDisconnectedLobbyRoom(reader.ReadLobbyPlayer());
        }
        public static void WriteMaxPlayerErrorEvent(this NetworkWriter writer, MaxPlayerError req)
        {
            // write MyType data here
        }

        public static MaxPlayerError ReadMaxPlayerErrorEvent(this NetworkReader reader)
        {
            // read MyType data here
            return new MaxPlayerError();
        }

        public static void WritePlayerJoinedToLobbyRoomEvent(this NetworkWriter writer, PlayerJoinedToLobbyRoom req)
        {
            // write MyType data here
            writer.WriteInt(req.RoomCode);
            writer.WriteArray<LobbyPlayer>(req.LobbyPlayers);
            writer.WriteLobbyPlayer(req.LobbyPlayer);
        }

        public static PlayerJoinedToLobbyRoom ReadPlayerJoinedToLobbyRoomEvent(this NetworkReader reader)
        {
            // read MyType data here
            return new PlayerJoinedToLobbyRoom(reader.ReadInt(), reader.ReadArray<LobbyPlayer>(), reader.ReadLobbyPlayer());
        }
        public static void WriteLobbyPlayer(this NetworkWriter writer, LobbyPlayer req)
        {
            // write MyType data here
            writer.WriteString(req.UserName);
            writer.WriteBool(req.IsLeader);
            writer.WriteBool(req.IsReady);
        }

        public static LobbyPlayer ReadLobbyPlayer(this NetworkReader reader)
        {
            // read MyType data here
            return new LobbyPlayer(reader.ReadString(), reader.ReadBool(), reader.ReadBool());
        }
        public static void WriteJoinedToLobbyRoomEvent(this NetworkWriter writer, NewPlayerJoinedToLobbyRoom req)
        {
            // write MyType data here
            writer.Write<LobbyPlayer>(req.Player);
        }

        public static NewPlayerJoinedToLobbyRoom ReadJoinedToLobbyRoomEvent(this NetworkReader reader)
        {
            // read MyType data here
            return new NewPlayerJoinedToLobbyRoom(reader.Read<LobbyPlayer>());
        }
        public static void WriteJoinLobbyRoomEvent(this NetworkWriter writer, JoinLobbyRoom req)
        {
            writer.WriteInt(req.RoomCode);
            // write MyType data here
        }

        public static JoinLobbyRoom ReadJoinLobbyRoomEvent(this NetworkReader reader)
        {
            // read MyType data here
            return new JoinLobbyRoom(reader.ReadInt());


        }
        public static void WriteCreateLobbyRoomEvent(this NetworkWriter writer, CreateLobbyRoom req)
        {
            writer.WriteInt(req.RoomCode);
            writer.WriteLobbyPlayer(req.LobbyPlayer);
            // write MyType data here
        }

        public static CreateLobbyRoom ReadCreateLobbyRoomEvent(this NetworkReader reader)
        {
            // read MyType data here
            return new CreateLobbyRoom(reader.ReadInt(),reader.ReadLobbyPlayer());


        }
        public static void WriteGetFriendsEvent(this NetworkWriter writer, GetPlayersEvent req)
        {
            // write MyType data here
        }

        public static GetPlayersEvent ReadGetFriendsEvent(this NetworkReader reader)
        {
            // read MyType data here
            return new GetPlayersEvent();

        }
        public static void WriteStartMatchEvent(this NetworkWriter writer, StartMatchEvent req)
        {
            // write MyType data here
        }

        public static StartMatchEvent ReadStartMatchEvent(this NetworkReader reader)
        {
            // read MyType data here
            return new StartMatchEvent();


        }
        #endregion
        #region Authentication Writers

        #region RegisterRequest
        public static void WriteRegisterRequest(this NetworkWriter writer, RegisterRequest req)
        {
            // write MyType data here
            writer.WriteString(req.Email);
            writer.WriteString(req.MoralisId);
            writer.WriteString(req.WalletId);
            writer.WriteString(req.UserName);
        }

        public static RegisterRequest ReadRegisterRequest(this NetworkReader reader)
        {
            // read MyType data here
            return new RegisterRequest(reader.ReadString(), reader.ReadString(), reader.ReadString(), reader.ReadString());


        }
        #endregion

        #region LoginRequest
        public static void WriteLoginRequest(this NetworkWriter writer, LoginEvent req)
        {
            // write MyType data here
            writer.WriteString(req.MoralisId);
            writer.WriteString(req.UserName);
        }

        public static LoginEvent ReadLoginRequest(this NetworkReader reader)
        {
            // read MyType data here
            return new LoginEvent(reader.ReadString(), reader.ReadString());

        }
        #endregion
        #endregion
        #region IEvent
        public static IResponseEvent ReadIEvent(this NetworkReader reader, Type type)
        {
            // Get the generic type definition
            MethodInfo method = typeof(NetworkReader).GetMethod("Read", BindingFlags.Instance | BindingFlags.Public);
            // Build a method with the specific type argument you're interested in
            method = method.MakeGenericMethod(type);
            return method.Invoke(reader, null) as IResponseEvent;
        }
        public static void WriteIEvent(this NetworkWriter writer, Type type, IEvent data)
        {
            // Get the generic type definition
            MethodInfo method = writer.GetType().GetMethod("Write", BindingFlags.Instance | BindingFlags.Public);
            // Build a method with the specific type argument you're interested in
            method = method.MakeGenericMethod(type);
            method.Invoke(writer, new object[] { data });
        }

        #endregion
    }
}