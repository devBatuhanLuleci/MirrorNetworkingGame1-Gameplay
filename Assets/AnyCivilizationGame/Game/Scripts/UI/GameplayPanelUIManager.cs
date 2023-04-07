using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayPanelUIManager : Panel
{
    #region Child panels
    [Header("Setup")]
    public Panel characterSelect;
    public Panel waitingPanel;
    public Panel joystickCanvas;
    public Panel GemModeGameplayCanvas;

    [Space]
    [SerializeField]
    public Panel startPanel = null;
    #endregion

    public static GameplayPanelUIManager Instance
    {
        get;   // get method
        private set;
    }


    #region MonoBehavior Methods

    private void Awake()
    {
        Instance = this;
        var currentPanel = startPanel ?? waitingPanel;
        Show(currentPanel);
    }
    #endregion


    public void SelectCharacter()
    {
        Show(characterSelect);
        joystickCanvas.Close();
    }
    public void AutoSelectCharacter()
    {
        var msg = new ReplanceCharacterMessage { name = "Ellen" };
        NetworkClient.Send(msg);

         Close(waitingPanel);
        //  joystickCanvas.Close();

        //joystickCanvas.Show();
        //GemModeGameplayCanvas.Show();
        Debug.Log($"gem mode canvas is null: {GemModeGameplayCanvas}");
       // CharacterSlected();
    }
    internal void CharacterSlected()
    {
        joystickCanvas.Show();
        GemModeGameplayCanvas.Show();
        if (currentPanel != null)
            currentPanel.Close();
    }
   
    public void DeactivateUltiButton()
    {
        if (joystickCanvas.TryGetComponent(out JoystickCanvasUIController joystickUIController))
        {
            joystickUIController.DeactivateUlti();
        }

    }
    public void ActivateUltiButton()
    {
        if (joystickCanvas.TryGetComponent(out JoystickCanvasUIController joystickUIController))
        {

            joystickUIController.ActivateUlti();
           
        }

    }
}
