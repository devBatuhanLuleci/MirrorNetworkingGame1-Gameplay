using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ACGAuthentication;

public class Client : MonoBehaviour
{
  public void SendHello()
    {

        var req = new LoginEvent("admin");
        LoadBalancer.Instance.ACGAuthenticationManager.SendClientRequestToServer(req);

    }

    public void StartMatch()
    {
        var ev = new CreateLobbyRoom();
        LoadBalancer.Instance.LobbyManager.SendClientRequestToServer(ev);
    }
}
