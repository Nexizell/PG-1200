using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Facebook.Unity;
using Rilisoft;
using UnityEngine;

public sealed class RewardedLikeButton : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CStart_003Ed__2 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public RewardedLikeButton _003C_003E4__this;

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
		public _003CStart_003Ed__2(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				LocalizationStore.AddEventCallAfterLocalize(_003C_003E4__this.RefreshRewardedLikeButton);
				_003C_003E4__this.RefreshRewardedLikeButton();
				goto IL_005c;
			case 1:
				_003C_003E1__state = -1;
				goto IL_005c;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_005c:
				if (MainMenuController.sharedController == null)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E4__this.Refresh();
				if (FB.IsLoggedIn)
				{
					return false;
				}
				break;
			}
			if (!FB.IsLoggedIn)
			{
				_003C_003E2__current = new WaitForSeconds(1f);
				_003C_003E1__state = 2;
				return true;
			}
			_003C_003E4__this.Refresh();
			return false;
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

	public UIButton rewardedLikeButton;

	public UILabel rewardedLikeCaption;

	private const int RewardGemsCount = 10;

	internal const string RewardKey = "RewardForLikeGained";

	private IEnumerator Start()
	{
		LocalizationStore.AddEventCallAfterLocalize(RefreshRewardedLikeButton);
		RefreshRewardedLikeButton();
		while (MainMenuController.sharedController == null)
		{
			yield return null;
		}
		Refresh();
		if (!FB.IsLoggedIn)
		{
			while (!FB.IsLoggedIn)
			{
				yield return new WaitForSeconds(1f);
			}
			Refresh();
		}
	}

	private void OnEnable()
	{
		Refresh();
	}

	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(RefreshRewardedLikeButton);
	}

	public void OnClick()
	{
		Application.OpenURL("https://www.facebook.com/PixelGun3DOfficial");
		try
		{
			TutorialQuestManager.Instance.AddFulfilledQuest("likeFacebook");
			QuestMediator.NotifySocialInteraction("likeFacebook");
			if (Storager.getInt("RewardForLikeGained") <= 0)
			{
				Storager.setInt("RewardForLikeGained", 1);
				int @int = Storager.getInt("GemsCurrency");
				Storager.setInt("GemsCurrency", @int + 10);
				AnalyticsFacade.CurrencyAccrual(10, "GemsCurrency");
				AnalyticsFacade.SendCustomEvent("Virality", new Dictionary<string, object> { { "Like Facebook Page", "Likes" } });
				CoinsMessage.FireCoinsAddedEvent(true);
			}
		}
		finally
		{
			Refresh();
		}
	}

	private void RefreshRewardedLikeButton()
	{
		if (rewardedLikeCaption == null)
		{
			UnityEngine.Debug.LogError("rewardedLikeCaption == null");
			return;
		}
		try
		{
			string format = LocalizationStore.Get("Key_1653");
			rewardedLikeCaption.text = string.Format(format, new object[1] { 10 });
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}

	internal void Refresh()
	{
		if (!FacebookController.FacebookSupported)
		{
			UnityEngine.Object.Destroy(this);
		}
		else if (rewardedLikeButton == null)
		{
			UnityEngine.Object.Destroy(this);
		}
		else if (Storager.hasKey("RewardForLikeGained") && Storager.getInt("RewardForLikeGained") > 0)
		{
			UnityEngine.Object.Destroy(rewardedLikeButton.gameObject);
			UnityEngine.Object.Destroy(this);
		}
		else if (!FB.IsLoggedIn)
		{
			rewardedLikeButton.gameObject.SetActive(false);
		}
		else if (!Storager.hasKey(Defs.IsFacebookLoginRewardaGained) || Storager.getInt(Defs.IsFacebookLoginRewardaGained) == 0)
		{
			rewardedLikeButton.gameObject.SetActive(false);
		}
		else if (ExpController.LobbyLevel <= 1)
		{
			rewardedLikeButton.gameObject.SetActive(false);
		}
		else if (MainMenuController.SavedShwonLobbyLevelIsLessThanActual())
		{
			rewardedLikeButton.gameObject.SetActive(false);
		}
		else
		{
			RefreshRewardedLikeButton();
			rewardedLikeButton.gameObject.SetActive(true);
		}
	}
}
