#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MeshTool : EditorWindow
{
    [MenuItem("Meshes/Mesh Tool")]
    public static void ShowMeshTool()
    {
        MeshTool window = GetWindow<MeshTool>();
        window.titleContent = new GUIContent("Mesh Tool");
    }

    private enum MenuState
    {
        Default, Splitter, Fixer
    };

    private MenuState state;

    private bool preserveUV2;

    private Mesh currentMesh;

    public class MeshObject
    {
        public MeshRenderer renderer;

        public MeshFilter filter;

        public MeshObject(MeshRenderer renderer, MeshFilter filter)
        {
            this.renderer = renderer;
            this.filter = filter;
        }
    }

    public void OnGUI()
    {
        switch (state)
        {
            case MenuState.Default:
            {
                if (GUILayout.Button("Mesh Splitter"))
                {
                    state = MenuState.Splitter;
                }

                if (GUILayout.Button("Mesh Fixer"))
                {
                    state = MenuState.Fixer;
                }
                
                break;
            }

            case MenuState.Splitter:
            {
                preserveUV2 = GUILayout.Toggle(preserveUV2, "Preserve Lightmap UVs");

                if (GUILayout.Button("Split current scene"))
                {
                    SplitCurrentScene(preserveUV2);
                }

                break;
            }

            case MenuState.Fixer:
            {
                currentMesh = (Mesh)EditorGUILayout.ObjectField(currentMesh, typeof(Mesh), allowSceneObjects: false);

                if (currentMesh != null)
                {
                    if (GUILayout.Button("Fix mesh"))
                    {
                        Mesh fixedMesh = FixMesh(currentMesh);
                        AssetDatabase.CreateAsset(fixedMesh, AssetDatabase.GetAssetPath(currentMesh));
                    }
                }

                break;
            }
        }

        if (state != MenuState.Default)
        {
            if (GUILayout.Button("Back"))
            {
                state = MenuState.Default;
            }
        }
    }

    private Mesh FixMesh(Mesh original)
    {
        Mesh mesh = new Mesh()
        {
            name = original.name,
            subMeshCount = original.subMeshCount,

            vertices = DefaultToNull(original.vertices),
            normals = DefaultToNull(original.normals),
            tangents = DefaultToNull(original.tangents),
            colors32 = DefaultToNull(original.colors32),

            uv  = DefaultToNull(original.uv),
            uv2 = DefaultToNull(original.uv2),
            uv3 = DefaultToNull(original.uv3),
            uv4 = DefaultToNull(original.uv4),
            uv5 = DefaultToNull(original.uv5),
            uv6 = DefaultToNull(original.uv6),
            uv7 = DefaultToNull(original.uv7),
            uv8 = DefaultToNull(original.uv8),
        };

        if (original.subMeshCount <= 1)
        {
            mesh.triangles = original.triangles;
        }
        else
        {
            for (int i = 0; i < original.subMeshCount; i++)
            {
                mesh.SetSubMesh(i, original.GetSubMesh(i));
            }

            for (int i = 0; i < original.subMeshCount; i++)
            {
                mesh.SetTriangles(mesh.GetTriangles(i), i);
            }
        }

        mesh.RecalculateBounds();

        return mesh;
    }

    private void SplitCurrentScene(bool includeUV2 = false)
    {
        List<MeshObject> objects = FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None)
            .Where(x => x.isPartOfStaticBatch)
            .Select(x => new MeshObject(x, x.GetComponent<MeshFilter>())).ToList();

        Mesh[] submeshes = SplitCombinedMesh(objects[0].filter.sharedMesh, includeUV2);
        AssetDatabase.StartAssetEditing();

        foreach (MeshObject meshObject in objects)
        {
            Mesh originalMesh = null;

            if (meshObject.renderer.sharedMaterials.Length <= 1)
            {
                originalMesh = submeshes[meshObject.renderer.subMeshStartIndex];
            }
            else
            {
                originalMesh = CombineMeshes(submeshes.Skip(meshObject.renderer.subMeshStartIndex)
                .Take(meshObject.renderer.sharedMaterials.Length).ToArray(), includeUV2);
            }

            originalMesh.vertices = ResetMeshPosition(originalMesh.vertices, meshObject.renderer.transform);
            originalMesh.RecalculateBounds();

            string name = meshObject.renderer.name;
            UnityEngine.SceneManagement.Scene currentScene = SceneManager.GetActiveScene();

            string path = Path.Combine(currentScene.path).Replace(".unity", "");
            string fullPath = Path.Combine(path, name + ".asset");

            int duplicationCount = 0;

            while (File.Exists(fullPath))
            {
                name = meshObject.renderer.name + "_" + duplicationCount++;
                fullPath = Path.Combine(path, name + ".asset");
            }

            originalMesh.name = name;
            meshObject.filter.sharedMesh = originalMesh;

            Directory.CreateDirectory(path);
            AssetDatabase.CreateAsset(originalMesh, fullPath);
        }

        AssetDatabase.StopAssetEditing();
    }

    private static Vector3[] ResetMeshPosition(Vector3[] vertices, Transform target)
    {
        Matrix4x4 inverse = target.localToWorldMatrix.inverse;
        Vector3[] positionedVertices = new Vector3[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            positionedVertices[i] = inverse.MultiplyPoint3x4(vertices[i]);
        }

        return positionedVertices;
    }

    private static bool DefaultCheck<T>(T value)
    {
        switch (value)
        {
            case Vector2 v2:
                return v2.x == 0f && v2.y == 0f;

            case Vector3 v3:
                return v3.x == 0f && v3.y == 0f && v3.z == 0f;

            case Vector4 v4:
                return v4.x == 0f && v4.y == 0f && v4.z == 0f && v4.w == 0f;

            case Color32 c32:
                return c32.r == 0 && c32.g == 0 && c32.b == 0 && c32.a == 0;

            default:
                Debug.LogError("tried running DefaultCheck for " + value.GetType().Name + ", no implementation");
                return false;
        }
    }

    private static T[] Validate<T>(T[] original)
    {
        switch (original)
        {
            case Vector2[] v2s:
                for (int i = 0; i < v2s.Length; i++)
                {
                    if (!float.IsFinite(v2s[i].x)) v2s[i].x = 0f;
                    if (!float.IsFinite(v2s[i].y)) v2s[i].y = 0f;
                }
                return v2s as T[];

            case Vector3[] v3s:
                for (int i = 0; i < v3s.Length; i++)
                {
                    if (!float.IsFinite(v3s[i].x)) v3s[i].x = 0f;
                    if (!float.IsFinite(v3s[i].y)) v3s[i].y = 0f;
                    if (!float.IsFinite(v3s[i].z)) v3s[i].z = 0f;
                }
                return v3s as T[];

            case Vector4[] v4s:
                for (int i = 0; i < v4s.Length; i++)
                {
                    if (!float.IsFinite(v4s[i].x)) v4s[i].x = 0f;
                    if (!float.IsFinite(v4s[i].y)) v4s[i].y = 0f;
                    if (!float.IsFinite(v4s[i].z)) v4s[i].z = 0f;
                    if (!float.IsFinite(v4s[i].w)) v4s[i].w = 0f;
                }
                return v4s as T[];
        }

        return original;
    }

    private static bool DefaultCheck<T>(T[] value)
    {
        if (value == null || value.Length == 0)
        {
            return true;
        }

        for (int i = 0; i < value.Length; i++)
        {
            if (!DefaultCheck(value[i]))
            {
                return false;
            }
        }

        return true;
    }

    private static T[] Select<T>(T[] list, SubMeshDescriptor submesh)
    {
        list = Validate(list);

        if (DefaultCheck(list))
        {
            return null;
        }

        return list.Skip(submesh.firstVertex).Take(submesh.vertexCount).ToArray();
    }

    private Mesh[] SplitCombinedMesh(Mesh combinedMesh, bool includeUV2 = false)
    {
        Mesh[] submeshes = new Mesh[combinedMesh.subMeshCount];

        for (int i = 0; i < combinedMesh.subMeshCount; i++)
        {
            SubMeshDescriptor descriptor = combinedMesh.GetSubMesh(i);

            Mesh submesh = new Mesh
            {
                vertices = Select(combinedMesh.vertices, descriptor),
                normals = Select(combinedMesh.normals, descriptor),
                tangents = Select(combinedMesh.tangents, descriptor),
                colors32 = Select(combinedMesh.colors32, descriptor),

                uv = Select(combinedMesh.uv, descriptor), uv3 = Select(combinedMesh.uv3, descriptor),
                uv4 = Select(combinedMesh.uv4, descriptor), uv5 = Select(combinedMesh.uv5, descriptor),
                uv6 = Select(combinedMesh.uv6, descriptor), uv7 = Select(combinedMesh.uv7, descriptor),
                uv8 = Select(combinedMesh.uv8, descriptor)
            };

            if (includeUV2)
            {
                submesh.uv2 = Select(combinedMesh.uv2, descriptor);
            }

            submesh.triangles = combinedMesh.GetTriangles(i).Select(x => x - descriptor.firstVertex).ToArray();
            submesh.RecalculateBounds();

            submeshes[i] = submesh;
        }

        return submeshes;
    }

    private void FillList<T>(List<T> list, T[] array, int length)
    {
        if (array == null || array.Length == 0)
        {
            list.AddRange(new List<T>(length));
        }
        else
        {
            list.AddRange(array);
        }
    }

    private T[] DefaultToNull<T>(List<T> list)
    {
        if (list.Count == 0 || DefaultCheck(list.ToArray()))
        {
            return null;
        }

        return list.ToArray();
    }

    private T[] DefaultToNull<T>(T[] array)
    {
        if (array.Length == 0 || DefaultCheck(array))
        {
            return null;
        }

        return Validate(array);
    }

    private Mesh CombineMeshes(Mesh[] meshes, bool includeUV2 = false)
    {
        Mesh mesh = new Mesh()
        {
            subMeshCount = meshes.Length
        };

        int vertexOffset = 0, indexOffset = 0;

        List<Vector4> tangents = new();
        List<Vector3> vertices = new(), normals = new();
        List<Vector2> uv = new(), uv2 = new(), uv3 = new(),
        uv4 = new(), uv5 = new(), uv6 = new(), uv7 = new(), uv8 = new();

        List<Color32> colors32 = new();
        List<List<int>> triangles = new();

        List<SubMeshDescriptor> descriptors = new();

        for (int i = 0; i < meshes.Length; i++)
        {
            SubMeshDescriptor descriptor = new SubMeshDescriptor
            {
                firstVertex = vertexOffset,
                indexStart = indexOffset,

                vertexCount = meshes[i].vertexCount,
                indexCount = meshes[i].triangles.Length
            };

            triangles.Add(meshes[i].triangles.Select(x => x + vertexOffset).ToList());

            vertexOffset += descriptor.vertexCount;
            indexOffset += descriptor.indexCount;

            FillList(vertices, meshes[i].vertices, descriptor.vertexCount);
            FillList(normals, meshes[i].normals, descriptor.vertexCount);
            FillList(tangents, meshes[i].tangents, descriptor.vertexCount);
            FillList(colors32, meshes[i].colors32, descriptor.vertexCount);

            FillList(uv,  meshes[i].uv,  descriptor.vertexCount);
            FillList(uv3, meshes[i].uv3, descriptor.vertexCount);
            FillList(uv4, meshes[i].uv4, descriptor.vertexCount);
            FillList(uv5, meshes[i].uv5, descriptor.vertexCount);
            FillList(uv6, meshes[i].uv6, descriptor.vertexCount);
            FillList(uv7, meshes[i].uv7, descriptor.vertexCount);
            FillList(uv8, meshes[i].uv8, descriptor.vertexCount);

            if (includeUV2)
            {
                FillList(uv, meshes[i].uv2, descriptor.vertexCount);
            }

            descriptors.Add(descriptor);
        }

        mesh.vertices = DefaultToNull(vertices);
        mesh.normals = DefaultToNull(normals);
        mesh.tangents = DefaultToNull(tangents);
        mesh.colors32 = DefaultToNull(colors32);

        mesh.uv  = DefaultToNull(uv);
        mesh.uv3 = DefaultToNull(uv3);
        mesh.uv4 = DefaultToNull(uv4);
        mesh.uv5 = DefaultToNull(uv5);
        mesh.uv6 = DefaultToNull(uv6);
        mesh.uv7 = DefaultToNull(uv7);
        mesh.uv8 = DefaultToNull(uv8);

        if (includeUV2)
        {
            mesh.uv2 = DefaultToNull(uv2);
        }

        for (int i = 0; i < meshes.Length; i++)
        {
            mesh.SetTriangles(triangles[i].ToArray(), i);
        }

        for (int i = 0; i < meshes.Length; i++)
        {
            mesh.SetSubMesh(i, descriptors[i]);
        }

        mesh.RecalculateBounds();

        return mesh;
    }
}
#endif