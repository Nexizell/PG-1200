using UnityEngine;
using UnityEngine.Events;

namespace Rilisoft
{
	public class UICenterOnPanelComponent : MonoBehaviour
	{
		[SerializeField]
		[ReadOnly]
		protected UIPanel _panel;

		[SerializeField]
		public Direction Direction = Direction.Horizontal;

		[SerializeField]
		public float Slack = 0.1f;

		[SerializeField]
		public UnityEvent OnCentered;

		[SerializeField]
		public UnityEvent OnCenteredLoss;

		[SerializeField]
		[ReadOnly]
		protected bool _centered;

		protected Vector2 Offset;

		public Vector3 Center
		{
			get
			{
				Vector3[] worldCorners = _panel.worldCorners;
				return (worldCorners[2] + worldCorners[0]) * 0.5f;
			}
		}

		public CenterDirection CenterDirection
		{
			get
			{
				if (!(Center.x - base.transform.position.x > 0f))
				{
					return CenterDirection.OnRight;
				}
				return CenterDirection.OnLeft;
			}
		}

		private void Awake()
		{
			_panel = NGUITools.FindInParents<UIPanel>(base.gameObject);
			OnCentered = OnCentered ?? new UnityEvent();
			OnCenteredLoss = OnCenteredLoss ?? new UnityEvent();
		}

		private void OnEnable()
		{
			_centered = false;
		}

		protected virtual void Update()
		{
			if (_panel == null)
			{
				_panel = NGUITools.FindInParents<UIPanel>(base.gameObject);
			}
			if (_panel == null)
			{
				return;
			}
			Offset = new Vector2(Mathf.Abs(Center.x - base.transform.position.x), Mathf.Abs(Center.y - base.transform.position.y));
			if (((Direction == Direction.Horizontal) ? Offset.x : Offset.y) <= Slack)
			{
				if (!_centered)
				{
					_centered = true;
					if (OnCentered != null)
					{
						OnCentered.Invoke();
					}
				}
			}
			else if (_centered)
			{
				_centered = false;
				if (OnCenteredLoss != null)
				{
					OnCenteredLoss.Invoke();
				}
			}
		}
	}
}
