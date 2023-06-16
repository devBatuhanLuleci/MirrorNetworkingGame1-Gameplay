using ACGAuthentication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Space]
    [Header("Lobby Room Setup")]
    public GameObject ReadyButton;
    public GameObject StartGameButton;

    private Dictionary<string, UserButton> users = new Dictionary<string, UserButton>();
    public int PlayersCount => users.Count;
    public void SendHello()
    {
        var req = new ACGAuthentication.LoginEvent("admin");
        SendClientRequestToServer(req);
    }

    public void StartMatch()
    {
        var ev = new CreateLobbyRoom();
        SendClientRequestToServer(ev);
    }
    public void StartRoom()
    {
        var ev = new StartLobbyRoom();
        SendClientRequestToServer(ev);
    }
    public void JoinRoom()
    {
        var ev = new JoinLobbyRoom(int.Parse(roomCodeInput.text));
        SendClientRequestToServer(ev);
    }
    public void JoinRoom(int roomID)
    {
        Debug.Log("JoinRoom *****");
        var ev = new JoinLobbyRoom(roomID);
        SendClientRequestToServer(ev);
    }
    public void StateChange()
    {
        Debug.Log("StateChange");
        var ev = new ReadyStateChange();
        SendClientRequestToServer(ev);
    }
    public void StateChanged(LobbyPlayer lobbyPlayer)
    {
        var count = 0;
        if (users.TryGetValue(lobbyPlayer.UserName, out var user))
        {
            user.SetState(lobbyPlayer.IsReady);
        }
        foreach (var item in users)
        {
            if (item.Value.IsReady)
            {
                count++;
            }
        }
        StartGameButton.GetComponent<Button>().interactable = false;

        if (count >= 1)
        {
            StartGameButton.GetComponent<Button>().interactable = true;
        }

    }
    public void LeaveRoom(string userName)
    {
        var mine = LoadBalancer.Instance.LobbyManager.LobbyPlayer;
        if (userName == mine.UserName)
        {
            HideLobby();
            StartGameButton.SetActive(false);
            ReadyButton.SetActive(false);
            ClearList();
        }
        RemoveUser(userName);
    }
    public void LeaveRoom()
    {
        var ev = new LeaveRoom();
        SendClientRequestToServer(ev);
    }
    public void GetUsers()
    {
        // TODO: Send get user list request for friend test.
        var ev = new GetPlayersEvent();
        SendClientRequestToServer(ev);
    }

    public void CreateRoom(int roomCode, string userName)
    {
        roomCodeTMP.text = roomCode.ToString();
        AddUser(userName);
        HideJoin();
        StartGameButton.SetActive(true);
        ReadyButton.SetActive(false);
    }
    public void JoinedRoom(int roomCode, string userName)
    {
        roomCodeTMP.text = roomCode.ToString();
        AddUser(userName);
        HideJoin();
        StartGameButton.SetActive(false);
        ReadyButton.SetActive(true);
    }
    public void JoinRoom(string userName)
    {
        AddUser(userName);
    }


    private void AddUser(string userName)
    {
        if (users.TryGetValue(userName, out var user))
        {
            user.SetUserName(userName);
        }
        else
        {
            var prefab = Instantiate(userPrefab, userListParent.transform).GetComponent<UserButton>();
            prefab.Init(userName, OnClickSendPlayRequest);
            users.Add(userName, prefab);
        }
    }
    private void RemoveUser(string userName)
    {
        if (users.TryGetValue(userName, out var userButton))
        {
            Destroy(userButton.gameObject);
            users.Remove(userName);
        }

        if (StartGameButton.activeSelf)
            StartGameButton.GetComponent<Button>().interactable = users.Count >= 1;
    }
    private void ClearList()
    {
        foreach (var item in users)
        {

            Destroy(item.Value.gameObject);

        }
        users.Clear();
    }

    public void OnClickSendPlayRequest(string userName)
    {
        Debug.Log("OnClickSendPlayRequest: " + userName);
    }

    private void SendClientRequestToServer(IEvent ev)
    {
        if (LoadBalancer.Instance == null) Debug.LogError("LoadBalancer is null!");
        if (LoadBalancer.Instance.LobbyManager == null) Debug.LogError("LobbyManager is null!");
        LoadBalancer.Instance.LobbyManager.SendClientRequestToServer(ev);
    }



    private void HideJoin()
    {
        StartButton.SetActive(false);
        JoinPanel.SetActive(false);

        ShowLobbyWait();
    }
    private void HideLobby()
    {
        StartButton.SetActive(true);
        JoinPanel.SetActive(true);
        ShowJoint();
    }

    private void ShowLobbyWait()
    {
        LobbyWaitPanel.SetActive(true);
    }
    private void ShowJoint()
    {
        LobbyWaitPanel.SetActive(false);
    }


}

