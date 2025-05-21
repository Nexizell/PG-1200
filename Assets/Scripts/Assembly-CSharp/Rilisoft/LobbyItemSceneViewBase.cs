using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public abstract class LobbyItemSceneViewBase : MonoBehaviour, ILobbyItemView
	{
		[CompilerGenerated]
		internal sealed class _003CSelfUpdateCoroutine_003Ed__18 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public LobbyItemSceneViewBase _003C_003E4__this;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return _003C_003E2__current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return _003C_003E2__current;
				}
			}

			[DebuggerHidden]
			public _003CSelfUpdateCoroutine_003Ed__18(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				int num = _003C_003E1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -1;
				}
				else
				{
					_003C_003E1__state = -1;
				}
				if (_003C_003E4__this._item != null)
				{
					_003C_003E4__this.UpdateCraftingState();
					_003C_003E4__this.OnSelfUpdate();
				}
				_003C_003E2__current = new WaitForSeconds(0.2f);
				_003C_003E1__state = 1;
				return true;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		private static string CRAFT_MATERIAL_FULL_NAME = "Materials\\BuildMaterial";

		[Tooltip("Объекты которые нужно скрывать при крафте")]
		[SerializeField]
		protected internal List<GameObject> _disableIfCrafting;

		protected LobbyCraftController _controller;

		private LobbyItem _item;

		private bool _isSelected;

		protected Dictionary<Renderer, Material[]> _viewMaterials = new Dictionary<Renderer, Material[]>();

		protected abstract GameObject ViewObjectGetter { get; }

		protected bool? _isCrafting { get; private set; }

		protected List<Renderer> _renderers { get; private set; }

		public LobbyItem LobbyItem
		{
			get
			{
				return _item;
			}
		}

		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				_isSelected = value;
				UpdateView();
			}
		}

		private void OnEnable()
		{
			StartCoroutine(SelfUpdateCoroutine());
		}

		private void OnDisable()
		{
			StopCoroutine(SelfUpdateCoroutine());
		}

		private IEnumerator SelfUpdateCoroutine()
		{
			while (true)
			{
				if (_item != null)
				{
					UpdateCraftingState();
					OnSelfUpdate();
				}
				yield return new WaitForSeconds(0.2f);
			}
		}

		protected void UpdateCraftingState()
		{
			if (_isCrafting.HasValue && _isCrafting == _item.IsCrafting)
			{
				return;
			}
			_isCrafting = _item.IsCrafting;
			if (_item.Slot == LobbyItemInfo.LobbyItemSlot.terrain || _item.Slot == LobbyItemInfo.LobbyItemSlot.skybox)
			{
				return;
			}
			if (_item.IsCrafting)
			{
				if (_renderers != null && _renderers.Any())
				{
					Material material = Resources.Load<Material>(CRAFT_MATERIAL_FULL_NAME);
					foreach (Renderer renderer in _renderers)
					{
						Material[] array = new Material[renderer.sharedMaterials.Length];
						for (int i = 0; i < array.Length; i++)
						{
							array[i] = material;
						}
						renderer.sharedMaterials = array;
					}
				}
				{
					foreach (GameObject item in _disableIfCrafting)
					{
						if (item != null)
						{
							item.SetActive(false);
						}
					}
					return;
				}
			}
			if (_renderers != null && _renderers.Any())
			{
				foreach (Renderer renderer2 in _renderers)
				{
					if (_viewMaterials.ContainsKey(renderer2))
					{
						renderer2.sharedMaterials = _viewMaterials[renderer2];
					}
				}
			}
			foreach (GameObject item2 in _disableIfCrafting)
			{
				if (item2 != null)
				{
					item2.SetActive(true);
				}
			}
		}

		public virtual void Setup(Transform root, LobbyCraftController controller, LobbyItem item)
		{
			_controller = controller;
			_item = item;
			base.gameObject.transform.SetParent(root);
			base.gameObject.transform.localPosition = Vector3.zero;
			base.gameObject.transform.localScale = Vector3.one;
			base.gameObject.transform.localRotation = Quaternion.identity;
			Collider[] componentsInChildren = base.gameObject.GetComponentsInChildren<Collider>(true);
			foreach (Collider collider in componentsInChildren)
			{
				if (!collider.isTrigger)
				{
					ButtonEventsHandler orAddComponent = collider.gameObject.GetOrAddComponent<ButtonEventsHandler>();
					orAddComponent.OnClickEvent.RemoveListener(OnClicked);
					orAddComponent.OnClickEvent.AddListener(OnClicked);
				}
			}
			_renderers = (from r in ViewObjectGetter.GetComponentsInChildren<Renderer>(true)
				where r.GetType() != typeof(ParticleSystemRenderer)
				select r).ToList();
			_viewMaterials.Clear();
			foreach (Renderer renderer in _renderers)
			{
				_viewMaterials.Add(renderer, renderer.sharedMaterials.ToArray());
			}
		}

		public virtual void Hide()
		{
		}

		public virtual void UpdateView()
		{
		}

		public virtual void Kill()
		{
			_isCrafting = null;
		}

		protected virtual void OnClicked()
		{
		}

		protected virtual void OnSelfUpdate()
		{
		}

		protected virtual void UnloadUnusedStuff()
		{
			if (_renderers == null)
			{
				return;
			}
			foreach (Renderer renderer in _renderers)
			{
				if (renderer == null || renderer.sharedMaterials == null)
				{
					continue;
				}
				Material[] sharedMaterials = renderer.sharedMaterials;
				foreach (Material material in sharedMaterials)
				{
					if (!(material == null) && !material.name.Contains("(Instance)"))
					{
						if (material.mainTexture != null && (material.mainTexture.name == null || (!material.mainTexture.name.StartsWith("Particle_Stack") && !material.mainTexture.name.ToLowerInvariant().Contains("water") && material.mainTexture.name != "decor_big_space_2" && material.mainTexture.name != "GlossTexture2" && material.mainTexture.name != "Castle_grass" && material.mainTexture.name != "Menu_Pers_shadow" && !material.mainTexture.name.ToLower().Contains("glass"))))
						{
							Resources.UnloadAsset(material.mainTexture);
						}
						Resources.UnloadAsset(material);
					}
				}
			}
		}

		public LobbyItemSceneViewBase()
		{
		}
	}
}
