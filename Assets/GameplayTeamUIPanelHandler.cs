using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayTeamUIPanelHandler : Panel
{
    public TextMeshProUGUI CrystalAmountText;
    public Transform CrystalBarParent;
    public int maxCrystalAmount = 10;

    public void ChangeCrystalAmountUI(int crystalAmount)
    {
        HandleCrystalAmountText(crystalAmount);
        HandleCrystalBar(crystalAmount);    


    }
    public void HandleCrystalBar(int crystalAmount)
    {

        //if (crystalAmount.Equals(0))
        //{

        //    ResetCrystalAmountBar();

        //}
        var ActiveChildCount = 0;
        for (int i = 0; i < CrystalBarParent.childCount; i++)
        {
            if (CrystalBarParent.GetChild(i).gameObject.activeSelf)
            {
                ActiveChildCount++;

            }
        }


        var CrystalAmount = Mathf.Min(crystalAmount, maxCrystalAmount);
        if (CrystalAmount < ActiveChildCount)
        {
            //Decrease bar amount

            var DecreaseAmount = ActiveChildCount;
            for (int i = CrystalAmount; i < DecreaseAmount; i++)
            {
                if (CrystalBarParent.GetChild(i).gameObject.activeSelf)
                {
                    CrystalBarParent.GetChild(i).gameObject.SetActive(false);

                }
           
            }

            //}
            //else if( crystalAmount > CrystalBarParent.childCount) 
            //{


            //    for (int i = 0; i < Mathf.Min(crystalAmount, maxCrystalAmount); i++)
            //    {
            //        if (!CrystalBarParent.GetChild(i).gameObject.activeSelf)
            //        {
            //            CrystalBarParent.GetChild(i).gameObject.SetActive(true);

            //        }
            //        // maxCrystalAmount
            //    }
            //}




        }

        //
        else if (CrystalAmount > ActiveChildCount)  
        {
            //Increase bar amount

            for (int i = 0; i < CrystalAmount; i++)
            {
                if (!CrystalBarParent.GetChild(i).gameObject.activeSelf)
                {
                    CrystalBarParent.GetChild(i).gameObject.SetActive(true);

                }

            }

        }
    }
    void ResetCrystalAmountBar()
    {
        for (int i = 0; i < CrystalBarParent.childCount; i++)
        {
            if (CrystalBarParent.GetChild(i).gameObject.activeSelf)
            {
                CrystalBarParent.GetChild(i).gameObject.SetActive(false);

            }

        }

    }
    public void HandleCrystalAmountText(int crystalAmount)
    {
        CrystalAmountText.text = crystalAmount.ToString();

    }


}