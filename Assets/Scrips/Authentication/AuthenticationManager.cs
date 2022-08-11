using Cysharp.Threading.Tasks;
using MoralisUnity;
using MoralisUnity.Kits.AuthenticationKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using WalletConnectSharp.Core.Models;

public class AuthenticationManager : Singleton<AuthenticationManager>
{
    #region Public Variables
    public User User { get; private set; }

    #endregion

    #region Events
    public UnityEvent OnUserUnregister;
    #endregion
    #region Login Methods

    public async void LoginWebAPI()
    {
        var moralisUser = await Moralis.GetUserAsync();
        var loginRequest = new LoginRequest(moralisUser.username);
        HttpClient.Instance.Get<User>(loginRequest, LoginSuccess, LoginFail);
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

    private void LoginSuccess(User user)
    {
        User = user;
    }

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
    public void MoralisOnStateChanged(AuthenticationKitState state)
    {
        Debug.Log($"AuthenticationManager moralis state changed. {state}");
        switch (state)
        {
            case AuthenticationKitState.None:
                break;
            case AuthenticationKitState.PreInitialized:
                break;
            case AuthenticationKitState.Initializing:
                break;
            case AuthenticationKitState.Initialized:
                // You have to wait or Unity will crash
                //await UniTask.DelayFrame(1);
                //AuthenticationKit.Instance.Connect();
                break;
            case AuthenticationKitState.WalletConnecting:
                break;
            case AuthenticationKitState.WalletConnected:

                break;
            case AuthenticationKitState.WalletSigning:
                break;
            case AuthenticationKitState.WalletSigned:
                break;
            case AuthenticationKitState.MoralisLoggingIn:
                break;
            case AuthenticationKitState.MoralisLoggedIn:
                LoginWebAPI();

                break;
            case AuthenticationKitState.Disconnecting:
                break;
            case AuthenticationKitState.Disconnected:
                break;
            default:
                break;
        }

    }
    #endregion



}
