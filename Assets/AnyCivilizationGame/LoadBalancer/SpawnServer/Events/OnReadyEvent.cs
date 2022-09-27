using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnReadyEvent : IEvent
{
    public int Port { get; private set; }
    public OnReadyEvent(int port)
    {
        Port = port;
    }
}
