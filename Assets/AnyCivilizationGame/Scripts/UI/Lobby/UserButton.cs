using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UserButton : MonoBehaviour
{

    public GameObject ReadyText;
    public string UserName { get; private set; }
    public UnityAction<string> OnClick { get; private set; }
    public bool IsReady { get; private set; }
    public void Init(string userName, UnityAction<string> onClick)
    {
        OnClick = onClick;
        SetUserName(userName);
        GetComponent<Button>().onClick.AddListener(OnCLicked);
        gameObject.SetActive(true);
    }

    public void SetUserName(string userName)
    {
        UserName = userName;
        GetComponentInChildren<TextMeshProUGUI>().text = userName;
    }
    public void SetState(bool state)
    {
        ReadyText.SetActive(state);
        IsReady = state;
    }
    private void OnCLicked()
    {
        if (OnClick != null)
            OnClick(UserName);
    }
}
