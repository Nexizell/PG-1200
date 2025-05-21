using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class CoinsUpdater : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CUpdateCoinsLabel_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CoinsUpdater _003C_003E4__this;

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
		public _003CUpdateCoinsLabel_003Ed__7(int _003C_003E1__state)
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
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
				_003C_003E1__state = -1;
				break;
			}
			if (!_003C_003E4__this._disposed)
			{
				if (!BankController.canShowIndication)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E4__this.UpdateMoney();
				_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.MyWaitForSeconds(1f));
				_003C_003E1__state = 2;
				return true;
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

	[CompilerGenerated]
	internal sealed class _003CMyWaitForSeconds_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private float _003CstartTime_003E5__1;

		public float tm;

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
		public _003CMyWaitForSeconds_003Ed__9(int _003C_003E1__state)
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
				if (!(Time.realtimeSinceStartup - _003CstartTime_003E5__1 < tm))
				{
					return false;
				}
			}
			else
			{
				_003C_003E1__state = -1;
				_003CstartTime_003E5__1 = Time.realtimeSinceStartup;
			}
			_003C_003E2__current = null;
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

	public static readonly string trainCoinsStub = "999";

	private UILabel coinsLabel;

	private string _trainingMsg = "0";

	private bool _disposed;

	private void Start()
	{
		coinsLabel = GetComponent<UILabel>();
		CoinsMessage.CoinsLabelDisappeared += _ReplaceMsgForTraining;
		string text = Storager.getInt("Coins").ToString();
		if (coinsLabel != null)
		{
			coinsLabel.text = text;
		}
	}

	private void OnEnable()
	{
		BankController.onUpdateMoney += UpdateMoney;
		StartCoroutine(UpdateCoinsLabel());
	}

	private void OnDisable()
	{
		BankController.onUpdateMoney -= UpdateMoney;
	}

	private void _ReplaceMsgForTraining(bool isGems, int count)
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			_trainingMsg = trainCoinsStub;
		}
	}

	private IEnumerator UpdateCoinsLabel()
	{
		while (!_disposed)
		{
			if (!BankController.canShowIndication)
			{
				yield return null;
				continue;
			}
			UpdateMoney();
			yield return StartCoroutine(MyWaitForSeconds(1f));
		}
	}

	private void UpdateMoney()
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			if (coinsLabel != null)
			{
				if (ShopNGUIController.sharedShop != null && ShopNGUIController.GuiActive)
				{
					coinsLabel.text = "999";
				}
				else
				{
					coinsLabel.text = _trainingMsg;
				}
			}
		}
		else
		{
			string text = Storager.getInt("Coins").ToString();
			if (coinsLabel != null)
			{
				coinsLabel.text = text;
			}
		}
	}

	public IEnumerator MyWaitForSeconds(float tm)
	{
		float startTime = Time.realtimeSinceStartup;
		do
		{
			yield return null;
		}
		while (Time.realtimeSinceStartup - startTime < tm);
	}

	private void OnDestroy()
	{
		CoinsMessage.CoinsLabelDisappeared -= _ReplaceMsgForTraining;
		_disposed = true;
	}
}
