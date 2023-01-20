using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Waiting : Panel
{
    public TextMeshProUGUI infoText;
    public string Info
    {
        get => infoText.text;
        set => infoText.text = value;
    }
}
