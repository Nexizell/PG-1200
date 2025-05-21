using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MarafonBonusWindow : BannerWindow
{
	[CompilerGenerated]
	internal sealed class _003CStartCentralizeBonusItem_003Ed__11 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MarafonBonusWindow _003C_003E4__this;

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
		public _003CStartCentralizeBonusItem_003Ed__11(int _003C_003E1__state)
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
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this.CentralizeScrollByCurrentBonus();
				return false;
			}
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

	public GameObject premiumInterface;

	public UIScrollView bonusScrollView;

	public UIGrid bonusScroll;

	public UILabel title;

	public GameObject bonusEverydayItem;

	public UIScrollView scrollView;

	public BonusEverydayItem[] superPrizes;

	private void FillBonusesForEveryday()
	{
		List<BonusMarafonItem> bonusItems = MarafonBonusController.Get.BonusItems;
		int currentBonusIndex = MarafonBonusController.Get.GetCurrentBonusIndex();
		BonusEverydayItem[] componentsInChildren = bonusScroll.GetComponentsInChildren<BonusEverydayItem>(true);
		bool flag = componentsInChildren.Length != 0;
		BonusEverydayItem bonusEverydayItem = null;
		GameObject gameObject = null;
		for (int i = 0; i < bonusItems.Count; i++)
		{
			if (!flag)
			{
				gameObject = UnityEngine.Object.Instantiate(this.bonusEverydayItem);
				gameObject.name = string.Format("{0:00}", new object[1] { i });
			}
			bonusEverydayItem = (flag ? componentsInChildren[i] : gameObject.GetComponent<BonusEverydayItem>());
			if (bonusEverydayItem != null)
			{
				bool isBonusWeekly = (i + 1) % 7 == 0 || i == bonusItems.Count - 1;
				bonusEverydayItem.FillData(i, currentBonusIndex, isBonusWeekly);
			}
			if (!flag)
			{
				bonusScroll.AddChild(gameObject.transform);
				gameObject.gameObject.SetActive(true);
			}
			bonusEverydayItem.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
		}
		bonusEverydayItem.SetBackgroundForBonusWeek();
		bonusScroll.Reposition();
	}

	private void FillPrizesForEveryweek()
	{
		List<BonusMarafonItem> bonusItems = MarafonBonusController.Get.BonusItems;
		int currentBonusIndex = MarafonBonusController.Get.GetCurrentBonusIndex();
		int num = 0;
		for (int i = 6; i < bonusItems.Count; i += 7)
		{
			BonusEverydayItem bonusEverydayItem = superPrizes[num];
			num++;
			if (bonusEverydayItem != null)
			{
				bonusEverydayItem.FillData(i, currentBonusIndex, false);
			}
		}
		int num2 = superPrizes.Length - 1;
		int bonusIndex = bonusItems.Count - 1;
		superPrizes[num2].FillData(bonusIndex, currentBonusIndex, false);
	}

	private void OnEnable()
	{
		StartCoroutine(StartCentralizeBonusItem());
	}

	public override void Show()
	{
		MarafonBonusController.Get.InitializeBonusItems();
		FillBonusesForEveryday();
		FillPrizesForEveryweek();
		base.Show();
	}

	public IEnumerator StartCentralizeBonusItem()
	{
		yield return null;
		CentralizeScrollByCurrentBonus();
	}

	private void ResetScrollPosition(GameObject centerElement)
	{
		bonusScroll.GetComponent<UICenterOnChild>().enabled = false;
		bonusScroll.Reposition();
	}

	public void OnGetRewardClick()
	{
		ButtonClickSound.TryPlayClick();
		scrollView.ResetPosition();
		MarafonBonusController.Get.TakeMarafonBonus();
		BannerWindowController.SharedController.HideBannerWindow();
	}

	private void CentralizeScrollByCurrentBonus()
	{
		if (bonusScroll == null)
		{
			return;
		}
		int currentBonusIndex = MarafonBonusController.Get.GetCurrentBonusIndex();
		Transform child = bonusScroll.GetChild(currentBonusIndex);
		if (child != null)
		{
			if (currentBonusIndex > 2 && currentBonusIndex < 27)
			{
				bonusScroll.GetComponent<UICenterOnChild>().springStrength = 8f;
				bonusScroll.GetComponent<UICenterOnChild>().CenterOn(child);
			}
			else if (currentBonusIndex >= 27)
			{
				bonusScroll.GetComponent<UICenterOnChild>().springStrength = 8f;
				Transform child2 = bonusScroll.GetChild(27);
				if (child2 != null)
				{
					bonusScroll.GetComponent<UICenterOnChild>().CenterOn(child2);
				}
			}
			child.localScale = Vector3.one;
		}
		bonusScroll.GetComponent<UICenterOnChild>().onCenter = ResetScrollPosition;
	}

	internal sealed override void Submit()
	{
		OnGetRewardClick();
	}

	private void Update()
	{
		if (premiumInterface.activeSelf != (PremiumAccountController.Instance != null && PremiumAccountController.Instance.isAccountActive))
		{
			premiumInterface.SetActive(PremiumAccountController.Instance != null && PremiumAccountController.Instance.isAccountActive);
		}
	}
}
