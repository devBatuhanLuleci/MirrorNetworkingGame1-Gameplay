using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UserButton : MonoBehaviour
{
    public string UserName { get; private set; }
    public UnityAction<string> OnClick { get; private set; }
    public void Init(string userName, UnityAction<string> onClick)
    {
        UserName = userName;
        OnClick = onClick;

        GetComponentInChildren<TextMeshProUGUI>().text = userName;
        GetComponent<Button>().onClick.AddListener(OnCLicked);
        gameObject.SetActive(true);
    }
    private void OnCLicked()
    {
        if (OnClick != null)
            OnClick(UserName);
    }
}
