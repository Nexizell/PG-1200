using System;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	[DisallowMultipleComponent]
	public sealed class SettingsGameCenterButtonController : MonoBehaviour
	{
		[SerializeField]
		protected internal GameObject _buttonsHolder;

		[SerializeField]
		protected internal GameObject _signOutButton;

		[SerializeField]
		protected internal GameObject _rateGameAndroidButton;

		[SerializeField]
		protected internal GameObject _rateGameIosButton;

		private void OnEnable()
		{
			RefreshRateAndroidVisibility(_rateGameAndroidButton);
			RefreshRateIosVisibility(_rateGameIosButton);
			RefreshGameCenterButton(_buttonsHolder);
			RefreshSignOutButton(_signOutButton);
		}

		private static void RefreshRateAndroidVisibility(GameObject rateGameAndroidButton)
		{
			if (!(rateGameAndroidButton == null))
			{
				if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
				{
					rateGameAndroidButton.SetActive(false);
				}
				else if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.GoogleLite)
				{
					rateGameAndroidButton.SetActive(false);
				}
				else
				{
					rateGameAndroidButton.SetActive(true);
				}
			}
		}

		private static void RefreshRateIosVisibility(GameObject rateGameIosButton)
		{
			if (!(rateGameIosButton == null))
			{
				if (BuildSettings.BuildTargetPlatform != RuntimePlatform.IPhonePlayer)
				{
					rateGameIosButton.SetActive(false);
				}
				else
				{
					rateGameIosButton.SetActive(true);
				}
			}
		}

		private static void RefreshGameCenterButton(GameObject buttonsHolder)
		{
			if (buttonsHolder == null)
			{
				return;
			}
			RuntimePlatform buildTargetPlatform = BuildSettings.BuildTargetPlatform;
			if (buildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				if (Application.isEditor)
				{
					buttonsHolder.SetActive(true);
					return;
				}
				if (Social.localUser == null)
				{
					buttonsHolder.SetActive(false);
					return;
				}
				bool authenticated = Social.localUser.authenticated;
				buttonsHolder.SetActive(authenticated);
			}
			else
			{
				buttonsHolder.SetActive(false);
			}
		}

		private static void RefreshSignOutButton(GameObject signOutButton)
		{
			if (!(signOutButton == null))
			{
				signOutButton.SetActive(false);
			}
		}

		public void HandleGameCenterButton()
		{
			ButtonClickSound.Instance.PlayClick();
			if (Application.isEditor)
			{
				Debug.Log("[Game Center] pressed.");
				return;
			}
			switch (BuildSettings.BuildTargetPlatform)
			{
			case RuntimePlatform.Android:
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					if (GameCircleSocial.Instance.localUser == null || !GameCircleSocial.Instance.localUser.authenticated)
					{
						AGSClient.ShowSignInPage();
					}
					else
					{
						AGSAchievementsClient.ShowAchievementsOverlay();
					}
				}
				break;
			case RuntimePlatform.IPhonePlayer:
			{
				GameCenterSingleton instance = GameCenterSingleton.Instance;
				if (instance.IsUserAuthenticated())
				{
					instance.ShowLeaderboardUI();
				}
				else
				{
					instance.updateGameCenter();
				}
				break;
			}
			}
		}

		public void HandleSignOutGameCenterButton()
		{
			ButtonClickSound.Instance.PlayClick();
			if (Application.isEditor)
			{
				Debug.Log("[Sign Out] pressed.");
			}
		}
	}
}
