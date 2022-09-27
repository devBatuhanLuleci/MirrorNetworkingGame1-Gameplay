using Mirror;
using System;
using UnityEngine;

namespace ACGAuthentication
{
    public class RegisterRequest : IEvent
    {

        public string Email { get; set; }
        public string MoralisId { get; set; }
        public string WalletId { get; set; }
        public string UserName { get; set; }
        public RegisterRequest(string email, string moralisId, string walletId, string userName)
        {
            Email = email;
            MoralisId = moralisId;
            WalletId = walletId;
            UserName = userName;
        }    

    }

}