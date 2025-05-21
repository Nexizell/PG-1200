namespace Edelweiss.DecalSystem
{
	public abstract class GenericDecalProjector<D, P, DM> : GenericDecalProjectorBase where D : GenericDecals<D, P, DM> where P : GenericDecalProjector<D, P, DM> where DM : GenericDecalsMesh<D, P, DM>
	{
		private DM m_DecalsMesh;

		public DM DecalsMesh
		{
			get
			{
				return m_DecalsMesh;
			}
			internal set
			{
				m_DecalsMesh = value;
			}
		}

		internal void ResetDecalsMesh()
		{
			DecalsMesh = (DM)null;
			base.IsActiveProjector = false;
			base.DecalsMeshLowerVertexIndex = 0;
			base.DecalsMeshUpperVertexIndex = 0;
			base.IsUV1ProjectionCalculated = false;
			base.IsUV2ProjectionCalculated = false;
			base.IsTangentProjectionCalculated = false;
		}
	}
}
