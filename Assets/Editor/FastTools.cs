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

    private void OnGUI()
    {
        GUILayout.Label("Scenes");
        if (GUILayout.Button("Load AppCenter"))
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene("Assets/Scenes/AppCenter.unity");
        }
        if (GUILayout.Button("Load UI Design"))
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene("Assets/Scenes/UI_Design.unity");
        }
        if (GUILayout.Button("Load Menu_Custom"))
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene("Assets/Sources/Scenes/Menu_Custom.unity");
        }

        GUILayout.Space(25);
        int currentLevel = Storager.getInt("currentLevel", false, 1);
        GUILayout.Label($"Current Level: {currentLevel}/{ExperienceController.maxLevel}");
        if (GUILayout.Button("Increase Level"))
        {
            if (currentLevel < ExperienceController.maxLevel)
            {
                Storager.setInt("currentLevel", currentLevel + 1);
            }
        }
        if (GUILayout.Button("Decrease Level"))
        {
            if (currentLevel > 1)
            {
                Storager.setInt("currentLevel", currentLevel - 1);
            }
        }

        GUILayout.Space(25);
        int coins = Storager.getInt("Coins", false);
        int gems = Storager.getInt("GemsCurrency", false);
        GUILayout.Label($"Coins: {coins}");
        if (GUILayout.Button("Add 50 Coins"))
        {
            Storager.setInt("Coins", coins + 50, false);
        }
        GUILayout.Label($"Gems: {gems}");
        if (GUILayout.Button("Add 50 Gems"))
        {
            Storager.setInt("GemsCurrency", gems + 50, false);
        }

        GUILayout.Space(25);
        GUILayout.Label("Training");
        if (GUILayout.Button("Complete Training"))
        {
            Storager.setInt(Defs.TrainingCompleted_4_4_Sett, 1);
        }
        if (GUILayout.Button("Uncomplete Training"))
        {
            Storager.setInt(Defs.TrainingCompleted_4_4_Sett, 0);
        }
        
        GUILayout.Space(60);
        GUILayout.Label("DANGEROUS");
        if (GUILayout.Button("Delete All PlayerPrefs"))
        {
            PlayerPrefs.DeleteAll();
        }
        GUI.enabled = true;
    }
}
