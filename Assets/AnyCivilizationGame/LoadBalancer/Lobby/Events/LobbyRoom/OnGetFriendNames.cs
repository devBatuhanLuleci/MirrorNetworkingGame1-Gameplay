using System.Collections;
using System.Collections.Generic;
using ACGAuthentication;
using UnityEngine;

public class OnGetFriendNames : IResponseEvent {
    public string[] FriendNames;
    public bool IsNewFriendNameAdd;
    public OnGetFriendNames (string[] friendNames, bool isNewFriendNameAdd) {
        FriendNames = friendNames;
        IsNewFriendNameAdd = isNewFriendNameAdd;
    }

    public void Invoke (EventManagerBase eventManagerBase) {
        
        MainPanelUIManager.Instance.GetPanel<FriendsPanel> ().FriendNamesArrayRead (FriendNames, IsNewFriendNameAdd);

    }
}