using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUltiActivePanel : Panel
{
    public Animator AttackUlti_Circle_Hint_Animation;




    public void Animate_AttackUlti_Circle_Hint_Animation()
    {
        if(!AttackUlti_Circle_Hint_Animation.gameObject.activeSelf)
        {

        AttackUlti_Circle_Hint_Animation.gameObject.SetActive(true);
        }

        AttackUlti_Circle_Hint_Animation.SetTrigger("AttackUlti_Circle_Hint_Activate");


    }
    public void Deactivate_AttackUlti_Circle_Hint_Animation()
    {
        if (AttackUlti_Circle_Hint_Animation.gameObject.activeSelf)
        AttackUlti_Circle_Hint_Animation.gameObject.SetActive(false);


    }

}
