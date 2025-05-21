using System;
using Facebook.Unity;
using Rilisoft;
using UnityEngine;

public sealed class MainMenuFreePanel : MonoBehaviour
{
	[SerializeField]
	protected internal GameObject _postNewsLabel;

	[SerializeField]
	protected internal GameObject _starParticleSocialGunButton;

	[SerializeField]
	protected internal GameObject _socialGunPanel;

	[SerializeField]
	protected internal ButtonHandler _youtubeButton;

	[SerializeField]
	protected internal ButtonHandler _postFacebookButton;

	[SerializeField]
	protected internal ButtonHandler _postTwitterButton;

	[SerializeField]
	protected internal ButtonHandler _rateUsButton;

	[SerializeField]
	protected internal ButtonHandler _backButton;

	[SerializeField]
	protected internal ButtonHandler _twitterSubcribeButton;

	[SerializeField]
	protected internal ButtonHandler _facebookSubcribeButton;

	[SerializeField]
	protected internal ButtonHandler _instagramSubcribeButton;

	[SerializeField]
	protected internal UILabel _socialGunEventTimerLabel;

	private void Start()
	{
		if (_socialGunPanel != null)
		{
			_socialGunPanel.SetActive(FacebookController.FacebookSupported);
		}
		_postNewsLabel.SetActive(false);
		if (_youtubeButton != null)
		{
			_youtubeButton.Clicked += HandleYoutubeClicked;
		}
		if (_postFacebookButton != null)
		{
			_postFacebookButton.Clicked += HandlePostFacebookClicked;
		}
		if (_postTwitterButton != null)
		{
			_postTwitterButton.Clicked += HandlePostTwittwerClicked;
		}
		if (_rateUsButton != null)
		{
			_rateUsButton.Clicked += HandleRateAsClicked;
		}
		if (_twitterSubcribeButton != null)
		{
			_twitterSubcribeButton.Clicked += HandleTwitterSubscribeClicked;
		}
		if (_facebookSubcribeButton != null)
		{
			_facebookSubcribeButton.Clicked += HandleFacebookSubscribeClicked;
		}
		if (_instagramSubcribeButton != null)
		{
			_instagramSubcribeButton.Clicked += HandleInstagramSubscribeClicked;
		}
		if (_backButton != null)
		{
			_backButton.Clicked += delegate
			{
				MainMenuController.sharedController._isCancellationRequested = true;
			};
		}
		FacebookController.SocialGunEventStateChanged += HandleSocialGunEventStateChanged;
		if (FacebookController.sharedController != null)
		{
			HandleSocialGunEventStateChanged(FacebookController.sharedController.SocialGunEventActive);
		}
	}

	private void Update()
	{
		bool flag = (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled) && !ShopNGUIController.GuiActive;
		if (_starParticleSocialGunButton != null && _starParticleSocialGunButton.activeInHierarchy != flag)
		{
			_starParticleSocialGunButton.SetActive(flag);
		}
		if (_postFacebookButton.gameObject.activeSelf != (FacebookController.FacebookSupported && FacebookController.FacebookPost_Old_Supported && FB.IsLoggedIn))
		{
			_postFacebookButton.gameObject.SetActive(FacebookController.FacebookSupported && FacebookController.FacebookPost_Old_Supported && FB.IsLoggedIn);
		}
		if (_postTwitterButton.gameObject.activeSelf != (TwitterController.TwitterSupported && TwitterController.TwitterSupported_OldPosts && TwitterController.IsLoggedIn))
		{
			_postTwitterButton.gameObject.SetActive(TwitterController.TwitterSupported && TwitterController.TwitterSupported_OldPosts && TwitterController.IsLoggedIn);
		}
		if (FacebookController.sharedController != null && FacebookController.sharedController.SocialGunEventActive)
		{
			_socialGunEventTimerLabel.text = string.Empty;
		}
	}

	private void OnDestroy()
	{
		FacebookController.SocialGunEventStateChanged -= HandleSocialGunEventStateChanged;
	}

	public void SetVisible(bool visible)
	{
		if (base.gameObject.activeSelf != visible)
		{
			base.gameObject.SetActive(visible);
		}
	}

	public void OnSocialGunButtonClicked()
	{
		MainMenuController.sharedController.OnSocialGunEventButtonClick();
	}

	private void HandleSocialGunEventStateChanged(bool enable)
	{
		_socialGunPanel.gameObject.SetActive(enable);
		GetComponentsInChildren<RewardedLikeButton>(true).ForEach(delegate(RewardedLikeButton b)
		{
			b.gameObject.SetActive(!enable);
		});
		if (FacebookController.sharedController != null)
		{
			_socialGunEventTimerLabel.text = string.Empty;
		}
	}

	private void HandleYoutubeClicked(object sender, EventArgs e)
	{
		if (!MainMenuController.ShopOpened)
		{
			if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
			{
				TrainingController.TrainingCompletedFlagForLogging = null;
			}
			ButtonClickSound.Instance.PlayClick();
			MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
			Application.OpenURL("http://www.youtube.com/channel/UCsClw1gnMrmF6ssIB_166_Q");
		}
	}

	private void HandlePostFacebookClicked(object sender, EventArgs e)
	{
		if (!MainMenuController.ShopOpened && !MainMenuController.ShowBannerOrLevelup())
		{
			if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
			{
				TrainingController.TrainingCompletedFlagForLogging = null;
			}
			ButtonClickSound.Instance.PlayClick();
			MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
			FacebookController.ShowPostDialog();
		}
	}

	private void HandlePostTwittwerClicked(object sender, EventArgs e)
	{
		if (MainMenuController.ShopOpened || MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		if (!Application.isEditor)
		{
			MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
			if (TwitterController.Instance != null)
			{
				TwitterController.Instance.PostStatusUpdate("Come and play with me in epic multiplayer shooter - Pixel Gun 3D! http://goo.gl/dQMf4n");
			}
		}
	}

	private void HandleRateAsClicked(object sender, EventArgs e)
	{
		if (MainMenuController.sharedController != null)
		{
			MainMenuController.sharedController.RateUs();
		}
	}

	private void HandleTwitterSubscribeClicked(object sender, EventArgs e)
	{
		if (!MainMenuController.ShopOpened)
		{
			if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
			{
				TrainingController.TrainingCompletedFlagForLogging = null;
			}
			ButtonClickSound.Instance.PlayClick();
			MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
			Application.OpenURL("https://twitter.com/PixelGun3D");
		}
	}

	private void HandleFacebookSubscribeClicked(object sender, EventArgs e)
	{
		if (!MainMenuController.ShopOpened)
		{
			if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
			{
				TrainingController.TrainingCompletedFlagForLogging = null;
			}
			ButtonClickSound.Instance.PlayClick();
			MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
			Application.OpenURL("http://pixelgun3d.com/facebook.html");
		}
	}

	private void HandleInstagramSubscribeClicked(object sender, EventArgs e)
	{
		if (!MainMenuController.ShopOpened)
		{
			if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
			{
				TrainingController.TrainingCompletedFlagForLogging = null;
			}
			ButtonClickSound.Instance.PlayClick();
			MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
			Application.OpenURL("http://www.instagram.com/pixelgun3d_official");
		}
	}
}
