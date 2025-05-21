using System;
using System.Linq;
using Rilisoft.NullExtensions;
using UnityEngine;

public class EventX3Banner : BannerWindow
{
	public GameObject amazonEventObject;

	public UILabel amazonEventCaptionLabel;

	public UILabel amazonEventTitleLabel;

	private void OnEnable()
	{
		bool isAmazonEventX3Active = PromoActionsManager.sharedManager.IsAmazonEventX3Active;
		amazonEventObject.SetActive(isAmazonEventX3Active);
		PromoActionsManager.EventAmazonX3Updated += OnAmazonEventUpdated;
		RefreshAmazonBonus();
	}

	private void OnDisable()
	{
		PromoActionsManager.EventAmazonX3Updated -= OnAmazonEventUpdated;
	}

	private void RefreshAmazonBonus()
	{
		UILabel[] componentsInChildren = amazonEventObject.GetComponentsInChildren<UILabel>();
		UILabel uILabel = amazonEventCaptionLabel ?? componentsInChildren.FirstOrDefault((UILabel l) => "CaptionLabel".Equals(l.name, StringComparison.OrdinalIgnoreCase));
		PromoActionsManager.AmazonEventInfo o = PromoActionsManager.sharedManager.Map((PromoActionsManager p) => p.AmazonEvent);
		if (uILabel != null)
		{
			uILabel.text = o.Map((PromoActionsManager.AmazonEventInfo e) => e.Caption) ?? string.Empty;
		}
		UILabel[] obj = (amazonEventTitleLabel ?? componentsInChildren.FirstOrDefault((UILabel l) => "TitleLabel".Equals(l.name, StringComparison.OrdinalIgnoreCase))).Map((UILabel t) => t.GetComponentsInChildren<UILabel>()) ?? new UILabel[0];
		float num = o.Map((PromoActionsManager.AmazonEventInfo e) => e.Percentage);
		string text = LocalizationStore.Get("Key_1672");
		UILabel[] array = obj;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].text = ((!"Key_1672".Equals(text, StringComparison.OrdinalIgnoreCase)) ? string.Format(text, new object[1] { num }) : string.Empty);
		}
	}

	private void OnAmazonEventUpdated()
	{
		amazonEventObject.SetActive(PromoActionsManager.sharedManager.IsAmazonEventX3Active);
		RefreshAmazonBonus();
	}
}
