using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACGAuthentication;

public class Client : MonoBehaviour
{
  public void SendHello()
    {

        var req = new LoginEvent("admin", "admin");
        LoadBalancer.Instance.AuthenticationManager.SendClientRequestToServer(req);

    }

    public void StartMatch()
    {
        var ev = new StartMatchEvent();
        LoadBalancer.Instance.LobbyManager.SendClientRequestToServer(ev);
    }
}
