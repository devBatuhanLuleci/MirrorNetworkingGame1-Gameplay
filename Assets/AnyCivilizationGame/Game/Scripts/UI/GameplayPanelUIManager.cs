using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
        joystickCanvas.Close();

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
       // ActivateGamePlayCanvasAnimation();
    }

    //   [ClientRpc(includeOwner =true)]
    public void EqulizeGamePlayCanvas(Panel GemModeGameplayCanvas)
    {
        this.GemModeGameplayCanvas = GemModeGameplayCanvas;

    }



    public void ActivateSequence(bool isActive)
    {

     
     
        //CrystalModeGamePlayCanvasUIController crystalModeGamePlayCanvasUIController = GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController;
        //crystalModeGamePlayCanvasUIController.ActivateCrystalnfoPanel(isActive);


        (GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController)?.ActivateCrystalnfoPanel(isActive);


    }
    public void ActivateCountDownTeamInfoTextPanel(bool newValue)
    {
        //CrystalModeGamePlayCanvasUIController crystalModeGamePlayCanvasUIController = GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController;
        //crystalModeGamePlayCanvasUIController.ActivateCountDownTeamInfoTextPanel(newValue);

        (GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController)?.ActivateCountDownTeamInfoTextPanel(newValue);

    }
    public void ActivateCountDownTextPanel(bool newValue)
    {
        //CrystalModeGamePlayCanvasUIController crystalModeGamePlayCanvasUIController = GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController;
        //crystalModeGamePlayCanvasUIController.ActivateCountDownTextPanel(newValue);

        (GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController)?.ActivateCountDownTextPanel(newValue);
    }
    public void ActivateFinishPanel(bool newValue)
    {
        //CrystalModeGamePlayCanvasUIController crystalModeGamePlayCanvasUIController = GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController;
        //crystalModeGamePlayCanvasUIController.ActivateCountDownTextPanel(newValue);

        (GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController)?.ActivateFinishPanel(newValue);
    }

    public void SetCrystalInfoText(float value)
    {
        //Debug.Log($"ezilip gelen : current value : {value}");
        
        //CrystalModeGamePlayCanvasUIController crystalModeGamePlayCanvasUIController = GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController;
        //crystalModeGamePlayCanvasUIController.HandleCrystalInfoText(value);

        (GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController)?.HandleCrystalInfoText(value);

    }
    public void SetTimeValueOnPanel(int value)
    {

        //CrystalModeGamePlayCanvasUIController crystalModeGamePlayCanvasUIController = GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController;
        //crystalModeGamePlayCanvasUIController.HandleCrystalModeGameTime(value);

        (GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController)?.HandleCrystalModeGameTime(value);

    }

    public void SetCrystalInfoPanelPos(Vector2 value)
    {
        //Debug.Log($"ezilip gelen : current value : {value}");

        //CrystalModeGamePlayCanvasUIController crystalModeGamePlayCanvasUIController = GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController;
        //crystalModeGamePlayCanvasUIController.HandleCrystalInfoPanelPos(value);

        (GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController)?.HandleCrystalInfoPanelPos(value);

    }
    public void SetCrystalModeCountDownTeamInfoValue(int value)
    {
        //Debug.Log($"ezilip gelen : current value : {value}");

        //CrystalModeGamePlayCanvasUIController crystalModeGamePlayCanvasUIController = GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController;
        //crystalModeGamePlayCanvasUIController.HandleCrystalModeGameTempCountdownValue(value);

        (GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController)?.HandleCrystalModeGameTempCountdownValue(value);

    }
    public void SetCrystalModeCountDownTeamInfoPanelScaleValue(float value)
    {
        //Debug.Log($"ezilip gelen : current value : {value}");

        //CrystalModeGamePlayCanvasUIController crystalModeGamePlayCanvasUIController = GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController;
        //crystalModeGamePlayCanvasUIController.HandleCrystalModeCountDownTeamInfoTextScale(value);

        (GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController)?.HandleCrystalModeCountDownTeamInfoTextScale(value);

    }
    public void SetWinnableTeamCountDownText_1(string TeamNameInfo)
    {
        //Debug.Log($"ezilip gelen : current value : {value}");
        //Debug.Log("TeamName: " + TeamNameInfo);
        //CrystalModeGamePlayCanvasUIController crystalModeGamePlayCanvasUIController = GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController;
        //crystalModeGamePlayCanvasUIController.HandleWinnerTeamCountDownText(TeamNameInfo);

        (GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController)?.HandleWinnableTeamCountDownText(TeamNameInfo);

    }

    public void SetWinnerTeamText(string TeamNameInfo)
    {
        //Debug.Log($"ezilip gelen : current value : {value}");
        //Debug.Log("TeamName: " + TeamNameInfo);
        //CrystalModeGamePlayCanvasUIController crystalModeGamePlayCanvasUIController = GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController;
        //crystalModeGamePlayCanvasUIController.HandleWinnerTeamCountDownText(TeamNameInfo);

        (GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController)?.HandleWinnerText(TeamNameInfo);

    }


    public void SetCrystalModeCountDownValue(int value)
    {
        //Debug.Log($"ezilip gelen : current value : {value}");

        //CrystalModeGamePlayCanvasUIController crystalModeGamePlayCanvasUIController = GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController;
        //crystalModeGamePlayCanvasUIController.HandleCrystalModeGameCountdownValue(value);

        (GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController)?.HandleCrystalModeGameCountdownValue(value);

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
