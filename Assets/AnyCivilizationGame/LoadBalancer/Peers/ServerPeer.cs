using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerPeer : PeerBase
{
    public ServerPeer(LoadBalancer loadBalancer, int connectionId) : base(loadBalancer, connectionId)
    {
    }
}
