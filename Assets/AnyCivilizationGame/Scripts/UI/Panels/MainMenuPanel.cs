using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuPanel : Panel
{
    [Header("Buttons")]
    public Button DroidsButton;
    public Button ShopButton;
    public Button UpdateButton;
    public Button ClanButton;
    public Button PlayButton;
    public Button SettingsMenuButton;

    [Header("Texts")]
    public TextMeshProUGUI MyNickName;

    private void Awake()
    {
        DroidsButton.onClick.AddListener(OnClick_DroidsButton);
        ShopButton.onClick.AddListener(OnClick_ShopButton);
        UpdateButton.onClick.AddListener(OnClick_UpdateButton);
        ClanButton.onClick.AddListener(OnClick_ClanButton);
        //  PlayButton.onClick.AddListener(OnClick_PlayButton);
        SettingsMenuButton.onClick.AddListener(OnClick_SettingsMenuButton);

        Profile profile = new Profile();
        MyNickName.text = profile.UserName;
        
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

}
