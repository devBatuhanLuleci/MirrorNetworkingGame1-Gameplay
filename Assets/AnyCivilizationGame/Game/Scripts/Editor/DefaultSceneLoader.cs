#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoadAttribute]
public static class DefaultSceneLoader {
    private static bool _isEnabled;

    [MenuItem ("FirstScene/Enable Initialization")]
    private static void EnableInitialization () {
        _isEnabled = true;
        EditorPrefs.SetBool ("DefaultSceneLoader.IsEnabled", _isEnabled);
    }

    [MenuItem ("FirstScene/Disable Initialization")]
    private static void DisableInitialization () {
        _isEnabled = false;
        EditorPrefs.SetBool ("DefaultSceneLoader.IsEnabled", _isEnabled);
    }

    public static bool IsEnabled {
        get { return _isEnabled; }
    }
    static DefaultSceneLoader () {
        
        _isEnabled = EditorPrefs.GetBool ("DefaultSceneLoader.IsEnabled", true);

        if (_isEnabled) {
            EditorApplication.playModeStateChanged += LoadDefaultScene;
        }

    }

    static void LoadDefaultScene (PlayModeStateChange state) {
        if (state == PlayModeStateChange.ExitingEditMode) {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo ();
        }

        if (state == PlayModeStateChange.EnteredPlayMode) {
            EditorSceneManager.LoadScene (0);
        }
    }
}
#endif