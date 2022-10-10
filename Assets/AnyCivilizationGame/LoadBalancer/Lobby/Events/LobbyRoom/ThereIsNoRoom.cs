using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThereIsNoRoom : IResponseEvent
{
    public int RoomId { get; set; }
    public ThereIsNoRoom(int roomId)
    {
        RoomId = roomId;
    }

    public void Invoke(EventManagerBase eventManagerBase)
    {
        Debug.Log("ThereIsNoRoom invoked");
        ErrorPanel.Show($"There is no room with {RoomId} Id.");        
    }
}
