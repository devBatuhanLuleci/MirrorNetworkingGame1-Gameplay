using System;
using System.Collections;
using System.Collections.Generic;
using Assets.HttpClient;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class JoinPanel : Panel {
    public TMP_Text WalletId;
    public TMP_InputField UserNameInput;
    public TMP_InputField EmailInput;

    public TMP_Text UserNameErrorMessage;
    public TMP_Text EmailErrorMessage;

    public Button RegisterButton;

    private void OnEnable () {
        AddListenerCall ();
    }
    private void OnDisable () {
        RemoveListenerCall ();
    }

    #region Listeners
    void AddListenerCall () {
        RegisterButton.onClick.AddListener (OnClick_Register);
    }
    void RemoveListenerCall () {
        RegisterButton.onClick.RemoveListener (OnClick_Register);
    }

    #endregion
    #region Buttons
    public void OnClick_Register () {
        // var createReq= new CreateRequest(m_MoralisIdInput.text, m_EmailInput.text,"admin-wallet");
        var createReq = new CreateRequest (WalletId.text,UserNameInput.text,EmailInput.text);
        HttpClient.Instance.Post<User> (createReq, OnCreateSuccess,OnCreateFail);

    }
    #endregion
    public void OnCreateSuccess (User user) {
        
        MainPanelUIManager.Instance.MainMenuPanelShow();
     /*   m_EmailTmp.text = user.email;
        Debug.Log ("OnCreateSuccess: " + user.email);

        m_EmailTmp.gameObject.SetActive (true);
        m_EmailInput.gameObject.SetActive (false);

        loginButton.gameObject.SetActive (false);
        registerButton.gameObject.SetActive (false);
    */
    }
    private void OnCreateFail (UnityWebRequest errorResponse) {
        
        
    }
}