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
    public string WalletId;  //MoralisId
    [FormField]
    public string UserName;  //Email
    [FormField]
    public string Email; //WalletId
    #endregion


    #region Properites
    public string Yeni = "test";
    #endregion

    public CreateRequest(string walletId, string userName, string email)
    {
        this.WalletId = walletId;
        this.UserName = userName;
        this.Email = email;
            
    }

   /* public CreateRequest(string moralisId, string email, string walletId)
    {
        this.MoralisId = moralisId;
        this.Email = email;
        WalletId = walletId;    
    }
    */
}

