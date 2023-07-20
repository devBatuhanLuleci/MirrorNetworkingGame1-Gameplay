using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QulityChanger : MonoBehaviour
{

    int avgFrameRate;
    string[] qualityNames = new string[0];
    int currentQualitySettings
    {
        get
        {
            return QualitySettings.GetQualityLevel();
        }
        set
        {
            value = Mathf.Clamp(value, 0, QualitySettings.names.Length-1);
            QualitySettings.SetQualityLevel(value);
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        qualityNames = QualitySettings.names;
        InvokeRepeating(nameof(CheckQuality), 0, 2);
    }
    void CheckQuality()
    {
        int oldQuality = currentQualitySettings;
        if (Mathf.Lerp(-1, 1, Mathf.InverseLerp(0, avgFrameRate, Application.targetFrameRate)) < .25f)
        {
            currentQualitySettings--;
        }
        else if (Mathf.Lerp(-1, 1, Mathf.InverseLerp(0, avgFrameRate, Application.targetFrameRate)) > .6f)
        {
            currentQualitySettings++;
        }

        if (oldQuality != currentQualitySettings)
        {
            Debug.Log($"quality changed from {oldQuality} to {currentQualitySettings}");
        }
    }
    public void Update()
    {
        float current = 0;
        current = Time.frameCount / Time.time;
        avgFrameRate = (int)current;
    }

    private void OnGUI()
    {
        var screenSize = new Vector2(Screen.width, Screen.height);
        var boxSize = new Vector2(4, 2);
        boxSize *= .1f * screenSize;
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        GUI.Label(new Rect(screenSize*.01f, boxSize), text: $"{avgFrameRate} FPS / {Application.targetFrameRate} FPS\nCurrent quality = {qualityNames[currentQualitySettings]} {currentQualitySettings}/{qualityNames.Length - 1}");

    }
}

