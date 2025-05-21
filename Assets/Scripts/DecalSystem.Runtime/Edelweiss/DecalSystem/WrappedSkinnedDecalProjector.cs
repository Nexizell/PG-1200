using UnityEngine;

namespace Edelweiss.DecalSystem
{
	public class WrappedSkinnedDecalProjector : SkinnedDecalProjectorBase
	{
		private Transform m_Transform;

		private SkinnedDecalProjectorComponent m_DecalProjector;

		public SkinnedDecalProjectorComponent WrappedSkinnedDecalProjectorComponent
		{
			get
			{
				return m_DecalProjector;
			}
		}

		public override Vector3 Position
		{
			get
			{
				return m_Transform.position;
			}
		}

		public override Quaternion Rotation
		{
			get
			{
				return m_Transform.rotation;
			}
		}

		public override Vector3 Scale
		{
			get
			{
				return m_Transform.localScale;
			}
		}

		public override float CullingAngle
		{
			get
			{
				return m_DecalProjector.cullingAngle;
			}
		}

		public override float MeshOffset
		{
			get
			{
				return m_DecalProjector.meshOffset;
			}
		}

		public override int UV1RectangleIndex
		{
			get
			{
				return m_DecalProjector.uv1RectangleIndex;
			}
		}

		public override int UV2RectangleIndex
		{
			get
			{
				return m_DecalProjector.uv2RectangleIndex;
			}
		}

		public WrappedSkinnedDecalProjector(SkinnedDecalProjectorComponent a_DecalProjector)
		{
			m_DecalProjector = a_DecalProjector;
			m_Transform = m_DecalProjector.transform;
		}
	}
}
