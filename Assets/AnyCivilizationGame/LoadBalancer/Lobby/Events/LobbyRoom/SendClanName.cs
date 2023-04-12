using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendClanName : IEvent
{
    public string ClanName { get; set; }

    public SendClanName(string clanName)
    {
        ClanName = clanName;
    }
}
