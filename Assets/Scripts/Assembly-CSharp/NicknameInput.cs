using System;
using Rilisoft;
using UnityEngine;

public sealed class NicknameInput : MonoBehaviour
{
	public UIInput input;

	private UIButton _okButton;

	private const string PlayerNameKey = "NamePlayer";

	private void HandleOkClicked(object sender, EventArgs e)
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		PlayerPrefs.SetString("NicknameRequested", "1");
		if (input != null)
		{
			if (input.value != null)
			{
				string text = input.value.Trim();
				string value = (string.IsNullOrEmpty(text) ? "Unnamed" : text);
				PlayerPrefs.SetString("NamePlayer", value);
				input.value = value;
			}
			if (_okButton != null)
			{
				_okButton.isEnabled = false;
			}
		}
		Singleton<SceneLoader>.Instance.LoadScene(Defs.MainMenuScene);
	}

	private void Start()
	{
		ButtonHandler componentInChildren = base.gameObject.GetComponentInChildren<ButtonHandler>();
		if (componentInChildren != null)
		{
			componentInChildren.Clicked += HandleOkClicked;
			_okButton = componentInChildren.GetComponent<UIButton>();
		}
		if (ExperienceController.sharedController != null && ExpController.Instance != null)
		{
			ExperienceController.sharedController.isShowRanks = false;
			ExpController.Instance.InterfaceEnabled = false;
		}
		if (input != null)
		{
			string playerNameOrDefault = ProfileController.GetPlayerNameOrDefault();
			input.value = playerNameOrDefault;
		}
	}
}
