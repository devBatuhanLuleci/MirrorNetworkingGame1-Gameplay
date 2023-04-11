using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendFriendName : IEvent
{
   public string FriendName { get; set; }

    public SendFriendName(string friendName)
    {
        FriendName = friendName;
    }
}
