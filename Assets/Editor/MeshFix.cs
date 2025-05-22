using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using System.Linq;
using System.IO;

public class MeshFix : Editor
{
    [MenuItem("Mesh Fix /Remove Combined Mesh")]
    public static void RemoveCombinedMesh()
    {
        Dictionary<string, Mesh> cachedMeshes = new Dictionary<string, Mesh>();
        Dictionary<string, int> duplicates = new Dictionary<string, int>();

        foreach (MeshFilter filter in FindObjectsByType<MeshFilter>(FindObjectsSortMode.None))
        {
            if (filter == null || filter.sharedMesh == null)
            {
                continue;
            }
            
            if (filter.sharedMesh.name.StartsWith("Combined Mesh (root scene)") || filter.sharedMesh.name.StartsWith("Combined Mesh (root_ scene)"))
            {
                MeshCollider collider = filter.GetComponent<MeshCollider>();

                if (collider != null && collider.sharedMesh != null)
                {
                    filter.sharedMesh = collider.sharedMesh;
                    continue;
                }

                if (!filter.sharedMesh.isReadable)
                {
                    File.WriteAllText(Application.dataPath + "/" + AssetDatabase.GetAssetPath(filter.sharedMesh).Replace("Assets", ""), File.ReadAllText(AssetDatabase.GetAssetPath(filter.sharedMesh)).Replace("m_IsReadable: 0", "m_IsReadable: 1"));
                    AssetDatabase.Refresh();
                }

                MeshRenderer renderer = filter.GetComponent<MeshRenderer>();
                filter.sharedMesh = ExtractSubmesh(filter.transform, filter.name, filter.sharedMesh, renderer.subMeshStartIndex, renderer.sharedMaterials.Length);

                if (cachedMeshes.ContainsKey(filter.name))
                {
                    if (!MeshesAreTheSame(filter.sharedMesh, cachedMeshes[filter.name]))
                    {
                        if (!duplicates.ContainsKey(filter.name))
                        {
                            duplicates.Add(filter.name, 0);
                        }

                        cachedMeshes.Add(filter.name + "_" + duplicates[filter.name], filter.sharedMesh);
                        duplicates[filter.name]++;
                    }
                    else
                    {
                        filter.sharedMesh = cachedMeshes[filter.name];
                    }
                }
                else
                {
                    cachedMeshes.Add(filter.name, filter.sharedMesh);
                }
            }
        }

        if (!Directory.Exists(Application.dataPath + "/SplitSubmeshes"))
        {
            Directory.CreateDirectory(Application.dataPath + "/SplitSubmeshes");
        }

        if (!Directory.Exists(Application.dataPath + "/SplitSubmeshes/" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name))
        {
            Directory.CreateDirectory(Application.dataPath + "/SplitSubmeshes/" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        foreach (KeyValuePair<string, Mesh> cachedMesh in cachedMeshes)
        {
            AssetDatabase.CreateAsset(cachedMesh.Value, "Assets/SplitSubmeshes/" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "/" + cachedMesh.Key + ".asset");
        }
    }

    [MenuItem("Mesh Fix/Remove All Non-Finite Vertices")]
    public static void FixInfinite()
    {
        foreach (Mesh original in AssetDatabase.FindAssets("t:Mesh").Select(x => AssetDatabase.LoadAssetAtPath<Mesh>(AssetDatabase.GUIDToAssetPath(x))))
        {
            if (!original.isReadable)
            {
                File.WriteAllText(Application.dataPath + "/" + AssetDatabase.GetAssetPath(original).Replace("Assets", ""), File.ReadAllText(AssetDatabase.GetAssetPath(original)).Replace("m_IsReadable: 0", "m_IsReadable: 1"));
            }

            AssetUtility.OverwriteAsset(original, FixMesh(original));
        }

        AssetDatabase.Refresh();
    }

    private static Mesh FixMesh(Mesh original)
    {   
        Mesh mesh = new Mesh();

        mesh.SetVertices(FixNonFiniteVector3s(original.vertices));
        mesh.SetNormals(FixNonFiniteVector3s(original.normals));
        mesh.SetTangents(FixNonFiniteVector4s(original.tangents));

        mesh.SetUVs(0, FixNonFiniteVector2s(original.uv));
        mesh.SetUVs(1, FixNonFiniteVector2s(original.uv2));
        mesh.SetUVs(2, FixNonFiniteVector2s(original.uv3));
        mesh.SetUVs(3, FixNonFiniteVector2s(original.uv4));
        mesh.SetUVs(4, FixNonFiniteVector2s(original.uv5));
        mesh.SetUVs(5, FixNonFiniteVector2s(original.uv6));
        mesh.SetUVs(6, FixNonFiniteVector2s(original.uv7));
        mesh.SetUVs(7, FixNonFiniteVector2s(original.uv8));

        mesh.colors = original.colors;
        mesh.colors32 = original.colors32;

        mesh.subMeshCount = original.subMeshCount;

        for (int i = 0; i < original.subMeshCount; i++)
        {
            mesh.SetTriangles(original.GetTriangles(i), i);
        }

        for (int i = 0; i < original.subMeshCount; i++)
        {
            mesh.SetSubMesh(i, original.GetSubMesh(i));
        }

        mesh.boneWeights = original.boneWeights;
        mesh.bindposes = original.bindposes;

        mesh.bounds = original.bounds;
        mesh.indexFormat = original.indexFormat;

        mesh.vertexBufferTarget = original.vertexBufferTarget;
        mesh.indexBufferTarget = original.indexBufferTarget;

        return mesh;
    }

    private static bool MeshesAreTheSame(Mesh x, Mesh y)
    {
        if (x.vertices.Length != y.vertices.Length)
        {
            return false;
        }

        for (int i = 0; i < x.vertices.Length; i++)
        {
            if (x.vertices[i] != y.vertices[i] || x.normals[i] != y.normals[i] || x.uv[i] != y.uv[i])
            {
                return false;
            }
        }

        return true;
    }

    private static Mesh ExtractSubmesh(Transform transform, string name, Mesh original, int subMeshIndex, int subMeshCount)
    {
        Mesh mesh = new Mesh
        {
            subMeshCount = subMeshCount
        };

        List<Vector3> vertices = new List<Vector3>(),
        normals = new List<Vector3>();

        List<Vector4> tangents = new List<Vector4>();
        List<BoneWeight> boneWeights = new List<BoneWeight>();

        List<Vector2> uv = new List<Vector2>(),
        uv2 = new List<Vector2>(),
        uv3 = new List<Vector2>(),
        uv4 = new List<Vector2>(),
        uv5 = new List<Vector2>(),
        uv6 = new List<Vector2>(),
        uv7 = new List<Vector2>(),
        uv8 = new List<Vector2>();

        List<Color> colors = new List<Color>();
        List<Color32> colors32 = new List<Color32>();

        List<int[]> triangles = new List<int[]>();
        List<SubMeshDescriptor> descriptors = new List<SubMeshDescriptor>();

        mesh.subMeshCount = subMeshCount;
        int vertexStart = 0, indexStart = 0;

        int offset = original.GetSubMesh(subMeshIndex).firstVertex;
        
        for (int i = 0; i < subMeshCount; i++)
        {
            SubMeshDescriptor subMesh = original.GetSubMesh(subMeshIndex + i);

            vertices.AddRange(RepositionVertices(transform, original.vertices.ToList().GetRange(subMesh.firstVertex, subMesh.vertexCount).ToArray()));
            normals.AddRange(InvertNormals(subMesh, original, original.normals));
            tangents.AddRange(CheckMeshPart(subMesh, original, original.tangents));
            boneWeights.AddRange(CheckMeshPart(subMesh, original, original.boneWeights));
            uv.AddRange(CheckMeshPart(subMesh, original, original.uv));
            uv2.AddRange(CheckMeshPart(subMesh, original, original.uv2));
            uv3.AddRange(CheckMeshPart(subMesh, original, original.uv3));
            uv4.AddRange(CheckMeshPart(subMesh, original, original.uv4));
            uv5.AddRange(CheckMeshPart(subMesh, original, original.uv5));
            uv6.AddRange(CheckMeshPart(subMesh, original, original.uv6));
            uv7.AddRange(CheckMeshPart(subMesh, original, original.uv7));
            uv8.AddRange(CheckMeshPart(subMesh, original, original.uv8));
            colors.AddRange(CheckMeshPart(subMesh, original, original.colors));
            colors32.AddRange(CheckMeshPart(subMesh, original, original.colors32));

            triangles.Add(original.GetTriangles(subMeshIndex + i).Select(x => x - subMesh.firstVertex + vertexStart).ToArray());
            descriptors.Add(new SubMeshDescriptor(indexStart, subMesh.indexCount, MeshTopology.Triangles) { firstVertex = vertexStart, vertexCount = subMesh.vertexCount });

            vertexStart += subMesh.vertexCount;
            indexStart += subMesh.indexCount;
        }

        if (!EmptyComponent(vertices)) mesh.SetVertices(vertices);
        if (!EmptyComponent(normals)) mesh.SetNormals(normals);
        if (!EmptyComponent(tangents)) mesh.SetTangents(tangents);
        if (!EmptyComponent(boneWeights)) mesh.boneWeights = boneWeights.ToArray();

        if (!EmptyComponent(uv)) mesh.SetUVs(0, uv);
        if (!EmptyComponent(uv2)) mesh.SetUVs(1, uv2);
        if (!EmptyComponent(uv3)) mesh.SetUVs(2, uv3);
        if (!EmptyComponent(uv4)) mesh.SetUVs(3, uv4);
        if (!EmptyComponent(uv5)) mesh.SetUVs(4, uv5);
        if (!EmptyComponent(uv6)) mesh.SetUVs(5, uv6);
        if (!EmptyComponent(uv7)) mesh.SetUVs(6, uv7);
        if (!EmptyComponent(uv8)) mesh.SetUVs(7, uv8);

        if (!EmptyComponent(colors)) mesh.SetColors(colors);
        if (!EmptyComponent(colors32)) mesh.colors32 = colors32.ToArray();

        for (int i = 0; i < subMeshCount; i++)
        {
            mesh.SetTriangles(triangles[i], i);
        }
        
        for (int i = 0; i < subMeshCount; i++)
        {
            mesh.SetSubMesh(i, descriptors[i]);
        }

        mesh.name = name;

        mesh.RecalculateNormals();
        mesh.Optimize();

        mesh.RecalculateBounds();

        return FixMesh(mesh);
    }

    private static bool EmptyComponent<T>(IEnumerable<T> values)
    {
        if (values.Count() == 0)
        {
            return true;
        }

        if (typeof(T) == typeof(Vector4))
        {
            foreach (object t in values)
            {
                if ((Vector4)t != Vector4.zero)
                {
                    return false;
                }
            }
        }
        else if (typeof(T) == typeof(Vector3))
        {
            foreach (object t in values)
            {
                if ((Vector3)t != Vector3.zero)
                {
                    return false;
                }
            }
        }
        else if (typeof(T) == typeof(Vector2))
        {
            foreach (object t in values)
            {
                if ((Vector2)t != Vector2.zero)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static Vector3[] RepositionVertices(Transform transform, Vector3[] vertices)
    {
        Matrix4x4 worldMatrix = transform.localToWorldMatrix.inverse;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = worldMatrix.MultiplyPoint3x4(vertices[i]);
        }

        return vertices;
    }

    private static Vector2[] FixNonFiniteVector2s(Vector2[] vectors)
    {
        for (int i = 0; i < vectors.Length; i++)
        {
            vectors[i] = new Vector2(FixNonFinite(vectors[i].x), FixNonFinite(vectors[i].y));
        }

        return vectors;
    }

    private static Vector3[] FixNonFiniteVector3s(Vector3[] vectors)
    {
        for (int i = 0; i < vectors.Length; i++)
        {
            vectors[i] = new Vector3(FixNonFinite(vectors[i].x), FixNonFinite(vectors[i].y), FixNonFinite(vectors[i].z));
        }

        return vectors;
    }

    private static Vector4[] FixNonFiniteVector4s(Vector4[] vectors)
    {
        for (int i = 0; i < vectors.Length; i++)
        {
            vectors[i] = new Vector4(FixNonFinite(vectors[i].x), FixNonFinite(vectors[i].y), FixNonFinite(vectors[i].z), FixNonFinite(vectors[i].w));
        }

        return vectors;
    }

    private static float FixNonFinite(float f)
    {
        return (float.IsNaN(f) || float.IsInfinity(f)) ? 0f : f;
    }

    private static Vector3[] InvertNormals(SubMeshDescriptor subMesh, Mesh original, Vector3[] originalPart)
    {
        Vector3[] normals = CheckMeshPart(subMesh, original, originalPart);

        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = new Vector3(1f - normals[i].x, 1f - normals[i].y, 1f - normals[i].z);
        }

        return normals;
    }

    private static T[] CheckMeshPart<T>(SubMeshDescriptor subMesh, Mesh original, T[] originalPart)
    {
        if (originalPart == null || originalPart.Length != original.vertices.Length)
        {
            return new T[subMesh.vertexCount];
        }

        return originalPart.ToList().GetRange(subMesh.firstVertex, subMesh.vertexCount).ToArray();
    }
}