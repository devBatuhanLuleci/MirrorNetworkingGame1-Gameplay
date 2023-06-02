
using ACGAuthentication;
using UnityEngine;

public class RoomInfoEvent : IResponseEvent
{
    public string[] teamA { get; set; }
    public string[] teamB { get; set; }

    public void Invoke(EventManagerBase eventManagerBase)
    {
        var spawnServer = eventManagerBase as SpawnServer;
        Debug.Log("RoomInfoEvent recived!");
        spawnServer.Debug("RoomInfoEvent recived!");

        foreach (var team in teamA)
        {
            spawnServer.Debug("Team A member:" + team);
        }

        foreach (var team in teamB)
        {
            spawnServer.Debug("Team B member:" + team);
        }
    }
}

