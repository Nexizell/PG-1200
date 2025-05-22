using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MeshRewrite : Editor
{
    [MenuItem("Assets/Rewrite Mesh")]
    public static void RewriteMeshes()
    {
        AssetDatabase.StartAssetEditing();

        foreach (Object selection in Selection.objects)
        {
            if (selection is Mesh mesh)
            {
                RewriteMesh(mesh);
            }
        }

        AssetDatabase.StopAssetEditing();
    }

    private static void RewriteMesh(Mesh mesh)
    {
        Mesh copy = new Mesh
        {
            name = mesh.name,
            subMeshCount = mesh.subMeshCount,
            bounds = mesh.bounds
        };

        if (!Empty(mesh.vertices)) copy.SetVertices(mesh.vertices);
        if (!Empty(mesh.normals)) copy.SetNormals(mesh.vertices);
        if (!Empty(mesh.tangents)) copy.SetTangents(mesh.tangents);

        if (mesh.uv.Length == mesh.vertexCount) copy.uv = mesh.uv;
        //if (mesh.uv2.Length == mesh.vertexCount) copy.uv2 = mesh.uv2;
        //if (mesh.uv3.Length == mesh.vertexCount) copy.uv3 = mesh.uv3;
        //if (mesh.uv4.Length == mesh.vertexCount) copy.uv4 = mesh.uv4;
        //if (mesh.uv5.Length == mesh.vertexCount) copy.uv5 = mesh.uv5;
        //if (mesh.uv6.Length == mesh.vertexCount) copy.uv6 = mesh.uv6;
        //if (mesh.uv7.Length == mesh.vertexCount) copy.uv7 = mesh.uv7;
        //if (mesh.uv8.Length == mesh.vertexCount) copy.uv8 = mesh.uv8;

        if (!Empty(mesh.colors)) copy.colors = mesh.colors;
        if (!Empty(mesh.colors32)) copy.colors32 = mesh.colors32;

        if (!Empty(mesh.bindposes)) copy.bindposes = mesh.bindposes;
        if (!Empty(mesh.boneWeights)) copy.boneWeights = mesh.boneWeights;

        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            copy.SetTriangles(mesh.GetTriangles(i, true), i);
        }

        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            copy.SetSubMesh(i, mesh.GetSubMesh(i));
        }

        string path = AssetDatabase.GetAssetPath(mesh);
        string meta = File.ReadAllText(path + ".meta");

        AssetDatabase.DeleteAsset(path);
        AssetDatabase.CreateAsset(copy, path);

        File.WriteAllText(path + ".meta", meta);
    }

    private static bool Empty<T>(T[] array)
    {
        return array == null || array.Length == 0;
    }


    private static bool Empty<T>(List<T> list)
    {
        return list == null || list.Count == 0;
    }
}
