using kcp2k;
using Mirror;
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
            StartServerNetwork();
        }
        else
        {
            StartClientNetwork();
        }
    }

    private void StartClientNetwork()
    {
        networkManager.networkAddress = ACGNetworkManager.Instance.NetworkAddress;
        networkManager.GetComponent<KcpTransport>().Port = ACGNetworkManager.Instance.Port;
        networkManager.StartClient();
        waitingText.GetComponent<TextMeshProUGUI>().text = $"Client Started. Port: {ACGNetworkManager.Instance.Port}";

    }
    private void StartServerNetwork()
    {
        networkManager.networkAddress = "localhost";
        networkManager.GetComponent<KcpTransport>().Port = ACGNetworkManager.Instance.Port;
        networkManager.StartServer();
        waitingText.GetComponent<TextMeshProUGUI>().text = $"Server listining: {ACGNetworkManager.Instance.Port}";
        LoadBalancer.Instance.SpawnServer.SendClientRequestToServer(new OnReadyEvent(ACGNetworkManager.Instance.Port));


    }

}
