using kcp2k;
using Mirror;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    #region Fields
    public static TestGameManager instance;
    public GameObject waitingText;

    NetworkManager networkManager;

    #endregion
    private void Awake()
    {
        networkManager = FindObjectOfType<NetworkManager>();
        if (ACGNetworkManager.Instance.IsServer)
        {
            Invoke("StartServerNetwork", 2f);
        }
        else
        {
            Invoke("StartClientNetwork", 2f);
        }
    }

    private void StartClientNetwork()
    {
        var vfxManagerPrefab = Resources.Load<GameObject>("VfxManager");
        if (vfxManagerPrefab == null)
        {
            Debug.LogError("vfxManagerPrefab is null");
        }
        else
        {
            NetworkClient.RegisterPrefab(vfxManagerPrefab.gameObject);
        }
        networkManager.networkAddress = ACGNetworkManager.Instance.NetworkAddress;
        networkManager.GetComponent<KcpTransport>().Port = ACGNetworkManager.Instance.Port;
        networkManager.StartClient();
        waitingText.GetComponent<TextMeshProUGUI>().text = $"Client Started. Port: {ACGNetworkManager.Instance.Port}";


    }
    private void StartServerNetwork()
    {
        if (networkManager == null)
        {
            Debug.LogError("when starting the server network manager is not found!");
            return;
        }

        if (!networkManager.TryGetComponent<KcpTransport>(out var transport))
        {
            Debug.LogError("when starting the server KcpTransport is not found!");
            return;
        }

        transport.Port = ACGNetworkManager.Instance.Port;
        networkManager.StartServer();
        string msg = $" <color=green> Server listining on </color> localhost:{ACGNetworkManager.Instance.Port}";
        Debug.LogError(msg);
        waitingText.GetComponent<TextMeshProUGUI>().text = $"Server listining: {ACGNetworkManager.Instance.Port}";
        LoadBalancer.Instance.SpawnServer.SendClientRequestToServer(new OnReadyEvent(ACGNetworkManager.Instance.Port));
        Setup();

    }


    private void Setup()
    {
        var vfxManagerPrefab = Resources.Load<GameObject>("VfxManager");
        if (vfxManagerPrefab == null)
        {
            Debug.LogError("vfxManagerPrefab is null");
        }
        else
        {
            var vfxManager = Instantiate(vfxManagerPrefab);
            NetworkServer.Spawn(vfxManager);
        }
    }

}


