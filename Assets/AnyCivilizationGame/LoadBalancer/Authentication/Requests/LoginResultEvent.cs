using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ACGAuthentication
{
    public class LoginResultEvent : IResponseEvent
    {
        public bool IsSuccess { get; set; }
        public LoginResultEvent(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
        public void Invoke(EventManagerBase eventManagerBase)
        {
            var acgAuth = eventManagerBase as ACGAuthenticationManager;
            if (IsSuccess)
            {
                acgAuth.Debug("Loggin successfully!");
                Debug.Log("Loggin successfully!");
                if (ACGDataManager.Instance.GameData.TerminalType == TerminalType.Client)
                {
                    MainPanelUIManager.Instance.OnUserLogged();
                }
                else
                {
                    // GameServer logics.
                    SceneManager.LoadScene(ACGDataManager.Instance.GameSceneName);
                }
            }
            else
            {
                Debug.Log("Loggin fail!");
                acgAuth.Debug("Loggin fail!");
                // TODO: retry
            }

        }
    }
}