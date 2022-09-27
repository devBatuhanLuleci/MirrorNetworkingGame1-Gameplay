using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxPlayerError : IResponseEvent
{
    public void Invoke(EventManagerBase eventManagerBase)
    {
        var lobbyManager = eventManagerBase as LobbyManager;
        PopupManager.Show<ErrorPanel>(new ErrorPanelValue
        {
            ErrorText = "Room is have max player count. You can't join this room."
        });

    }
}
