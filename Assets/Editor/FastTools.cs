using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

class FastTools : EditorWindow
{
    [MenuItem("Window/Fast Tools")]

    public static void ShowWindow()
    {
        GetWindow(typeof(FastTools));
    }

    void OnGUI()
    {
        if (GUILayout.Button("Load AppCenter"))
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene("Assets/Scenes/AppCenter.unity");
        }
        if (GUILayout.Button("Load Menu_Custom"))
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene("Assets/Sources/Scenes/Menu_Custom.unity");
        }
        if (GUILayout.Button("Load UI Design"))
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene("Assets/Scenes/UI_Design.unity");
        }
            
        GUI.enabled = true;
    }
}
