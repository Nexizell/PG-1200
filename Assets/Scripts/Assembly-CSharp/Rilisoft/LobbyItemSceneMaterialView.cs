using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	public class LobbyItemSceneMaterialView : LobbyItemSceneViewBase, ILobbyItemView
	{
		private Material[] _baseMaterials;

		protected override GameObject ViewObjectGetter
		{
			get
			{
				return base.gameObject.transform.parent.GetChild(0).gameObject;
			}
		}

		public override void Setup(Transform root, LobbyCraftController controller, LobbyItem item)
		{
			base.Setup(root, controller, item);
			GameObject gameObject2 = root.GetChild(0).gameObject;
			if (base._renderers != null && base._renderers.Any())
			{
				_baseMaterials = base._renderers[0].sharedMaterials.ToArray();
				_viewMaterials[base._renderers[0]] = base.LobbyItem.Info.Materials.Select((string m) => Resources.Load<Material>(m)).ToArray();
				base._renderers[0].sharedMaterials = _viewMaterials[base._renderers[0]];
			}
		}

		public override void Hide()
		{
			if (base._renderers != null && base._renderers.Any())
			{
				base._renderers[0].sharedMaterials = _baseMaterials;
			}
		}

		public override void UpdateView()
		{
			UpdateCraftingState();
		}

		public override void Kill()
		{
			Object.Destroy(base.gameObject);
		}
	}
}
