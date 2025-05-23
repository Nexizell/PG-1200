using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering.HighDefinition;
using System.Collections.Generic;

public class HDRPMaterialConverter : EditorWindow
{
    [MenuItem("Tools/HDRP Material Converter")]
    public static void ShowWindow()
    {
        GetWindow<HDRPMaterialConverter>("HDRP Material Converter");
    }

    private bool convertAllMaterials = true;
    private bool createBackup = true;
    private Vector2 scrollPosition;

    void OnGUI()
    {
        GUILayout.Label("HDRP Material Converter", EditorStyles.boldLabel);
        GUILayout.Space(10);

        EditorGUILayout.HelpBox("This tool will convert Mobile/Diffuse materials to HDRP Lit materials and update texture import settings.", MessageType.Info);
        GUILayout.Space(10);

        convertAllMaterials = EditorGUILayout.Toggle("Convert All Materials in Project", convertAllMaterials);
        createBackup = EditorGUILayout.Toggle("Create Backup of Original Materials", createBackup);

        GUILayout.Space(20);

        if (GUILayout.Button("Convert Materials", GUILayout.Height(30)))
        {
            ConvertMaterials();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Update Texture Import Settings Only", GUILayout.Height(25)))
        {
            UpdateTextureImportSettings();
        }
    }

    void ConvertMaterials()
    {
        string[] materialGuids = AssetDatabase.FindAssets("t:Material");
        List<Material> materialsToConvert = new List<Material>();

        // Find materials using Mobile/Diffuse shader
        foreach (string guid in materialGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (material != null && (material.shader.name.Contains("Mobile/Diffuse") || 
                material.shader.name.Contains("Standard") || 
                material.shader.name.Contains("Legacy")))
            {
                materialsToConvert.Add(material);
            }
        }

        if (materialsToConvert.Count == 0)
        {
            EditorUtility.DisplayDialog("No Materials Found", "No materials using Mobile/Diffuse or Standard shaders were found.", "OK");
            return;
        }

        int convertedCount = 0;
        Shader hdrpLitShader = Shader.Find("HDRP/Lit");

        if (hdrpLitShader == null)
        {
            EditorUtility.DisplayDialog("HDRP Shader Not Found", "HDRP/Lit shader not found. Make sure HDRP is properly installed.", "OK");
            return;
        }

        foreach (Material material in materialsToConvert)
        {
            try
            {
                // Create backup if requested
                if (createBackup)
                {
                    string materialPath = AssetDatabase.GetAssetPath(material);
                    string backupPath = materialPath.Replace(".mat", "_backup.mat");
                    AssetDatabase.CopyAsset(materialPath, backupPath);
                }

                // Store old texture references
                Texture2D mainTexture = material.GetTexture("_MainTex") as Texture2D;
                Color mainColor = material.HasProperty("_Color") ? material.color : Color.white;

                // Convert to HDRP Lit shader
                material.shader = hdrpLitShader;

                // Map properties from old shader to HDRP Lit
                if (mainTexture != null)
                {
                    material.SetTexture("_BaseColorMap", mainTexture);
                    UpdateTextureForHDRP(mainTexture);
                }

                material.SetColor("_BaseColor", mainColor);

                // Set common HDRP properties
                material.SetFloat("_Metallic", 0.0f);
                material.SetFloat("_Smoothness", 0.5f);

                // Enable surface options for HDRP
                material.SetFloat("_SurfaceType", 0); // Opaque
                material.SetFloat("_BlendMode", 0);   // Alpha
                material.SetFloat("_CullMode", 2);    // Back
                material.SetFloat("_ZWrite", 1);      // On

                EditorUtility.SetDirty(material);
                convertedCount++;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to convert material {material.name}: {e.Message}");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Conversion Complete", 
            $"Successfully converted {convertedCount} materials to HDRP Lit shader.", "OK");
    }

    void UpdateTextureImportSettings()
    {
        string[] textureGuids = AssetDatabase.FindAssets("t:Texture2D");
        int updatedCount = 0;

        foreach (string guid in textureGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer != null)
            {
                bool needsUpdate = false;

                // Update texture import settings for HDRP
                if (importer.textureType != TextureImporterType.Default)
                {
                    importer.textureType = TextureImporterType.Default;
                    needsUpdate = true;
                }

                if (!importer.sRGBTexture && IsColorTexture(path))
                {
                    importer.sRGBTexture = true;
                    needsUpdate = true;
                }

                if (importer.mipmapEnabled == false)
                {
                    importer.mipmapEnabled = true;
                    needsUpdate = true;
                }

                // Set appropriate texture format for different platforms
                TextureImporterPlatformSettings pcSettings = importer.GetPlatformTextureSettings("Standalone");
                if (pcSettings.format != TextureImporterFormat.DXT5 && pcSettings.format != TextureImporterFormat.BC7)
                {
                    pcSettings.format = TextureImporterFormat.BC7;
                    importer.SetPlatformTextureSettings(pcSettings);
                    needsUpdate = true;
                }

                if (needsUpdate)
                {
                    importer.SaveAndReimport();
                    updatedCount++;
                }
            }
        }

        EditorUtility.DisplayDialog("Texture Update Complete", 
            $"Updated import settings for {updatedCount} textures.", "OK");
    }

    void UpdateTextureForHDRP(Texture2D texture)
    {
        if (texture == null) return;

        string path = AssetDatabase.GetAssetPath(texture);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

        if (importer != null)
        {
            // Ensure proper settings for HDRP
            importer.textureType = TextureImporterType.Default;
            importer.sRGBTexture = true; // Most diffuse textures should be sRGB
            importer.mipmapEnabled = true;

            // Set compression format
            TextureImporterPlatformSettings settings = importer.GetPlatformTextureSettings("Standalone");
            settings.format = TextureImporterFormat.BC7;
            importer.SetPlatformTextureSettings(settings);

            importer.SaveAndReimport();
        }
    }

    bool IsColorTexture(string path)
    {
        string fileName = System.IO.Path.GetFileNameWithoutExtension(path).ToLower();
        return !fileName.Contains("normal") && !fileName.Contains("height") && 
               !fileName.Contains("bump") && !fileName.Contains("metallic") && 
               !fileName.Contains("roughness") && !fileName.Contains("ao");
    }
}

// Additional utility class for batch operations
public class HDRPBatchProcessor
{
    [MenuItem("Assets/Convert Selected Materials to HDRP Lit", false, 1000)]
    static void ConvertSelectedMaterials()
    {
        Object[] selectedObjects = Selection.objects;
        List<Material> materials = new List<Material>();

        foreach (Object obj in selectedObjects)
        {
            if (obj is Material)
            {
                materials.Add(obj as Material);
            }
        }

        if (materials.Count == 0)
        {
            EditorUtility.DisplayDialog("No Materials Selected", "Please select materials to convert.", "OK");
            return;
        }

        Shader hdrpLitShader = Shader.Find("HDRP/Lit");
        if (hdrpLitShader == null)
        {
            EditorUtility.DisplayDialog("HDRP Shader Not Found", "HDRP/Lit shader not found.", "OK");
            return;
        }

        foreach (Material material in materials)
        {
            Texture2D mainTexture = material.GetTexture("_MainTex") as Texture2D;
            Color mainColor = material.HasProperty("_Color") ? material.color : Color.white;

            material.shader = hdrpLitShader;

            if (mainTexture != null)
            {
                material.SetTexture("_BaseColorMap", mainTexture);
            }

            material.SetColor("_BaseColor", mainColor);
            material.SetFloat("_Metallic", 0.0f);
            material.SetFloat("_Smoothness", 0.5f);

            EditorUtility.SetDirty(material);
        }

        AssetDatabase.SaveAssets();
        EditorUtility.DisplayDialog("Conversion Complete", $"Converted {materials.Count} materials.", "OK");
    }

    [MenuItem("Assets/Convert Selected Materials to HDRP Lit", true)]
    static bool ValidateConvertSelectedMaterials()
    {
        return Selection.objects.Length > 0;
    }
}