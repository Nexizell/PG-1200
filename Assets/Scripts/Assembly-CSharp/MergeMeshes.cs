using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Tasharen/Merge Meshes")]
public class MergeMeshes : MonoBehaviour
{
	public enum PostMerge
	{
		DisableRenderers = 0,
		DestroyRenderers = 1,
		DisableGameObjects = 2,
		DestroyGameObjects = 3
	}

	public Material material;

	public PostMerge afterMerging;

	private string mName;

	private Transform mTrans;

	private Mesh mMesh;

	private MeshFilter mFilter;

	private MeshRenderer mRen;

	private List<GameObject> mDisabledGO = new List<GameObject>();

	private List<Renderer> mDisabledRen = new List<Renderer>();

	private bool mMerge = true;

	private void Start()
	{
		if (mMerge)
		{
			Merge(true);
		}
		base.enabled = false;
	}

	private void Update()
	{
		if (mMerge)
		{
			Merge(true);
		}
		base.enabled = false;
	}

	public void Merge(bool immediate)
	{
		if (!immediate)
		{
			mMerge = true;
			base.enabled = true;
			return;
		}
		mMerge = false;
		mName = base.name;
		mFilter = GetComponent<MeshFilter>();
		mTrans = base.transform;
		Clear();
		MeshFilter[] componentsInChildren = GetComponentsInChildren<MeshFilter>();
		if (componentsInChildren.Length == 0 || (mFilter != null && componentsInChildren.Length == 1))
		{
			return;
		}
		GameObject gameObject = base.gameObject;
		Matrix4x4 worldToLocalMatrix = gameObject.transform.worldToLocalMatrix;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		MeshFilter[] array = componentsInChildren;
		foreach (MeshFilter meshFilter in array)
		{
			if (meshFilter == mFilter)
			{
				continue;
			}
			if (meshFilter.gameObject.isStatic)
			{
				Debug.LogError("MergeMeshes can't merge objects marked as static", meshFilter.gameObject);
				continue;
			}
			Mesh sharedMesh = meshFilter.sharedMesh;
			if (material == null)
			{
				material = meshFilter.GetComponent<Renderer>().sharedMaterial;
			}
			num += sharedMesh.vertexCount;
			num2 += sharedMesh.triangles.Length;
			if (sharedMesh.normals != null)
			{
				num3 += sharedMesh.normals.Length;
			}
			if (sharedMesh.tangents != null)
			{
				num4 += sharedMesh.tangents.Length;
			}
			if (sharedMesh.colors != null)
			{
				num5 += sharedMesh.colors.Length;
			}
			if (sharedMesh.uv != null)
			{
				num6 += sharedMesh.uv.Length;
			}
			if (sharedMesh.uv2 != null)
			{
				num7 += sharedMesh.uv2.Length;
			}
		}
		if (num == 0)
		{
			Debug.LogWarning("Unable to find any non-static objects to merge", this);
			return;
		}
		Vector3[] array2 = new Vector3[num];
		int[] array3 = new int[num2];
		Vector2[] array4 = ((num6 == num) ? new Vector2[num] : null);
		Vector2[] array5 = ((num7 == num) ? new Vector2[num] : null);
		Vector3[] array6 = ((num3 == num) ? new Vector3[num] : null);
		Vector4[] array7 = ((num4 == num) ? new Vector4[num] : null);
		Color[] array8 = ((num5 == num) ? new Color[num] : null);
		int num8 = 0;
		int num9 = 0;
		int num10 = 0;
		array = componentsInChildren;
		foreach (MeshFilter meshFilter2 in array)
		{
			if (meshFilter2 == mFilter || meshFilter2.gameObject.isStatic)
			{
				continue;
			}
			Mesh sharedMesh2 = meshFilter2.sharedMesh;
			if (sharedMesh2.vertexCount == 0)
			{
				continue;
			}
			Matrix4x4 localToWorldMatrix = meshFilter2.transform.localToWorldMatrix;
			Renderer component = meshFilter2.GetComponent<Renderer>();
			if (afterMerging != PostMerge.DestroyRenderers)
			{
				component.enabled = false;
				mDisabledRen.Add(component);
			}
			if (afterMerging == PostMerge.DisableGameObjects)
			{
				GameObject gameObject2 = meshFilter2.gameObject;
				Transform parent = gameObject2.transform;
				while (parent != mTrans)
				{
					if (parent.GetComponent<Rigidbody>() != null)
					{
						gameObject2 = parent.gameObject;
						break;
					}
					parent = parent.parent;
				}
				mDisabledGO.Add(gameObject2);
				TWTools.SetActive(gameObject2, false);
			}
			Vector3[] vertices = sharedMesh2.vertices;
			Vector3[] array9 = ((array6 != null) ? sharedMesh2.normals : null);
			Vector4[] array10 = ((array7 != null) ? sharedMesh2.tangents : null);
			Vector2[] array11 = ((array4 != null) ? sharedMesh2.uv : null);
			Vector2[] array12 = ((array5 != null) ? sharedMesh2.uv2 : null);
			Color[] array13 = ((array8 != null) ? sharedMesh2.colors : null);
			int[] triangles = sharedMesh2.triangles;
			int j = 0;
			for (int num11 = vertices.Length; j < num11; j++)
			{
				array2[num10] = worldToLocalMatrix.MultiplyPoint3x4(localToWorldMatrix.MultiplyPoint3x4(vertices[j]));
				if (array6 != null)
				{
					array6[num10] = worldToLocalMatrix.MultiplyVector(localToWorldMatrix.MultiplyVector(array9[j]));
				}
				if (array8 != null)
				{
					array8[num10] = array13[j];
				}
				if (array4 != null)
				{
					array4[num10] = array11[j];
				}
				if (array5 != null)
				{
					array5[num10] = array12[j];
				}
				if (array7 != null)
				{
					Vector4 vector = array10[j];
					Vector3 v = new Vector3(vector.x, vector.y, vector.z);
					v = worldToLocalMatrix.MultiplyVector(localToWorldMatrix.MultiplyVector(v));
					vector.x = v.x;
					vector.y = v.y;
					vector.z = v.z;
					array7[num10] = vector;
				}
				num10++;
			}
			int k = 0;
			for (int num12 = triangles.Length; k < num12; k++)
			{
				array3[num9++] = num8 + triangles[k];
			}
			num8 = num10;
			if (afterMerging == PostMerge.DestroyGameObjects)
			{
				Object.Destroy(meshFilter2.gameObject);
			}
			else if (afterMerging == PostMerge.DestroyRenderers)
			{
				Object.Destroy(component);
			}
		}
		if (afterMerging == PostMerge.DestroyGameObjects)
		{
			componentsInChildren = null;
			mDisabledGO.Clear();
		}
		if (array2.Length != 0)
		{
			if (mMesh == null)
			{
				mMesh = new Mesh();
				mMesh.hideFlags = HideFlags.DontSave;
			}
			else
			{
				mMesh.Clear();
			}
			mMesh.name = mName;
			mMesh.vertices = array2;
			mMesh.normals = array6;
			mMesh.tangents = array7;
			mMesh.colors = array8;
			mMesh.uv = array4;
			mMesh.uv2 = array5;
			mMesh.triangles = array3;
			mMesh.RecalculateBounds();
			if (mFilter == null)
			{
				mFilter = gameObject.AddComponent<MeshFilter>();
				mFilter.mesh = mMesh;
			}
			if (mRen == null)
			{
				mRen = gameObject.AddComponent<MeshRenderer>();
			}
			mRen.sharedMaterial = material;
			mRen.enabled = true;
			gameObject.name = mName + " (" + array3.Length / 3 + " tri)";
		}
		else
		{
			Release();
		}
		base.enabled = false;
	}

	public void Clear()
	{
		int i = 0;
		for (int count = mDisabledGO.Count; i < count; i++)
		{
			GameObject gameObject = mDisabledGO[i];
			if ((bool)gameObject)
			{
				TWTools.SetActive(gameObject, true);
			}
		}
		int j = 0;
		for (int count2 = mDisabledRen.Count; j < count2; j++)
		{
			Renderer renderer = mDisabledRen[j];
			if ((bool)renderer)
			{
				renderer.enabled = true;
			}
		}
		mDisabledGO.Clear();
		mDisabledRen.Clear();
		if (mRen != null)
		{
			mRen.enabled = false;
		}
	}

	public void Release()
	{
		Clear();
		TWTools.Destroy(mRen);
		TWTools.Destroy(mFilter);
		TWTools.Destroy(mMesh);
		mFilter = null;
		mMesh = null;
		mRen = null;
	}
}
