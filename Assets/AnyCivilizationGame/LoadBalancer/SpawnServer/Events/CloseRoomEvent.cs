using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRoomEvent : IResponseEvent
{
    public void Invoke(EventManagerBase eventManagerBase)
    {
        Application.Quit(); 
    }
}
