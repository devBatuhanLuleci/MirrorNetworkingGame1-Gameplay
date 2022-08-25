using Assets.HttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class LoginRequest : IGetRequest
    {
        public string Url => $"Auth/login/{moralisId}";

        public string moralisId;
        public LoginRequest(string moralisID)
        {
            moralisId = moralisID;
        }
    }

