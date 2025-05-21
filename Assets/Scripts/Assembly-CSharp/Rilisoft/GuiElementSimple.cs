using UnityEngine;

namespace Rilisoft
{
	public class GuiElementSimple : GuiElementBase
	{
		[SerializeField]
		protected internal PrefabHandler _prefabHandler = new PrefabHandler();

		public GameObject View { get; private set; }

		public override bool IsVisible
		{
			get
			{
				if (View != null)
				{
					return View.gameObject.activeInHierarchy;
				}
				return false;
			}
		}

		[ContextMenu("UpdateView")]
		public void UpdateView()
		{
			if (View == null && base.gameObject.transform.childCount > 0)
			{
				View = base.gameObject.transform.GetChild(0).gameObject;
			}
			if (View == null)
			{
				GameObject gameObject = Object.Instantiate(_prefabHandler.Prefab);
				gameObject.SetLayerRecursively(base.gameObject.layer);
				gameObject.transform.SetParent(base.transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				View = gameObject;
			}
		}

		protected override void WhenPush()
		{
			UpdateView();
			View.gameObject.SetActiveSafe(true);
		}

		protected override void WhenPop()
		{
			if (View != null)
			{
				View.gameObject.gameObject.SetActive(false);
			}
		}

		protected override void OnEnable()
		{
			PushRequest();
		}

		protected override void OnDisable()
		{
			if (base.IsTopInStack)
			{
				PopRequest();
			}
		}
	}
}
