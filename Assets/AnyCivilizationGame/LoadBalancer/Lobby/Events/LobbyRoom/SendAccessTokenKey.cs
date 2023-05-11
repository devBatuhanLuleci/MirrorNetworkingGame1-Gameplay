using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendAccessTokenKey : IEvent
{
    public string AccessTokenKey { get; set; }

    public SendAccessTokenKey(string accessTokenKey)
    {
        AccessTokenKey = accessTokenKey;
    }
}
