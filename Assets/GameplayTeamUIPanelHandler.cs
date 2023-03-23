using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayTeamUIPanelHandler:Panel
{
    public TextMeshProUGUI CrystalAmountText;



    public void ChangeCrystalAmountText(int crystalAmount)
    {
        CrystalAmountText.text = crystalAmount.ToString();  

    }

}