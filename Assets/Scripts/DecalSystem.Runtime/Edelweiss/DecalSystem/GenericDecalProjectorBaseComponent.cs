using UnityEngine;

namespace Edelweiss.DecalSystem
{
	public abstract class GenericDecalProjectorBaseComponent : MonoBehaviour
	{
		private Transform m_CachedTransform;

		public LayerMask affectedLayers = -1;

		public bool affectInactiveRenderers;

		public bool affectOtherDecals;

		public float cullingAngle = 180f;

		public float meshOffset;

		public int uv1RectangleIndex;

		public int uv2RectangleIndex;

		public Transform CachedTransform
		{
			get
			{
				if (m_CachedTransform == null)
				{
					m_CachedTransform = base.transform;
				}
				return m_CachedTransform;
			}
		}

		private void OnEnable()
		{
			m_CachedTransform = GetComponent<Transform>();
		}

		public GenericDecalsBase GetDecalsBase()
		{
			GenericDecalsBase genericDecalsBase = null;
			Transform parent = base.transform;
			while (parent != null && genericDecalsBase == null)
			{
				genericDecalsBase = parent.GetComponent<GenericDecalsBase>();
				parent = parent.parent;
			}
			return genericDecalsBase;
		}

		public Bounds WorldBounds()
		{
			Matrix4x4 matrix4x = UnscaledLocalToWorldMatrix();
			Vector3 vector = 0.5f * CachedTransform.localScale;
			Vector3 vector2 = new Vector3(0f, 0f - Mathf.Abs(vector.y), 0f);
			Vector3 center = matrix4x.MultiplyPoint3x4(Vector3.zero);
			Bounds result = new Bounds(center, Vector3.zero);
			center = vector2 + new Vector3(vector.x, vector.y, vector.z);
			center = matrix4x.MultiplyPoint3x4(center);
			result.Encapsulate(center);
			center = vector2 + new Vector3(vector.x, vector.y, 0f - vector.z);
			center = matrix4x.MultiplyPoint3x4(center);
			result.Encapsulate(center);
			center = vector2 + new Vector3(vector.x, 0f - vector.y, vector.z);
			center = matrix4x.MultiplyPoint3x4(center);
			result.Encapsulate(center);
			center = vector2 + new Vector3(vector.x, 0f - vector.y, 0f - vector.z);
			center = matrix4x.MultiplyPoint3x4(center);
			result.Encapsulate(center);
			center = vector2 + new Vector3(0f - vector.x, vector.y, vector.z);
			center = matrix4x.MultiplyPoint3x4(center);
			result.Encapsulate(center);
			center = vector2 + new Vector3(0f - vector.x, vector.y, 0f - vector.z);
			center = matrix4x.MultiplyPoint3x4(center);
			result.Encapsulate(center);
			center = vector2 + new Vector3(0f - vector.x, 0f - vector.y, vector.z);
			center = matrix4x.MultiplyPoint3x4(center);
			result.Encapsulate(center);
			center = vector2 + new Vector3(0f - vector.x, 0f - vector.y, 0f - vector.z);
			center = matrix4x.MultiplyPoint3x4(center);
			result.Encapsulate(center);
			return result;
		}

		private Matrix4x4 UnscaledLocalToWorldMatrix()
		{
			return Matrix4x4.TRS(CachedTransform.position, CachedTransform.rotation, Vector3.one);
		}

		public Matrix4x4 ProjectorToWorldMatrix()
		{
			return Matrix4x4.TRS(CachedTransform.position, CachedTransform.rotation, CachedTransform.localScale);
		}
	}
}
