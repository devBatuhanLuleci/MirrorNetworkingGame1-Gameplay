using System.Collections;
using System.Collections.Generic;
using ACGAuthentication;
using UnityEngine;

public class OnGetClanNames : IResponseEvent {
    public string[] ClanNames;

    public OnGetClanNames (string[] clanNames) {
        ClanNames = clanNames;
    }

    public void Invoke (EventManagerBase eventManagerBase) 
    {
        MainPanelUIManager.Instance.GetPanel<ClanPanel> ().ClanNamesArrayRead (ClanNames);
       
    }
}