using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public class ReviewHUDWindow : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CCrt_OnShowThanks_003Ed__33 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ReviewHUDWindow _003C_003E4__this;

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
		public _003CCrt_OnShowThanks_003Ed__33(int _003C_003E1__state)
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
				_003C_003E2__current = new WaitForEndOfFrame();
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if (_003C_003E4__this.NeedShowThanks)
				{
					_003C_003E4__this.NeedShowThanks = false;
					if (_003C_003E4__this.objThanks != null)
					{
						_003C_003E4__this.objThanks.SetActive(true);
					}
					_003C_003E4__this.AddBackSubscription();
					_003C_003E2__current = new WaitForSeconds(3f);
					_003C_003E1__state = 2;
					return true;
				}
				break;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E4__this.OnCloseThanks();
				break;
			}
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

	private static ReviewHUDWindow _instance;

	[Header("Добавить все звезды в список в их порядке активации при нажатии")]
	public StarReview[] arrStarByOrder;

	[Header("Окна")]
	public GameObject objWindowRating;

	public GameObject objWindowGoToStore;

	public GameObject objWindowEnterMsg;

	public GameObject objThanks;

	[Header("Другое")]
	public UIInput inputMsg;

	public UIButton btnSendMsg;

	public UILabel lbTitle5Stars;

	public static bool isShow;

	private bool _NeedShowThanks;

	public int countStarForReview;

	private bool isInputMsgForReview;

	private IDisposable _backSubscription;

	public static ReviewHUDWindow Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = InfoWindowController.Instance.gameObject.GetComponentInChildren<ReviewHUDWindow>();
				return _instance;
			}
			return _instance;
		}
	}

	public string TitleTextTranslate
	{
		get
		{
			return "";
		}
	}

	public bool NeedShowThanks
	{
		get
		{
			return _NeedShowThanks;
		}
		set
		{
			_NeedShowThanks = value;
		}
	}

	private void Awake()
	{
		_instance = this;
		if (arrStarByOrder != null)
		{
			for (int i = 0; i < arrStarByOrder.Length; i++)
			{
				arrStarByOrder[i].numOrderStar = i;
				arrStarByOrder[i].lbNumStar.text = (i + 1).ToString();
			}
		}
	}

	private void OnEnable()
	{
		OnShowThanks();
	}

	private void OnDestroy()
	{
		_instance = null;
	}

	public void ShowWindowRating()
	{
		if (BannerWindowController.SharedController != null)
		{
			if (BannerWindowController.SharedController.viewedBannersCountInConnectScene != 0)
			{
				return;
			}
			BannerWindowController.SharedController.viewedBannersCountInConnectScene++;
		}
		ReviewController.CheckActiveReview();
		if (ReviewController.IsNeedActive)
		{
			OnShowWidowRating();
		}
	}

	public void SelectStar(StarReview curStar)
	{
		if (curStar != null)
		{
			countStarForReview = curStar.numOrderStar + 1;
		}
		if (arrStarByOrder == null)
		{
			return;
		}
		for (int i = 0; i < arrStarByOrder.Length; i++)
		{
			if (curStar != null && i <= curStar.numOrderStar)
			{
				arrStarByOrder[i].SetActiveStar(true);
			}
			else
			{
				arrStarByOrder[i].SetActiveStar(false);
			}
		}
	}

	public void OnChangeMsgReview()
	{
		UpdateStateBtnSendMsg(true);
	}

	public void OnClickStarRating()
	{
		if (countStarForReview > 0 && countStarForReview <= 4)
		{
			OnShowWindowEnterMessage();
		}
		else
		{
			SendMsgReview();
		}
	}

	public void OnSendMsgWithRating()
	{
		SendMsgReview();
	}

	private void SendMsgReview(bool isClickSend = true)
	{
		OnCloseAllWindow();
		if (countStarForReview > 0)
		{
			string msgReview = "";
			if (isInputMsgForReview)
			{
				msgReview = inputMsg.value;
			}
			if (countStarForReview == 5)
			{
				AnalyticsFacade.SendCustomEventToFacebook("5star_rating", null);
			}
			AnalyticsStuff.RateUsFake(true, countStarForReview, isInputMsgForReview && countStarForReview != 5);
			ReviewController.SendReview(countStarForReview, msgReview);
			if (isClickSend)
			{
				NeedShowThanks = true;
				isShow = true;
				OnShowThanks();
			}
		}
	}

	public void OnClickClose()
	{
		isShow = false;
		AnalyticsStuff.RateUsFake(countStarForReview != 0, countStarForReview);
		SendMsgReview(false);
	}

	private void OnCloseAllWindow()
	{
		isShow = false;
		if ((bool)objWindowRating)
		{
			objWindowRating.SetActive(false);
		}
		if ((bool)objWindowEnterMsg)
		{
			objWindowEnterMsg.SetActive(false);
		}
		if ((bool)objWindowGoToStore)
		{
			objWindowGoToStore.SetActive(false);
		}
		if ((bool)objThanks)
		{
			objThanks.SetActive(false);
		}
		RemoveBackSubscription();
	}

	private void OnShowWindowEnterMessage()
	{
		UpdateStateBtnSendMsg(false);
		if ((bool)objWindowRating)
		{
			objWindowRating.SetActive(false);
		}
		if ((bool)objWindowEnterMsg)
		{
			objWindowEnterMsg.SetActive(true);
		}
	}

	private void AddBackSubscription()
	{
		if (_backSubscription == null)
		{
			_backSubscription = BackSystem.Instance.Register(OnClickClose, "Review HUD (Window with 5 stars)");
		}
	}

	private void OnShowWidowRating()
	{
		if (ConnectScene.isReturnFromGame && BannerWindowController.SharedController != null)
		{
			BannerWindowController.SharedController.viewedBannersCountInConnectScene++;
		}
		isShow = true;
		countStarForReview = 0;
		SelectStar(null);
		ReviewController.IsSendReview = true;
		ReviewController.IsNeedActive = false;
		if ((bool)lbTitle5Stars)
		{
			lbTitle5Stars.text = TitleTextTranslate;
		}
		if ((bool)objWindowRating)
		{
			objWindowRating.SetActive(true);
		}
		if ((bool)objWindowEnterMsg)
		{
			objWindowEnterMsg.SetActive(false);
		}
		if ((bool)objWindowGoToStore)
		{
			objWindowGoToStore.SetActive(false);
		}
		AddBackSubscription();
	}

	private void OnShowWindowGoToStore()
	{
		if ((bool)objWindowRating)
		{
			objWindowRating.SetActive(false);
		}
		if ((bool)objWindowEnterMsg)
		{
			objWindowGoToStore.SetActive(true);
		}
	}

	private void OnShowThanks()
	{
		if (NeedShowThanks)
		{
			StartCoroutine(Crt_OnShowThanks());
		}
	}

	private IEnumerator Crt_OnShowThanks()
	{
		yield return new WaitForEndOfFrame();
		if (NeedShowThanks)
		{
			NeedShowThanks = false;
			if (objThanks != null)
			{
				objThanks.SetActive(true);
			}
			AddBackSubscription();
			yield return new WaitForSeconds(3f);
			OnCloseThanks();
		}
	}

	public void OnCloseThanks()
	{
		if (objThanks != null)
		{
			objThanks.SetActive(false);
		}
		isShow = false;
		RemoveBackSubscription();
	}

	private void UpdateStateBtnSendMsg(bool val)
	{
		isInputMsgForReview = val;
		if (isInputMsgForReview)
		{
			btnSendMsg.enabled = true;
			btnSendMsg.state = UIButtonColor.State.Normal;
		}
		else
		{
			btnSendMsg.enabled = false;
			btnSendMsg.state = UIButtonColor.State.Disabled;
		}
	}

	[ContextMenu("Find all stars")]
	private void FindStars()
	{
		arrStarByOrder = GetComponentsInChildren<StarReview>(true);
	}

	[ContextMenu("Show window")]
	public void TestShow()
	{
		ReviewController.IsNeedActive = true;
		ShowWindowRating();
	}

	private void RemoveBackSubscription()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
	}
}
