using SimpleInputNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoGameManager : MonoBehaviour
{
    public Joystick MovementJoystick;
    public Joystick AttackJoystick;

    public static DemoGameManager instance;



    
    private void Awake()
    {
        instance = this;    
    }
}
