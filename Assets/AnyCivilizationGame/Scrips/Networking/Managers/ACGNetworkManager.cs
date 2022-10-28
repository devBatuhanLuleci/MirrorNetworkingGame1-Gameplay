using kcp2k;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ACGNetworkManager : MonoBehaviour
{
    private static ACGNetworkManager instance;
    public static ACGNetworkManager Instance { get { return instance; } }
    [SerializeField]
    private string gameSceneName = "GameScene";
    public ushort Port { get; private set; }
    public string NetworkAddress { get; private set; }
    public bool IsServer { get; private set; }

    public void Awake()
    {
        InitSingleton();
    }

    private void InitSingleton()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void StartServer(ushort port)
    {
        IsServer = true;
        Port = port;
        //var transport = GetComponent<KcpTransport>();
        //transport.Port = port;
        //StartServer();
        SceneManager.LoadScene(gameSceneName);
    }

    public void StartClient(string netAddress, ushort port)
    {
        IsServer = false;
        Port = port;
        NetworkAddress = netAddress;

        //var transport = GetComponent<KcpTransport>();

        //networkAddress = netAddress;
        //transport.Port = port;
        //transport.enabled = true;
        //StartClient();
        SceneManager.LoadScene(gameSceneName);

    }
    //public void StartClient( ushort port)
    //{
    //    IsServer = false;
    //    Port = port;
    //    Debug.LogError("ben clientim:");

    //    //var transport = GetComponent<KcpTransport>();

    //    //networkAddress = netAddress;
    //    //transport.Port = port;
    //    //transport.enabled = true;
    //    //StartClient();
    //    SceneManager.LoadScene(gameSceneName);

    //}



}
