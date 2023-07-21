using System.Linq;
using UnityEngine;

[System.Serializable]
public struct FrameRateQualityLevel
{
    public int frameRateThreshold;
}

public class QualityChanger : MonoBehaviour
{
    public int targetFrameRate = -2; // Default target frame rate
    private FrameRateQualityLevel[] qualityLevels;

    private GUIStyle outlineStyle;
    private GUIStyle mainStyle;
    int currentFPS = -1;

    private void Start()
    {
        Application.targetFrameRate = 60;

        // Create the GUI styles for the centered text with a nice color and font size
        outlineStyle = new GUIStyle();
        outlineStyle.alignment = TextAnchor.UpperCenter;
        outlineStyle.fontSize = Mathf.RoundToInt(24 * Screen.height / 1080f); // Scale the font size based on the screen resolution
        outlineStyle.normal.textColor = Color.black; // Black outline color
        outlineStyle.fontStyle = FontStyle.Bold;

        mainStyle = new GUIStyle(outlineStyle);
        mainStyle.normal.textColor = Color.white; // White main text color
        mainStyle.border = new RectOffset(2, 2, 2, 2); // Adjust the outline border size
        InvokeRepeating(nameof(SetQualityLevelFromFPS), 3, 2);
    }

    private void Update()
    {
        currentFPS = Mathf.FloorToInt(1f / Time.unscaledDeltaTime);
        if (targetFrameRate != Application.targetFrameRate)
        {
            targetFrameRate = Application.targetFrameRate;
            GenerateQualityLevels();
            SetQualityLevelFromFPS();
        }

    }

    private void GenerateQualityLevels()
    {
        int numQualityLevels = QualitySettings.names.Length;
        qualityLevels = new FrameRateQualityLevel[numQualityLevels];

        for (int i = 0; i < numQualityLevels; i++)
        {
            qualityLevels[i].frameRateThreshold = targetFrameRate - (targetFrameRate / (numQualityLevels - 1)) * i;
        }
    }

    private void SetQualityLevelFromFPS()
    {
        if (Mathf.Abs(currentFPS - targetFrameRate) <= 5)
        {
            return;
        }
        if (qualityLevels == null || qualityLevels.Length <= 0)
        {
            GenerateQualityLevels();
        }
        int selectedQualityLevel = 0;
        if (currentFPS >= targetFrameRate)
        {
            selectedQualityLevel = qualityLevels.Length - 1;
        }
        else
        {
            // Find the appropriate quality level based on the current FPS
            for (int i = 0; i < qualityLevels.Length - 1; i++)
            {
                int frameRateThreshold1 = qualityLevels[i].frameRateThreshold;
                int frameRateThreshold2 = qualityLevels[i + 1].frameRateThreshold;
                if (currentFPS <= frameRateThreshold1 && currentFPS > frameRateThreshold2)
                {
                    selectedQualityLevel = qualityLevels.Length - 1 - i;
                    break;
                }
            }
        }

        // Apply the selected quality level
        if (selectedQualityLevel >= 0 && selectedQualityLevel != QualitySettings.GetQualityLevel())
        {
            QualitySettings.SetQualityLevel(selectedQualityLevel, true);
            Debug.Log("Applying Quality Level: " + QualitySettings.names[selectedQualityLevel] + " for FPS: " + currentFPS);
        }
    }

    private void OnGUI()
    {

        // Show the GUI text with outline at the top center of the screen, scaled to fit different resolutions
        float scalingFactor = Screen.height / 1080f;
        float xPos = 0;
        float yPos = 20 * scalingFactor;
        float width = Screen.width;
        float height = 60 * scalingFactor;

        int qualityLevel = QualitySettings.GetQualityLevel();
        string qualityName = QualitySettings.names[qualityLevel];

        GUI.Label(new Rect(xPos - 1, yPos, width, height), "Current Quality Level: " + qualityName + "\nCurrent FPS: " + currentFPS + " / " + targetFrameRate, outlineStyle);
        GUI.Label(new Rect(xPos + 1, yPos, width, height), "Current Quality Level: " + qualityName + "\nCurrent FPS: " + currentFPS + " / " + targetFrameRate, outlineStyle);
        GUI.Label(new Rect(xPos, yPos - 1, width, height), "Current Quality Level: " + qualityName + "\nCurrent FPS: " + currentFPS + " / " + targetFrameRate, outlineStyle);
        GUI.Label(new Rect(xPos, yPos + 1, width, height), "Current Quality Level: " + qualityName + "\nCurrent FPS: " + currentFPS + " / " + targetFrameRate, outlineStyle);

        GUI.Label(new Rect(xPos, yPos, width, height), "Current Quality Level: " + qualityName + "\nCurrent FPS: " + currentFPS + " / " + targetFrameRate, mainStyle);
    }
}
