using System.Collections;
using System.Collections.Generic;
using ACGAuthentication;
using UnityEngine;

public class SendNotificationInfo : IEvent {
    public string NotificationInfo { get; set; }
    public NotificationManager.InfoType InfoType;
    public SendNotificationInfo (string notificationInfo, NotificationManager.InfoType infoType) {
        NotificationInfo = notificationInfo;
        InfoType = infoType;
    }
}