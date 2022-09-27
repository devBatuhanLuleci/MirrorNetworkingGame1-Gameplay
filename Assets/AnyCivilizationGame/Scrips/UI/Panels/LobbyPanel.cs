using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : Panel
{
    public GameObject userListParent;
    public Button userPrefab;
    public TextMeshProUGUI roomCodeTMP;
    public TMP_InputField roomCodeInput;

    public GameObject StartButton;
    public GameObject JoinPanel;
    public GameObject LobbyWaitPanel;
    public void SendHello()
    {

        var req = new ACGAuthentication.LoginEvent("admin", "admin");
        LoadBalancer.Instance.AuthenticationManager.SendClientRequestToServer(req);

    }

    public void StartMatch()
    {
        var ev = new StartMatchEvent();
        LoadBalancer.Instance.LobbyManager.SendClientRequestToServer(ev);
    }

    public void JoinRoom()
    {
        var ev = new JoinLobbyRoom(int.Parse(roomCodeInput.text));

        LoadBalancer.Instance.LobbyManager.SendClientRequestToServer(ev);

    }

    public void GetUsers()
    {
        // TODO: Send get user list request for friend test.
        var ev = new GetPlayersEvent();
        LoadBalancer.Instance.LobbyManager.SendClientRequestToServer(ev);
    }

    public void CreateRoom(int roomCode, string userName)
    {
        roomCodeTMP.text = roomCode.ToString();
        AddUser(userName);
        HideJoin();
    }
    public void JoinRoom(string userName)
    {
        AddUser(userName);
    }
    public void RemoveRoom(string userName)
    {
        RemoveUser(userName);
    }
    private void HideJoin()
    {
        StartButton.SetActive(false);
        JoinPanel.SetActive(false);

        ShowLobbyWait();
    }

    private void ShowLobbyWait()
    {
        LobbyWaitPanel.SetActive(true);
    }

    public void RefreshFriends(int[] friends)
    {
        ClearList();
        for (int i = 0; i < friends.Length; i++)
        {
            var friend = friends[i];
            AddUser(friend.ToString());
        }
    }

    private void AddUser(string userName)
    {
        var prefab = Instantiate(userPrefab, userListParent.transform).GetComponent<UserButton>();
        prefab.Init(userName, OnClickSendPlayRequest);
    }
    private void RemoveUser(string userName)
    {
        var users = userListParent.GetComponentsInChildren<UserButton>();
        foreach (var user in users)
        {
            if (user.UserName == userName)
                Destroy(user.gameObject);
        }
    }
    private void ClearList()
    {
        foreach (var item in userListParent.GetComponentsInChildren<Transform>())
        {
            if (item.GetInstanceID() != userListParent.transform.GetInstanceID())
            {
                Destroy(item.gameObject);
            }
        }
    }

    public void OnClickSendPlayRequest(string userName)
    {
        Debug.Log("OnClickSendPlayRequest: " + userName);
    }
}

