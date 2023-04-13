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

        Debug.Log("içeri bu şekilde girdim :" + isActive);

        //TODO:  burada sıkıntı var GemModeGameplayCanvas boş geliyor.   çünkü tam yüklenmiyor.
        Debug.Log(GemModeGameplayCanvas);
        CrystalModeGamePlayCanvasUIController crystalModeGamePlayCanvasUIController = GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController;
        crystalModeGamePlayCanvasUIController.Activate(isActive);




    }
    public void SetCrystalInfoText(float value)
    {
        //Debug.Log($"ezilip gelen : current value : {value}");
        
        CrystalModeGamePlayCanvasUIController crystalModeGamePlayCanvasUIController = GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController;
        crystalModeGamePlayCanvasUIController.HandleCrystalInfoText(value);


    }

    public void SetCrystalInfoPanelPos(Vector2 value)
    {
        //Debug.Log($"ezilip gelen : current value : {value}");

        CrystalModeGamePlayCanvasUIController crystalModeGamePlayCanvasUIController = GemModeGameplayCanvas as CrystalModeGamePlayCanvasUIController;
        crystalModeGamePlayCanvasUIController.HandleCrystalInfoPanelPos(value);


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
