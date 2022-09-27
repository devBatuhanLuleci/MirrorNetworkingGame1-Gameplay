using ACGAuthentication;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PeerBase 
{
    #region Public Fiealds
    public int connectionId { get; private set; }

    #endregion


    #region Private Properties
    private LoadBalancer loadBalancer;
    #endregion


    public PeerBase(LoadBalancer loadBalancer, int connectionId)
    {
        this.loadBalancer = loadBalancer;
        this.connectionId = connectionId;
    }

    public void Send(ArraySegment<byte> segment, int channelId = Channels.Reliable)
    {
        // NetworkReader and NetworkReader will using fore read and write
        //loadBalancer.ServerSend(connectionId, segment, channelId);
    }
    public void Disconnect()
    {

        loadBalancer.ServerDisconnect(connectionId);
        OnDisconnected();
    }



    protected virtual void OnDisconnected()
    {

    }
}
