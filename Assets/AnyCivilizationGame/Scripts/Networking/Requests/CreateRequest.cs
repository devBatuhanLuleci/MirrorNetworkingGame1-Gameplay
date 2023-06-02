using Assets.HttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CreateRequest : PostRequest
{

    public override string Url => "Auth/create";

    #region FormFields
    [FormField]
    public string Email;
    [FormField]
    public string WalletId;
    #endregion


    #region Properites
    public string Yeni = "test";
    #endregion

    public CreateRequest( string email, string walletId)
    {
        this.Email = email;
        WalletId = walletId;    
    }
}

