using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using Unity.Linq;
using UnityEngine;

public sealed class DailyQuestsButton : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CUpdateUI_003Ed__14 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private WaitForSeconds _003Cdelay_003E5__1;

		public DailyQuestsButton _003C_003E4__this;

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
		public _003CUpdateUI_003Ed__14(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				_003C_003E4__this.SetUI();
			}
			else
			{
				_003C_003E1__state = -1;
				_003Cdelay_003E5__1 = new WaitForSeconds(0.5f);
			}
			_003C_003E2__current = _003Cdelay_003E5__1;
			_003C_003E1__state = 1;
			return true;
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

	public bool inBannerSystem = true;

	[SerializeField]
	protected internal DailyQuestsBannerController controller;

	public GameObject rewardIndicator;

	[SerializeField]
	protected internal GameObject dailyQuestsParent;

	public event Action OnClickAction;

	private void Awake()
	{
		if (inBannerSystem)
		{
			QuestSystem.Instance.Updated += HandleQuestSystemUpdate;
		}
		else if (GameConnect.isDaterRegim)
		{
			base.gameObject.SetActive(false);
		}
		SetUI();
	}

	private void OnEnable()
	{
		SetUI();
		if (QuestSystem.Instance == null || !QuestSystem.Instance.Enabled)
		{
			if (LevelCompleteScript.IsRunning)
			{
				base.gameObject.SetActive(false);
			}
			else if (NetworkStartTableNGUIController.sharedController != null)
			{
				base.gameObject.SetActive(false);
			}
		}
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(UpdateUI());
		}
	}

	private void OnDisable()
	{
		controller = null;
		if (dailyQuestsParent != null)
		{
			dailyQuestsParent.transform.DestroyChildren();
		}
	}

	private void OnDestroy()
	{
		if (inBannerSystem)
		{
			QuestSystem.Instance.Updated -= HandleQuestSystemUpdate;
		}
	}

	private void OnClick()
	{
		ButtonClickSound.TryPlayClick();
		if ((BankController.Instance != null && BankController.Instance.InterfaceEnabled) || (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown) || ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		if (this.OnClickAction != null)
		{
			this.OnClickAction();
		}
		else if (inBannerSystem)
		{
			bool flag = BannerWindowController.SharedController == null;
		}
		else
		{
			if (LoadingInAfterGame.isShowLoading)
			{
				return;
			}
			if ((controller == null || controller.gameObject == null) && dailyQuestsParent != null)
			{
				dailyQuestsParent.transform.DestroyChildren();
				DailyQuestsBannerController dailyQuestsBannerController = UnityEngine.Object.Instantiate(Resources.Load<DailyQuestsBannerController>("Windows/DailyQuests-Window"));
				if (dailyQuestsBannerController != null)
				{
					dailyQuestsBannerController.transform.parent = dailyQuestsParent.transform;
					dailyQuestsBannerController.transform.localPosition = Vector3.zero;
					dailyQuestsBannerController.transform.localRotation = Quaternion.identity;
					dailyQuestsBannerController.transform.localScale = Vector3.one;
					int layer = base.gameObject.layer;
					dailyQuestsBannerController.gameObject.layer = layer;
					foreach (GameObject item in dailyQuestsBannerController.gameObject.Descendants())
					{
						item.layer = layer;
					}
					dailyQuestsBannerController.inBannerSystem = inBannerSystem;
				}
				controller = dailyQuestsBannerController;
			}
			if (controller != null)
			{
				controller.Show();
			}
		}
	}

	private void CheckUnrewardedEvent(object sender, EventArgs e)
	{
		SetUI();
	}

	public void SetUI()
	{
		if (QuestSystem.Instance.QuestProgress != null && QuestSystem.Instance.Enabled)
		{
			bool anyActiveQuest;
			int num;
			if (QuestSystem.Instance.QuestProgress != null)
			{
				anyActiveQuest = QuestSystem.Instance.AnyActiveQuest;
			}
			else
				num = 0;
			if (rewardIndicator != null && QuestSystem.Instance.QuestProgress != null)
			{
				bool active = QuestSystem.Instance.QuestProgress.HasUnrewaredAccumQuests();
				rewardIndicator.SetActive(active);
			}
		}
	}

	private IEnumerator UpdateUI()
	{
		WaitForSeconds delay = new WaitForSeconds(0.5f);
		while (true)
		{
			yield return delay;
			SetUI();
		}
	}

	private void HandleQuestSystemUpdate(object sender, EventArgs e)
	{
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log("Refreshing after quest system update.");
		}
		SetUI();
	}
}
