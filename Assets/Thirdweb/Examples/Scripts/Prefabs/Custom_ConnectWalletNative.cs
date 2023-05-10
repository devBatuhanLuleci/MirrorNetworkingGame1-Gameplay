using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using WalletConnectSharp.Core.Models;

public class Custom_ConnectWalletNative : Prefab_ConnectWalletNative {
    public TMP_Text AccountText;
    public override void Start () {
        address = null;
        OnConnect (supportedWallets[0]);
        connectButton.SetActive (false);
    }

    public override async void OnConnected () {
        connectButton.SetActive (false);
        AccountText.gameObject.SetActive (true);
        AccountText.text = address;
        
        MainPanelUIManager.Instance.loginPanel.GetComponent<LoginPanel> ().LoginControl (address);

        /*  var loginReq = new LoginRequest(address);
          HttpClient.Instance.Get<User>(loginReq, OnLoginSuccess, OnLoginFail);
          */

    }

  /*  private void OnLoginFail (UnityWebRequest errorResponse) {
        if (errorResponse.responseCode == 404) // User not found.
        {
            // go Register
            Debug.Log ("Register olmaya git.");
            MainPanelUIManager.Instance.JoinPanelShow ();
        }

    }

    private void OnLoginSuccess (User obj) {

        Debug.Log ($"OnLoginSuccess: {obj.moralisId} exists.");
        MainPanelUIManager.Instance.MainMenuPanelShow ();

    }
    */
}