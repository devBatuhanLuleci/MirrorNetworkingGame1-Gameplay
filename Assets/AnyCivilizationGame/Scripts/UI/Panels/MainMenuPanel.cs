using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ACGAuthentication;

public class MainMenuPanel : Panel
{
    [Header("Buttons")]
    public Button DroidsButton;
    public Button ShopButton;
    public Button UpdateButton;
    public Button ClanButton;
    public Button PlayButton;
    public Button SettingsMenuButton;
    public Button ProfileButton;

    [Header("Texts")]
    public TextMeshProUGUI MyNickName;

    [Header("Droid")]
    [SerializeField] GameObject MainDroid;

    private void Awake()
    {
        /*  Profile profile = new Profile();
          MyNickName.text = profile.UserName; */
    }
    private void OnEnable()
    {
        AddListenerCall();

        if (MainDroid == null)
            return;

        MainDroid.SetActive(true);
    }
    private void OnDisable()
    {
        RemoveListenerCall();

        if (MainDroid == null)
            return;

        MainDroid.SetActive(false);
    }

    #region Listeners
    void AddListenerCall()
    {
        DroidsButton.onClick.AddListener(OnClick_DroidsButton);
        ShopButton.onClick.AddListener(OnClick_ShopButton);
        UpdateButton.onClick.AddListener(OnClick_UpdateButton);
        ClanButton.onClick.AddListener(OnClick_ClanButton);
        //  PlayButton.onClick.AddListener(OnClick_PlayButton);
        SettingsMenuButton.onClick.AddListener(OnClick_SettingsMenuButton);
        ProfileButton.onClick.AddListener(OnClick_ProfileButton);
    }
    void RemoveListenerCall()
    {
        DroidsButton.onClick.RemoveListener(OnClick_DroidsButton);
        ShopButton.onClick.RemoveListener(OnClick_ShopButton);
        UpdateButton.onClick.RemoveListener(OnClick_UpdateButton);
        ClanButton.onClick.RemoveListener(OnClick_ClanButton);
        //  PlayButton.onClick.AddListener(OnClick_PlayButton);
        SettingsMenuButton.onClick.RemoveListener(OnClick_SettingsMenuButton);
        ProfileButton.onClick.RemoveListener(OnClick_ProfileButton);
    }
    #endregion

    #region Buttons

    public void OnClick_PlayButton()
    {

    }
    public void OnClick_DroidsButton()
    {
        MainPanelUIManager.Instance.DroidsPanelShow();
    }
    public void OnClick_ShopButton()
    {
        MainPanelUIManager.Instance.ShopPanelShow();
    }
    public void OnClick_UpdateButton()
    {
        MainPanelUIManager.Instance.UpdatePanelShow();
    }
    public void OnClick_ClanButton()
    {
        MainPanelUIManager.Instance.ClanPanelShow();
        MainPanelUIManager.Instance.clanPanel.GetComponent<ClanPanel>().isDirectMainMenu = true;
    }
    /*  public void OnClick_PlayButton()  //gameplay panel eklenince aktiflesecek
     {
         MainPanelUIManager.Instance.();
     }*/
    public void OnClick_SettingsMenuButton()
    {
        MainPanelUIManager.Instance.SettingsMenuPanelShow();
    }
    public void OnClick_ProfileButton()
    {
        MainPanelUIManager.Instance.MyProfilePanelShow();
        MainPanelUIManager.Instance.myProfilePanel.GetComponent<MyProfilePanel>().isDirectMyProfilePanel = true;
    }
    #endregion

    private void SendClientRequestToServer(IEvent ev)
    {
        if (LoadBalancer.Instance == null) Debug.LogError("LoadBalancer is null!");
        if (LoadBalancer.Instance.LobbyManager == null) Debug.LogError("LobbyManager is null!");
        LoadBalancer.Instance.LobbyManager.SendClientRequestToServer(ev);
    }
}
