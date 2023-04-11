using System.Collections;
using System.Collections.Generic;
using ACGAuthentication;
using UnityEngine;

public class OnSendNotificationInfo : IResponseEvent {
    public string NotificationInfo; // { get; set; }
    public NotificationManager.InfoType InfoType;

    public OnSendNotificationInfo (string notificationInfo, NotificationManager.InfoType infoType) {
        NotificationInfo = notificationInfo;
        InfoType = infoType;
    }
    public void Invoke (EventManagerBase eventManagerBase) {
        NotificationManager.IncomingInfo(NotificationInfo,InfoType);
        // MainPanelUIManager.Instance.GetPanel<ClanPanel> ().SendClan (ClanName);
    }
}