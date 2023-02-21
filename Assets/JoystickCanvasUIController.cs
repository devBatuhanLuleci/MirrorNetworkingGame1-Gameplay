using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickCanvasUIController : MonoBehaviour
{
    private JoystickCanvas joystickCanvas;

    public Image AttackUlti_OutFill_Orange_Image;


    private void Awake()
    {
        joystickCanvas = GetComponent<JoystickCanvas>();
    }
    public void DeactivateUlti()
    {

        joystickCanvas.AttackUltiJoystick.Deactivate();


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
        AttackUlti_OutFill_Orange_Image.fillAmount = ultimateFillRate/100f;
    }
}
