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

        //Auto select ellen 
        var msg = new ReplanceCharacterMessage { name = "Ellen" };
        NetworkClient.Send(msg);

        waitingPanel.Close();
        
       
    }
    internal void CharacterSlected()
    {
        joystickCanvas.Show();
        GemModeGameplayCanvas.Show();
        if (currentPanel != null)
            currentPanel.Close();
    }
   
    public void Init_CrystalModeGameplayCanvas(Panel GemModeGameplayCanvas)
    {
        EqulizeGamePlayCanvas(GemModeGameplayCanvas);
        ActivateGamePlayCanvas();
    }

    //   [ClientRpc(includeOwner =true)]
    public void EqulizeGamePlayCanvas(Panel GemModeGameplayCanvas)
    {
        this.GemModeGameplayCanvas = GemModeGameplayCanvas;

    }


  //  [Command(requiresAuthority =false)]
    public void ActivateGamePlayCanvas()
    {

        GemModeGameplayCanvas.Show();

    }
    public void SetCrystalInfoText(float value)
    {
        //Debug.Log($"ezilip gelen : current value : {value}");
        
        CrystalModeGamePlayCanvasUIController crystalModeGamePlayCanvasUIController = GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController;
        crystalModeGamePlayCanvasUIController.HandleCrystalInfoText(value);


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
