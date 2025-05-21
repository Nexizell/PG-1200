using System;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	public class LobbyItemSceneView : LobbyItemSceneViewBase, ILobbyItemView
	{
		private string[] m_primitiveMeshNames;

		protected override GameObject ViewObjectGetter
		{
			get
			{
				return base.gameObject;
			}
		}

		private string[] PrimitiveMeshesNames
		{
			get
			{
				if (m_primitiveMeshNames == null)
				{
					m_primitiveMeshNames = Enum.GetNames(typeof(PrimitiveType)) ?? new string[0];
				}
				return m_primitiveMeshNames;
			}
		}

		public override void Setup(Transform root, LobbyCraftController controller, LobbyItem item)
		{
			base.Setup(root, controller, item);
			base.gameObject.SetActive(true);
		}

		public override void UpdateView()
		{
			base.gameObject.SetActive(true);
			UpdateCraftingState();
		}

		public override void Hide()
		{
			base.gameObject.SetActive(false);
		}

		public override void Kill()
		{
			UnloadUnusedStuff();
			base.gameObject.SetActive(false);
			base.gameObject.transform.SetParent(null);
			UnityEngine.Object.Destroy(base.gameObject);
		}

		protected override void OnClicked()
		{
			_controller.SelectItemOnSceneRequest(base.LobbyItem);
		}

		protected override void UnloadUnusedStuff()
		{
			try
			{
				if (base._renderers == null)
				{
					return;
				}
				Func<Mesh, bool> func = (Mesh mesh) => mesh != null && mesh.name != null && PrimitiveMeshesNames.Contains(mesh.name);
				foreach (Renderer renderer in base._renderers)
				{
					if (renderer == null)
					{
						continue;
					}
					MeshFilter component = renderer.GetComponent<MeshFilter>();
					if (component != null && component.sharedMesh != null && !func(component.sharedMesh))
					{
						Resources.UnloadAsset(component.sharedMesh);
					}
					if (renderer is SkinnedMeshRenderer)
					{
						SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
						if (skinnedMeshRenderer.sharedMesh != null && !func(skinnedMeshRenderer.sharedMesh))
						{
							Resources.UnloadAsset(skinnedMeshRenderer.sharedMesh);
						}
					}
				}
				base.UnloadUnusedStuff();
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in UnloadUnusedStuff: {0}", ex);
			}
		}
	}
}
