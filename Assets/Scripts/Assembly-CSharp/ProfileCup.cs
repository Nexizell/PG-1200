using Rilisoft;
using UnityEngine;

[RequireComponent(typeof(UICenterOnPanelComponent))]
[RequireComponent(typeof(UISprite))]
public class ProfileCup : MonoBehaviour
{
	private UISprite _cup;

	public RatingSystem.RatingLeague League;

	public GameObject Outline;

	public GameObject LockedObject;

	private LeaguesGUIController _controller;

	private UICenterOnPanelComponent _centerMonitor;

	public UISprite Cup
	{
		get
		{
			return _cup ?? (_cup = GetComponent<UISprite>());
		}
	}

	private void Start()
	{
		_controller = base.gameObject.GetComponentInParents<LeaguesGUIController>();
		_centerMonitor = GetComponent<UICenterOnPanelComponent>();
		_centerMonitor.OnCentered.RemoveListener(OnCentered);
		_centerMonitor.OnCentered.AddListener(OnCentered);
	}

	private void OnCentered()
	{
		_controller.CupCentered(this);
	}

	private void OnEnable()
	{
		Outline.SetActive(League == RatingSystem.instance.currentLeague);
		bool active = League > RatingSystem.instance.currentLeague;
		LockedObject.SetActive(active);
		Cup.spriteName = string.Format("{0} {1}", new object[2] { League, 1 });
	}
}
