using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ACG_LoginPanelManager;

public class RegisterPanel : Panel
{


    #region RegisterFields

    #region EmailField

    [Header("Email Attributes")][Space] public TMP_InputField EmailField;
    public TMP_Text EmailConfirmStatusText;

    public const string MatchEmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    #endregion

    #region KVKKField

    [Header("KVKK Attributes")][Space] public TMP_Text KVKKStatusText;
    public Toggle KVKKToggle;

    #endregion

    #endregion

    #region MonoBehavior Methods
    private void Awake()
    {
        KVKKToggle.onValueChanged.AddListener(delegate { ToggleValueChanged(KVKKToggle); });
    }
    #endregion

    public bool Validate()
    {
        if (!IsEmail(EmailField.text))
        {
            EmailConfirmStatusText.gameObject.SetActive(true);
            return false;
        }

        if (!IsKVKKAccepted())
        {
            KVKKStatusText.gameObject.SetActive(true);
            return false;
        }

        return true;

    }

    public bool IsKVKKAccepted()
    {
        return KVKKToggle.isOn;
    }


    #region UI event handlers
    public void OnClickRegister()
    {
        if(!Validate()) return;

        AuthenticationManager.Instance.CreateUserWithMoralis(EmailField.text);
    }

    void ToggleValueChanged(Toggle change)
    {
        if (KVKKStatusText.gameObject.activeSelf)
        {
            KVKKStatusText.gameObject.SetActive(false);
        }
        // m_Text.text = "New Value : " + m_Toggle.isOn;
    }


    public bool IsEmail(string email)
    {
        if (email != null) return Regex.IsMatch(email, MatchEmailPattern);
        else return false;
    }

    #endregion


}
