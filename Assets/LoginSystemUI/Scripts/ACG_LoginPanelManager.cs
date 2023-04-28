using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
//using MoralisUnity.Kits.AuthenticationKit;
using UnityEngine.UI;

public class ACG_LoginPanelManager : Panel
{

    public static ACG_LoginPanelManager Instance;
    public enum Panels
    {
        Login,
        Loading,
        Register,
        PickCharacter
    }

    public Panels current_Panel;

    [Space] [Header("Login System Panels")] [Space]
    public GameObject LoginPanel;

    public GameObject LoadingPanel;
    public GameObject RegisterPanel;
    public GameObject PickCharacterPanel;
    public GameObject CharacterNFTMintPanel;

    #region RegisterFields

    #region EmailField

    [Header("Email Attributes")] [Space] public TMP_InputField EmailField;
    public TMP_Text EmailConfirmStatusText;

    public const string MatchEmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    #endregion

    #region KVKKField

    [Header("KVKK Attributes")] [Space] public TMP_Text KVKKStatusText;
    public Toggle KVKKToggle;

    #endregion

    #region WalletAndMoralisField

    [SerializeField] private TMP_InputField walletAdressField;
    [SerializeField] private TMP_InputField moralisIDField;

    #endregion

    #endregion

    #region CharacterPickField
    
    
    
    
    
    #endregion
    private void Awake()
    {
        KVKKToggle.onValueChanged.AddListener(delegate { ToggleValueChanged(KVKKToggle); });
        Instance = this;
    }

    void ToggleValueChanged(Toggle change)
    {
        if (KVKKStatusText.gameObject.activeSelf)
        {
            KVKKStatusText.gameObject.SetActive(false);
        }
        // m_Text.text = "New Value : " + m_Toggle.isOn;
    }

    private void OnEnable()
    {
        LoginPanel.SetActive(true);
        current_Panel = Panels.Login;
    }

    private void Update()
    {
        if (current_Panel == Panels.Login)
        {
            if (Input.GetKeyDown(KeyCode.Return)) //login , if there is an account

            {
                Switch_To_PickCharacter_Panel();
            }

            if (Input.GetKeyDown(KeyCode.R)) //register  , if there is no account

            {
                Switch_To_Register_Panel();
            }
        }

        if (current_Panel == Panels.Register)
        {
            if (EmailConfirmStatusText.gameObject
                .activeSelf) // kullanýcý daha önce yanlýþ girmiþ ise e-mail 'i pop'up'ý sil.
            {
                if (EmailField.isFocused && Input.anyKeyDown)
                {
                    EmailConfirmStatusText.gameObject.SetActive(false);
                }
            }

            if (Input.GetKeyDown(KeyCode.Return)) //Enter tuþu ile register olmayý dene.
            {
                TryRegister();
            }
        }
    }

    public void Switch_To_PickCharacter_Panel()
    {
        StartCoroutine(WaitForCharacterPanel(1f));
    }

    IEnumerator WaitForCharacterPanel(float waitTime)
    {
        // Debug.Log("MoralisID=" + AuthenticationKit.Instance.moralisID + "   walletAddr=" +
        //           AuthenticationKit.Instance.walletAddr);
        
        
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(false);
        LoadingPanel.SetActive(true);
        current_Panel = Panels.Loading;
        yield return new WaitForSeconds(waitTime);
        LoadingPanel.SetActive(false);
        PickCharacterPanel.SetActive(true);
        current_Panel = Panels.PickCharacter;
    }

    private void Init_WalletAdress()
    {
        //walletAdressField.text = AuthenticationKit.Instance.walletAddr;
    }

    private void Init_MoralisID()
    {
        //moralisIDField.text = AuthenticationKit.Instance.moralisID;

    }

    public void Switch_To_Register_Panel()
    {
        StartCoroutine(WaitForRegisterPanel(2f));
    }

    IEnumerator WaitForRegisterPanel(float waitTime)
    {
        // Debug.Log("MoralisID=" + AuthenticationKit.Instance.moralisID + "   walletAddr=" +
        //           AuthenticationKit.Instance.walletAddr);
        Init_WalletAdress();
        Init_MoralisID();
        Debug.Log("backspace");
        LoginPanel.SetActive(false);
        LoadingPanel.SetActive(true);
        current_Panel = Panels.Loading;
        yield return new WaitForSeconds(waitTime);
        LoadingPanel.SetActive(false);
        RegisterPanel.SetActive(true);
        current_Panel = Panels.Register;
    }


    public bool IsEmail(string email)
    {
        if (email != null) return Regex.IsMatch(email, MatchEmailPattern);
        else return false;
    }


    public void OnClick_RegisterButton()
    {
        TryRegister();
    }

  
    public bool IsKVKKAccepted()
    {
        return KVKKToggle.isOn;
    }

    public void TryRegister()
    {
        if (!IsEmail(EmailField.text))
        {
            EmailConfirmStatusText.gameObject.SetActive(true);
            return;
        }

        if (!IsKVKKAccepted())
        {
            KVKKStatusText.gameObject.SetActive(true);
            return;
        }

        if (IsEmail(EmailField.text) && IsKVKKAccepted())
        {
            Debug.Log("Input is EMAÝL");
            Switch_To_PickCharacter_Panel();
        }
    }

    public void Switch_To_Character_NFTMint_Panel()
    {
        PickCharacterPanel.SetActive(false);
        CharacterNFTMintPanel.SetActive(true);

    }
    public void Switch_To_Pick_Character_Panel_From_NFTMint()
    {
        CharacterNFTMintPanel.SetActive(false);
        PickCharacterPanel.SetActive(true);

    }
}