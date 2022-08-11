using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class SpinnerWithText : MonoBehaviour
{
    public string Text
    {
        get
        {
            return _title.text;
        }
        set
        {
            _title.text = value;
        }
    }
    private TMP_Text _title;

    public string m_title;

    private void Awake()
    {
        _title = GetComponentInChildren<TMP_Text>();
        _title.text = m_title;
    }
}
