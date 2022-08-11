using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIManager : Singleton<MainUIManager>
{
    [Header("Setup")]
    [SerializeField]
    private Panel LoadingPanel;

    private void Start()
    {
        CheckServer();
    }

    /// <summary>
    /// Check Server update
    /// </summary>
    private void CheckServer()
    {
        StartCoroutine(FakeServerCheck(MoralisLogin));
    }

    private void MoralisLogin()
    {
        LoadingPanel.Close();
    }

    #region FakeArea
    IEnumerator FakeServerCheck(Action callBack)
    {
        yield return new WaitForSeconds(1);
        if (callBack != null)
            callBack();
    }
    #endregion



}
