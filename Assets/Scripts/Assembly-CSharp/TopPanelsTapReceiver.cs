using System;
using UnityEngine;

public class TopPanelsTapReceiver : MonoBehaviour
{
	public static event Action OnClicked;

	private void Start()
	{
		base.gameObject.SetActive(Defs.isMulti);
	}

	private void OnClick()
	{
		ButtonClickSound.TryPlayClick();
		TopPanelsTapReceiver.OnClicked();
	}

	static TopPanelsTapReceiver()
	{
		TopPanelsTapReceiver.OnClicked = delegate
		{
		};
	}
}
