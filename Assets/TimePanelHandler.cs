using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimePanelHandler : Panel
{
    public TextMeshProUGUI timeText;



    public void ChangeTimeText(int countDownTime)
    {
    
        timeText.text = countDownTime.ConvertThisSecondToMinute();
      

    }

  
}
