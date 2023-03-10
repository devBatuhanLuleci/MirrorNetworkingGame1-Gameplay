using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class LoadingPanel : Panel
{
    [SerializeField]
    private TextMeshProUGUI statusText;

    private void Start()
    {
        StartCoroutine(FakeWait(1));
    }

    public void Info(string msg)
    {
        statusText.text = msg;
    }
        

    #region FakeArea
    IEnumerator FakeWait(float time)
    {
        yield return new WaitForSeconds(1);
        MainPanelUIManager.Instance.Login();
    }
    #endregion
}
