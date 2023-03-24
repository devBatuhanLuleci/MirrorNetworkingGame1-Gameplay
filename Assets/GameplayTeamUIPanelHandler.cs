using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayTeamUIPanelHandler : Panel
{
    public TextMeshProUGUI CrystalAmountText;
    public Transform CrystalBars;


    public void ChangeCrystalAmountUI(int crystalAmount)
    {
        HandleCrystalAmountText(crystalAmount);
        HandleCrystalBar(crystalAmount);    


    }
    public void HandleCrystalBar(int crystalAmount)
    {
        if (CrystalBars.childCount > crystalAmount)
        {


            for (int i = 0; i < crystalAmount; i++)
            {
                if (!CrystalBars.GetChild(i).gameObject.activeSelf)
                {
                    CrystalBars.GetChild(i).gameObject.SetActive(true);

                }

            }


        }
    }
    public void HandleCrystalAmountText(int crystalAmount)
    {
        CrystalAmountText.text = crystalAmount.ToString();

    }


}