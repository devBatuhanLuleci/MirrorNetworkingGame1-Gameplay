using Mirror;
using UnityEngine;
namespace ACGAuthentication
{

    public class LoginEvent : IEvent
    {

        public string UserName { get; set; }
        public string MoralisId { get; set; }

        public LoginEvent(string userName, string moralisId)
        {
            UserName = userName;
            MoralisId = moralisId;
        }          
       
    }

}
