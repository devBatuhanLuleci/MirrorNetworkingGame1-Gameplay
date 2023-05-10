using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HttpClient;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using WalletConnectSharp.Unity;
public class LoginPanel : Panel {
    /*  public TMP_Text m_EmailTmp;
      public TMP_InputField m_MoralisIdInput;
      public TMP_InputField m_EmailInput;

      public Button loginButton;
      public Button registerButton;
      */
    public Button LoginButton;
    public WalletConnect walletConnect;
    private void OnEnable () {
        AddListenerCall ();
    }
    private void OnDisable () {
        RemoveListenerCall ();
    }

    #region Listeners
    void AddListenerCall () {
        LoginButton.onClick.AddListener (OnClick_Login);
    }
    void RemoveListenerCall () {
        LoginButton.onClick.RemoveListener (OnClick_Login);
    }

    #endregion
    #region Buttons

    public void OnClick_Login () {
        if(walletConnect is null)
        {
            Debug.LogError("Login panelin icine WalletConnect i at");
        }
        walletConnect.OpenDeepLink();
       /* var loginReq = new LoginRequest (m_MoralisIdInput.text);
        HttpClient.Instance.Get<User> (loginReq, OnLoginSuccess, OnLoginFail);
        */
    }
    /*   public void OnClickRegister()
       {
           var createReq= new CreateRequest(m_MoralisIdInput.text, m_EmailInput.text,"admin-wallet");
           HttpClient.Instance.Post<User>(createReq, OnCreateSuccess);

       } */
    #endregion
    public void LoginControl (string address) {
        var loginReq = new LoginRequest (address);
        HttpClient.Instance.Get<User> (loginReq, OnLoginSuccess, OnLoginFail);
    }
    private void OnLoginFail (UnityWebRequest errorResponse) {
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

    /*   public void OnLoginSuccess (User user) {
           m_EmailTmp.text = user.email;
           Debug.Log ("OnLoginSuccess: " + user.email);
       }
       private void OnLoginFail (UnityWebRequest errorRespons) {
           if (errorRespons.responseCode == 404) // User not found.
           {
               // go Register
               m_EmailTmp.gameObject.SetActive (false);
               m_EmailInput.gameObject.SetActive (true);

               loginButton.gameObject.SetActive (false);
               registerButton.gameObject.SetActive (true);
           }
       }
       */

    /*    public void OnCreateSuccess(User user)
        {
            m_EmailTmp.text = user.email;
            Debug.Log("OnCreateSuccess: " + user.email);

            m_EmailTmp.gameObject.SetActive(true);
            m_EmailInput.gameObject.SetActive(false);

            loginButton.gameObject.SetActive(false);
            registerButton.gameObject.SetActive(false);

        }
        */

}