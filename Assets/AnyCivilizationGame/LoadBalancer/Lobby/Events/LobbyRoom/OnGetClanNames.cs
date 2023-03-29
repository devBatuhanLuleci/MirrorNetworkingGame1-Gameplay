using System.Collections;
using System.Collections.Generic;
using ACGAuthentication;
using UnityEngine;

public class OnGetClanNames : IResponseEvent {
    public string[] ClanNames;
    public bool IsNewClanNameCreate;
    public OnGetClanNames (string[] clanNames, bool isNewClanNameCreate) {
        ClanNames = clanNames;
        IsNewClanNameCreate = isNewClanNameCreate;
    }

    public void Invoke (EventManagerBase eventManagerBase) {
        MainPanelUIManager.Instance.GetPanel<ClanPanel> ().ClanNamesArrayRead (ClanNames, IsNewClanNameCreate);

    }
}