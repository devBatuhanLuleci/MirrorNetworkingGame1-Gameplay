using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : Panel
{
    public Button BackButton;
    public Button LanguageButton;
    public Button MetamaskButton;
    public Button PrivacyPolicyButton;
    public Button TermsofServiceButton;
    [SerializeField] Panel BackPanel;
   
    private void OnEnable()
    {
        AddListenerCall();
    }
    private void OnDisable()
    {
        RemoveListenerCall();
    }

    #region Listeners
    void AddListenerCall()
    {
        BackButton.onClick.AddListener(OnClick_BackButton);
        LanguageButton.onClick.AddListener(OnClick_LanguageButton);
        MetamaskButton.onClick.AddListener(OnClick_MetamaskButton);
        PrivacyPolicyButton.onClick.AddListener(OnClick_PrivacyPolicyButton);
        TermsofServiceButton.onClick.AddListener(OnClick_TermsofServiceButton);
    }
    void RemoveListenerCall()
    {
        BackButton.onClick.RemoveListener(OnClick_BackButton);
        LanguageButton.onClick.RemoveListener(OnClick_LanguageButton);
        MetamaskButton.onClick.RemoveListener(OnClick_MetamaskButton);
        PrivacyPolicyButton.onClick.RemoveListener(OnClick_PrivacyPolicyButton);
        TermsofServiceButton.onClick.RemoveListener(OnClick_TermsofServiceButton);
    }
    #endregion

    #region Buttons
    public void OnClick_BackButton()
    {
        MainPanelUIManager.Instance.BackButton(BackPanel);
    }
    public void OnClick_LanguageButton()
    {
        MainPanelUIManager.Instance.LanguagePanel();
    }
    public void OnClick_MetamaskButton()
    {

    }
    public void OnClick_PrivacyPolicyButton()
    {
        Application.OpenURL("https://anygamelabs.com/");
    }
    public void OnClick_TermsofServiceButton()
    {
        Application.OpenURL("https://anygamelabs.com/");
    }
    #endregion
}
