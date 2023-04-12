using System.Collections;
using System.Collections.Generic;
using ACGAuthentication;
using UnityEngine;

public class OnSendFriendName : IResponseEvent {
    public string FriendName { get; private set; }

    public OnSendFriendName (string friendName) {
        FriendName = friendName;
    }
    public void Invoke (EventManagerBase eventManagerBase) {
        // var lobbyManager = eventManagerBase as LobbyManager;
        // MainPanelUIManager.Instance.GetPanel<ClanPanel> ().SendClan (ClanName);

    }
}