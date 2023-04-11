using System;
using System.Collections;
using System.Collections.Generic;
using ACGAuthentication;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FriendsPanel : Panel {
    [Header ("Buttons")]
    public Button BackButton;
    public Button FriendAddButton;

    [Header ("InputFields")]
    [SerializeField] TMP_InputField FriendNameInputField;

    [Header ("Panels")]
    [SerializeField] Panel BackPanel;

    [Header ("Others")]
    [SerializeField] public bool isDirectMainMenu = false;
    [SerializeField] GameObject FriendItemPrefab;
    [SerializeField] GameObject List;
    [SerializeField] GameObject Content;
    [SerializeField] List<GameObject> Elements = new List<GameObject> ();
    int TotalElements = 0;
    public string[] friendNames;

    private void OnEnable () {
        AddListenerCall ();
        GetFriends ();
    }
    private void OnDisable () {
        RemoveListenerCall ();
    }

    #region Listeners
    void AddListenerCall () {
        BackButton.onClick.AddListener (OnClick_BackButton);
        FriendAddButton.onClick.AddListener (OnClick_FriendAddButton);
    }
    void RemoveListenerCall () {
        BackButton.onClick.RemoveListener (OnClick_BackButton);
        FriendAddButton.onClick.RemoveListener (OnClick_FriendAddButton);
    }
    #endregion
    private void SendClientRequestToServer (IEvent ev) {
        if (LoadBalancer.Instance == null) Debug.LogError ("LoadBalancer is null!");
        if (LoadBalancer.Instance.LobbyManager == null) Debug.LogError ("LobbyManager is null!");
        LoadBalancer.Instance.LobbyManager.SendClientRequestToServer (ev);
    }
    public void GetFriends () {
        var ev = new GetFriendNames ();
        SendClientRequestToServer (ev);
    }
    public void SendFriend (string friendName) {

        var ev = new SendFriendName (friendName);
        SendClientRequestToServer (ev);
    }
    public void SendDestroyFriend (string friendName) {

        // var ev = new SendFriendName (friendName);
        // SendClientRequestToServer (ev);
    }
    public void FriendNamesArrayRead (string[] FriendNames, bool _isNewFriendNameAdd) {

        friendNames = FriendNames;

        FriendsCreator (_isNewFriendNameAdd);
    }
    #region Buttons
    public void OnClick_BackButton () {

        ClearInputField ();
        MainPanelUIManager.Instance.BackButton (BackPanel);
    }
    public void OnClick_FriendAddButton () {

        string FriendNameText = FriendNameInputField.text.Trim ();

        if (string.IsNullOrEmpty (FriendNameText))
            return;

        SendFriend (FriendNameText);

    }
    public void OnClick_FriendProfileButton () {

        ClearInputField ();
        MainPanelUIManager.Instance.FriendProfilePanelShow ();
        MainPanelUIManager.Instance.friendProfilePanel.GetComponent<FriendProfilePanel> ().isDirectFriendsPanel = true;

    }
    public void OnClick_FriendDestroyButton () { // silmek icin id kullanmak ?  
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        Debug.Log ("Clicked Friend Item: " + clickedButton.transform.parent.GetChild (1).GetComponent<TextMeshProUGUI> ().text);
        string _DestroyFriendName = clickedButton.transform.parent.GetChild (1).GetComponent<TextMeshProUGUI> ().text;
       // SendDestroyFriend (_DestroyFriendName);
    }
    #endregion
    public void FriendsCreator (bool _isNewFriendNameAdd) {

        ElementListClear ();

        for (int i = 0; i < friendNames.Length; i++) {
            FriendCreate (friendNames[i], i + 1);

            if (i == friendNames.Length - 1) {
                ItemLengthControl ();
            }
        }

        if (!_isNewFriendNameAdd)
            return;

        ClearInputField ();
        OnClick_FriendProfileButton ();

        Debug.Log ("Yeni Friend olusturuldu.");

    }
    void FriendCreate (string _friendName, int i) {

        GameObject _friendItem = Instantiate (FriendItemPrefab, FriendItemPrefab.transform.GetComponent<RectTransform> ());
        _friendItem.transform.GetComponent<RectTransform> ().SetParent (Content.transform.GetComponent<RectTransform> ());
        _friendItem.transform.localScale = new Vector3 (1, 1, 1);
        _friendItem.transform.GetChild (0).GetComponent<TextMeshProUGUI> ().text = (i).ToString ();
        _friendItem.transform.GetChild (1).GetComponent<TextMeshProUGUI> ().text = _friendName;
        _friendItem.transform.GetChild (2).GetComponent<Button> ().onClick.AddListener (OnClick_FriendProfileButton);
        _friendItem.transform.GetChild (4).GetComponent<Button> ().onClick.AddListener (OnClick_FriendDestroyButton);
        _friendItem.SetActive (true);
    }

    void ItemLengthControl () {

        TotalElements = Content.transform.childCount;
        for (int i = TotalElements - 1; i > 0; i--) {
            if (Content.transform.GetChild (i).gameObject.activeSelf) {
                Elements.Add (Content.transform.GetChild (i).gameObject);
            } else {
                Destroy (Content.transform.GetChild (i).gameObject);
            }
        }
        Elements.Reverse ();

    }
    void ClearInputField () {
        FriendNameInputField.text = string.Empty;

    }
    void ElementListClear () {
        if (Elements.Count > 0) {

            Elements.Clear ();
            foreach (Transform _go in Content.transform) {
                if (_go != Content.transform && _go != Content.transform.GetChild (0).transform)
                    _go.gameObject.SetActive (false);
            }
        }
    }
}