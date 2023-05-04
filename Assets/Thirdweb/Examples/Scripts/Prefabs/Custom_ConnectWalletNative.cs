using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        MainPanelUIManager.Instance.MainMenuPanelShow();
    }
}