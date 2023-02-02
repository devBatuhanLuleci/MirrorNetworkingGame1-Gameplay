using kcp2k;
using Mirror;
using Newtonsoft.Json;
using Oddworm.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;


public class ACGDataManager : MonoBehaviour
{

    #region Singleton

    private static ACGDataManager instance;
    public static ACGDataManager Instance { get { return instance; } }
    #endregion

    [Header("Test Area")]
    [SerializeField] private string BootArgs = "";

    [Space]
    public DataAdaptorType AdapterType;
    [SerializeField]
    private string gameSceneName = "GameScene";

    public GameData GameData;
    public LobbyPlayer LobbyPlayer { get; set; }
    [SerializeField]
    public Dictionary<string, CharacterData> Characters;
    [SerializeField]
    public Profile profile;

    private DataAdaptor DataAdaptor;



    public void Start()
    {
        Setup();
        GetData();
        DataAdaptor = DataAdaptorFactory.Get(AdapterType);
    }
    private async void GetData()
    {
        try
        {
            Characters = await DataAdaptor.GetDataAsync();
            profile = await DataAdaptor.GetProfileAsync();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            Characters = new Dictionary<string, CharacterData>();
        }

        // logs.  
        Debug.Log("--------------------Start------------------------------");
        Debug.Log("UserName:" + profile.UserName);
        foreach (var charcter in profile.Characters)
        {
            foreach (var attribute in charcter.Value.Attributes)
            {
                var att = Characters[charcter.Key].Attributes[attribute.Key];
                att.Level = attribute.Value;
                Debug.Log($" {charcter.Key} - {attribute.Key} Level:{attribute.Value} value: {att.Value}");
            }
        }
        Debug.Log("--------------------End------------------------------");

    }
    private void Setup()
    {
        InitSingleton();
        InitDatas();
        HandleCommands();
        ConnectToTheMaster();
    }

    public void OnConnectedToMasterServer()
    {
        // TODO: Get Game Data from master
        StartAuth();
    }
    public void StartAuth()
    {
        AuthenticationManager.Instance.StartAuth();
    }

    private void ConnectToTheMaster()
    {
        if (!String.IsNullOrEmpty(GameData.HostAddress))
        {
            LoadBalancer.Instance.StartClient(GameData.HostAddress);
        }
        else
        {
            LoadBalancer.Instance.StartClient();
        }
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
    public void StartServer()
    {
        SceneManager.LoadScene(gameSceneName);
    }
    // TODO: this will be moved to its new location at a later date.
    public void StartClient(string gameServerAddress, ushort port)
    {
        GameData.Port = port;
        GameData.GameServerAddress = gameServerAddress;
        SceneManager.LoadScene(gameSceneName);
    }
    public void StartClient()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    #region HandleCommands
    private void HandleCommands()
    {
#if UNITY_EDITOR
        CommandLineInitializer.LoadCommandLine(" " + BootArgs);
#endif
        GameData.TerminalType = TerminalType.Client;
        GameData.GameServerAddress = LoadBalancer.Instance.Host.GetStringValue();

        if (CommandLine.HasKey("-server"))
        {
            Debug.LogError("Starting as Server");
            GameData.TerminalType = TerminalType.Server;
        }

        if (CommandLine.HasKey("-client"))
        {
            GameData.GameServerAddress = CommandLine.GetString("-client", GameData.GameServerAddress);
            Debug.LogError("starting as Client");
        }

        if (CommandLine.HasKey("-port"))
        {
            var Port = CommandLine.GetInt("-port", -1);
            Debug.LogErrorFormat("Port: {0}", Port);
            GameData.Port = (ushort)Port;
        }
        if (CommandLine.HasKey("-host"))
        {
            var Host = CommandLine.GetString("-host", "");
            GameData.HostAddress = Host;
            Debug.LogErrorFormat("Host: {0}", Host);
        }

    }


    #endregion
}
