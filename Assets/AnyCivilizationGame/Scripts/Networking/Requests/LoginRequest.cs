using Assets.HttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class LoginRequest : IGetRequest
    {
        public string Url => $"Auth/login/{walletId}";

        public string walletId;
        public LoginRequest(string moralisID)
        {
            walletId = moralisID;
        }
    }

