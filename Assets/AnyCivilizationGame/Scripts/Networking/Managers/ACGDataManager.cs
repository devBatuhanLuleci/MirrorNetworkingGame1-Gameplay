using kcp2k;
using Mirror;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ACGDataManager : MonoBehaviour
{
    private static ACGDataManager instance;
    public static ACGDataManager Instance { get { return instance; } }
    [SerializeField]
    private string gameSceneName = "GameScene";

    public GameData GameData { get; private set; }
    public LobbyPlayer LobbyPlayer { get;  set; }

    public void Awake()
    {
        InitSingleton();
        InitDatas();
    }

    private void InitDatas()
    {
        GameData = new GameData();
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
        GameData.IsServer = true;
        GameData.Port = port;
        //var transport = GetComponent<KcpTransport>();
        //transport.Port = port;
        //StartServer();
        SceneManager.LoadScene(gameSceneName);
    }

    public void StartClient(string netAddress, ushort port)
    {
        GameData.IsServer = false;
        GameData.Port = port;
        GameData.NetworkAddress = netAddress;

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
