using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyStateChanged : IResponseEvent
{
    public LobbyPlayer LobbyPlayer { get; private set; }
    public ReadyStateChanged(LobbyPlayer lobbyPlayer)
    {
        this.LobbyPlayer = lobbyPlayer;
    }

    public void Invoke(EventManagerBase eventManagerBase)
    {
        Debug.Log("ReadyStateChanged Invoked");
        var lobbyManager = (LobbyManager)eventManagerBase;

        MainPanelUIManager.Instance.GetPanel<LobbyPanel>().StateChanged(LobbyPlayer);
    }
}
