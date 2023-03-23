using System;
using System.Collections;
using System.Collections.Generic;
using ACGAuthentication;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClanPanel : Panel {
    [Header ("Buttons")]
    public Button BackButton;
    public Button ClanProfileView_BackButton;
    public Button CreateClanProfile_BackButton;
    public Button SearchButton;
    public Button CreateClanButton;
    public Button ClanJoinButton;
    public Button ClanInviteButton;

    [Header ("InputFields")]
    [SerializeField] TMP_InputField SearchInputField;
    [SerializeField] TMP_InputField CreateClanInputField;

    [Header ("Panels")]
    [SerializeField] Panel BackPanel;
    [SerializeField] GameObject MainFeatureViews_Panel;
    [SerializeField] GameObject ClanProfileView_Panel;
    [SerializeField] GameObject CreateClanProfile_Panel;

    [Header ("Others")]
    [SerializeField] public bool isDirectMainMenu = false;
    [SerializeField] GameObject ClanItemPrefab;
    [SerializeField] GameObject List;
    [SerializeField] GameObject Content;
    [SerializeField] GameObject[] Elements;
    int TotalElements = 0;

    public string[] clanNames; //= { "ClanZET", "ClanROZ", "ClanPIZ", "ClanKSK", "ClanRET", "ClanLKS", "ClanQWE", "ClanKNG", "ClanFYT", "ClanKES" };

    private void OnEnable () {

        AddListenerCall ();

    }
    private void OnDisable () {
        RemoveListenerCall ();
    }
    public void ClanNamesArrayRead (string[] ClanNames) {

        clanNames = ClanNames;

        ClansCreator ();
        ItemLengthControl ();
    }
    private void Awake () {
        GetClans ();
    }
    #region Listeners
    void AddListenerCall () {
        BackButton.onClick.AddListener (OnClick_BackButton);
        ClanProfileView_BackButton.onClick.AddListener (OnClick_ClanProfileView_BackButton);
        CreateClanProfile_BackButton.onClick.AddListener (OnClick_CreateClanProfile_BackButton);
        SearchButton.onClick.AddListener (OnClick_SearchButton);
        CreateClanButton.onClick.AddListener (OnClick_CreateClanButton);
        ClanJoinButton.onClick.AddListener (OnClick_ClanJoinButton);
        ClanInviteButton.onClick.AddListener (OnClick_ClanInviteButton);
    }
    void RemoveListenerCall () {
        BackButton.onClick.RemoveListener (OnClick_BackButton);
        ClanProfileView_BackButton.onClick.RemoveListener (OnClick_ClanProfileView_BackButton);
        CreateClanProfile_BackButton.onClick.RemoveListener (OnClick_CreateClanProfile_BackButton);
        SearchButton.onClick.RemoveListener (OnClick_SearchButton);
        CreateClanButton.onClick.RemoveListener (OnClick_CreateClanButton);
        ClanJoinButton.onClick.RemoveListener (OnClick_ClanJoinButton);
        ClanInviteButton.onClick.RemoveListener (OnClick_ClanInviteButton);
    }
    #endregion

    private void SendClientRequestToServer (IEvent ev) {
        if (LoadBalancer.Instance == null) Debug.LogError ("LoadBalancer is null!");
        if (LoadBalancer.Instance.LobbyManager == null) Debug.LogError ("LobbyManager is null!");
        LoadBalancer.Instance.LobbyManager.SendClientRequestToServer (ev);
    }
    public void GetClans () {
        var ev = new GetClanNames ();
        SendClientRequestToServer (ev);
    }

    #region Buttons
    public void OnClick_BackButton () {
        if (isDirectMainMenu) {
            MainPanelUIManager.Instance.BackButton (BackPanel);
        } else {
            MainPanelUIManager.Instance.BackButton (MainPanelUIManager.Instance.settingMenuPanel);
        }

        ClearInputField ();
    }
    public void OnClick_ClanProfileView_BackButton () {
        ClanProfileView_BackButton.gameObject.SetActive (false);
        BackButton.gameObject.SetActive (true);
        List.SetActive (true);
        ClanProfileView_Panel.SetActive (false);
        MainFeatureViews_Panel.SetActive (true);
    }
    public void OnClick_CreateClanProfile_BackButton () {
        CreateClanProfile_BackButton.gameObject.SetActive (false);
        BackButton.gameObject.SetActive (true);
        List.SetActive (true);
        CreateClanProfile_Panel.SetActive (false);
        MainFeatureViews_Panel.SetActive (true);
    }
    public void OnClick_SearchButton () {
        string searchText = SearchInputField.text.Trim ();

        if (string.IsNullOrEmpty (searchText)) {
            foreach (GameObject element in Elements) {
                element.SetActive (true);
            }
            return;
        }

        int searchTxtLength = searchText.Length;

        //int searchedElements = 0;

        foreach (GameObject element in Elements) {
            //searchedElements += 1;

            // if (element.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text.Length >= searchTxtLength)
            // {
            //searchText.ToLower() == element.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text.Substring(0, searchTxtLength).ToLower()
            if (element.transform.GetChild (1).GetComponent<TextMeshProUGUI> ().text.Contains (searchText, StringComparison.OrdinalIgnoreCase)) {
                element.SetActive (true);
            } else {
                element.SetActive (false);
            }
            // }
        }
    }
    public void OnClick_CreateClanButton () {
        string ClanNameText = CreateClanInputField.text.Trim ();

        if (string.IsNullOrEmpty (ClanNameText))
            return;

        int sum = 0;

        foreach (GameObject element in Elements) {
            if (string.Equals (ClanNameText, element.transform.GetChild (1).GetComponent<TextMeshProUGUI> ().text, StringComparison.OrdinalIgnoreCase)) {
                sum += 1;
                Debug.Log ("Clan zaten mevcut.");
                break;
            }
        }

        if (sum > 0)
            return;

        ClanCreate (CreateClanInputField.text.Trim (), Elements.Length + 1);

        ClearInputField ();

        BackButton.gameObject.SetActive (false);
        CreateClanProfile_BackButton.gameObject.SetActive (true);
        List.SetActive (false);
        MainFeatureViews_Panel.SetActive (false);
        CreateClanProfile_Panel.SetActive (true);

        ItemLengthControl ();
        Debug.Log ("Yeni Clan olusturuldu.");

    }
    public void OnClick_ClanJoinButton () {
        // MainPanelUIManager.Instance.BackButton();
    }
    public void OnClick_ClanInviteButton () {
        // MainPanelUIManager.Instance.BackButton();
    }
    public void OnClick_ClanProfileButton () {
        // MainPanelUIManager.Instance.ClanProfilePanelShow();
        // MainPanelUIManager.Instance.clanProfilePanel.GetComponent<ClanProfilePanel>().isDirectClanPanel = true;
        BackButton.gameObject.SetActive (false);
        ClanProfileView_BackButton.gameObject.SetActive (true);
        List.SetActive (false);
        MainFeatureViews_Panel.SetActive (false);
        ClanProfileView_Panel.SetActive (true);

        ClearInputField ();
    }
    #endregion

    public void ClansCreator () {
      
        if (Elements.Length > 0)
            return;

        for (int i = 0; i < clanNames.Length; i++) {
            ClanCreate (clanNames[i], i + 1);
        }
    }
    void ClanCreate (string _clanName, int i) {
        GameObject _clanItem = Instantiate (ClanItemPrefab, ClanItemPrefab.transform.GetComponent<RectTransform> ());
        _clanItem.transform.GetComponent<RectTransform> ().SetParent (Content.transform.GetComponent<RectTransform> ());
        _clanItem.transform.localScale = new Vector3 (1, 1, 1);
        _clanItem.transform.GetChild (0).GetComponent<TextMeshProUGUI> ().text = (i).ToString ();
        _clanItem.transform.GetChild (1).GetComponent<TextMeshProUGUI> ().text = _clanName;
        _clanItem.transform.GetChild (2).GetComponent<Button> ().onClick.AddListener (OnClick_ClanProfileButton);
        _clanItem.SetActive (true);
    }

    void ItemLengthControl () {
        TotalElements = Content.transform.childCount;
        Elements = new GameObject[TotalElements - 1];

        for (int i = 1; i < TotalElements; i++) {
            Elements[i - 1] = Content.transform.GetChild (i).gameObject;
        }
    }
    void ClearInputField () {
        SearchInputField.text = string.Empty;
        CreateClanInputField.text = string.Empty;
    }
}