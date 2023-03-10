using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltiJoystickUIController : Panel
{

    public AttackUltiPassivePanel Attack_Ulti_Passive_Panel;
    public AttackUltiActivePanel Attack_Ulti_Active_Panel;
    public Image AttackUlti_Passive_Fill_Orange_Image;
    private UltiJoystick ultiJoystick;
    public Image Passive_Ulti_Thumb;
    public Image Active_Ulti_Thumb;
    int i = 0;
    int c = 0;
    private void Awake()
    {
        ultiJoystick = GetComponent<UltiJoystick>();
       ShowPassiveUltiPanel();
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (i % 2 == 0)
            {
                ShowActiveUltiPanel();
            }
            else
            {
                ShowPassiveUltiPanel();
            }

            i++;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            c += 10;
            ChangeUltimateFillRate(c);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            ShowPassiveUltiPanel();
        }
    }
    public void ShowPassiveUltiPanel()
    {
       
        Attack_Ulti_Passive_Panel.Show();
         ultiJoystick.InitJoystickStats(Passive_Ulti_Thumb,false);
        Attack_Ulti_Active_Panel.Close();


    }
    public void ShowActiveUltiPanel()
    {


        Attack_Ulti_Active_Panel.ShowSmoothly();
        ultiJoystick.InitJoystickStats(Active_Ulti_Thumb, true);
        Attack_Ulti_Active_Panel.Animate_AttackUlti_Circle_Hint_Animation();

        Attack_Ulti_Passive_Panel.Close();


    }



    public void ChangeUltimateFillRate(float ultimateFillRate)
    {

        AttackUlti_Passive_Fill_Orange_Image.fillAmount = ultimateFillRate / 100f;

     

    }

}
