using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : Panel
{
    #region Child panels
    [Header("Setup")]
    public Panel characterSelect;
    public Panel waitingPanel;
    public Panel joystickCanvas;


    [Space]
    [SerializeField]
    public Panel startPanel = null;
    #endregion

    public static GameUIManager Instance
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

    internal void CharacterSlected()
    {
        joystickCanvas.Show();
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
