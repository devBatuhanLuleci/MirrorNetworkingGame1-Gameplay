using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class LoginWalletIdPanel : Panel
{
    [SerializeField] TMP_InputField walletIdInput;

    public void OnClickLogin()
    {
        AuthenticationManager.Instance.LoginWebAPI(walletIdInput.text);
    }
}
