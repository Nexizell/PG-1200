namespace Edelweiss.DecalSystem
{
	public abstract class SkinnedDecalProjectorBase : GenericDecalProjector<SkinnedDecals, SkinnedDecalProjectorBase, SkinnedDecalsMesh>
	{
		private int m_DecalsMeshLowerBonesIndex;

		private int m_DecalsMeshUpperBonesIndex;

		public int DecalsMeshLowerBonesIndex
		{
			get
			{
				return m_DecalsMeshLowerBonesIndex;
			}
			internal set
			{
				m_DecalsMeshLowerBonesIndex = value;
			}
		}

		public int DecalsMeshUpperBonesIndex
		{
			get
			{
				return m_DecalsMeshUpperBonesIndex;
			}
			internal set
			{
				m_DecalsMeshUpperBonesIndex = value;
			}
		}

		public int DecalsMeshBonesCount
		{
			get
			{
				return DecalsMeshUpperBonesIndex - DecalsMeshLowerBonesIndex + 1;
			}
		}
	}
}
