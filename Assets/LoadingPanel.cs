using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoadingPanel : Panel
{

    private void Start()
    {
        StartCoroutine(FakeWait(1));
    }


    #region FakeArea
    IEnumerator FakeWait(float time)
    {
        yield return new WaitForSeconds(1);
        MainUIManager.Instance.MoralisLogin();
    }
    #endregion
}
