using MoralisUnity.Kits.AuthenticationKit;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

class MainPanelUIManager : Panel
{
    [Header("Setup")]
    public Panel loadingPanel;
    public Panel loginPanel;
    public Panel registerPanel;
    public Panel PickCharacterPanel;
    public Panel CharacterNFTMintPanel;
    public Panel lobbyPanel;
    public Panel joinLoginPanel;
    public Panel joinPanel;
    public Panel mainMenuPanel;
    public Panel droidsPanel;
    public Panel droidProfilePanel;
    public Panel shopPanel;
    public Panel shopDroidBuyPanel;
    public Panel updatePanel;
    public Panel clanPanel;
    public Panel clanProfilePanel;
    public Panel settingMenuPanel;
    public Panel settingsPanel;
    public Panel languagePanel;
    public Panel myProfilePanel;
    public Panel myProfilePhotoPanel;
    public Panel roomCodePanel;
    public Panel lastMatchPanel;
    public Panel roomCodeDroidChoosePanel;
    public Panel roomCodeOnlineFriendsPanel;
    public Panel friendsPanel;
    public Panel friendProfilePanel;
    public Panel friendLastMatchPanel;
    public Panel leaderBoardPanel;
    public Panel mailBoxPanel;

    [Space]
    [SerializeField]
    public Panel startPanel = null;

    

    public static MainPanelUIManager Instance
    {
        get;   // get method
        private set;
    }


    #region MonoBehavior Methods

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Initialize();
        CheckServer();
    }


    #endregion
    #region Methods

    private void Initialize()
    {
        // if statPnael not null set startpanel else set LadingPanel
        var currentPanel = startPanel ?? loadingPanel;
        Show(currentPanel);
       // AddListeners();
    }



    public void MoralisLogin()
    {
        Show(loginPanel);
    }

    public void PickCharacterPanelShow()
    {
        Show(PickCharacterPanel);
    }

    #region UI Panel and Button Methods
    public void BackButton(Panel panel)
    {
        Show(panel);
    }
    /* public void ClanSearchButton()
     {

     }
     public void ClanCreateButton()
     {

     }
     public void FriendAddButton()
     {

     }
     public void GlobalButton()
     {

     }
     public void LocalButton()
     {

     }
     public void ClanButton()
     {

     }
      public void ChatSendButton()
     {

     }
      public void InviteButton()
     {

     }
     */

    public void JoinPanelShow()
    {
        Show(joinPanel);
    }
    public void LoginPanelShow()
    {
        Show(loginPanel);
    }
    public void DroidsPanelShow()
    {
        Show(droidsPanel);
    }
    public void ShopPanelShow()
    {
        Show(shopPanel);
    }
    public void ShopDroidBuyPanelShow()
    {
        Show(shopDroidBuyPanel);
    }
    public void UpdatePanelShow()
    {
        Show(updatePanel);
    }
    public void ClanPanelShow()
    {
        Show(clanPanel);
    }
    /* public void GamePlayPanelShow()
     {
         //gameplay panel gelince eklenecek
     }*/
    public void SettingsMenuPanelShow()
    {
        Show(settingMenuPanel);
    }
    public void DroidProfilePanelShow()
    {
        Show(droidProfilePanel);
    }

    public void MyProfilePanelShow()
    {
        Show(myProfilePanel);
    }
    public void SettingsPanelShow()
    {
        Show(settingsPanel);
    }
    public void FriendsPanelShow()
    {
        Show(friendsPanel);
    }
    public void MailBoxPanelShow()
    {
        Show(mailBoxPanel);
    }
    public void LeaderBoardPanelShow()
    {
        Show(leaderBoardPanel);
    }
    public void MyProfilePhotoPanelShow()
    {
        Show(myProfilePhotoPanel);
    }
    public void LastMatchPanelShow()
    {
        Show(lastMatchPanel);
    }
    public void FriendProfilePanelShow()
    {
        Show(friendProfilePanel);
    }
    public void FriendLastMatchPanelShow()
    {
        Show(friendLastMatchPanel);
    }
    public void ClanProfilePanelShow()
    {
        Show(clanProfilePanel);
    }
    public void RoomCodeOnlineFriendsPanel()
    {
        Show(roomCodeOnlineFriendsPanel);
    }
    public void RoomCodeDroidChoosePanel()
    {
        Show(roomCodeDroidChoosePanel);
    }
    public void LanguagePanel()
    {
        Show(languagePanel);
    }

    #endregion

    /// <summary>
    /// Check Server update
    /// </summary>
    private void CheckServer()
    {
    }


    #endregion

    #region Listeners

    private void AddListeners()
    {
        AuthenticationKit.Instance.OnConnected.AddListener(OnConnected);
        AuthenticationManager.Instance.OnUserUnregister.AddListener(OnUserUnregister);
        AuthenticationManager.Instance.OnUserLogged.AddListener(OnUserLogged);

    }
    private void RemoveListeners()
    {
        AuthenticationKit.Instance.OnConnected.RemoveListener(OnConnected);
        AuthenticationManager.Instance.OnUserUnregister.RemoveListener(OnUserUnregister);
        AuthenticationManager.Instance.OnUserLogged.RemoveListener(OnUserLogged);
    }

    #endregion

    #region Moralis Event Handler
    private void OnConnected()
    {
        loginPanel.Close();
        Debug.Log("User successfully logged so time to connect to the game server. :)");
    }
    #endregion

    #region Authentication Event Handler

    private void OnUserLogged()
    {
        // TODO check user have any nft character 
        // ?f not  open Nft buy panel
        Show(lobbyPanel);

    }
    private void OnUserUnregister()
    {
        Show(registerPanel);
    }

    internal void Login()
    {

        var loginType = AuthenticationManager.Instance.LoginType;
        var isServer = AuthenticationManager.Instance.IsServer;
        if (isServer) return;
        switch (loginType)
        {
            case LoginType.Moralis:
                MoralisLogin();
                break;
            case LoginType.User:
                Debug.Log("login with custom user");
                break;
            case LoginType.Admin:
                Debug.Log("login with Admin");
                break;
            case LoginType.None:
                Show(lobbyPanel);
                break;
            default:
                break;
        }
    }
    #endregion

}
