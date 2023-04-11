using System;
using ACGAuthentication;
using UnityEngine;

public class NotificationManager : Singleton<NotificationManager> {
    public enum InfoType {
        Info, //standart Debug
        InfoClient, // serverdan Clienta Debug
        InfoServer, //Clienttan servera  Debug
        InfoClientShow //  serverdan Clienta Popup
    }
    public static void Info (string message) {
        Debug.Log (message);
    }
    public static void IncomingInfo (string message, InfoType infoType) {
        switch (infoType) {
            case InfoType.InfoClient:
                Info (message);
                break;
            case InfoType.InfoClientShow:
                Info (message);
                break;
        }
    }
    public static void SendInfo (string message, InfoType infoType) {
        var ev = new SendNotificationInfo (message, infoType);
        SendClientRequestToServer (ev);
    }
    private static void SendClientRequestToServer (IEvent ev) {
        if (LoadBalancer.Instance == null) Debug.LogError ("LoadBalancer is null!");
        if (LoadBalancer.Instance.LobbyManager == null) Debug.LogError ("LobbyManager is null!");
        LoadBalancer.Instance.LobbyManager.SendClientRequestToServer (ev);
    }

}