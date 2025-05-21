namespace Edelweiss.DecalSystem
{
	public abstract class GenericDecals<D, P, DM> : GenericDecalsBase where D : GenericDecals<D, P, DM> where P : GenericDecalProjector<D, P, DM> where DM : GenericDecalsMesh<D, P, DM>
	{
	}
}
