using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSendAccessTokenKey : IResponseEvent
{
    public bool IsAccessTokenKey { get; set; }

    public OnSendAccessTokenKey (bool isAccessTokenKey) {
        IsAccessTokenKey = isAccessTokenKey;
    }
    public void Invoke (EventManagerBase eventManagerBase) {
        //Access token cevabını buraya geliyor..
        MainPanelUIManager.Instance.GetPanel<LoginPanel> ().AccessTokenResponse(IsAccessTokenKey);
        // var lobbyManager = eventManagerBase as LobbyManager;
        // MainPanelUIManager.Instance.GetPanel<ClanPanel> ().SendClan (ClanName);

    }
}
