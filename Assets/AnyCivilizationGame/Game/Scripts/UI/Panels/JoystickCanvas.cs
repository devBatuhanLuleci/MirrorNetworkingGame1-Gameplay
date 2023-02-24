using SimpleInputNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickCanvas : Panel
{
    public Joystick MovementJoystick;
    public Joystick AttackBasicJoystick;
    public UltiJoystick AttackUltiJoystick;

    [HideInInspector]
    public JoystickCanvasUIController joystickCanvasUIController;
    private void Awake()
    {
        joystickCanvasUIController = GetComponent<JoystickCanvasUIController>();
    }

}
