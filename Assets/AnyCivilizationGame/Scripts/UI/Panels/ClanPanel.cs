using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClanPanel : Panel
{
    public Button BackButton;
    public Button SearchButton;
    public Button CreateClanButton;
    public Button ClanJoinButton;
    public Button ClanInviteButton;
    [SerializeField] TMP_InputField SearchInputField;
    [SerializeField] TMP_InputField CreateClanInputField;
    [SerializeField] Panel BackPanel;
    [SerializeField] public bool isDirectMainMenu = false;
    [SerializeField] GameObject ClanItemPrefab;
    [SerializeField] GameObject List;
    [SerializeField] GameObject Content;
    [SerializeField] GameObject[] Elements;
    int TotalElements = 0;

    string[] clanNames = { "ClanZET", "ClanROZ", "ClanPIZ", "ClanKSK", "ClanRET", "ClanLKS", "ClanQWE", "ClanKNG", "ClanFYT", "ClanKES" };
    private void OnEnable()
    {
        AddListenerCall();

        ClansCreator();
        ItemLengthControl();
    }
    private void OnDisable()
    {
        RemoveListenerCall();
    }

    #region Listeners
    void AddListenerCall()
    {
        BackButton.onClick.AddListener(OnClick_BackButton);
        SearchButton.onClick.AddListener(OnClick_SearchButton);
        CreateClanButton.onClick.AddListener(OnClick_CreateClanButton);
        ClanJoinButton.onClick.AddListener(OnClick_ClanJoinButton);
        ClanInviteButton.onClick.AddListener(OnClick_ClanInviteButton);
    }
    void RemoveListenerCall()
    {
        BackButton.onClick.RemoveListener(OnClick_BackButton);
        SearchButton.onClick.RemoveListener(OnClick_SearchButton);
        CreateClanButton.onClick.RemoveListener(OnClick_CreateClanButton);
        ClanJoinButton.onClick.RemoveListener(OnClick_ClanJoinButton);
        ClanInviteButton.onClick.RemoveListener(OnClick_ClanInviteButton);
    }
    #endregion

    #region Buttons
    public void OnClick_BackButton()
    {
        if (isDirectMainMenu)
        {
            MainPanelUIManager.Instance.BackButton(BackPanel);
        }
        else
        {
            MainPanelUIManager.Instance.BackButton(MainPanelUIManager.Instance.settingMenuPanel);
        }

    }
    public void OnClick_SearchButton()
    {
        string searchText = SearchInputField.text.Trim();

        if (string.IsNullOrEmpty(searchText))
        {
            foreach (GameObject element in Elements)
            {
                element.SetActive(true);
            }
            return;
        }


        int searchTxtLength = searchText.Length;

        int searchedElements = 0;

        foreach (GameObject element in Elements)
        {
            searchedElements += 1;

            if (element.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text.Length >= searchTxtLength)
            {
                if (searchText.ToLower() == element.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text.Substring(0, searchTxtLength).ToLower())
                {
                    element.SetActive(true);
                }
                else
                {
                    element.SetActive(false);
                }
            }
        }
    }
    public void OnClick_CreateClanButton()
    {
        string ClanNameText = CreateClanInputField.text.Trim();

        if (string.IsNullOrEmpty(ClanNameText))
            return;

        int sum = 0;

        foreach (GameObject element in Elements)
        {
            if (string.Equals(ClanNameText, element.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text, StringComparison.OrdinalIgnoreCase))
            {
                sum += 1;
                Debug.Log("Clan zaten mevcut.");
                break;
            }
        }

        if (sum > 0)
            return;

        ClanCreate(CreateClanInputField.text.Trim(), Elements.Length + 1);

        CreateClanInputField.text = string.Empty;

        ItemLengthControl();
        Debug.Log("Yeni Clan olusturuldu.");

    }
    public void OnClick_ClanJoinButton()
    {
        // MainPanelUIManager.Instance.BackButton();
    }
    public void OnClick_ClanInviteButton()
    {
        // MainPanelUIManager.Instance.BackButton();
    }
    public void OnClick_ClanProfileButton()
    {
        MainPanelUIManager.Instance.ClanProfilePanelShow();
        MainPanelUIManager.Instance.clanProfilePanel.GetComponent<ClanProfilePanel>().isDirectClanPanel = true;
    }
    #endregion

    public void ClansCreator()
    {
        for (int i = 0; i < clanNames.Length; i++)
        {
            ClanCreate(clanNames[i], i + 1);
        }
    }
    void ClanCreate(string _clanName, int i)
    {
        GameObject _clanItem = Instantiate(ClanItemPrefab);
        _clanItem.transform.GetComponent<RectTransform>().SetParent(Content.transform.GetComponent<RectTransform>());
        _clanItem.transform.localScale = new Vector3(1, 1, 1);
        _clanItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i).ToString();
        _clanItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _clanName;
        _clanItem.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(OnClick_ClanProfileButton);
        _clanItem.SetActive(true);
    }

    void ItemLengthControl()
    {
        TotalElements = Content.transform.childCount;
        Elements = new GameObject[TotalElements - 1];

        for (int i = 1; i < TotalElements; i++)
        {
            Elements[i - 1] = Content.transform.GetChild(i).gameObject;
        }
    }
}
