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

    // TODO: this will be moved to its new location at a later date.
    public void StartServer(ushort port)
    {
        GameData.IsServer = true;
        GameData.Port = port;
        SceneManager.LoadScene(gameSceneName);
    }
    // TODO: this will be moved to its new location at a later date.
    public void StartClient(string netAddress, ushort port)
    {
        GameData.IsServer = false;
        GameData.Port = port;
        GameData.NetworkAddress = netAddress;
        SceneManager.LoadScene(gameSceneName);

    }

}
