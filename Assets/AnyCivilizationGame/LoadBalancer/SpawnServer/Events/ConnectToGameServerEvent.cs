using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectToGameServerEvent : IResponseEvent
{
    public ushort Port { get; private set; }
    public string Host { get; private set; }

    public ConnectToGameServerEvent(ushort port, string host)
    {
        Port = port;
        Host = host;
    }


    public void Invoke(EventManagerBase authenticationManager)
    {
        Debug.Log($"Start game on {Host}:{Port}");
        ACGNetworkManager.Instance.StartClient(Host, Port);
    }
}
