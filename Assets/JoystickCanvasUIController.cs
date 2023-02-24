using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickCanvasUIController : MonoBehaviour
{
    private JoystickCanvas joystickCanvas;



    private void Awake()
    {
        joystickCanvas = GetComponent<JoystickCanvas>();
    }
    public void DeactivateUlti()
    {

        joystickCanvas.AttackUltiJoystick.ultiJoystickUIController.ShowPassiveUltiPanel();

        //joystickCanvas.AttackUltiJoystick.Deactivate();


    }
   

    public void DeactivateButtons()
    {
        joystickCanvas.AttackUltiJoystick.Deactivate();
        joystickCanvas.AttackBasicJoystick.Deactivate();
        joystickCanvas.MovementJoystick.Deactivate();
    }
    public void ActivateButtons()
    {

        joystickCanvas.AttackUltiJoystick.Activate();
        joystickCanvas.AttackBasicJoystick.Activate();
        joystickCanvas.MovementJoystick.Activate();
    }

    public void ActivateUlti()
    {
        joystickCanvas.AttackUltiJoystick.Activate();

    }

    public void ChangeUltimateFillRate(float ultimateFillRate)
    {
        if (joystickCanvas.AttackUltiJoystick.TryGetComponent(out UltiJoystickUIController ultiJoystickUIController))
            {

            ultiJoystickUIController.ChangeUltimateFillRate(ultimateFillRate);
        }

    }
}
