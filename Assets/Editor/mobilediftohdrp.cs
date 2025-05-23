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
    
    [Header("Default Material Properties")]
    private float defaultMetallic = 0.0f;
    private float defaultSmoothness = 0.2f;
    private bool useSmartDefaults = true;

    void OnGUI()
    {
        GUILayout.Label("HDRP Material Converter", EditorStyles.boldLabel);
        GUILayout.Space(10);

        EditorGUILayout.HelpBox("This tool will convert Mobile/Diffuse materials to HDRP Lit materials and update texture import settings.", MessageType.Info);
        GUILayout.Space(10);

        convertAllMaterials = EditorGUILayout.Toggle("Convert All Materials in Project", convertAllMaterials);
        createBackup = EditorGUILayout.Toggle("Create Backup of Original Materials", createBackup);
        
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Material Properties", EditorStyles.boldLabel);
        useSmartDefaults = EditorGUILayout.Toggle("Use Smart Material Detection", useSmartDefaults);
        EditorGUILayout.HelpBox("Smart detection automatically sets appropriate metallic/smoothness values based on material names.", MessageType.Info);
        
        if (!useSmartDefaults)
        {
            defaultMetallic = EditorGUILayout.Slider("Default Metallic", defaultMetallic, 0f, 1f);
            defaultSmoothness = EditorGUILayout.Slider("Default Smoothness", defaultSmoothness, 0f, 1f);
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Convert Materials", GUILayout.Height(30)))
        {
            ConvertMaterials();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Fix Overly Glossy Materials", GUILayout.Height(25)))
        {
            FixGlossyMaterials();
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

        EditorUtility.DisplayProgressBar("Finding Materials", "Scanning project for materials...", 0f);

        for (int i = 0; i < materialGuids.Length; i++)
        {
            EditorUtility.DisplayProgressBar("Finding Materials", $"Scanning materials... {i + 1}/{materialGuids.Length}", (float)i / materialGuids.Length);
            
            string path = AssetDatabase.GUIDToAssetPath(materialGuids[i]);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (material != null && (material.shader.name.Contains("Mobile/Diffuse") || 
                material.shader.name.Contains("Standard") || 
                material.shader.name.Contains("Legacy")))
            {
                materialsToConvert.Add(material);
            }
        }

        EditorUtility.ClearProgressBar();

        if (materialsToConvert.Count == 0)
        {
            EditorUtility.DisplayDialog("No Materials Found", "No materials using Mobile/Diffuse or Standard shaders were found.", "OK");
            return;
        }

        bool proceed = EditorUtility.DisplayDialog("Materials Found", 
            $"Found {materialsToConvert.Count} materials to convert.\n\nThis may take several minutes for large projects.\n\nProceed?", 
            "Yes", "Cancel");

        if (!proceed) return;

        int convertedCount = 0;
        Shader hdrpLitShader = Shader.Find("HDRP/Lit");

        if (hdrpLitShader == null)
        {
            EditorUtility.DisplayDialog("HDRP Shader Not Found", "HDRP/Lit shader not found. Make sure HDRP is properly installed.", "OK");
            return;
        }

        HashSet<Texture2D> processedTextures = new HashSet<Texture2D>();
        const int batchSize = 50;

        for (int i = 0; i < materialsToConvert.Count; i++)
        {
            Material material = materialsToConvert[i];
            
            if (EditorUtility.DisplayCancelableProgressBar("Converting Materials", 
                $"Converting {material.name}... ({i + 1}/{materialsToConvert.Count})", 
                (float)i / materialsToConvert.Count))
            {
                EditorUtility.ClearProgressBar();
                EditorUtility.DisplayDialog("Conversion Cancelled", $"Converted {convertedCount} materials before cancellation.", "OK");
                return;
            }

            try
            {
                if (createBackup)
                {
                    string materialPath = AssetDatabase.GetAssetPath(material);
                    string backupPath = materialPath.Replace(".mat", "_backup.mat");
                    AssetDatabase.CopyAsset(materialPath, backupPath);
                }

                Texture2D mainTexture = material.GetTexture("_MainTex") as Texture2D;
                Color mainColor = material.HasProperty("_Color") ? material.color : Color.white;

                material.shader = hdrpLitShader;

                if (mainTexture != null)
                {
                    material.SetTexture("_BaseColorMap", mainTexture);
                    
                    if (!processedTextures.Contains(mainTexture))
                    {
                        UpdateTextureForHDRP(mainTexture);
                        processedTextures.Add(mainTexture);
                    }
                }

                material.SetColor("_BaseColor", mainColor);
                SetMaterialProperties(material);

                material.SetFloat("_SurfaceType", 0);
                material.SetFloat("_BlendMode", 0);
                material.SetFloat("_CullMode", 2);
                material.SetFloat("_ZWrite", 1);

                EditorUtility.SetDirty(material);
                convertedCount++;

                if (i % batchSize == 0)
                {
                    AssetDatabase.SaveAssets();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to convert material {material.name}: {e.Message}");
            }
        }

        EditorUtility.ClearProgressBar();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Conversion Complete", 
            $"Successfully converted {convertedCount} materials to HDRP Lit shader.\nProcessed {processedTextures.Count} unique textures.", "OK");
    }

    void FixGlossyMaterials()
    {
        string[] materialGuids = AssetDatabase.FindAssets("t:Material");
        List<Material> hdrpMaterials = new List<Material>();

        foreach (string guid in materialGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (material != null && material.shader.name == "HDRP/Lit")
            {
                hdrpMaterials.Add(material);
            }
        }

        if (hdrpMaterials.Count == 0)
        {
            EditorUtility.DisplayDialog("No HDRP Materials Found", "No HDRP/Lit materials found to fix.", "OK");
            return;
        }

        int fixedCount = 0;

        for (int i = 0; i < hdrpMaterials.Count; i++)
        {
            Material material = hdrpMaterials[i];
            
            EditorUtility.DisplayProgressBar("Fixing Materials", 
                $"Fixing {material.name}... ({i + 1}/{hdrpMaterials.Count})", 
                (float)i / hdrpMaterials.Count);

            if (material.GetFloat("_Smoothness") > 0.4f || material.GetFloat("_Metallic") > 0.1f)
            {
                SetMaterialProperties(material);
                EditorUtility.SetDirty(material);
                fixedCount++;
            }
        }

        EditorUtility.ClearProgressBar();
        AssetDatabase.SaveAssets();
        
        EditorUtility.DisplayDialog("Fix Complete", 
            $"Fixed {fixedCount} overly glossy materials.", "OK");
    }

    void SetMaterialProperties(Material material)
    {
        if (useSmartDefaults)
        {
            string materialName = material.name.ToLower();
            
            if (materialName.Contains("metal") || materialName.Contains("steel") || 
                materialName.Contains("iron") || materialName.Contains("aluminum") ||
                materialName.Contains("chrome") || materialName.Contains("gold") ||
                materialName.Contains("silver") || materialName.Contains("copper"))
            {
                material.SetFloat("_Metallic", 1.0f);
                material.SetFloat("_Smoothness", 0.8f);
            }
            else if (materialName.Contains("glass") || materialName.Contains("mirror") ||
                     materialName.Contains("water") || materialName.Contains("ice"))
            {
                material.SetFloat("_Metallic", 0.0f);
                material.SetFloat("_Smoothness", 0.95f);
            }
            else if (materialName.Contains("plastic") || materialName.Contains("vinyl"))
            {
                material.SetFloat("_Metallic", 0.0f);
                material.SetFloat("_Smoothness", 0.6f);
            }
            else if (materialName.Contains("wood") || materialName.Contains("stone") ||
                     materialName.Contains("concrete") || materialName.Contains("brick") ||
                     materialName.Contains("dirt") || materialName.Contains("sand") ||
                     materialName.Contains("fabric") || materialName.Contains("cloth") ||
                     materialName.Contains("leather") || materialName.Contains("paper"))
            {
                material.SetFloat("_Metallic", 0.0f);
                material.SetFloat("_Smoothness", 0.1f);
            }
            else if (materialName.Contains("paint") || materialName.Contains("wall"))
            {
                material.SetFloat("_Metallic", 0.0f);
                material.SetFloat("_Smoothness", 0.3f);
            }
            else
            {
                material.SetFloat("_Metallic", 0.0f);
                material.SetFloat("_Smoothness", 0.2f);
            }
        }
        else
        {
            material.SetFloat("_Metallic", defaultMetallic);
            material.SetFloat("_Smoothness", defaultSmoothness);
        }
    }

    void UpdateTextureImportSettings()
    {
        string[] textureGuids = AssetDatabase.FindAssets("t:Texture2D");
        int updatedCount = 0;
        const int batchSize = 100;

        for (int i = 0; i < textureGuids.Length; i++)
        {
            if (EditorUtility.DisplayCancelableProgressBar("Updating Textures", 
                $"Processing textures... ({i + 1}/{textureGuids.Length})", 
                (float)i / textureGuids.Length))
            {
                EditorUtility.ClearProgressBar();
                EditorUtility.DisplayDialog("Update Cancelled", $"Updated {updatedCount} textures before cancellation.", "OK");
                return;
            }

            string path = AssetDatabase.GUIDToAssetPath(textureGuids[i]);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer != null)
            {
                bool needsUpdate = false;

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

            if (i % batchSize == 0)
            {
                System.GC.Collect();
            }
        }

        EditorUtility.ClearProgressBar();
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
            importer.textureType = TextureImporterType.Default;
            importer.sRGBTexture = true;
            importer.mipmapEnabled = true;

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
            material.SetFloat("_Smoothness", 0.2f);

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