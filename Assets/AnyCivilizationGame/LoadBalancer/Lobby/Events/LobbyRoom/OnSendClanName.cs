using System.Collections;
using System.Collections.Generic;
using ACGAuthentication;
using UnityEngine;

public class OnSendClanName : IResponseEvent {
    public string ClanName { get; private set; }

    public OnSendClanName (string clanName) {
        ClanName = clanName;
    }
    public void Invoke (EventManagerBase eventManagerBase) {
       // var lobbyManager = eventManagerBase as LobbyManager;
       // MainPanelUIManager.Instance.GetPanel<ClanPanel> ().SendClan (ClanName);

    }
}