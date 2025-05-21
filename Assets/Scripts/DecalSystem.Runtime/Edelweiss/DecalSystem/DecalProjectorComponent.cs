namespace Edelweiss.DecalSystem
{
	public class DecalProjectorComponent : GenericDecalProjectorComponent<Decals, DecalProjectorBase, DecalsMesh>
	{
		public bool affectMeshes = true;

		public bool affectTerrains = true;
	}
}
