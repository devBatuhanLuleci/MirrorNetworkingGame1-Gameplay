using SimpleInputNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UltiJoystick : Joystick
{
    [HideInInspector]
    public UltiJoystickUIController ultiJoystickUIController;


  

    public override void Awake()
    {
        base.Awake();
        if (TryGetComponent(out UltiJoystickUIController UltiJoystickUIController))
        {
            ultiJoystickUIController = UltiJoystickUIController;




        }
    }


    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
   
        base.OnPointerDown(eventData);
     
        if (ultiJoystickUIController != null)
        {
        ultiJoystickUIController.Attack_Ulti_Active_Panel.Deactivate_AttackUlti_Circle_Hint_Animation();

        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        
      ultiJoystickUIController.Attack_Ulti_Active_Panel.Animate_AttackUlti_Circle_Hint_Animation();


    }

}
