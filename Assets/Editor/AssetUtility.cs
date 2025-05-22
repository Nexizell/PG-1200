using System.IO;
using UnityEditor;
using UnityEngine;

public static class AssetUtility
{
    #if UNITY_EDITOR
    public static void OverwriteAsset(Object originalAsset, Object newAsset)
    {
        string path = AssetDatabase.GetAssetPath(originalAsset);
        string meta = File.ReadAllText(path + ".meta");

        AssetDatabase.DeleteAsset(path);
        AssetDatabase.CreateAsset(newAsset, path);

        File.WriteAllText(path + ".meta", meta);
    }
    #endif
}
