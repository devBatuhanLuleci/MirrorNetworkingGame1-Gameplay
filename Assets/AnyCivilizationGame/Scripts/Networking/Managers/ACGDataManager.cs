using DG.Tweening;
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
    public string GameSceneName = "GameScene";

    public GameData GameData;
    public LobbyPlayer LobbyPlayer { get; set; }
    [SerializeField]
    public Dictionary<string, CharacterData> Characters;
    [SerializeField]
    public Profile profile;
    private DataAdaptor DataAdaptor;
    bool playerJoined = false;
    public void Start()
    {
        DataAdaptor = DataAdaptorFactory.Get(AdapterType);
        //GetData();

        Setup();
    }
    public CharacterData GetCharacterData()
    {
        var characterProfileData = profile.Characters.ElementAt(0).Value;
        return Characters[characterProfileData.Name];
    }
    public async void GetData()
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
        //Debug.Log("--------------------Start------------------------------");
        //Debug.Log("UserName:" + profile.UserName);
        foreach (var charcter in profile.Characters)
        {
            foreach (var attribute in charcter.Value.Attributes)
            {
                var att = Characters[charcter.Key].Attributes[attribute.Key];
                att.Level = attribute.Value;
                Debug.Log($" {charcter.Key} - {attribute.Key} Level:{attribute.Value} value: {att.Value}");
            }
        }
        var charcterData = GetCharacterData();
        //Debug.LogError($"charcterData name: {charcterData.Name}");
        //Debug.Log("--------------------End------------------------------");

    }
    private void Setup()
    {
        InitSingleton();
        InitDatas();
        HandleCommands();
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
        AuthenticationManager.Instance.User.accessToken = "game-server";
        LoadBalancer.Instance.StartClient("localhost");
    }
    // TODO: this will be moved to its new location at a later date.
    public void StartClient(string gameServerAddress, ushort port)
    {
        GameData.Port = port;
        GameData.GameServerAddress = gameServerAddress;
        SceneManager.LoadScene(GameSceneName);
    }
    public void StartClient()
    {
        SceneManager.LoadScene(GameSceneName);
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
            Debug.Log("Starting as Server");
            GameData.TerminalType = TerminalType.Server;
        }

        if (CommandLine.HasKey("-client"))
        {
            GameData.GameServerAddress = CommandLine.GetString("-client", GameData.GameServerAddress);
            Debug.Log("starting as Client");
        }

        if (CommandLine.HasKey("-port"))
        {
            var Port = CommandLine.GetInt("-port", -1);
            Debug.LogFormat("Port: {0}", Port);
            GameData.Port = (ushort)Port;
        }
        if (CommandLine.HasKey("-host"))
        {
            var Host = CommandLine.GetString("-host", "");
            GameData.HostAddress = Host;
            Debug.LogFormat("Host: {0}", Host);
        }

        if (CommandLine.HasKey("-walletId"))
        {
            try
            {
                var walletIdStr = CommandLine.GetString("-walletId", "");
                if (int.TryParse(walletIdStr, out int walletIdInt))
                {
                    LoginWithWalletId(walletIdStr, walletIdInt);
                }
                else
                {
                    Debug.Log("*** wallet id not integer !! its " + $"->{walletIdStr}<-");
                }

            }
            catch (Exception ex)
            {

                Debug.LogError(ex.Message);
                throw;
            }


        }
    }

    private void LoginWithWalletId(string walletIdStr, int walletIdInt)
    {
        Debug.Log("*** wallet id's integer is " + $"->{walletIdInt}<-");
        StartCoroutine(WaitForPlayerLogin());
        IEnumerator WaitForPlayerLogin()
        {
            Debug.Log("**** WaitForPlayerLogin 1");
            bool isRoomCreator = (walletIdInt + 1) % 2 == 0;
            yield return new WaitUntil(() => MainPanelUIManager.Instance != null);
            Debug.Log("**** WaitForPlayerLogin 2");
            var mainPannelUIManager = MainPanelUIManager.Instance;
            var lobbyPanel = mainPannelUIManager.GetPanel<LobbyPanel>();
            AuthenticationManager.Instance.LoginWebAPI(walletIdStr);
            var mainPanelUIManager = mainPannelUIManager.GetPanel<MainPanelUIManager>();
            yield return new WaitUntil(() => mainPannelUIManager.RoomLobbyUIOpened == true);
            Debug.Log("**** room lobby oppened");
            int roomId = (walletIdInt / 2) - 1;
            if (isRoomCreator)
            {
                lobbyPanel.StartMatch();
                yield return new WaitUntil(() => playerJoined = true && lobbyPanel.PlayersCount >= 2);
                yield return new WaitForSeconds(UnityEngine.Random.Range(0.0f, 2.0f));
                Debug.Log("**** StartRoom");
                lobbyPanel.StartRoom();
            }
            else
            {
                yield return new WaitForSeconds(.5f);
                Debug.Log("**** try join room " + roomId);
                lobbyPanel.JoinRoom(roomId);
                Debug.Log("**** StateChange");
                lobbyPanel.StateChange();
            }
            Debug.Log($"**** Login with wallet ID {walletIdStr} " + (isRoomCreator ? "creating room " + roomId : "joining room " + (roomId).ToString()));
        }
    }

    internal void PlayerJoined()
    {
        playerJoined = true;
    }
    #endregion
}
