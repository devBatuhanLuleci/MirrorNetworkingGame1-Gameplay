using Mirror;
using UnityEngine;
namespace ACGAuthentication
{

    public class LoginEvent : IEvent
    {

        public string AccessToken { get; set; }


        public LoginEvent(string accessToken)
        {
            AccessToken = accessToken;
        }

    }

}
