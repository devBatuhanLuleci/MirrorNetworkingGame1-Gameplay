using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.HttpClient;
using System;
using MoralisUnity.Kits.AuthenticationKit;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    public TMP_Text m_EmailTmp;
    public TMP_InputField m_MoralisIdInput;
    public TMP_InputField m_EmailInput;

    public Button loginButton;
    public Button registerButton;

    private void Start()
    {
        m_MoralisIdInput.text = AuthenticationKit.Instance.moralisID;
        m_MoralisIdInput.enabled = false;
    }

    public void OnClickLogin()
    {
        var loginReq = new LoginRequest(m_MoralisIdInput.text);
        HttpClient.Instance.Get<User>(loginReq, OnLoginSuccess, OnLoginFail);
    }


    public void OnClickRegister()
    {
        var createReq= new CreateRequest(m_MoralisIdInput.text, m_EmailInput.text);
        HttpClient.Instance.Post<User>(createReq, OnCreateSuccess);

    }



    public void OnLoginSuccess(User user)
    {
        m_EmailTmp.text = user.email;
        Debug.Log("OnLoginSuccess: " + user.email);
    }
    private void OnLoginFail(UnityWebRequest errorRespons)
    {
        if (errorRespons.responseCode == 404) // User not found.
        {
            // go Register
            m_EmailTmp.gameObject.SetActive(false);
            m_EmailInput.gameObject.SetActive(true);

            loginButton.gameObject.SetActive(false);
            registerButton.gameObject.SetActive(true);
        }
    }


    public void OnCreateSuccess(User user)
    {
        m_EmailTmp.text = user.email;
        Debug.Log("OnCreateSuccess: " + user.email);

        m_EmailTmp.gameObject.SetActive(true);
        m_EmailInput.gameObject.SetActive(false);

        loginButton.gameObject.SetActive(false);
        registerButton.gameObject.SetActive(false);

    }

}




