
//using Cysharp.Threading.Tasks;
using kcp2k;
//using MoralisUnity;
//using MoralisUnity.Kits.AuthenticationKit;
//using MoralisUnity.Platform.Objects;
using Oddworm.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
//using WalletConnectSharp.Core.Models;

public class AuthenticationManager : Singleton<AuthenticationManager>
{
    #region Configurations
    [SerializeField]
    private LoginType loginType = LoginType.WalledId;


    [SerializeField] private string walletId = "admin";
    #endregion
    #region Public Variables
    [SerializeField]
    private User _user;
    public User User { get { return _user; } private set { _user = value; } }
    public LoginType LoginType => loginType;

    #endregion

    #region Private Variables
    private bool isServer = false;
    private bool isClient = false;
    public bool IsServer => isServer;
    public bool IsClient => isClient;
    #endregion
    #region Events
    public UnityEvent OnUserUnregister;
    public UnityEvent OnUserLogged;
    #endregion

    #region MonoBehaviour Call  Back
 
    #endregion
    #region Public Methods

    #region CommandLine
    public void StartAuth()
    {
        var gameData = ACGDataManager.Instance.GameData;
    //    MainPanelUIManager.Instance.GetPanel<LoadingPanel>().Info("Connecting to master server...");  //Yeni Loading Panelimiz olmadıgı icin kapattım
        if (gameData.TerminalType == TerminalType.Client)
        {
            if (gameData.Port != 0) ClientReady();
            else Login();
        }

        else
        {
            // TODO:
            // Setup server.
            MainPanelUIManager.Instance.GetPanel<LoadingPanel>().Info($"listining on {ACGDataManager.Instance.GameData.Port}");
            ServerReady();
        }
    }


    public void ServerReady()
    {
        ACGDataManager.Instance.StartServer();
    }
    public void ClientReady()
    {
        ACGDataManager.Instance.StartClient();
    }
    #endregion
    #region Login Methods

    public void Login()
    {
        Debug.Log($"Login Type: {loginType}");
        switch (loginType)
        {
            case LoginType.WalledId:
                LoginWebAPI(walletId);
                break;        
            case LoginType.Admin:
                LoginWebAPI("admin-wallet");
                break;
            case LoginType.None:
                OnUserLogged.Invoke();
                break;
        }
    }
    public void LoginWebAPI(string walletId = "")
    {
        var loginRequest = new LoginRequest(walletId);
        HttpClient.Instance.Get<User>(loginRequest, LoginSuccess, LoginFail);
    }
    public async void CreateUserWithMoralis(string email)
    {
       // var moralisUser = await Moralis.GetUserAsync();
      //  var createRequest = new CreateRequest(moralisUser.username, email, moralisUser.ethAddress);
     //   HttpClient.Instance.Post<User>(createRequest, CreateUserSuccess, CreateUserFail);

    }

    private void LoginSuccess(User user)
    {
        User = user;
        OnUserLogged.Invoke();
    }

    private void CreateUserSuccess(User user)
    {
        User = user;
        OnUserLogged.Invoke();

    }
    private void LoginFail(UnityWebRequest errorRespons)
    {
        var msg = $"<color=red> Login Fail </color> " + errorRespons.error;
        Debug.Log(msg);
        if (errorRespons.responseCode == 404) // User not found.
        {
            // go Register
            if (OnUserUnregister != null)
                OnUserUnregister.Invoke();
        }
    }

    private void CreateUserFail(UnityWebRequest errorRespons)
    {
        var msg = $"<color=red> Login Fail </color> " + errorRespons.error;
        Debug.Log(msg);

    }
    #endregion
    #endregion

    #region Moralis State Handling
    public void MoralisOnConnected()
    {
        Debug.Log("AuthenticationManager moralis connected.");
    }
    public void MoralisOnDisconnected()
    {
        Debug.Log("AuthenticationManager moralis disconnected.");

    }
    //public async void MoralisOnStateChanged(AuthenticationKitState state)
    //{
    //    Debug.Log($"AuthenticationManager moralis state changed. {state}");
    //    switch (state)
    //    {
    //        case AuthenticationKitState.None:
    //            break;
    //        case AuthenticationKitState.PreInitialized:
    //            break;
    //        case AuthenticationKitState.Initializing:
    //            break;
    //        case AuthenticationKitState.Initialized:
    //            // You have to wait or Unity will crash
    //            //await UniTask.DelayFrame(1);
    //            //AuthenticationKit.Instance.Connect();
    //            break;
    //        case AuthenticationKitState.WalletConnecting:
    //            break;
    //        case AuthenticationKitState.WalletConnected:

    //            break;
    //        case AuthenticationKitState.WalletSigning:
    //            break;
    //        case AuthenticationKitState.WalletSigned:
    //            break;
    //        case AuthenticationKitState.MoralisLoggingIn:
    //            break;
    //        case AuthenticationKitState.MoralisLoggedIn:
    //            var moralisUser = await Moralis.GetUserAsync();
    //            LoginWebAPI(moralisUser.username);
    //            break;
    //        case AuthenticationKitState.Disconnecting:
    //            break;
    //        case AuthenticationKitState.Disconnected:
    //            break;
    //        default:
    //            break;
    //    }

    //}
    #endregion
}
