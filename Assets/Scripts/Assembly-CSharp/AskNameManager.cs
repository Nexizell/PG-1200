using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public class AskNameManager : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CWaitAndShowWindow_003Ed__23 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AskNameManager _003C_003E4__this;

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
		public _003CWaitAndShowWindow_003Ed__23(int _003C_003E1__state)
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
				if (_003C_003E4__this.AskIsCompleted())
				{
					return false;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				break;
			}
			if (!_003C_003E4__this.CanShowWindow)
			{
				if (_003C_003E4__this.AskIsCompleted())
				{
					return false;
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			_003C_003E4__this.OnShowWindowSetName();
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

	public static AskNameManager instance;

	public GameObject objWindow;

	public GameObject objPanelSetName;

	public GameObject objPanelEnterName;

	public UILabel lbPlayerName;

	public UIInput inputPlayerName;

	public UIButton btnSetName;

	public GameObject objLbWarning;

	public const string keyNameAlreadySet = "keyNameAlreadySet";

	private int _NameAlreadySet = -1;

	private string curChooseName = "";

	private bool isAutoName;

	public static bool isComplete;

	public static bool isShow;

	private IDisposable _backSubcripter;

	private bool CanShowWindow
	{
		get
		{
			if (NameAlreadySet)
			{
				return false;
			}
			if (TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.ShopCompleted)
			{
				return false;
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				return true;
			}
			return !CloudSyncController.IsSynchronizingWithCloud;
		}
	}

	private bool NameAlreadySet
	{
		get
		{
			if (_NameAlreadySet == -1)
			{
				_NameAlreadySet = Load.LoadInt("keyNameAlreadySet");
			}
			return _NameAlreadySet == 1;
		}
		set
		{
			_NameAlreadySet = (value ? 1 : 0);
			Save.SaveInt("keyNameAlreadySet", _NameAlreadySet);
		}
	}

	private bool CanSetName
	{
		get
		{
			if (!string.IsNullOrEmpty(curChooseName.Trim()))
			{
				return true;
			}
			return false;
		}
	}

	public static event Action onComplete;

	private void Awake()
	{
		instance = this;
		isComplete = false;
		isShow = false;
		objWindow.SetActive(false);
		objPanelSetName.SetActive(false);
		objPanelEnterName.SetActive(false);
		objLbWarning.SetActive(false);
		AskIsCompleted();
		MainMenuController.onEnableMenuForAskname += ShowWindow;
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
		MainMenuController.onEnableMenuForAskname -= ShowWindow;
		instance = null;
	}

	public void ShowWindow()
	{
		StopCoroutine("WaitAndShowWindow");
		StartCoroutine("WaitAndShowWindow");
	}

	private IEnumerator WaitAndShowWindow()
	{
		if (AskIsCompleted())
		{
			yield break;
		}
		while (!CanShowWindow)
		{
			if (AskIsCompleted())
			{
				yield break;
			}
			yield return null;
			yield return null;
		}
		OnShowWindowSetName();
	}

	private bool AskIsCompleted()
	{
		int num;
		if (!NameAlreadySet)
		{
			num = (TrainingController.TrainingCompleted ? 1 : 0);
			if (num == 0)
			{
				goto IL_002a;
			}
		}
		else
		{
			num = 1;
		}
		isComplete = true;
		if (AskNameManager.onComplete != null)
		{
			AskNameManager.onComplete();
		}
		goto IL_002a;
		IL_002a:
		return (byte)num != 0;
	}

	private void OnShowWindowSetName()
	{
		if (_backSubcripter != null)
		{
			_backSubcripter.Dispose();
		}
		_backSubcripter = BackSystem.Instance.Register(delegate
		{
		});
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log("+ OnShowWindowSetName");
		}
		isShow = true;
		curChooseName = GetNameForAsk();
		lbPlayerName.text = curChooseName;
		inputPlayerName.value = curChooseName;
		CheckActiveBtnSetName();
		objPanelSetName.SetActive(true);
		objWindow.SetActive(true);
		isAutoName = true;
	}

	public void OnShowWindowEnterName()
	{
		objPanelEnterName.SetActive(true);
		objPanelSetName.SetActive(false);
		OnStartEnterName();
	}

	private string GetNameForAsk()
	{
		return ProfileController.GetPlayerNameOrDefault();
	}

	private void CheckActiveBtnSetName()
	{
		BoxCollider component = btnSetName.GetComponent<BoxCollider>();
		objLbWarning.SetActive(false);
		if (CanSetName)
		{
			component.enabled = true;
			btnSetName.SetState(UIButtonColor.State.Normal, true);
		}
		else
		{
			objLbWarning.SetActive(true);
			component.enabled = false;
			btnSetName.SetState(UIButtonColor.State.Disabled, true);
		}
	}

	public void OnStartEnterName()
	{
		if (isAutoName)
		{
			inputPlayerName.isSelected = true;
			curChooseName = "";
			inputPlayerName.value = curChooseName;
			CheckActiveBtnSetName();
			isAutoName = false;
		}
	}

	public void OnChangeName()
	{
		curChooseName = inputPlayerName.value;
		CheckActiveBtnSetName();
	}

	public void SaveChooseName()
	{
		if (ProfileController.Instance != null)
		{
			ProfileController.Instance.SaveNamePlayer(curChooseName);
		}
		if (PlayerPanel.instance != null)
		{
			PlayerPanel.instance.UpdateNickPlayer();
		}
		NameAlreadySet = true;
		OnCloseAllWindow();
	}

	private void OnCloseAllWindow()
	{
		if (_backSubcripter != null)
		{
			_backSubcripter.Dispose();
		}
		objWindow.SetActive(false);
		isComplete = true;
		if (AskNameManager.onComplete != null)
		{
			AskNameManager.onComplete();
		}
		isShow = false;
		if (!TrainingController.TrainingCompleted && CloudSyncController.AreProgressInCurrentPullResult())
		{
			TrainingController.OnGetProgress();
			CoroutineRunner.Instance.StartCoroutine(CloudSyncController.SynchronizeWithCloud_DataAlreadyPulled(false, true));
		}
	}

	[ContextMenu("Show Window")]
	public void TestShow()
	{
		isComplete = false;
		OnShowWindowSetName();
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus && objWindow != null && objWindow.activeInHierarchy)
		{
			curChooseName = "Player";
			SaveChooseName();
		}
	}
}
