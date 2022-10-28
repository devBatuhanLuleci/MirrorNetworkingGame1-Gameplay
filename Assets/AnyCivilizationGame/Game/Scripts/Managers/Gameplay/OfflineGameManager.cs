using SimpleInputNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineGameManager : MonoBehaviour
{
    public Joystick MovementJoystick;
    public Joystick AttackJoystick;

    public static OfflineGameManager instance;



    
    private void Awake()
    {
        instance = this;    
    }
}
