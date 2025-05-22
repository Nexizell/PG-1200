using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FyberPlugin;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectScene : MonoBehaviour
{
	public struct infoServer
	{
		public string ipAddress;

		public int port;

		public string name;

		public string map;

		public int playerLimit;

		public int connectedPlayers;

		public string coments;
	}

	public struct infoClient
	{
		public string ipAddress;

		public string name;

		public string coments;
	}

	[CompilerGenerated]
	internal sealed class _003C_003Ec__DisplayClass95_0
	{
		public GameConnect.GameMode gameMode;

		internal bool _003CAnimateModeOpen_003Eb__0(BtnCategory b)
		{
			return b.GameMode == gameMode;
		}
	}

	[CompilerGenerated]
	internal sealed class _003CAnimateModeOpen_003Ed__95 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ConnectScene _003C_003E4__this;

		private int _003CcurrentLevel_003E5__1;

		private int _003CsupportedModeCount_003E5__2;

		private int _003CcurrentStage_003E5__3;

		private GameConnect.GameMode[] _003CorderedSupportedModeArray_003E5__4;

		private BtnCategory _003CcurrentModeButton_003E5__5;

		private int _003CupperBound_003E5__6;

		private int _003CstoragedStage_003E5__7;

		private int _003CcurrentStateDuel_003E5__8;

		private int _003CstoragedStageDuel_003E5__9;

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
		public _003CAnimateModeOpen_003Ed__95(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num4;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003E4__this.animationStarted = true;
				_003C_003E4__this.localBtn.GetComponent<UIButton>().isEnabled = TrainingController.TrainingCompleted;
				_003C_003E4__this.randomBtn.GetComponent<UIButton>().isEnabled = TrainingController.TrainingCompleted;
				_003C_003E4__this.customBtn.GetComponent<UIButton>().isEnabled = TrainingController.TrainingCompleted;
				_003C_003E4__this.goBtn.GetComponent<UIButton>().isEnabled = TrainingController.TrainingCompleted;
				if (_003C_003E4__this.modeAnimObj != null)
				{
					_003C_003E4__this.modeAnimObj.SetActive(false);
				}
				_003CstoragedStageDuel_003E5__9 = Storager.getInt("ModeUnlockDuel");
				_003CcurrentStateDuel_003E5__8 = ((RatingSystem.instance.currentRating >= 300) ? _003CstoragedStageDuel_003E5__9 : 0);
				_003C_003E4__this.modeDuelRatingNeedLabel.text = 300.ToString();
				_003C_003E4__this.modeDuelRatingNeed.SetActive(RatingSystem.instance.currentRating < 300);
				_003C_003E4__this.modeButtonDuel.isEnable = _003CcurrentStateDuel_003E5__8 == 1;
				_003CorderedSupportedModeArray_003E5__4 = GameModeLocker.Instance.GetOrderedSupportedModes().ToArray();
				_003CsupportedModeCount_003E5__2 = _003CorderedSupportedModeArray_003E5__4.Length;
				_003CstoragedStage_003E5__7 = Storager.getInt("ModeUnlockStage");
				if (Storager.getInt("TrainingCompleted_4_4_Sett") == 1)
				{
					_003CstoragedStage_003E5__7 = _003CsupportedModeCount_003E5__2;
				}
				_003CcurrentStage_003E5__3 = Mathf.Clamp(_003CstoragedStage_003E5__7, 0, _003CsupportedModeCount_003E5__2);
				for (int i = 0; i < _003C_003E4__this.modeButtonByLevel.Length; i++)
				{
					BtnCategory btnCategory = _003C_003E4__this.modeButtonByLevel[i];
					btnCategory.isEnable = !GameModeLocker.Instance.IsLocked(btnCategory.GameMode) && i < _003CcurrentStage_003E5__3;
				}
				_003CcurrentLevel_003E5__1 = ((ExperienceController.sharedController != null) ? ExperienceController.sharedController.currentLevel : 31);
				if (_003C_003E4__this.modeUnlockLabelByLevel != null)
				{
					for (int j = 1; j < _003C_003E4__this.modeButtonByLevel.Length; j++)
					{
						int num2 = j - 1;
						if (num2 >= _003C_003E4__this.modeUnlockLabelByLevel.Length)
						{
							break;
						}
						GameConnect.GameMode gameMode = _003C_003E4__this.modeButtonByLevel[j].GameMode;
						bool flag = GameModeLocker.Instance.IsLocked(gameMode);
						_003C_003E4__this.modeUnlockLabelByLevel[num2].gameObject.SetActive(flag);
						if (flag && gameMode != GameConnect.GameMode.Duel)
						{
							int unlockLevel = GameModeLocker.GetUnlockLevel(gameMode);
							int num3 = Mathf.Clamp(unlockLevel, 0, 36);
							UnityEngine.Debug.LogFormat("level: {0}, clampedLevel: {1}, gameMode: {2}", unlockLevel, num3, gameMode);
							_003C_003E4__this.modeUnlockLabelByLevel[num2].text = string.Format(LocalizationStore.Get("Key_1923"), new object[1] { num3 });
						}
					}
				}
				if (_003CcurrentStage_003E5__3 < Mathf.Min(_003CcurrentLevel_003E5__1, _003CsupportedModeCount_003E5__2))
				{
					BannerWindowController.SharedController.AddBannersTimeout(20.1f);
				}
				_003C_003E2__current = new WaitForSeconds(0.8f);
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				_003CupperBound_003E5__6 = Mathf.Min(_003CcurrentLevel_003E5__1, _003CsupportedModeCount_003E5__2);
				goto IL_05f7;
			case 2:
				_003C_003E1__state = -1;
				_003CcurrentModeButton_003E5__5.isEnable = true;
				_003C_003E2__current = new WaitForSeconds(1.4f);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E4__this.modeAnimObj.SetActive(false);
				goto IL_050e;
			case 4:
				_003C_003E1__state = -1;
				goto IL_059d;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E4__this.modeButtonDuel.isEnable = true;
				_003C_003E2__current = new WaitForSeconds(1.4f);
				_003C_003E1__state = 6;
				return true;
			case 6:
				{
					_003C_003E1__state = -1;
					_003C_003E4__this.modeAnimObj.SetActive(false);
					_003CcurrentStateDuel_003E5__8 = 1;
					break;
				}
				IL_05f7:
				if (_003CcurrentStage_003E5__3 < _003CupperBound_003E5__6)
				{
					_003C_003Ec__DisplayClass95_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass95_0();
					BannerWindowController.SharedController.AddBannersTimeout(20.1f);
					if (_003CcurrentStage_003E5__3 == 1)
					{
						BannerWindowController.SharedController.ClearBannerStates();
					}
					CS_0024_003C_003E8__locals0.gameMode = _003CorderedSupportedModeArray_003E5__4[_003CcurrentStage_003E5__3];
					int num = Array.FindIndex(_003C_003E4__this.modeButtonByLevel, (BtnCategory b) => b.GameMode == CS_0024_003C_003E8__locals0.gameMode);
					if (num >= 0)
					{
						_003CcurrentModeButton_003E5__5 = _003C_003E4__this.modeButtonByLevel[num];
						if (_003CcurrentStage_003E5__3 != 0)
						{
							if (_003C_003E4__this.modeAnimObj == null)
							{
								_003C_003E4__this.CreateAnimUnlock();
							}
							_003C_003E4__this.modeAnimObj.transform.SetParent(_003C_003E4__this.categoryButtonsController.transform.parent);
							_003C_003E4__this.modeAnimObj.transform.position = _003CcurrentModeButton_003E5__5.transform.position;
							_003C_003E4__this.modeAnimObj.transform.localScale = _003CcurrentModeButton_003E5__5.transform.localScale;
							_003C_003E4__this.modeAnimObj.SetActive(true);
							_003C_003E2__current = new WaitForSeconds(0.1f);
							_003C_003E1__state = 2;
							return true;
						}
						goto IL_050e;
					}
					goto IL_05e7;
				}
				if (_003CstoragedStage_003E5__7 != _003CcurrentStage_003E5__3)
				{
					Storager.setInt("ModeUnlockStage", _003CcurrentStage_003E5__3);
				}
				if (_003CcurrentStateDuel_003E5__8 == 0 && RatingSystem.instance.currentRating >= 300)
				{
					if (_003C_003E4__this.modeAnimObj == null)
					{
						_003C_003E4__this.CreateAnimUnlock();
					}
					_003C_003E4__this.modeAnimObj.transform.SetParent(_003C_003E4__this.categoryButtonsController.transform.parent);
					_003C_003E4__this.modeAnimObj.transform.position = _003C_003E4__this.modeButtonDuel.transform.position;
					_003C_003E4__this.modeAnimObj.SetActive(true);
					_003C_003E2__current = new WaitForSeconds(0.1f);
					_003C_003E1__state = 5;
					return true;
				}
				break;
				IL_050e:
				if (_003CcurrentStage_003E5__3 == 0 && !TrainingController.TrainingCompleted)
				{
					if (_003C_003E4__this.finger == null)
					{
						_003C_003E4__this.CreateFinger();
					}
					UIScrollView component = _003C_003E4__this.scrollViewSelectMapTransform.GetComponent<UIScrollView>();
					component.onDragStarted = (UIScrollView.OnDragNotification)Delegate.Combine(component.onDragStarted, new UIScrollView.OnDragNotification(_003C_003E4__this.StopFingerAnim));
					AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Connect_Scene);
					_003C_003E2__current = _003C_003E4__this.FingerAnimationCoroutine();
					_003C_003E1__state = 4;
					return true;
				}
				goto IL_059d;
				IL_05e7:
				num4 = _003CcurrentStage_003E5__3 + 1;
				_003CcurrentStage_003E5__3 = num4;
				goto IL_05f7;
				IL_059d:
				if (_003CcurrentStage_003E5__3 == 1)
				{
					HintController.instance.ShowHintByName("deathmatch");
					HintController.instance.ShowHintByName("gobattletimeout");
				}
				Storager.setInt("ModeUnlockStage", _003CcurrentStage_003E5__3 + 1);
				_003CcurrentModeButton_003E5__5 = null;
				goto IL_05e7;
			}
			if (_003CstoragedStageDuel_003E5__9 != _003CcurrentStateDuel_003E5__8)
			{
				Storager.setInt("ModeUnlockDuel", _003CcurrentStateDuel_003E5__8);
			}
			if (!TrainingController.TrainingCompleted)
			{
				_003C_003E4__this.goBtn.GetComponent<UIButton>().isEnabled = true;
				HintController.instance.ShowHintByName("gobattle");
			}
			_003C_003E4__this.animationStarted = false;
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
	internal sealed class _003CFingerAnimationCoroutine_003Ed__98 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ConnectScene _003C_003E4__this;

		private string _003CfromName_003E5__1;

		private Animator _003CfingerAnimator_003E5__2;

		private string _003CtoName_003E5__3;

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
		public _003CFingerAnimationCoroutine_003Ed__98(int _003C_003E1__state)
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
				_003C_003E4__this.finger.SetActive(true);
				_003CfromName_003E5__1 = _003C_003E4__this.grid.transform.GetChild(1).GetComponent<MapPreviewController>().sceneMapName;
				_003CtoName_003E5__3 = _003C_003E4__this.grid.transform.GetChild(2).GetComponent<MapPreviewController>().sceneMapName;
				_003C_003E2__current = _003C_003E4__this.finger.MoveOverTime(_003C_003E4__this.finger.transform.position, _003C_003E4__this.grid.transform.GetChild(1).transform.position, 1f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if (_003C_003E4__this._stopFingerAnimation)
				{
					return false;
				}
				_003CfingerAnimator_003E5__2 = _003C_003E4__this.finger.GetComponentInChildren<Animator>(true);
				_003CfingerAnimator_003E5__2.SetTrigger("touch");
				_003C_003E2__current = new WaitForSeconds(0.8f);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				if (_003C_003E4__this._stopFingerAnimation)
				{
					return false;
				}
				_003C_003E4__this.SelectMap(_003CfromName_003E5__1);
				_003C_003E2__current = new WaitForSeconds(1f);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				if (_003C_003E4__this._stopFingerAnimation)
				{
					return false;
				}
				_003C_003E2__current = _003C_003E4__this.finger.MoveOverTime(_003C_003E4__this.grid.transform.GetChild(1).transform.position, _003C_003E4__this.grid.transform.GetChild(2).transform.position, 1f);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003CfingerAnimator_003E5__2.SetTrigger("touch");
				_003C_003E2__current = new WaitForSeconds(0.8f);
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				if (_003C_003E4__this._stopFingerAnimation)
				{
					return false;
				}
				_003C_003E4__this.SelectMap(_003CtoName_003E5__3);
				_003C_003E2__current = new WaitForSeconds(1f);
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E4__this.finger.gameObject.SetActive(false);
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

	[CompilerGenerated]
	internal sealed class _003COnApplicationPause_003Ed__103 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public bool pausing;

		public ConnectScene _003C_003E4__this;

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
		public _003COnApplicationPause_003Ed__103(int _003C_003E1__state)
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
				if (pausing)
				{
					_003C_003E4__this.lanScan.StopBroadCasting();
					break;
				}
				_003C_003E2__current = new WaitForSeconds(1f);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				_003C_003E4__this.StartSearchLocalServers();
				InterstitialManager.Instance.ResetAdProvider();
				if (MobileAdManager.Instance.SuppressShowOnReturnFromPause)
				{
					MobileAdManager.Instance.SuppressShowOnReturnFromPause = false;
					break;
				}
				string reasonToDismissFakeInterstitial = GetReasonToDismissFakeInterstitial();
				if (string.IsNullOrEmpty(reasonToDismissFakeInterstitial))
				{
					ReplaceAdmobPerelivController.IncreaseTimesCounter();
					if (!_003C_003E4__this.loadAdmobRunning)
					{
						_003C_003E4__this.StartCoroutine(_003C_003E4__this.WaitLoadingAndShowReplaceAdmobPereliv("On return from pause to Connect Scene"));
					}
				}
				else
				{
					UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=magenta>Dismissing fake interstitial. {0}</color>" : "Dismissing fake interstitial. {0}", reasonToDismissFakeInterstitial);
				}
				break;
			}
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
	internal sealed class _003CWaitLoadingAndShowReplaceAdmobPereliv_003Ed__105 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ConnectScene _003C_003E4__this;

		public bool loadData;

		public string context;

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
		public _003CWaitLoadingAndShowReplaceAdmobPereliv_003Ed__105(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
			case 2:
			case 3:
				try
				{
					break;
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			case -2:
			case -1:
			case 0:
				break;
			}
		}

		private bool MoveNext()
		{
			bool result;
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					result = false;
					goto end_IL_0000;
				case 0:
					_003C_003E1__state = -1;
					if (_003C_003E4__this.loadReplaceAdmobPerelivRunning)
					{
						break;
					}
					_003C_003E1__state = -3;
					_003C_003E4__this.loadReplaceAdmobPerelivRunning = true;
					if (loadData && !ReplaceAdmobPerelivController.sharedController.DataLoading && !ReplaceAdmobPerelivController.sharedController.DataLoaded)
					{
						ReplaceAdmobPerelivController.sharedController.LoadPerelivData();
					}
					goto IL_00b7;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00b7;
				case 2:
					_003C_003E1__state = -3;
					goto IL_0102;
				case 3:
					{
						_003C_003E1__state = -3;
						goto IL_013a;
					}
					IL_013a:
					if (BannerWindowController.SharedController != null && BannerWindowController.SharedController.viewedBannersCountInConnectScene != 0)
					{
						UnityEngine.Debug.LogFormat("Cancel showing fake: {0}", BannerWindowController.SharedController.viewedBannersCountInConnectScene);
						result = false;
						goto IL_01b5;
					}
					if (BannerWindowController.SharedController != null)
					{
						BannerWindowController.SharedController.viewedBannersCountInConnectScene++;
					}
					ReplaceAdmobPerelivController.TryShowPereliv(context);
					ReplaceAdmobPerelivController.sharedController.DestroyImage();
					_003C_003Em__Finally1();
					break;
					IL_01b5:
					_003C_003Em__Finally1();
					goto end_IL_0000;
					IL_0102:
					if (!_003C_003E4__this.mainPanel.activeInHierarchy)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 2;
						result = true;
					}
					else
					{
						_003C_003E2__current = new WaitForSeconds(0.5f);
						_003C_003E1__state = 3;
						result = true;
					}
					goto end_IL_0000;
					IL_00b7:
					if (!(ReplaceAdmobPerelivController.sharedController == null) && ReplaceAdmobPerelivController.sharedController.DataLoaded)
					{
						if (_003C_003E4__this.mainPanel != null)
						{
							goto IL_0102;
						}
						goto IL_013a;
					}
					if (!ReplaceAdmobPerelivController.sharedController.DataLoading)
					{
						_003C_003E4__this.loadReplaceAdmobPerelivRunning = false;
						result = false;
						goto IL_01b5;
					}
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					result = true;
					goto end_IL_0000;
				}
				result = false;
				end_IL_0000:;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
			return result;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			_003C_003E4__this.loadReplaceAdmobPerelivRunning = false;
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	internal sealed class _003CWaitLoadingAndShowInterstitialCoroutine_003Ed__108 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ConnectScene _003C_003E4__this;

		private float _003CloadAttemptTime_003E5__1;

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
		public _003CWaitLoadingAndShowInterstitialCoroutine_003Ed__108(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
			case 2:
			case 3:
				try
				{
					break;
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			case -2:
			case -1:
			case 0:
				break;
			}
		}

		private bool MoveNext()
		{
			bool result;
			try
			{
				List<Task<Ad>> list2;
				List<Task<Ad>> list3;
				List<Task<Ad>> list;
				switch (_003C_003E1__state)
				{
				default:
					result = false;
					goto end_IL_0000;
				case 0:
					_003C_003E1__state = -1;
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.Log("Starting WaitLoadingAndShowInterstitialCoroutine()    " + InterstitialManager.Instance.Provider);
					}
					if (!_003C_003E4__this.loadAdmobRunning)
					{
						_003C_003E4__this.loadAdmobRunning = true;
						_003C_003E1__state = -3;
						_003CloadAttemptTime_003E5__1 = Time.realtimeSinceStartup;
						if (Defs.IsDeveloperBuild)
						{
							UnityEngine.Debug.Log("FyberFacade.Instance.Requests.Count: " + FyberFacade.Instance.Requests.Count);
						}
						if (FyberFacade.Instance.Requests.Count == 0)
						{
							Task<Ad> value = FyberFacade.Instance.RequestImageInterstitial("WaitLoadingAndShowInterstitialCoroutine(), requests count: 0");
							FyberFacade.Instance.Requests.AddLast(value);
						}
						if (Defs.IsDeveloperBuild)
						{
							UnityEngine.Debug.Log("Waiting either at least one loading request completed successfully, or all failed...");
						}
						goto IL_0103;
					}
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.Log("Quitting WaitLoadingAndShowInterstitialCoroutine() because loadAdmobRunning==true");
					}
					result = false;
					goto end_IL_0000;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0103;
				case 2:
					_003C_003E1__state = -3;
					goto IL_03bf;
				case 3:
					{
						_003C_003E1__state = -3;
						break;
					}
					IL_03bf:
					if (!_003C_003E4__this.mainPanel.activeInHierarchy)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 2;
						result = true;
					}
					else
					{
						_003C_003E2__current = new WaitForSeconds(0.5f);
						_003C_003E1__state = 3;
						result = true;
					}
					goto end_IL_0000;
					IL_0103:
					if (FyberFacade.Instance.Requests.Any((Task<Ad> r) => ((Task)r).IsCompleted && !((Task)r).IsFaulted))
					{
						if (Defs.IsDeveloperBuild)
						{
							UnityEngine.Debug.LogFormat("Found successfully completed request among {0}", FyberFacade.Instance.Requests.Count);
						}
						goto IL_01f4;
					}
					if (FyberFacade.Instance.Requests.All((Task<Ad> r) => ((Task)r).IsCompleted))
					{
						if (Defs.IsDeveloperBuild)
						{
							UnityEngine.Debug.Log("All requests are completed.");
						}
						goto IL_01f4;
					}
					if (Time.realtimeSinceStartup - _003CloadAttemptTime_003E5__1 > 5.2f)
					{
						if (Defs.IsDeveloperBuild)
						{
							UnityEngine.Debug.Log("Loading timed out.");
						}
						goto IL_01f4;
					}
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					result = true;
					goto end_IL_0000;
					IL_01f4:
					list = FyberFacade.Instance.Requests.Where((Task<Ad> r) => ((Task)r).IsCompleted).ToList();
					list2 = list.Where((Task<Ad> r) => ((Task)r).IsFaulted && ((Exception)(object)((Task)r).Exception).InnerException is AdNotAwailableException).ToList();
					if (list2.Count > 0)
					{
						if (Defs.IsDeveloperBuild)
						{
							UnityEngine.Debug.Log("Removing not filled requests: " + list2.Count);
						}
						foreach (Task<Ad> item in list2)
						{
							FyberFacade.Instance.Requests.Remove(item);
							list = null;
						}
					}
					if (list == null)
					{
						list = FyberFacade.Instance.Requests.Where((Task<Ad> r) => ((Task)r).IsCompleted).ToList();
					}
					list3 = list.Where((Task<Ad> r) => ((Task)r).IsFaulted && ((Exception)(object)((Task)r).Exception).InnerException is AdRequestException).ToList();
					if (list3.Count > 0)
					{
						if (Defs.IsDeveloperBuild)
						{
							UnityEngine.Debug.Log("Removing failed requests: " + list3.Count);
						}
						foreach (Task<Ad> item2 in list3)
						{
							FyberFacade.Instance.Requests.Remove(item2);
							list = null;
						}
					}
					if (!(_003C_003E4__this.mainPanel != null))
					{
						break;
					}
					goto IL_03bf;
				}
				if (!isReturnFromGame)
				{
					UnityEngine.Debug.Log("Not in appropriate state: ");
					result = false;
					goto IL_052d;
				}
				if (BannerWindowController.SharedController != null && BannerWindowController.SharedController.viewedBannersCountInConnectScene != 0)
				{
					UnityEngine.Debug.LogFormat("Cancel showing interstitial. count: {0}", BannerWindowController.SharedController.viewedBannersCountInConnectScene);
					result = false;
					goto IL_052d;
				}
				if (PhotonNetwork.inRoom)
				{
					UnityEngine.Debug.Log("Cancel showing interstitial. PhotonNetwork.inRoom");
					result = false;
					goto IL_052d;
				}
				if (BannerWindowController.SharedController != null)
				{
					BannerWindowController.SharedController.viewedBannersCountInConnectScene++;
				}
				Dictionary<string, string> eventParams = new Dictionary<string, string>
				{
					{ "af_content_type", "Interstitial" },
					{ "af_content_id", "Interstitial (ConnectScene)" }
				};
				AnalyticsFacade.SendCustomEventToAppsFlyer("af_content_view", eventParams);
				MenuBackgroundMusic.sharedMusic.Stop();
				Task<AdResult> obj = FyberFacade.Instance.ShowInterstitial(new Dictionary<string, string> { { "Context", "Connect Scene" } }, "WaitLoadingAndShowInterstitialCoroutine()");
				InterstitialCounter.Instance.IncrementRealInterstitialCount();
				Storager.setInt("PendingInterstitial", 8);
				obj.ContinueWith((Action<Task<AdResult>>)delegate(Task<AdResult> t)
				{
					MenuBackgroundMusic.sharedMusic.Play();
					Storager.setInt("PendingInterstitial", 0);
					_003C_003E4__this.isStartShowAdvert = false;
					if (((Task)t).IsFaulted)
					{
						UnityEngine.Debug.LogWarningFormat("[Rilisoft] Showing interstitial failed: {0}", ((Exception)(object)((Task)t).Exception).InnerException.Message);
					}
				});
				_003C_003E4__this._lastTimeInterstitialShown = Time.realtimeSinceStartup;
				_003C_003Em__Finally1();
				result = false;
				goto end_IL_0000;
				IL_052d:
				_003C_003Em__Finally1();
				end_IL_0000:;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
			return result;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			_003C_003E4__this.loadAdmobRunning = false;
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log("Finishing WaitLoadingAndShowInterstitialCoroutine()    " + InterstitialManager.Instance.Provider);
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	internal sealed class _003CShowReview_003Ed__171 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

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
		public _003CShowReview_003Ed__171(int _003C_003E1__state)
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
				_003C_003E2__current = new WaitForSeconds(1f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if (NeedShowReviewInConnectScene)
				{
					NeedShowReviewInConnectScene = false;
					ReviewHUDWindow.Instance.ShowWindowRating();
				}
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

	[CompilerGenerated]
	internal sealed class _003CStartControllInterstitial_003Ed__172 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ConnectScene _003C_003E4__this;

		private float _003CstartWaitingTime_003E5__1;

		private double _003CdelayInSeconds_003E5__2;

		private string _003CdismissReason_003E5__3;

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
		public _003CStartControllInterstitial_003Ed__172(int _003C_003E1__state)
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
				goto IL_00ca;
			}
			_003C_003E1__state = -1;
			_003CdismissReason_003E5__3 = GetReasonToDismissInterstitialConnectScene();
			if (string.IsNullOrEmpty(_003CdismissReason_003E5__3))
			{
				UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=magenta>{0}.LoadMapPreview(), InterstitialRequest: {1}</color>" : "{0}.LoadMapPreview(), InterstitialRequest: {1}", _003C_003E4__this.GetType().Name, InterstitialRequest);
				if (InterstitialRequest)
				{
					AdsConfigMemento lastLoadedConfig = AdsConfigManager.Instance.LastLoadedConfig;
					string playerCategory = AdsConfigManager.GetPlayerCategory(lastLoadedConfig);
					ReturnInConnectSceneAdPointMemento returnInConnectScene = lastLoadedConfig.AdPointsConfig.ReturnInConnectScene;
					_003CdelayInSeconds_003E5__2 = returnInConnectScene.GetFinalDelayInSeconds(playerCategory);
					_003CstartWaitingTime_003E5__1 = Time.realtimeSinceStartup;
					goto IL_00ca;
				}
			}
			else
			{
				UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=magenta>Dismissing wait for interstitial. {0}</color>" : "Dismissing wait for interstitial. {0}", _003CdismissReason_003E5__3);
			}
			goto IL_0108;
			IL_00ca:
			if ((double)(Time.realtimeSinceStartup - _003CstartWaitingTime_003E5__1) < _003CdelayInSeconds_003E5__2)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			goto IL_0108;
			IL_0108:
			InterstitialRequest = false;
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
	internal sealed class _003CSetFonLoadingWaitForReset_003Ed__199 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ConnectScene _003C_003E4__this;

		public string _mapName;

		public bool isAddCountRun;

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
		public _003CSetFonLoadingWaitForReset_003Ed__199(int _003C_003E1__state)
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
				_003C_003E4__this.GetMapName(_mapName, isAddCountRun);
				if (_003C_003E4__this._loadingNGUIController != null)
				{
					UnityEngine.Object.Destroy(_003C_003E4__this._loadingNGUIController.gameObject);
					_003C_003E4__this._loadingNGUIController = null;
				}
				goto IL_0086;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0086;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0086:
				if (WeaponManager.sharedManager == null)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				break;
			}
			if (WeaponManager.sharedManager.LockGetWeaponPrefabs > 0)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			}
			_003C_003E4__this.ShowLoadingGUI(_mapName);
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
	internal sealed class _003CMoveToGameScene_003Ed__203 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string _goMapName;

		public ConnectScene _003C_003E4__this;

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
		public _003CMoveToGameScene_003Ed__203(int _003C_003E1__state)
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
				UnityEngine.Debug.Log("MoveToGameScene=" + _goMapName);
				Defs.isGameFromFriends = false;
				Defs.isGameFromClans = false;
				if (WeaponManager.sharedManager != null)
				{
					WeaponManager.sharedManager.Reset(Defs.filterMaps.ContainsKey(_goMapName) ? Defs.filterMaps[_goMapName] : 0);
				}
				GlobalGameController.countKillsBlue = 0;
				GlobalGameController.countKillsRed = 0;
				goto IL_00b5;
			case 1:
				_003C_003E1__state = -1;
				goto IL_00b5;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Resources.UnloadUnusedAssets();
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.SetFonLoadingWaitForReset(_goMapName, true));
				_003C_003E1__state = 4;
				return true;
			case 4:
			{
				_003C_003E1__state = -1;
				_003C_003E4__this.loadingMapPanel.SetActive(true);
				_003C_003E4__this.isGoInPhotonGame = true;
				AsyncOperation asyncOperation = Singleton<SceneLoader>.Instance.LoadSceneAsync(Defs.PromSceneName);
				if (FriendsController.sharedController != null)
				{
					FriendsController.sharedController.GetFriendsData();
				}
				_003C_003E2__current = asyncOperation;
				_003C_003E1__state = 5;
				return true;
			}
			case 5:
				{
					_003C_003E1__state = -1;
					for (int i = 0; i < _003C_003E4__this.grid.transform.childCount; i++)
					{
						UnityEngine.Object.Destroy(_003C_003E4__this.grid.transform.GetChild(i).gameObject);
					}
					_003C_003E4__this.mapPreview.Clear();
					return false;
				}
				IL_00b5:
				if (PhotonNetwork.room == null)
				{
					_003C_003E2__current = 0;
					_003C_003E1__state = 1;
					return true;
				}
				PlayerPrefs.SetString("RoomName", PhotonNetwork.room.name);
				PhotonNetwork.isMessageQueueRunning = false;
				_003C_003E4__this.mapPreview.Clear();
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
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

	public const int TIME_MATCH_DEATH_ESCAPE_MINUTES = 10;

	public GameObject armoryButton;

	public UILabel rulesLabel;

	public UITable customButtonsGrid;

	public List<infoServer> servers = new List<infoServer>();

	private float posNumberOffPlayersX = -139f;

	private string goMapName;

	public static GameConnect.GameMode curSelectMode;

	public GameObject mapPreviewTexture;

	public GameObject grid;

	public Transform ScrollTransform;

	public Transform selectMapPanelTransform;

	public MapPreviewController selectMap;

	public float widthCell;

	public int countMap;

	public UISprite fonMapPreview;

	public UIPanel mapPreviewPanel;

	public GameObject mainPanel;

	public GameObject localBtn;

	public GameObject customBtn;

	public GameObject randomBtn;

	public GameObject goBtn;

	public GameObject backBtn;

	public GameObject connectToPhotonPanel;

	public GameObject failInternetLabel;

	public GameObject loadingMapPanel;

	public GameObject selectMapPanel;

	public Transform scrollViewSelectMapTransform;

	public UITexture loadingToDraw;

	public static bool isReturnFromGame;

	private bool isSetUseMap;

	public string gameNameFilter;

	public List<GameObject> gamesInfo = new List<GameObject>();

	public DisableObjectFromTimer serverIsNotAvalible;

	public DisableObjectFromTimer accountBlockedLabel;

	private float timerShowBan = -1f;

	private bool isConnectingToPhoton;

	private bool isCancelConnectingToPhoton;

	private int pressButton;

	private List<RoomInfo> filteredRoomList = new List<RoomInfo>();

	private int countNoteCaptureDeadmatch = 5;

	private int countNoteCaptureCOOP = 5;

	private int countNoteCaptureHunger = 5;

	private int countNoteCaptureFlag = 5;

	private int countNoteCaptureCompany = 5;

	public static ConnectScene sharedController;

	private string password = "";

	public LANBroadcastService lanScan;

	private RoomInfo joinRoomInfoFromCustom;

	private bool firstConnectToPhoton;

	private bool isGoInPhotonGame;

	private bool isMainPanelActiv = true;

	private AdvertisementController _advertisementController;

	public CategoryButtonsController categoryButtonsController;

	public BtnCategory deathmatchToggle;

	public BtnCategory teamFightToogle;

	public BtnCategory flagCaptureToogle;

	public BtnCategory capturePointsToogle;

	public BtnCategory duelToggle;

	public bool isStartShowAdvert;

	private Action actAfterConnectToPhoton;

	public const string PendingInterstitialKey = "PendingInterstitial";

	private GameInfo[] roomFields;

	public static Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> mapStatistics = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>();

	public static string selectedMap = "";

	public GameObject modeAnimObj;

	public GameObject finger;

	public BtnCategory[] modeButtonByLevel;

	public UILabel[] modeUnlockLabelByLevel;

	public BtnCategory modeButtonDuel;

	public UILabel modeDuelRatingNeedLabel;

	public GameObject modeDuelRatingNeed;

	private bool inCustomPanel;

	private Texture randomMapPreview;

	private Dictionary<string, Texture> mapPreview = new Dictionary<string, Texture>();

	private string _folderPreviews = string.Empty;

	private bool fingerStopped;

	private bool animationStarted;

	private bool _stopFingerAnimation;

	private bool loadReplaceAdmobPerelivRunning;

	private bool loadAdmobRunning;

	private int _countOfLoopsRequestAdThisTime;

	private float _lastTimeInterstitialShown;

	public static bool NeedShowReviewInConnectScene = false;

	private Vector3 startPosNameServerNameInput = Vector3.zero;

	private bool cantCancel;

	private IDisposable _someWindowSubscription;

	private bool isInitCustomPanel;

	private int _tempMinValue = 3;

	private int _tempMaxValue = 7;

	private int _tempStep = 2;

	private int daterStep = 5;

	private int daterMinValue = 5;

	private int daterMaxValue = 10;

	private int levelCurrentRegim = -1;

	private bool secondOnEnable;

	private IDisposable _backSubscription;

	private int countNote = 1;

	private string _logCache = string.Empty;

	private float startPosX;

	private bool refreshMapsPanel;

	private int maxcount = 1;

	private bool isFirstGamesReposition;

	private int countColumn = 3;

	private float _widthCell = 282f;

	private float _heightCell = 1f;

	private float _scale = 1f;

	private float borderWidth = 10f;

	private LoadingNGUIController _loadingNGUIController;

	private LANBroadcastService.ReceivedMessage[] _copy;

	private Vector3 posSelectMapPanelInMainMenu = Vector3.up * 10000f;

	public static bool isEnable { get; private set; }

	internal static bool InterstitialRequest { get; set; }

	internal static bool ReplaceAdmobWithPerelivRequest { get; set; }

	private Texture GetMapTexture(string mapName)
	{
		if (mapPreview.ContainsKey(mapName))
		{
			return mapPreview[mapName];
		}
		Texture texture = Resources.Load(string.Format("LevelLoadingsPreview{0}/Loading_{1}", new object[2] { _folderPreviews, mapName })) as Texture;
		if (texture != null)
		{
			mapPreview.Add(mapName, texture);
		}
		return texture;
	}

	public static Texture MainLoadingTexture()
	{
		if (BackgroundsManager.LoadingBackgroudTexture != null)
		{
			return BackgroundsManager.LoadingBackgroudTexture;
		}
		return Resources.Load<Texture>(Device.isRetinaAndStrong ? "main_loading_Hi" : "main_loading");
	}

	public static void GoToClans()
	{
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = "Clans";
		LoadConnectScene.noteToShow = null;
		SceneManager.LoadScene(Defs.PromSceneName);
	}

	public static void GoToFriends()
	{
		FriendsController friendsController = FriendsController.sharedController;
		if (friendsController != null)
		{
			friendsController.GetFriendsData();
		}
		MainMenuController.friendsOnStart = true;
		GoToLobby();
	}

	public static void GoToLobby()
	{
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = Defs.MainMenuScene;
		LoadConnectScene.noteToShow = null;
		SceneManager.LoadScene(Defs.PromSceneName);
		GameConnect.SetGameMode(GameConnect.GameMode.Deathmatch);
	}

	public static void Local()
	{
		GameConnect.Disconnect();
		if (Defs.isGameFromFriends)
		{
			GoToFriends();
		}
		else if (MainMenuController.NavigateToMinigame.HasValue)
		{
			GoToLobby();
		}
		else if (Defs.isGameFromClans)
		{
			GoToClans();
		}
		else
		{
			GoToLobby();
		}
	}

	public static void GoToProfile()
	{
		PlayerPrefs.SetInt(Defs.SkinEditorMode, 1);
		GlobalGameController.EditingLogo = 0;
		GlobalGameController.EditingCape = 0;
		SceneManager.LoadScene("SkinEditor");
	}

	public void StopFingerAnim()
	{
		if (finger != null && finger.activeSelf)
		{
			fingerStopped = true;
			finger.SetActive(false);
			UIScrollView component = scrollViewSelectMapTransform.GetComponent<UIScrollView>();
			component.onDragStarted = (UIScrollView.OnDragNotification)Delegate.Remove(component.onDragStarted, new UIScrollView.OnDragNotification(StopFingerAnim));
		}
	}

	private void OnEnableWhenAnimate()
	{
		if (animationStarted)
		{
			StopFingerAnim();
			if (modeAnimObj != null)
			{
				modeAnimObj.SetActive(false);
			}
			fingerStopped = false;
		}
		StartCoroutine(AnimateModeOpen());
	}

	private void CreateFinger()
	{
		GameObject original = Resources.Load<GameObject>("ConnectScene/ContainerFingerAnimInConnectScene");
		finger = UnityEngine.Object.Instantiate(original);
		finger.transform.parent = base.transform.GetChild(0);
		finger.transform.localScale = Vector3.one;
		finger.transform.localPosition = Vector3.zero;
	}

	private void CreateAnimUnlock()
	{
		GameObject original = Resources.Load<GameObject>("ConnectScene/UnlockAnimationInConnectScene");
		modeAnimObj = UnityEngine.Object.Instantiate(original);
		modeAnimObj.transform.parent = base.transform.GetChild(0);
		modeAnimObj.transform.localScale = Vector3.one;
		modeAnimObj.transform.localPosition = Vector3.zero;
		modeAnimObj.GetComponent<AudioSource>().enabled = Defs.isSoundFX;
	}

	private IEnumerator AnimateModeOpen()
	{
		animationStarted = true;
		localBtn.GetComponent<UIButton>().isEnabled = TrainingController.TrainingCompleted;
		randomBtn.GetComponent<UIButton>().isEnabled = TrainingController.TrainingCompleted;
		customBtn.GetComponent<UIButton>().isEnabled = TrainingController.TrainingCompleted;
		goBtn.GetComponent<UIButton>().isEnabled = TrainingController.TrainingCompleted;
		if (modeAnimObj != null)
		{
			modeAnimObj.SetActive(false);
		}
		int storagedStageDuel = Storager.getInt("ModeUnlockDuel");
		int currentStateDuel = ((RatingSystem.instance.currentRating >= 300) ? storagedStageDuel : 0);
		modeDuelRatingNeedLabel.text = 300.ToString();
		modeDuelRatingNeed.SetActive(RatingSystem.instance.currentRating < 300);
		modeButtonDuel.isEnable = currentStateDuel == 1;
		GameConnect.GameMode[] orderedSupportedModeArray = GameModeLocker.Instance.GetOrderedSupportedModes().ToArray();
		int supportedModeCount = orderedSupportedModeArray.Length;
		int storagedStage = Storager.getInt("ModeUnlockStage");
		if (Storager.getInt("TrainingCompleted_4_4_Sett") == 1)
		{
			storagedStage = supportedModeCount;
		}
		int currentStage = Mathf.Clamp(storagedStage, 0, supportedModeCount);
		for (int i = 0; i < modeButtonByLevel.Length; i++)
		{
			BtnCategory btnCategory = modeButtonByLevel[i];
			btnCategory.isEnable = !GameModeLocker.Instance.IsLocked(btnCategory.GameMode) && i < currentStage;
		}
		int currentLevel = ((ExperienceController.sharedController != null) ? ExperienceController.sharedController.currentLevel : 31);
		if (modeUnlockLabelByLevel != null)
		{
			for (int j = 1; j < modeButtonByLevel.Length; j++)
			{
				int num = j - 1;
				if (num >= modeUnlockLabelByLevel.Length)
				{
					break;
				}
				GameConnect.GameMode gameMode2 = modeButtonByLevel[j].GameMode;
				bool flag = GameModeLocker.Instance.IsLocked(gameMode2);
				modeUnlockLabelByLevel[num].gameObject.SetActive(flag);
				if (flag && gameMode2 != GameConnect.GameMode.Duel)
				{
					int unlockLevel = GameModeLocker.GetUnlockLevel(gameMode2);
					int num2 = Mathf.Clamp(unlockLevel, 0, 36);
					UnityEngine.Debug.LogFormat("level: {0}, clampedLevel: {1}, gameMode: {2}", unlockLevel, num2, gameMode2);
					modeUnlockLabelByLevel[num].text = string.Format(LocalizationStore.Get("Key_1923"), new object[1] { num2 });
				}
			}
		}
		if (currentStage < Mathf.Min(currentLevel, supportedModeCount))
		{
			BannerWindowController.SharedController.AddBannersTimeout(20.1f);
		}
		yield return new WaitForSeconds(0.8f);
		int upperBound = Mathf.Min(currentLevel, supportedModeCount);
		while (currentStage < upperBound)
		{
			BannerWindowController.SharedController.AddBannersTimeout(20.1f);
			if (currentStage == 1)
			{
				BannerWindowController.SharedController.ClearBannerStates();
			}
			GameConnect.GameMode gameMode = orderedSupportedModeArray[currentStage];
			int num3 = Array.FindIndex(modeButtonByLevel, (BtnCategory b) => b.GameMode == gameMode);
			if (num3 >= 0)
			{
				BtnCategory currentModeButton = modeButtonByLevel[num3];
				if (currentStage != 0)
				{
					if (modeAnimObj == null)
					{
						CreateAnimUnlock();
					}
					modeAnimObj.transform.SetParent(categoryButtonsController.transform.parent);
					modeAnimObj.transform.position = currentModeButton.transform.position;
					modeAnimObj.transform.localScale = currentModeButton.transform.localScale;
					modeAnimObj.SetActive(true);
					yield return new WaitForSeconds(0.1f);
					currentModeButton.isEnable = true;
					yield return new WaitForSeconds(1.4f);
					modeAnimObj.SetActive(false);
				}
				if (currentStage == 0 && !TrainingController.TrainingCompleted)
				{
					if (finger == null)
					{
						CreateFinger();
					}
					UIScrollView component = scrollViewSelectMapTransform.GetComponent<UIScrollView>();
					component.onDragStarted = (UIScrollView.OnDragNotification)Delegate.Combine(component.onDragStarted, new UIScrollView.OnDragNotification(StopFingerAnim));
					AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Connect_Scene);
					yield return FingerAnimationCoroutine();
				}
				if (currentStage == 1)
				{
					HintController.instance.ShowHintByName("deathmatch");
					HintController.instance.ShowHintByName("gobattletimeout");
				}
				Storager.setInt("ModeUnlockStage", currentStage + 1);
			}
			int num4 = currentStage + 1;
			currentStage = num4;
		}
		if (storagedStage != currentStage)
		{
			Storager.setInt("ModeUnlockStage", currentStage);
		}
		if (currentStateDuel == 0 && RatingSystem.instance.currentRating >= 300)
		{
			if (modeAnimObj == null)
			{
				CreateAnimUnlock();
			}
			modeAnimObj.transform.SetParent(categoryButtonsController.transform.parent);
			modeAnimObj.transform.position = modeButtonDuel.transform.position;
			modeAnimObj.SetActive(true);
			yield return new WaitForSeconds(0.1f);
			modeButtonDuel.isEnable = true;
			yield return new WaitForSeconds(1.4f);
			modeAnimObj.SetActive(false);
			currentStateDuel = 1;
		}
		if (storagedStageDuel != currentStateDuel)
		{
			Storager.setInt("ModeUnlockDuel", currentStateDuel);
		}
		if (!TrainingController.TrainingCompleted)
		{
			goBtn.GetComponent<UIButton>().isEnabled = true;
			HintController.instance.ShowHintByName("gobattle");
		}
		animationStarted = false;
	}

	public void StopFingerAnimation()
	{
		if (finger != null)
		{
			finger.SetActive(false);
		}
		_stopFingerAnimation = true;
	}

	private IEnumerator FingerAnimationCoroutine()
	{
		finger.SetActive(true);
		string fromName = grid.transform.GetChild(1).GetComponent<MapPreviewController>().sceneMapName;
		string toName = grid.transform.GetChild(2).GetComponent<MapPreviewController>().sceneMapName;
		yield return finger.MoveOverTime(finger.transform.position, grid.transform.GetChild(1).transform.position, 1f);
		if (_stopFingerAnimation)
		{
			yield break;
		}
		Animator fingerAnimator = finger.GetComponentInChildren<Animator>(true);
		fingerAnimator.SetTrigger("touch");
		yield return new WaitForSeconds(0.8f);
		if (_stopFingerAnimation)
		{
			yield break;
		}
		SelectMap(fromName);
		yield return new WaitForSeconds(1f);
		if (!_stopFingerAnimation)
		{
			yield return finger.MoveOverTime(grid.transform.GetChild(1).transform.position, grid.transform.GetChild(2).transform.position, 1f);
			fingerAnimator.SetTrigger("touch");
			yield return new WaitForSeconds(0.8f);
			if (!_stopFingerAnimation)
			{
				SelectMap(toName);
				yield return new WaitForSeconds(1f);
				finger.gameObject.SetActive(false);
			}
		}
	}

	private void PreLoadMapPreviewsTextures()
	{
		List<SceneInfo> list = new List<SceneInfo>();
		GameConnect.GameMode[] array = new GameConnect.GameMode[5]
		{
			GameConnect.GameMode.Deathmatch,
			GameConnect.GameMode.TeamFight,
			GameConnect.GameMode.FlagCapture,
			GameConnect.GameMode.CapturePoints,
			GameConnect.GameMode.Duel
		};
		foreach (GameConnect.GameMode needMode in array)
		{
			AllScenesForMode allScenesForMode = ((SceneInfoController.instance != null) ? SceneInfoController.instance.GetListScenesForMode(needMode) : null);
			if (allScenesForMode != null)
			{
				list.AddRange(allScenesForMode.avaliableScenes);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			GetMapTexture(list[j].NameScene);
		}
	}

	private void Awake()
	{
		sharedController = this;
		if (isReturnFromGame)
		{
			Defs.countReturnInConnectScene++;
		}
		SceneInfoController.instance.UpdateListAvaliableMap();
		_folderPreviews = (Device.isRetinaAndStrong ? "/Hi" : string.Empty);
		randomMapPreview = Resources.Load(string.Format("LevelLoadingsPreview{0}/Random_Map", new object[1] { _folderPreviews })) as Texture;
		GameObject original = Resources.Load("ConnectPanel") as GameObject;
		connectToPhotonPanel = UnityEngine.Object.Instantiate(original);
		connectToPhotonPanel.transform.SetParent(mainPanel.transform.parent, false);
		connectToPhotonPanel.SetActive(false);
		failInternetLabel = connectToPhotonPanel.GetComponent<ConnectPanel>().failInternetLabel;
		if (connectToPhotonPanel != null)
		{
			ButtonHandler cancelButton = connectToPhotonPanel.GetComponent<ConnectPanel>().cancelButton;
			if (cancelButton != null)
			{
				cancelButton.Clicked += HandleCancelFromConnectToPhotonBtnClicked;
			}
		}
		PreLoadMapPreviewsTextures();
	}

	private void Start()
	{
		if (mapPreviewTexture.activeSelf)
		{
			mapPreviewTexture.SetActive(false);
		}
		if (!mainPanel.activeSelf)
		{
			mainPanel.SetActive(true);
		}
		if (!selectMapPanel.activeSelf)
		{
			selectMapPanel.SetActive(true);
		}
		if (accountBlockedLabel.gameObject.activeSelf)
		{
			accountBlockedLabel.gameObject.SetActive(false);
		}
		if (serverIsNotAvalible.gameObject.activeSelf)
		{
			serverIsNotAvalible.gameObject.SetActive(false);
		}
		GlobalGameController.CountKills = 0;
		GlobalGameController.Score = 0;
		StartSearchLocalServers();
		int @int = PlayerPrefs.GetInt("RegimMulty", 2);
		GameConnect.GameMode gameMode = ((!TrainingController.TrainingCompleted) ? GameConnect.GameMode.TeamFight : ((GameConnect.GameMode)@int));
		if (gameMode == GameConnect.GameMode.Duel && RatingSystem.instance.currentRating < 300)
		{
			gameMode = GameConnect.GameMode.TeamFight;
		}
		GameConnect.SetGameMode(gameMode);
		teamFightToogle.wasPressed = false;
		deathmatchToggle.wasPressed = false;
		flagCaptureToogle.wasPressed = false;
		capturePointsToogle.wasPressed = false;
		duelToggle.wasPressed = false;
		if (gameMode == GameConnect.GameMode.Deathmatch)
		{
			categoryButtonsController.BtnClicked(deathmatchToggle.btnName, true);
		}
		if (gameMode == GameConnect.GameMode.FlagCapture)
		{
			categoryButtonsController.BtnClicked(flagCaptureToogle.btnName, true);
		}
		if (gameMode == GameConnect.GameMode.Duel)
		{
			categoryButtonsController.BtnClicked(duelToggle.btnName, true);
		}
		if (gameMode == GameConnect.GameMode.TeamFight)
		{
			categoryButtonsController.BtnClicked(teamFightToogle.btnName, true);
		}
		if (gameMode == GameConnect.GameMode.CapturePoints)
		{
			categoryButtonsController.BtnClicked(capturePointsToogle.btnName, true);
		}
		deathmatchToggle.Clicked += SetRegimDeathmatch;
		teamFightToogle.Clicked += SetRegimTeamFight;
		flagCaptureToogle.Clicked += SetRegimFlagCapture;
		capturePointsToogle.Clicked += SetRegimCapturePoints;
		duelToggle.Clicked += SetRegimDuel;
		GameConnect.GameMode gameMode2 = GameConnect.gameMode;
		selectMapPanel.GetComponent<UIPanel>().ResetAndUpdateAnchors();
		fonMapPreview.ResetAndUpdateAnchors();
		SetRegim(gameMode2);
		if (isReturnFromGame || MainMenuController.isShowConnectSceneOnStart)
		{
			base.gameObject.SetActive(true);
			MainMenuController.sharedController.mainPanel.SetActive(false);
			MainMenuHeroCamera.Instance.MainCamera.enabled = false;
			NickLabelStack.sharedStack.gameObject.SetActive(false);
			Defs.isMulti = true;
			MenuBackgroundMusic.keepPlaying = true;
			isEnable = true;
			ShowInterstitial();
		}
		else
		{
			base.gameObject.SetActive(false);
			isEnable = false;
		}
	}

	private void ShowInterstitial()
	{
		InitializeBannerWindow();
		InterstitialManager.Instance.ResetAdProvider();
		string text = string.Empty;
		if (NeedShowReviewInConnectScene)
		{
			text = "NeedShowReviewInConnectScene";
		}
		else if (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64)
		{
			string text2 = (ReplaceAdmobWithPerelivRequest ? GetReasonToDismissFakeInterstitial() : "Replace interstitial request not performed.");
			if (string.IsNullOrEmpty(text2))
			{
				ReplaceAdmobWithPerelivRequest = false;
				StartCoroutine(WaitLoadingAndShowReplaceAdmobPereliv("Connect Scene", false));
				text = "ReplaceAdmobWithPereliv";
			}
			else
			{
				UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=magenta>Dismissing fake interstitial. {0}</color>" : "Dismissing fake interstitial. {0}", text2);
				if (!InterstitialRequest)
				{
					text = "InterstitialRequest == false";
				}
				else
				{
					text = GetReasonToDismissInterstitialConnectScene();
					if (string.IsNullOrEmpty(text))
					{
						isStartShowAdvert = true;
						StartCoroutine(WaitLoadingAndShowInterstitialCoroutine("Connect Scene", false));
					}
				}
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=magenta>Dismissing interstitial. {0}</color>" : "Dismissing interstitial. {0}", text);
		}
	}

	private IEnumerator OnApplicationPause(bool pausing)
	{
		if (pausing)
		{
			lanScan.StopBroadCasting();
			yield break;
		}
		yield return new WaitForSeconds(1f);
		StartSearchLocalServers();
		InterstitialManager.Instance.ResetAdProvider();
		if (MobileAdManager.Instance.SuppressShowOnReturnFromPause)
		{
			MobileAdManager.Instance.SuppressShowOnReturnFromPause = false;
			yield break;
		}
		string reasonToDismissFakeInterstitial = GetReasonToDismissFakeInterstitial();
		if (string.IsNullOrEmpty(reasonToDismissFakeInterstitial))
		{
			ReplaceAdmobPerelivController.IncreaseTimesCounter();
			if (!loadAdmobRunning)
			{
				StartCoroutine(WaitLoadingAndShowReplaceAdmobPereliv("On return from pause to Connect Scene"));
			}
		}
		else
		{
			UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=magenta>Dismissing fake interstitial. {0}</color>" : "Dismissing fake interstitial. {0}", reasonToDismissFakeInterstitial);
		}
	}

	private IEnumerator WaitLoadingAndShowReplaceAdmobPereliv(string context, bool loadData = true)
	{
		if (loadReplaceAdmobPerelivRunning)
		{
			yield break;
		}
		try
		{
			loadReplaceAdmobPerelivRunning = true;
			if (loadData && !ReplaceAdmobPerelivController.sharedController.DataLoading && !ReplaceAdmobPerelivController.sharedController.DataLoaded)
			{
				ReplaceAdmobPerelivController.sharedController.LoadPerelivData();
			}
			while (ReplaceAdmobPerelivController.sharedController == null || !ReplaceAdmobPerelivController.sharedController.DataLoaded)
			{
				if (!ReplaceAdmobPerelivController.sharedController.DataLoading)
				{
					loadReplaceAdmobPerelivRunning = false;
					yield break;
				}
				yield return null;
			}
			if (mainPanel != null)
			{
				while (!mainPanel.activeInHierarchy)
				{
					yield return null;
				}
				yield return new WaitForSeconds(0.5f);
			}
			if (BannerWindowController.SharedController != null && BannerWindowController.SharedController.viewedBannersCountInConnectScene != 0)
			{
				UnityEngine.Debug.LogFormat("Cancel showing fake: {0}", BannerWindowController.SharedController.viewedBannersCountInConnectScene);
				yield break;
			}
			if (BannerWindowController.SharedController != null)
			{
				BannerWindowController.SharedController.viewedBannersCountInConnectScene++;
			}
			ReplaceAdmobPerelivController.TryShowPereliv(context);
			ReplaceAdmobPerelivController.sharedController.DestroyImage();
		}
		finally
		{
			loadReplaceAdmobPerelivRunning = false;
		}
	}

	private IEnumerator WaitLoadingAndShowInterstitialCoroutine(string context, bool loadData = true)
	{
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log("Starting WaitLoadingAndShowInterstitialCoroutine()    " + InterstitialManager.Instance.Provider);
		}
		if (loadAdmobRunning)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log("Quitting WaitLoadingAndShowInterstitialCoroutine() because loadAdmobRunning==true");
			}
			yield break;
		}
		loadAdmobRunning = true;
		try
		{
			float loadAttemptTime = Time.realtimeSinceStartup;
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log("FyberFacade.Instance.Requests.Count: " + FyberFacade.Instance.Requests.Count);
			}
			if (FyberFacade.Instance.Requests.Count == 0)
			{
				Task<Ad> value = FyberFacade.Instance.RequestImageInterstitial("WaitLoadingAndShowInterstitialCoroutine(), requests count: 0");
				FyberFacade.Instance.Requests.AddLast(value);
			}
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log("Waiting either at least one loading request completed successfully, or all failed...");
			}
			while (true)
			{
				if (FyberFacade.Instance.Requests.Any((Task<Ad> r) => ((Task)r).IsCompleted && !((Task)r).IsFaulted))
				{
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.LogFormat("Found successfully completed request among {0}", FyberFacade.Instance.Requests.Count);
					}
					break;
				}
				if (FyberFacade.Instance.Requests.All((Task<Ad> r) => ((Task)r).IsCompleted))
				{
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.Log("All requests are completed.");
					}
					break;
				}
				if (Time.realtimeSinceStartup - loadAttemptTime > 5.2f)
				{
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.Log("Loading timed out.");
					}
					break;
				}
				yield return null;
			}
			List<Task<Ad>> list = FyberFacade.Instance.Requests.Where((Task<Ad> r) => ((Task)r).IsCompleted).ToList();
			List<Task<Ad>> list2 = list.Where((Task<Ad> r) => ((Task)r).IsFaulted && ((Exception)(object)((Task)r).Exception).InnerException is AdNotAwailableException).ToList();
			if (list2.Count > 0)
			{
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.Log("Removing not filled requests: " + list2.Count);
				}
				foreach (Task<Ad> item in list2)
				{
					FyberFacade.Instance.Requests.Remove(item);
					list = null;
				}
			}
			if (list == null)
			{
				list = FyberFacade.Instance.Requests.Where((Task<Ad> r) => ((Task)r).IsCompleted).ToList();
			}
			List<Task<Ad>> list3 = list.Where((Task<Ad> r) => ((Task)r).IsFaulted && ((Exception)(object)((Task)r).Exception).InnerException is AdRequestException).ToList();
			if (list3.Count > 0)
			{
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.Log("Removing failed requests: " + list3.Count);
				}
				foreach (Task<Ad> item2 in list3)
				{
					FyberFacade.Instance.Requests.Remove(item2);
				}
			}
			if (mainPanel != null)
			{
				while (!mainPanel.activeInHierarchy)
				{
					yield return null;
				}
				yield return new WaitForSeconds(0.5f);
			}
			if (!isReturnFromGame)
			{
				UnityEngine.Debug.Log("Not in appropriate state: ");
				yield break;
			}
			if (BannerWindowController.SharedController != null && BannerWindowController.SharedController.viewedBannersCountInConnectScene != 0)
			{
				UnityEngine.Debug.LogFormat("Cancel showing interstitial. count: {0}", BannerWindowController.SharedController.viewedBannersCountInConnectScene);
				yield break;
			}
			if (PhotonNetwork.inRoom)
			{
				UnityEngine.Debug.Log("Cancel showing interstitial. PhotonNetwork.inRoom");
				yield break;
			}
			if (BannerWindowController.SharedController != null)
			{
				BannerWindowController.SharedController.viewedBannersCountInConnectScene++;
			}
			Dictionary<string, string> eventParams = new Dictionary<string, string>
			{
				{ "af_content_type", "Interstitial" },
				{ "af_content_id", "Interstitial (ConnectScene)" }
			};
			AnalyticsFacade.SendCustomEventToAppsFlyer("af_content_view", eventParams);
			MenuBackgroundMusic.sharedMusic.Stop();
			Task<AdResult> obj = FyberFacade.Instance.ShowInterstitial(new Dictionary<string, string> { { "Context", "Connect Scene" } }, "WaitLoadingAndShowInterstitialCoroutine()");
			InterstitialCounter.Instance.IncrementRealInterstitialCount();
			Storager.setInt("PendingInterstitial", 8);
			obj.ContinueWith((Action<Task<AdResult>>)delegate(Task<AdResult> t)
			{
				MenuBackgroundMusic.sharedMusic.Play();
				Storager.setInt("PendingInterstitial", 0);
				isStartShowAdvert = false;
				if (((Task)t).IsFaulted)
				{
					UnityEngine.Debug.LogWarningFormat("[Rilisoft] Showing interstitial failed: {0}", ((Exception)(object)((Task)t).Exception).InnerException.Message);
				}
			});
			_lastTimeInterstitialShown = Time.realtimeSinceStartup;
		}
		finally
		{
			loadAdmobRunning = false;
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.Log("Finishing WaitLoadingAndShowInterstitialCoroutine()    " + InterstitialManager.Instance.Provider);
			}
		}
	}

	private void InitializeBannerWindow()
	{
		_advertisementController = base.gameObject.GetComponent<AdvertisementController>();
		if (_advertisementController == null)
		{
			_advertisementController = base.gameObject.AddComponent<AdvertisementController>();
		}
		BannerWindowController.SharedController.advertiseController = _advertisementController;
	}

	private void SetUnLockedButton(UIToggle butToogle)
	{
		UIButton component = butToogle.gameObject.GetComponent<UIButton>();
		component.normalSprite = "yell_btn";
		component.hoverSprite = "yell_btn";
		component.pressedSprite = "green_btn_n";
		butToogle.transform.Find("LockedSprite").gameObject.SetActive(false);
		butToogle.transform.Find("Checkmark").GetComponent<UISprite>().spriteName = "green_btn";
	}

	public void SetRegimDeathmatch(object sender, EventArgs e)
	{
		if (HintController.instance != null)
		{
			HintController.instance.HideHintByName("deathmatch");
		}
		if (GameConnect.gameMode != 0)
		{
			SetRegim(GameConnect.GameMode.Deathmatch);
		}
	}

	private void SetRegimTeamFight(object sender, EventArgs e)
	{
		if (GameConnect.gameMode != GameConnect.GameMode.TeamFight)
		{
			SetRegim(GameConnect.GameMode.TeamFight);
		}
	}

	private void SetRegimFlagCapture(object sender, EventArgs e)
	{
		if (HintController.instance != null)
		{
			HintController.instance.HideHintByName("deathmatch");
		}
		if (GameConnect.gameMode != GameConnect.GameMode.FlagCapture)
		{
			SetRegim(GameConnect.GameMode.FlagCapture);
		}
	}

	private void SetRegimCapturePoints(object sender, EventArgs e)
	{
		if (HintController.instance != null)
		{
			HintController.instance.HideHintByName("deathmatch");
		}
		if (GameConnect.gameMode != GameConnect.GameMode.CapturePoints)
		{
			SetRegim(GameConnect.GameMode.CapturePoints);
		}
	}

	private void SetRegimDuel(object sender, EventArgs e)
	{
		if (GameConnect.gameMode != GameConnect.GameMode.Duel)
		{
			SetRegim(GameConnect.GameMode.Duel);
		}
	}

	public void HandleJoinRoomFromEnterPasswordBtnClicked(object sender, EventArgs e)
	{
		if (CustomPanelConnectScene.Instance.enterPasswordInput.value.Equals(joinRoomInfoFromCustom.customProperties[GameConnect.passwordProperty].ToString()))
		{
			JoinToRoomPhotonAfterCheck();
			return;
		}
		CustomPanelConnectScene.Instance.enterPasswordPanel.SetActive(false);
		if (ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.isShowRanks = true;
		}
		CustomPanelConnectScene.Instance.customPanel.SetActive(true);
		Invoke("UpdateFilteredRoomListInvoke", 0.03f);
	}

	private void OnPaswordSelected()
	{
		password = CustomPanelConnectScene.Instance.setPasswordInput.value;
	}

	public void OnJoinToMap(string mapName)
	{
		goMapName = mapName;
		if (!string.IsNullOrEmpty(goMapName))
		{
			if (WeaponManager.sharedManager != null)
			{
				WeaponManager.sharedManager.Reset(Defs.filterMaps.ContainsKey(goMapName) ? Defs.filterMaps[goMapName] : 0);
			}
			StartCoroutine(SetFonLoadingWaitForReset(goMapName));
			loadingMapPanel.SetActive(true);
			ActivityIndicator.IsActiveIndicator = true;
		}
	}

	public void HandleCreateRoomBtnClicked(object sender, EventArgs e)
	{
		if (Storager.getInt("localMultyKey") == 1)
		{
			Defs.isInet = false;
			if (selectMap.mapID == -1)
			{
				selectMap.mapID = GameConnect.GetRandomMapIndex(GameConnect.gameMode);
			}
		}
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(selectMap.mapID);
		if (infoScene == null)
		{
			return;
		}
		string text = infoScene.gameObject.name;
		if (infoScene.isPremium && Storager.getInt(text + "Key") == 0 && !PremiumAccountController.MapAvailableDueToPremiumAccount(text))
		{
			GameConnect.Disconnect();
			return;
		}
		string text2 = FilterBadWorld.FilterString(CustomPanelConnectScene.Instance.nameServerInput.value);
		bool flag = false;
		if (Defs.isInet)
		{
			RoomInfo[] roomList = PhotonNetwork.GetRoomList();
			for (int i = 0; i < roomList.Length; i++)
			{
				if (roomList[i].name.Equals(text2))
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			CustomPanelConnectScene.Instance.nameAlreadyUsedLabel.timer = 3f;
			CustomPanelConnectScene.Instance.nameAlreadyUsedLabel.gameObject.SetActive(true);
			return;
		}
		goMapName = text;
		PlayerPrefs.SetString("MapName", goMapName);
		PlayerPrefs.SetString("MaxKill", "4");
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset(Defs.filterMaps.ContainsKey(text) ? Defs.filterMaps[text] : 0);
		}
		StartCoroutine(SetFonLoadingWaitForReset(goMapName));
		loadingMapPanel.SetActive(true);
		int maxPlayersForMode = GameConnect.GetMaxPlayersForMode(GameConnect.gameMode);
		int matchMinutesForMode = GameConnect.GetMatchMinutesForMode(GameConnect.gameMode);
		if (Defs.isInet)
		{
			loadingMapPanel.SetActive(true);
			ActivityIndicator.IsActiveIndicator = true;
			GameConnect.CreateGameRoom(text2, maxPlayersForMode, infoScene.indexMap, matchMinutesForMode, CustomPanelConnectScene.Instance.setPasswordInput.value, GameConnect.gameMode);
		}
		else
		{
			PlayerPrefs.SetString("ServerName", string.IsNullOrEmpty(text2) ? "Server" : text2);
			PlayerPrefs.SetString("PlayersLimits", maxPlayersForMode.ToString());
			Singleton<SceneLoader>.Instance.LoadSceneAsync(Defs.PromSceneName);
		}
	}

	private void HandleGoToCreateRoomBtnClicked(object sender, EventArgs e)
	{
		password = string.Empty;
		SetPosSelectMapPanelInCreatePanel();
		CustomPanelConnectScene.Instance.createPanel.SetActive(true);
		CustomPanelConnectScene.Instance.setPasswordInput.gameObject.SetActive(Defs.isInet);
		CustomPanelConnectScene.Instance.nameServerInput.transform.localPosition = (Defs.isInet ? startPosNameServerNameInput : new Vector3(0f, startPosNameServerNameInput.y, startPosNameServerNameInput.z));
		selectMapPanel.SetActive(true);
		SetUseMasMap(false);
		CustomPanelConnectScene.Instance.customPanel.SetActive(false);
		CustomPanelConnectScene.Instance.nameAlreadyUsedLabel.timer = -1f;
		CustomPanelConnectScene.Instance.nameAlreadyUsedLabel.gameObject.SetActive(false);
	}

	private void HandleShowSearchPanelBtnClicked(object sender, EventArgs e)
	{
		CustomPanelConnectScene.Instance.customPanel.SetActive(false);
		if (CustomPanelConnectScene.Instance.searchInput != null)
		{
			CustomPanelConnectScene.Instance.searchInput.value = gameNameFilter;
		}
		CustomPanelConnectScene.Instance.searchPanel.SetActive(true);
	}

	public void HandleSearchBtnClicked(object sender, EventArgs e)
	{
		CustomPanelConnectScene.Instance.customPanel.SetActive(true);
		if (CustomPanelConnectScene.Instance.searchInput != null)
		{
			gameNameFilter = CustomPanelConnectScene.Instance.searchInput.value;
		}
		updateFilteredRoomList(gameNameFilter);
		CustomPanelConnectScene.Instance.scrollGames.ResetPosition();
		CustomPanelConnectScene.Instance.searchPanel.SetActive(false);
	}

	public void HandleCancelFromConnectToPhotonBtnClicked(object sender, EventArgs e)
	{
		HandleCancelFromConnectToPhotonBtnClicked();
	}

	public void HandleCancelFromConnectToPhotonBtnClicked()
	{
		if (!cantCancel)
		{
			if (_someWindowSubscription != null)
			{
				_someWindowSubscription.Dispose();
			}
			if (failInternetLabel != null)
			{
				failInternetLabel.SetActive(false);
			}
			if (connectToPhotonPanel != null)
			{
				connectToPhotonPanel.SetActive(false);
			}
			if (actAfterConnectToPhoton != null)
			{
				actAfterConnectToPhoton = null;
			}
			else
			{
				GameConnect.Disconnect();
			}
		}
	}

	private void LogBuyMap(string context)
	{
		try
		{
			AnalyticsStuff.LogSales(context, "Premium Maps");
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("LogBuyMap exception: " + ex);
		}
	}

	public void ShowBankWindow()
	{
		if (BankController.Instance != null)
		{
			EventHandler backFromBankHandler = null;
			backFromBankHandler = delegate
			{
				BankController.Instance.BackRequested -= backFromBankHandler;
				mainPanel.transform.root.gameObject.SetActive(true);
				BankController.Instance.SetInterfaceEnabledWithDesiredCurrency(false, null);
			};
			BankController.Instance.BackRequested += backFromBankHandler;
			mainPanel.transform.root.gameObject.SetActive(false);
			BankController.Instance.SetInterfaceEnabledWithDesiredCurrency(true, null);
		}
		else
		{
			UnityEngine.Debug.LogWarning("BankController.Instance == null");
		}
	}

	private void HandleCoinsShopClicked(object sender, EventArgs e)
	{
		ShowBankWindow();
	}

	public void HandleLocalBtnClicked()
	{
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		Defs.isInet = false;
		CustomBtnAct();
	}

	private void ShowConnectToPhotonPanel()
	{
		cantCancel = false;
		_someWindowSubscription = BackSystem.Instance.Register(HandleCancelFromConnectToPhotonBtnClicked, "Connect to Photon panel");
		if (FriendsController.sharedController != null && FriendsController.sharedController.Banned == 1)
		{
			accountBlockedLabel.timer = 3f;
			accountBlockedLabel.gameObject.SetActive(true);
		}
		else
		{
			ConnectToPhoton();
			connectToPhotonPanel.SetActive(true);
		}
	}

	public void HandleCustomBtnClicked()
	{
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		actAfterConnectToPhoton = CustomBtnAct;
		PhotonNetwork.autoJoinLobby = true;
		ShowConnectToPhotonPanel();
	}

	private void CustomBtnAct()
	{
		gameNameFilter = string.Empty;
		if (Defs.isInet)
		{
			Invoke("UpdateFilteredRoomListInvoke", 0.03f);
		}
		CustomPanelConnectScene.Instance.showSearchPanelBtn.SetActive(Defs.isInet);
		mainPanel.SetActive(false);
		selectMapPanel.SetActive(false);
		CustomPanelConnectScene.Instance.customPanel.SetActive(true);
		inCustomPanel = true;
		CustomPanelConnectScene.Instance.headCustomPanel.SetText(LocalizationStore.Get(GameConnect.gameModesLocalizeKey[(int)GameConnect.gameMode]));
		password = string.Empty;
		CustomPanelConnectScene.Instance.incorrectPasswordLabel.timer = -1f;
		CustomPanelConnectScene.Instance.incorrectPasswordLabel.gameObject.SetActive(false);
		CustomPanelConnectScene.Instance.gameIsfullLabel.timer = -1f;
		CustomPanelConnectScene.Instance.gameIsfullLabel.gameObject.SetActive(false);
		if (isInitCustomPanel)
		{
			return;
		}
		startPosNameServerNameInput = CustomPanelConnectScene.Instance.nameServerInput.transform.localPosition;
		CustomPanelConnectScene.Instance.setPasswordInput.onSubmit.Add(new EventDelegate(delegate
		{
			OnPaswordSelected();
		}));
		CustomPanelConnectScene.Instance.enterPasswordInput.onSubmit.Add(new EventDelegate(EnterPassInputSubmit));
		isInitCustomPanel = true;
		if (CustomPanelConnectScene.Instance.showSearchPanelBtn != null)
		{
			ButtonHandler component = CustomPanelConnectScene.Instance.showSearchPanelBtn.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += HandleShowSearchPanelBtnClicked;
			}
		}
		if (CustomPanelConnectScene.Instance.createRoomBtn != null)
		{
			ButtonHandler component2 = CustomPanelConnectScene.Instance.createRoomBtn.GetComponent<ButtonHandler>();
			if (component2 != null)
			{
				component2.Clicked += HandleGoToCreateRoomBtnClicked;
			}
		}
		if (CustomPanelConnectScene.Instance.goToCreateRoomBtn != null)
		{
			ButtonHandler component3 = CustomPanelConnectScene.Instance.goToCreateRoomBtn.GetComponent<ButtonHandler>();
			if (component3 != null)
			{
				component3.Clicked += HandleCreateRoomBtnClicked;
			}
		}
		if (CustomPanelConnectScene.Instance.searchBtn != null)
		{
			ButtonHandler component4 = CustomPanelConnectScene.Instance.searchBtn.GetComponent<ButtonHandler>();
			if (component4 != null)
			{
				component4.Clicked += HandleSearchBtnClicked;
			}
		}
		if (CustomPanelConnectScene.Instance.joinRoomFromEnterPasswordBtn != null)
		{
			ButtonHandler component5 = CustomPanelConnectScene.Instance.joinRoomFromEnterPasswordBtn.GetComponent<ButtonHandler>();
			if (component5 != null)
			{
				component5.Clicked += HandleJoinRoomFromEnterPasswordBtnClicked;
			}
		}
	}

	private void UpdateFilteredRoomListInvoke()
	{
		updateFilteredRoomList(gameNameFilter);
	}

	public void HandleRandomBtnClicked()
	{
		if (Storager.getInt("localMultyKey") == 1)
		{
			HandleCreateRoomBtnClicked(null, null);
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		actAfterConnectToPhoton = RandomBtnAct;
		PhotonNetwork.autoJoinLobby = false;
		ShowConnectToPhotonPanel();
	}

	private void RandomBtnAct()
	{
		GameConnect.ConnectToRandomRoom();
	}

	public void HandleGoBtnClicked()
	{
		if (Storager.getInt("localMultyKey") == 1)
		{
			HandleCreateRoomBtnClicked(null, null);
			return;
		}
		if (selectMap.mapID == -1)
		{
			HandleRandomBtnClicked();
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging.HasValue)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		actAfterConnectToPhoton = GoBtnAct;
		PhotonNetwork.autoJoinLobby = false;
		ShowConnectToPhotonPanel();
	}

	private void GoBtnAct()
	{
		if (selectMap.mapID == -1)
		{
			RandomBtnAct();
			return;
		}
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(selectMap.mapID);
		if (!(infoScene == null))
		{
			GameConnect.ConnectToRandomRoom(infoScene);
		}
	}

	public void HandleBackBtnClicked()
	{
		if (mainPanel != null && mainPanel.activeSelf)
		{
			if (FriendsController.sharedController != null)
			{
				FriendsController.sharedController.GetFriendsData();
			}
			MainMenuHeroCamera.Instance.MainCamera.enabled = true;
			NickLabelStack.sharedStack.gameObject.SetActive(true);
			BannerWindowController.SharedController.ResetScene();
			MenuBackgroundMusic.keepPlaying = true;
			MainMenuController.sharedController.mainPanel.SetActive(true);
			base.gameObject.SetActive(false);
			isGoInPhotonGame = false;
			isReturnFromGame = false;
			MainMenuController.isShowConnectSceneOnStart = false;
			NeedShowReviewInConnectScene = false;
		}
		if (inCustomPanel && CustomPanelConnectScene.Instance.customPanel != null && CustomPanelConnectScene.Instance.customPanel.activeSelf)
		{
			inCustomPanel = false;
			CustomPanelConnectScene.Instance.connectToWiFIInCreateLabel.SetActive(false);
			CustomPanelConnectScene.Instance.connectToWiFIInCustomLabel.SetActive(false);
			CustomPanelConnectScene.Instance.createRoomUIBtn.isEnabled = true;
			Defs.isInet = true;
			CustomPanelConnectScene.Instance.customPanel.SetActive(false);
			while (CustomPanelConnectScene.Instance.gridGamesTransform.childCount > 0)
			{
				Transform child = CustomPanelConnectScene.Instance.gridGamesTransform.GetChild(0);
				child.parent = null;
				gamesInfo.Remove(child.gameObject);
				UnityEngine.Object.Destroy(child.gameObject);
			}
			mainPanel.SetActive(true);
			selectMapPanel.SetActive(true);
			GameConnect.Disconnect();
		}
		if (inCustomPanel && CustomPanelConnectScene.Instance.searchPanel != null && CustomPanelConnectScene.Instance.searchPanel.activeSelf)
		{
			CustomPanelConnectScene.Instance.searchInput.value = gameNameFilter;
			CustomPanelConnectScene.Instance.searchPanel.SetActive(false);
			CustomPanelConnectScene.Instance.customPanel.SetActive(true);
		}
		if (inCustomPanel && CustomPanelConnectScene.Instance.createPanel != null && CustomPanelConnectScene.Instance.createPanel.activeSelf)
		{
			CustomPanelConnectScene.Instance.createPanel.SetActive(false);
			SetUseMasMap(true, true);
			CustomPanelConnectScene.Instance.customPanel.SetActive(true);
		}
		if (inCustomPanel && CustomPanelConnectScene.Instance.enterPasswordPanel != null && CustomPanelConnectScene.Instance.enterPasswordPanel.activeSelf)
		{
			CustomPanelConnectScene.Instance.enterPasswordPanel.SetActive(false);
			CustomPanelConnectScene.Instance.customPanel.SetActive(true);
			if (ExperienceController.sharedController != null)
			{
				ExperienceController.sharedController.isShowRanks = true;
			}
		}
	}

	private void HandleUnlockBtnClicked(object sender, EventArgs e)
	{
		int _price = 0;
		string _storagerPurchasedKey = "";
		if (GameConnect.gameMode == GameConnect.GameMode.FlagCapture)
		{
			_price = Defs.CaptureFlagPrice;
			_storagerPurchasedKey = Defs.CaptureFlagPurchasedKey;
		}
		if (GameConnect.gameMode == GameConnect.GameMode.DeadlyGames)
		{
			_price = Defs.HungerGamesPrice;
			_storagerPurchasedKey = Defs.hungerGamesPurchasedKey;
		}
		((Action)delegate
		{
			int num = Storager.getInt("Coins") - _price;
			if (num >= 0)
			{
				Storager.setInt(_storagerPurchasedKey, 1);
				Storager.setInt("Coins", num);
				ShopNGUIController.SpendBoughtCurrency("Coins", _price);
				if (Application.platform != RuntimePlatform.IPhonePlayer)
				{
					PlayerPrefs.Save();
				}
				if (coinsPlashka.thisScript != null)
				{
					coinsPlashka.thisScript.enabled = false;
				}
				customBtn.SetActive(true);
				randomBtn.SetActive(true);
				goBtn.SetActive(true);
			}
			else
			{
				StoreKitEventListener.State.PurchaseKey = "Mode opened";
				if (BankController.Instance != null)
				{
					EventHandler handleBackFromBank = null;
					handleBackFromBank = delegate
					{
						BankController.Instance.BackRequested -= handleBackFromBank;
						mainPanel.transform.root.gameObject.SetActive(true);
						BankController.Instance.SetInterfaceEnabledWithDesiredCurrency(false, null);
					};
					BankController.Instance.BackRequested += handleBackFromBank;
					mainPanel.transform.root.gameObject.SetActive(false);
					BankController.Instance.SetInterfaceEnabledWithDesiredCurrency(true, "Coins", true);
				}
				else
				{
					UnityEngine.Debug.LogWarning("BankController.Instance == null");
				}
			}
		})();
	}

	private static void SetFlagsForDeathmatchRegim()
	{
		Defs.isMulti = true;
		Defs.isInet = true;
		GameConnect.SetGameMode(GameConnect.GameMode.Deathmatch);
	}

	private void SetRegim(GameConnect.GameMode _regim)
	{
		levelCurrentRegim = ExperienceController.sharedController.currentLevel;
		if (_regim != GameConnect.GameMode.Dater)
		{
			PlayerPrefs.SetInt("RegimMulty", (int)_regim);
		}
		GameConnect.SetGameMode(_regim);
		curSelectMode = GameConnect.gameMode;
		Defs.isMulti = true;
		Defs.isInet = true;
		localBtn.SetActive(BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64 && (_regim == GameConnect.GameMode.Deathmatch || _regim == GameConnect.GameMode.TeamFight));
		customButtonsGrid.Reposition();
		if (randomBtn != null)
		{
			randomBtn.SetActive(true);
		}
		if (customBtn != null)
		{
			customBtn.SetActive(true);
		}
		if (goBtn != null)
		{
			goBtn.SetActive(true);
		}
		rulesLabel.text = LocalizationStore.Get(GameConnect.gameModesRulesLocalizeKey[(int)(GameConnect.isDaterRegim ? GameConnect.GameMode.Dater : GameConnect.gameMode)]);
		SetUseMasMap();
	}

	private void OnEnable()
	{
		if (levelCurrentRegim != ExperienceController.sharedController.currentLevel && levelCurrentRegim != -1)
		{
			SetRegim(GameConnect.gameMode);
		}
		if (ExperienceController.sharedController != null && (!isReturnFromGame || secondOnEnable))
		{
			ExperienceController.sharedController.isShowRanks = true;
		}
		isEnable = true;
		GameConnect.OnJoinToMap = (Action<string>)Delegate.Combine(GameConnect.OnJoinToMap, new Action<string>(OnJoinToMap));
		GameConnect.OnJoinRoomFailed = (Action<bool>)Delegate.Combine(GameConnect.OnJoinRoomFailed, new Action<bool>(OnJoinRoomFailed));
		GameConnect.OnFailedToConnect = (GameConnect.OnDisconnectReason)Delegate.Combine(GameConnect.OnFailedToConnect, new GameConnect.OnDisconnectReason(OnFailedToConnect));
		GameConnect.OnConnectedMaster = (Action)Delegate.Combine(GameConnect.OnConnectedMaster, new Action(OnConnectedToMaster));
		GameConnect.OnDisconnected = (Action)Delegate.Combine(GameConnect.OnDisconnected, new Action(OnDisconnected));
		GameConnect.OnJoinedToRoom = (Action)Delegate.Combine(GameConnect.OnJoinedToRoom, new Action(OnJoinedRoom));
		GameConnect.OnReceivedRoomList = (Action)Delegate.Combine(GameConnect.OnReceivedRoomList, new Action(OnReceivedRoomListUpdate));
		FriendsController.sharedController.UpdatePopularityMaps();
		FriendsController.MapPopularityUpdated += OnUpdateMapPopularity;
		Defs.isGameFromFriends = false;
		ExperienceController.RefreshExpControllers();
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.SaveWeaponAsLastUsed(0);
		}
		if (FriendsController.sharedController != null)
		{
			FriendsController.sharedController.profileInfo.Clear();
		}
		if (PhotonNetwork.connectionState == ConnectionState.Connected)
		{
			firstConnectToPhoton = true;
		}
		SetPosSelectMapPanelInMainMenu();
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(delegate
		{
			if (BannerWindowController.SharedController != null && BannerWindowController.SharedController.IsAnyBannerShown)
			{
				BannerWindowController.SharedController.HideBannerWindow();
			}
			else
			{
				HandleBackBtnClicked();
			}
		}, "Connect Scene");
		if (secondOnEnable)
		{
			rulesLabel.text = LocalizationStore.Get(GameConnect.gameModesRulesLocalizeKey[(int)(GameConnect.isDaterRegim ? GameConnect.GameMode.Dater : GameConnect.gameMode)]);
		}
		if (secondOnEnable || isReturnFromGame || MainMenuController.isShowConnectSceneOnStart)
		{
			OnEnableWhenAnimate();
			if (FriendsController.sharedController != null)
			{
				FriendsController.sharedController.SendOurDataInConnectScene();
			}
			FriendsController.SendX2REwardAnalytics();
		}
		if (!secondOnEnable)
		{
			secondOnEnable = true;
		}
		if (NeedShowReviewInConnectScene)
		{
			StartCoroutine(ShowReview());
		}
	}

	private void OnDisable()
	{
		isEnable = false;
		GameConnect.OnJoinToMap = (Action<string>)Delegate.Remove(GameConnect.OnJoinToMap, new Action<string>(OnJoinToMap));
		GameConnect.OnJoinRoomFailed = (Action<bool>)Delegate.Remove(GameConnect.OnJoinRoomFailed, new Action<bool>(OnJoinRoomFailed));
		GameConnect.OnFailedToConnect = (GameConnect.OnDisconnectReason)Delegate.Remove(GameConnect.OnFailedToConnect, new GameConnect.OnDisconnectReason(OnFailedToConnect));
		GameConnect.OnConnectedMaster = (Action)Delegate.Remove(GameConnect.OnConnectedMaster, new Action(OnConnectedToMaster));
		GameConnect.OnDisconnected = (Action)Delegate.Remove(GameConnect.OnDisconnected, new Action(OnDisconnected));
		GameConnect.OnJoinedToRoom = (Action)Delegate.Remove(GameConnect.OnJoinedToRoom, new Action(OnJoinedRoom));
		GameConnect.OnReceivedRoomList = (Action)Delegate.Remove(GameConnect.OnReceivedRoomList, new Action(OnReceivedRoomListUpdate));
		FriendsController.MapPopularityUpdated -= OnUpdateMapPopularity;
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
	}

	private void Update()
	{
	}

	private bool IsUseMap(int indMap)
	{
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(curSelectMode, indMap);
		if (infoScene != null)
		{
			return !infoScene.isPremium || Storager.getInt(infoScene.NameScene + "Key") != 0 || PremiumAccountController.MapAvailableDueToPremiumAccount(infoScene.NameScene);
		}
		return false;
	}

	private static void ResetWeaponManagerForDeathmatch()
	{
		SetFlagsForDeathmatchRegim();
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset();
		}
	}

	private IEnumerator ShowReview()
	{
		yield return new WaitForSeconds(1f);
		if (NeedShowReviewInConnectScene)
		{
			NeedShowReviewInConnectScene = false;
			ReviewHUDWindow.Instance.ShowWindowRating();
		}
	}

	[Obsolete]
	private IEnumerator StartControllInterstitial()
	{
		string dismissReason = GetReasonToDismissInterstitialConnectScene();
		if (string.IsNullOrEmpty(dismissReason))
		{
			UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=magenta>{0}.LoadMapPreview(), InterstitialRequest: {1}</color>" : "{0}.LoadMapPreview(), InterstitialRequest: {1}", GetType().Name, InterstitialRequest);
			if (InterstitialRequest)
			{
				AdsConfigMemento lastLoadedConfig = AdsConfigManager.Instance.LastLoadedConfig;
				string playerCategory = AdsConfigManager.GetPlayerCategory(lastLoadedConfig);
				ReturnInConnectSceneAdPointMemento returnInConnectScene = lastLoadedConfig.AdPointsConfig.ReturnInConnectScene;
				double delayInSeconds = returnInConnectScene.GetFinalDelayInSeconds(playerCategory);
				float startWaitingTime = Time.realtimeSinceStartup;
				while ((double)(Time.realtimeSinceStartup - startWaitingTime) < delayInSeconds)
				{
					yield return null;
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=magenta>Dismissing wait for interstitial. {0}</color>" : "Dismissing wait for interstitial. {0}", dismissReason);
		}
		InterstitialRequest = false;
	}

	internal static string GetReasonToDismissFakeInterstitial()
	{
		try
		{
			AdsConfigMemento lastLoadedConfig = AdsConfigManager.Instance.LastLoadedConfig;
			if (lastLoadedConfig == null)
			{
				return "Ads config is `null`.";
			}
			if (lastLoadedConfig.Exception != null)
			{
				return lastLoadedConfig.Exception.Message;
			}
			string playerCategory = AdsConfigManager.GetPlayerCategory(lastLoadedConfig);
			InterstitialConfigMemento interstitialConfig = lastLoadedConfig.InterstitialConfig;
			bool realInterstitialsEnabled = interstitialConfig.GetEnabled(playerCategory);
			FakeInterstitialConfigMemento fakeInterstitialConfig = lastLoadedConfig.FakeInterstitialConfig;
			DateTime? serverTime = FriendsController.GetServerTime();
			if (!serverTime.HasValue)
			{
				return "Server time not received";
			}
			double timeSpanSinceLastShowInMinutes = AdsConfigManager.GetTimeSpanSinceLastShowInMinutes(serverTime.Value);
			double timeoutBetweenShowInMinutes = interstitialConfig.GetTimeoutBetweenShowInMinutes(playerCategory);
			if (timeSpanSinceLastShowInMinutes < timeoutBetweenShowInMinutes)
			{
				return "TimeoutBetweenShowInMinutes";
			}
			return fakeInterstitialConfig.GetDisabledReason(playerCategory, ExperienceController.GetCurrentLevel(), InterstitialCounter.Instance.FakeInterstitialCount, InterstitialCounter.Instance.FakeInterstitialCount + InterstitialCounter.Instance.RealInterstitialCount, realInterstitialsEnabled);
		}
		catch (Exception ex)
		{
			return ex.ToString();
		}
	}

	internal static int GetReasonCodeToDismissInterstitialConnectScene()
	{
		DateTime? serverTime = FriendsController.GetServerTime();
		if (!serverTime.HasValue)
		{
			return 800;
		}
		AdsConfigMemento lastLoadedConfig = AdsConfigManager.Instance.LastLoadedConfig;
		if (lastLoadedConfig == null)
		{
			return 100;
		}
		if (lastLoadedConfig.Exception != null)
		{
			return 200;
		}
		int interstitialDisabledReasonCode = AdsConfigManager.GetInterstitialDisabledReasonCode(lastLoadedConfig);
		if (interstitialDisabledReasonCode != 0)
		{
			return 300 + interstitialDisabledReasonCode;
		}
		ReturnInConnectSceneAdPointMemento returnInConnectScene = lastLoadedConfig.AdPointsConfig.ReturnInConnectScene;
		if (returnInConnectScene == null)
		{
			return 400;
		}
		string playerCategory = AdsConfigManager.GetPlayerCategory(lastLoadedConfig);
		int disabledReasonCode = returnInConnectScene.GetDisabledReasonCode(playerCategory);
		if (disabledReasonCode != 0)
		{
			return 500 + disabledReasonCode;
		}
		int currentDailyInterstitialCount = FyberFacade.Instance.GetCurrentDailyInterstitialCount(serverTime.Value);
		int finalImpressionMaxCountPerDay = returnInConnectScene.GetFinalImpressionMaxCountPerDay(playerCategory);
		if (currentDailyInterstitialCount >= finalImpressionMaxCountPerDay)
		{
			return 600;
		}
		double totalMinutes = InGameTimeKeeper.Instance.CurrentInGameTime.TotalMinutes;
		double finalMinInGameTimePerDayInMinutes = returnInConnectScene.GetFinalMinInGameTimePerDayInMinutes(playerCategory);
		if (totalMinutes < finalMinInGameTimePerDayInMinutes)
		{
			return 700;
		}
		return 0;
	}

	internal static string GetReasonToDismissInterstitialConnectScene()
	{
		DateTime? serverTime = FriendsController.GetServerTime();
		if (!serverTime.HasValue)
		{
			return "Server time not received";
		}
		AdsConfigMemento lastLoadedConfig = AdsConfigManager.Instance.LastLoadedConfig;
		if (lastLoadedConfig == null)
		{
			return "Ads config is `null`.";
		}
		if (lastLoadedConfig.Exception != null)
		{
			return lastLoadedConfig.Exception.Message;
		}
		string interstitialDisabledReason = AdsConfigManager.GetInterstitialDisabledReason(lastLoadedConfig);
		if (!string.IsNullOrEmpty(interstitialDisabledReason))
		{
			return interstitialDisabledReason;
		}
		ReturnInConnectSceneAdPointMemento returnInConnectScene = lastLoadedConfig.AdPointsConfig.ReturnInConnectScene;
		if (returnInConnectScene == null)
		{
			return string.Format("`{0}` config is `null`", new object[1] { returnInConnectScene.Id });
		}
		string playerCategory = AdsConfigManager.GetPlayerCategory(lastLoadedConfig);
		string disabledReason = returnInConnectScene.GetDisabledReason(playerCategory);
		if (!string.IsNullOrEmpty(disabledReason))
		{
			return disabledReason;
		}
		int currentDailyInterstitialCount = FyberFacade.Instance.GetCurrentDailyInterstitialCount(serverTime.Value);
		int finalImpressionMaxCountPerDay = returnInConnectScene.GetFinalImpressionMaxCountPerDay(playerCategory);
		if (currentDailyInterstitialCount >= finalImpressionMaxCountPerDay)
		{
			return string.Format(CultureInfo.InvariantCulture, "`interstitialCount: {0}` >= `maxInterstitialCount: {1}` for `{2}`", currentDailyInterstitialCount, finalImpressionMaxCountPerDay, playerCategory);
		}
		double totalMinutes = InGameTimeKeeper.Instance.CurrentInGameTime.TotalMinutes;
		double finalMinInGameTimePerDayInMinutes = returnInConnectScene.GetFinalMinInGameTimePerDayInMinutes(playerCategory);
		if (totalMinutes < finalMinInGameTimePerDayInMinutes)
		{
			return string.Format(CultureInfo.InvariantCulture, "`inGameTimeMinutes: {0:f2}` < `minInGameTimePerDayInMinutes: {1:f2}` for `{2}`", totalMinutes, finalMinInGameTimePerDayInMinutes, playerCategory);
		}
		return string.Empty;
	}

	public void OnUpdateMapPopularity()
	{
		if (grid == null)
		{
			return;
		}
		for (int i = 0; i < grid.transform.childCount; i++)
		{
			if (grid.transform.GetChild(i).gameObject.activeSelf)
			{
				MapPreviewController component = grid.transform.GetChild(i).GetComponent<MapPreviewController>();
				if (component.mapID != -1)
				{
					component.UpdatePopularity();
				}
			}
		}
	}

	private void SetUseMasMap(bool isUseRandom = true, bool isOffSelectMapPanel = false)
	{
		if (isOffSelectMapPanel)
		{
			scrollViewSelectMapTransform.GetComponent<UIPanel>().alpha = 0.001f;
		}
		isSetUseMap = true;
		SpringPanel component = ScrollTransform.GetComponent<SpringPanel>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
		scrollViewSelectMapTransform.GetComponent<UIScrollView>().ResetPosition();
		if (isUseRandom && !isOffSelectMapPanel)
		{
			SetPosSelectMapPanelInMainMenu();
		}
		int maxCountMapsInRegims = SceneInfoController.instance.GetMaxCountMapsInRegims();
		maxCountMapsInRegims++;
		AllScenesForMode allScenesForMode = ((SceneInfoController.instance != null) ? SceneInfoController.instance.GetListScenesForMode(curSelectMode) : null);
		if (allScenesForMode == null)
		{
			UnityEngine.Debug.LogError("modeInfo == null");
			return;
		}
		if (grid.transform.childCount < maxCountMapsInRegims)
		{
			float num = 15f;
			int num2 = (((double)((float)Screen.width / (float)Screen.height) < 1.5) ? 3 : 4);
			float num3 = (fonMapPreview.localSize.x - (float)num2 * num - num) / (float)num2;
			float num4 = 1f;
			float num5 = 1f;
			mapPreviewTexture.SetActive(true);
			for (int i = grid.transform.childCount; i < maxCountMapsInRegims; i++)
			{
				GameObject obj = UnityEngine.Object.Instantiate(mapPreviewTexture);
				obj.transform.SetParent(grid.transform, false);
				MapPreviewController component2 = obj.GetComponent<MapPreviewController>();
				num5 = num3 / component2.mapPreviewTexture.localSize.x;
				num4 = component2.mapPreviewTexture.localSize.y * num5;
				obj.transform.GetChild(0).localScale = new Vector3(num5, num5, 1f);
				obj.name = "Map_" + i;
			}
			mapPreviewTexture.SetActive(false);
			grid.GetComponent<UIGrid>().cellWidth = num3 + num;
			grid.GetComponent<UIGrid>().cellHeight = num4 + num;
			grid.GetComponent<UIGrid>().maxPerLine = num2;
			grid.GetComponent<UIGrid>().Reposition();
		}
		List<SceneInfo> list = ((ExperienceController.sharedController.currentLevel < 2) ? allScenesForMode.avaliableScenes.Shuffle().ToList() : allScenesForMode.avaliableScenes);
		if (isUseRandom)
		{
			MapPreviewController component3 = grid.transform.GetChild(0).gameObject.GetComponent<MapPreviewController>();
			component3.mapPreviewTexture.mainTexture = randomMapPreview;
			component3.curSceneInfo = null;
			component3.NameMapLbl.GetComponent<SetHeadLabelText>().SetText(LocalizationStore.Get("Key_2463"));
			component3.bottomPanel.SetActive(false);
			component3.mapID = -1;
			component3.sceneMapName = "Random";
		}
		for (int j = 0; j < list.Count; j++)
		{
			SceneInfo sceneInfo = list[j];
			GameObject gameObject = grid.transform.GetChild(j + (isUseRandom ? 1 : 0)).gameObject;
			if (!gameObject.activeSelf)
			{
				gameObject.SetActive(true);
			}
			MapPreviewController component4 = gameObject.GetComponent<MapPreviewController>();
			bool flag;
			int num6;
			if (sceneInfo.isPremium && Storager.getInt(sceneInfo.NameScene + "Key") == 0)
			{
				flag = !PremiumAccountController.MapAvailableDueToPremiumAccount(sceneInfo.NameScene);
			}
			else
				num6 = 0;
			if (!component4.bottomPanel.activeSelf)
			{
				component4.bottomPanel.SetActive(true);
			}
			component4.curSceneInfo = sceneInfo;
			component4.mapPreviewTexture.mainTexture = GetMapTexture(sceneInfo.NameScene);
			component4.NameMapLbl.GetComponent<SetHeadLabelText>().SetText(sceneInfo.TranslatePreviewName.ToUpper());
			component4.SizeMapNameLbl[0].SetActive(sceneInfo.sizeMap == InfoSizeMap.small);
			component4.SizeMapNameLbl[1].SetActive(sceneInfo.sizeMap == InfoSizeMap.normal);
			component4.SizeMapNameLbl[2].SetActive(sceneInfo.sizeMap == InfoSizeMap.big || sceneInfo.sizeMap == InfoSizeMap.veryBig);
			component4.mapID = sceneInfo.indexMap;
			component4.sceneMapName = sceneInfo.NameScene;
			if (sceneInfo.AvaliableWeapon == ModeWeapon.knifes)
			{
				component4.milee.SetActive(true);
				component4.milee.GetComponent<UILabel>().text = LocalizationStore.Get("Key_0096");
			}
			else if (sceneInfo.AvaliableWeapon == ModeWeapon.sniper)
			{
				component4.milee.SetActive(true);
				component4.milee.GetComponent<UILabel>().text = LocalizationStore.Get("Key_0949");
			}
			else if (GameConnect.isDaterRegim)
			{
				component4.milee.SetActive(true);
				component4.milee.GetComponent<UILabel>().text = LocalizationStore.Get("Key_1421");
			}
			else
			{
				component4.milee.SetActive(false);
			}
			component4.UpdatePopularity();
		}
		scrollViewSelectMapTransform.GetComponent<UIScrollView>().ResetPosition();
		grid.transform.localPosition = new Vector3(ScrollTransform.GetComponent<UIPanel>().baseClipRegion.x, grid.transform.localPosition.y, grid.transform.localPosition.z);
		for (int k = list.Count + (isUseRandom ? 1 : 0); k < grid.transform.childCount; k++)
		{
			GameObject gameObject2 = grid.transform.GetChild(k).gameObject;
			if (gameObject2.activeSelf)
			{
				gameObject2.SetActive(false);
			}
		}
		SelectMap(selectedMap);
		refreshMapsPanel = true;
		if (isOffSelectMapPanel)
		{
			selectMapPanel.SetActive(false);
			scrollViewSelectMapTransform.GetComponent<UIPanel>().alpha = 1f;
			SetPosSelectMapPanelInMainMenu();
		}
		isSetUseMap = false;
	}

	private void LateUpdate()
	{
		if (refreshMapsPanel)
		{
			scrollViewSelectMapTransform.GetComponent<UIPanel>().SetDirty();
			scrollViewSelectMapTransform.GetComponent<UIPanel>().Refresh();
			refreshMapsPanel = false;
		}
	}

	private void SelectMap(string map)
	{
		selectedMap = map;
		float num = scrollViewSelectMapTransform.GetComponent<UIScrollView>().bounds.extents.y * 2f;
		float y = scrollViewSelectMapTransform.GetComponent<UIPanel>().GetViewSize().y;
		if (!string.IsNullOrEmpty(selectedMap))
		{
			grid.transform.GetChild(0);
			foreach (Transform item in grid.transform)
			{
				string sceneMapName = item.GetComponent<MapPreviewController>().sceneMapName;
				if (!selectedMap.Equals(sceneMapName))
				{
					continue;
				}
				if (num > y)
				{
					float num2 = -1f * (item.localPosition.y + _heightCell * 0.5f);
					if (num2 < 0f)
					{
						num2 = 0f;
					}
					float y2 = scrollViewSelectMapTransform.GetComponent<UIPanel>().clipSoftness.y;
					float num3 = num - y + y2;
					if (num2 > num3)
					{
						num2 = num3;
					}
					scrollViewSelectMapTransform.localPosition = new Vector3(scrollViewSelectMapTransform.localPosition.x, num2, scrollViewSelectMapTransform.localPosition.z);
				}
				item.GetComponent<UIToggle>().value = true;
				selectMap = item.GetComponent<MapPreviewController>();
				break;
			}
			selectedMap = "";
		}
		else
		{
			grid.transform.GetChild(0).GetComponent<UIToggle>().value = true;
			selectMap = grid.transform.GetChild(0).GetComponent<MapPreviewController>();
		}
	}

	public void OnReceivedRoomListUpdate()
	{
		if ((!inCustomPanel || CustomPanelConnectScene.Instance.customPanel.activeSelf) && Defs.isInet && Defs.isInet)
		{
			Invoke("UpdateFilteredRoomListInvoke", 0.03f);
		}
	}

	private void SetRoomInfo(GameInfo _gameInfo, int index)
	{
		_gameInfo.index = index;
		if (filteredRoomList.Count > index)
		{
			_gameInfo.gameObject.SetActive(true);
			RoomInfo roomInfo = filteredRoomList[index];
			string text = roomInfo.name;
			if (text.Length == 36 && text.IndexOf("-") == 8 && text.LastIndexOf("-") == 23)
			{
				text = LocalizationStore.Get("Key_0088");
			}
			_gameInfo.serverNameLabel.text = text;
			_gameInfo.countPlayersLabel.text = roomInfo.playerCount + "/" + roomInfo.maxPlayers;
			bool flag = string.IsNullOrEmpty(roomInfo.customProperties[GameConnect.passwordProperty].ToString());
			_gameInfo.openSprite.SetActive(flag);
			_gameInfo.closeSprite.SetActive(!flag);
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(roomInfo.customProperties[GameConnect.mapProperty].ToString()));
			string text2 = infoScene.TranslatePreviewName.ToUpper();
			_gameInfo.mapNameLabel.SetText(text2);
			_gameInfo.roomInfo = roomInfo;
			_gameInfo.mapTexture.mainTexture = GetMapTexture(infoScene.NameScene);
			_gameInfo.SizeMapNameLbl[0].SetActive(infoScene.sizeMap == InfoSizeMap.small);
			_gameInfo.SizeMapNameLbl[1].SetActive(infoScene.sizeMap == InfoSizeMap.normal);
			_gameInfo.SizeMapNameLbl[2].SetActive(infoScene.sizeMap == InfoSizeMap.big || infoScene.sizeMap == InfoSizeMap.veryBig);
		}
		else
		{
			_gameInfo.gameObject.SetActive(false);
		}
	}

	public void updateFilteredRoomList(string gFilter)
	{
		filteredRoomList.Clear();
		RoomInfo[] roomList = PhotonNetwork.GetRoomList();
		bool flag = !string.IsNullOrEmpty(gFilter);
		for (int i = 0; i < roomList.Length; i++)
		{
			if (!flag && roomList[i].playerCount == roomList[i].maxPlayers)
			{
				continue;
			}
			if (!GameConnect.isDaterRegim && roomList[i].customProperties[GameConnect.platformProperty] != null)
			{
				string text = roomList[i].customProperties[GameConnect.platformProperty].ToString();
				int myPlatformConnect = (int)GameConnect.myPlatformConnect;
				if (!text.Equals(myPlatformConnect.ToString()) && !roomList[i].customProperties[GameConnect.platformProperty].ToString().Equals(3.ToString()))
				{
					continue;
				}
			}
			if ((!Defs.isABTestBalansCohortActual || !(ExpController.Instance != null) || GameConnect.GetTierForRoom() != 0 || (roomList[i].customProperties[GameConnect.ABTestProperty] != null && (int)roomList[i].customProperties[GameConnect.ABTestProperty] == 1)) && (Defs.isABTestBalansCohortActual || !(ExpController.Instance != null) || GameConnect.GetTierForRoom() != 0 || roomList[i].customProperties[GameConnect.ABTestProperty] == null || (int)roomList[i].customProperties[GameConnect.ABTestProperty] != 1))
			{
				bool flag2 = true;
				if (flag)
				{
					flag2 = roomList[i].name.StartsWith(gFilter) && (roomList[i].name.Length != 36 || roomList[i].name.IndexOf("-") != 8 || roomList[i].name.LastIndexOf("-") != 23);
				}
				if (flag2 && IsUseMap((int)roomList[i].customProperties[GameConnect.mapProperty]))
				{
					filteredRoomList.Add(roomList[i]);
				}
			}
		}
		if (countNote > 10)
		{
			countNote = 1;
		}
		countNote = 50;
		if (filteredRoomList.Count < countNote)
		{
			countNote = filteredRoomList.Count;
		}
		while (countNote < gamesInfo.Count)
		{
			UnityEngine.Object.Destroy(gamesInfo[gamesInfo.Count - 1]);
			gamesInfo.RemoveAt(gamesInfo.Count - 1);
		}
		if (countNote > gamesInfo.Count)
		{
			countColumn = (((double)((float)Screen.width / (float)Screen.height) < 1.5) ? 3 : 4);
			_widthCell = (CustomPanelConnectScene.Instance.fonGames.localSize.x - (float)(countColumn * 10)) / (float)countColumn;
			if (countNote > gamesInfo.Count)
			{
				CustomPanelConnectScene.Instance.gameInfoItemPrefab.SetActive(true);
			}
			while (countNote > gamesInfo.Count)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(CustomPanelConnectScene.Instance.gameInfoItemPrefab);
				gameObject.name = "GameInfo_" + gamesInfo.Count;
				gameObject.transform.SetParent(CustomPanelConnectScene.Instance.gridGamesTransform, false);
				_scale = _widthCell / gameObject.GetComponent<GameInfo>().mapTexture.localSize.x;
				_heightCell = gameObject.GetComponent<GameInfo>().mapTexture.localSize.y * _scale;
				gameObject.transform.GetChild(0).transform.localScale = new Vector3(_scale, _scale, _scale);
				gamesInfo.Add(gameObject);
				CustomPanelConnectScene.Instance.gameInfoItemPrefab.SetActive(false);
			}
			if (CustomPanelConnectScene.Instance.gameInfoItemPrefab.activeSelf)
			{
				CustomPanelConnectScene.Instance.gameInfoItemPrefab.SetActive(false);
			}
			CustomPanelConnectScene.Instance.gridGames.GetComponent<UIGrid>().cellWidth = _widthCell + borderWidth;
			CustomPanelConnectScene.Instance.gridGames.GetComponent<UIGrid>().cellHeight = _heightCell + borderWidth;
			CustomPanelConnectScene.Instance.gridGames.GetComponent<UIGrid>().maxPerLine = countColumn;
		}
		float num = CustomPanelConnectScene.Instance.scrollGames.bounds.extents.y * 2f;
		float y = CustomPanelConnectScene.Instance.scrollGames.GetComponent<UIPanel>().GetViewSize().y;
		CustomPanelConnectScene.Instance.gridGames.Reposition();
		if (!isFirstGamesReposition || num < y)
		{
			CustomPanelConnectScene.Instance.gridGames.transform.localPosition = new Vector3((0f - (_widthCell + borderWidth)) * ((float)countColumn * 0.5f - 0.5f), CustomPanelConnectScene.Instance.gridGames.transform.localPosition.y, CustomPanelConnectScene.Instance.gridGames.transform.localPosition.z);
			CustomPanelConnectScene.Instance.scrollGames.ResetPosition();
			isFirstGamesReposition = true;
		}
		for (int j = 0; j < countNote; j++)
		{
			SetRoomInfo(gamesInfo[j].GetComponent<GameInfo>(), j);
		}
	}

	private void OnPhotonJoinRoomFailed()
	{
		ActivityIndicator.IsActiveIndicator = false;
		loadingMapPanel.SetActive(false);
		if (inCustomPanel)
		{
			CustomPanelConnectScene.Instance.gameIsfullLabel.timer = 3f;
			CustomPanelConnectScene.Instance.gameIsfullLabel.gameObject.SetActive(true);
			CustomPanelConnectScene.Instance.incorrectPasswordLabel.timer = -1f;
			CustomPanelConnectScene.Instance.incorrectPasswordLabel.gameObject.SetActive(false);
		}
		UnityEngine.Debug.Log("OnPhotonJoinRoomFailed");
	}

	private void OnJoinedRoom()
	{
		cantCancel = true;
		UnityEngine.Debug.Log("OnJoinedRoom " + PhotonNetwork.room.customProperties[GameConnect.mapProperty].ToString());
		PhotonNetwork.isMessageQueueRunning = false;
		NotificationController.ResetPaused();
		GlobalGameController.healthMyPlayer = 0f;
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(PhotonNetwork.room.customProperties[GameConnect.mapProperty].ToString()));
		goMapName = infoScene.NameScene;
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset(Defs.filterMaps.ContainsKey(goMapName) ? Defs.filterMaps[goMapName] : 0);
		}
		StartCoroutine(MoveToGameScene(infoScene.NameScene));
	}

	private void OnJoinRoomFailed(bool onRoomCreate)
	{
		if (onRoomCreate)
		{
			CustomPanelConnectScene.Instance.nameAlreadyUsedLabel.timer = 3f;
			CustomPanelConnectScene.Instance.nameAlreadyUsedLabel.gameObject.SetActive(true);
		}
		else
		{
			CustomPanelConnectScene.Instance.gameIsfullLabel.timer = 3f;
			CustomPanelConnectScene.Instance.gameIsfullLabel.gameObject.SetActive(true);
			CustomPanelConnectScene.Instance.incorrectPasswordLabel.timer = -1f;
			CustomPanelConnectScene.Instance.incorrectPasswordLabel.gameObject.SetActive(false);
		}
		loadingMapPanel.SetActive(false);
		ActivityIndicator.IsActiveIndicator = false;
	}

	private void OnDisconnected()
	{
		UnityEngine.Debug.Log("OnDisconnectedFromPhoton");
		if ((!mainPanel.activeSelf || loadingMapPanel.activeSelf) && firstConnectToPhoton && Defs.isInet)
		{
			mainPanel.SetActive(true);
			selectMapPanel.SetActive(true);
			CustomPanelConnectScene.Instance.createPanel.SetActive(false);
			SetUseMasMap();
			CustomPanelConnectScene.Instance.customPanel.SetActive(false);
			if (inCustomPanel)
			{
				CustomPanelConnectScene.Instance.searchPanel.SetActive(false);
				CustomPanelConnectScene.Instance.enterPasswordPanel.SetActive(false);
				while (CustomPanelConnectScene.Instance.gridGamesTransform.childCount > 0)
				{
					Transform child = CustomPanelConnectScene.Instance.gridGamesTransform.GetChild(0);
					child.parent = null;
					gamesInfo.Remove(child.gameObject);
					UnityEngine.Object.Destroy(child.gameObject);
				}
			}
			if (ExperienceController.sharedController != null)
			{
				ExperienceController.sharedController.isShowRanks = true;
			}
			loadingMapPanel.SetActive(false);
			SetPosSelectMapPanelInMainMenu();
			serverIsNotAvalible.timer = 3f;
			serverIsNotAvalible.gameObject.SetActive(true);
			UICamera.selectedObject = null;
			GameConnect.GameMode gameMode = GameConnect.gameMode;
			ResetWeaponManagerForDeathmatch();
			SetRegim(gameMode);
			inCustomPanel = false;
		}
		if (actAfterConnectToPhoton != null)
		{
			Invoke("ConnectToPhoton", 0.5f);
		}
		if (connectToPhotonPanel.activeSelf)
		{
			failInternetLabel.SetActive(true);
		}
	}

	private void OnFailedToConnect(int parameters)
	{
		UnityEngine.Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + (DisconnectCause)parameters);
		if (connectToPhotonPanel.activeSelf)
		{
			failInternetLabel.SetActive(true);
		}
		if (!isCancelConnectingToPhoton)
		{
			Invoke("ConnectToPhoton", 1f);
		}
	}

	public void OnConnectedToMaster()
	{
		UnityEngine.Debug.Log("OnConnectedToMaster");
		firstConnectToPhoton = true;
		PhotonNetwork.playerName = ProfileController.GetPlayerNameOrDefault();
		if (connectToPhotonPanel.activeSelf && actAfterConnectToPhoton != new Action(RandomBtnAct))
		{
			connectToPhotonPanel.SetActive(false);
		}
		if (actAfterConnectToPhoton != null)
		{
			actAfterConnectToPhoton();
			actAfterConnectToPhoton = null;
		}
		else
		{
			GameConnect.Disconnect();
		}
	}

	private IEnumerator SetFonLoadingWaitForReset(string _mapName = "", bool isAddCountRun = false)
	{
		GetMapName(_mapName, isAddCountRun);
		if (_loadingNGUIController != null)
		{
			UnityEngine.Object.Destroy(_loadingNGUIController.gameObject);
			_loadingNGUIController = null;
		}
		while (WeaponManager.sharedManager == null)
		{
			yield return null;
		}
		while (WeaponManager.sharedManager.LockGetWeaponPrefabs > 0)
		{
			yield return null;
		}
		ShowLoadingGUI(_mapName);
	}

	private void SetFonLoading(string _mapName = "", bool isAddCountRun = false)
	{
		GetMapName(_mapName, isAddCountRun);
		if (_loadingNGUIController != null)
		{
			UnityEngine.Object.Destroy(_loadingNGUIController.gameObject);
			_loadingNGUIController = null;
		}
		ShowLoadingGUI(_mapName);
	}

	private void ShowLoadingGUI(string _mapName)
	{
		BannerWindowController.SharedController.HideBannerWindowNoShowNext();
		_loadingNGUIController = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("LoadingGUI")).GetComponent<LoadingNGUIController>();
		_loadingNGUIController.SceneToLoad = _mapName;
		_loadingNGUIController.loadingNGUITexture.mainTexture = LoadConnectScene.textureToShow;
		_loadingNGUIController.transform.parent = loadingMapPanel.transform;
		_loadingNGUIController.transform.localPosition = Vector3.zero;
		_loadingNGUIController.Init();
	}

	private void GetMapName(string _mapName, bool isAddCountRun)
	{
		UnityEngine.Debug.Log("setFonLoading " + _mapName);
		if (GameConnect.isCOOP)
		{
			int @int = PlayerPrefs.GetInt("CountRunCoop", 0);
			if (isAddCountRun)
			{
				PlayerPrefs.SetInt("CountRunCoop", PlayerPrefs.GetInt("CountRunCoop", 0) + 1);
			}
			Resources.Load("NoteLoadings/note_Time_Survival_" + @int % countNoteCaptureCOOP);
		}
		else if (GameConnect.isCompany)
		{
			int int2 = PlayerPrefs.GetInt("CountRunCompany", 0);
			Resources.Load("NoteLoadings/note_Team_Battle_" + int2 % countNoteCaptureCompany);
			if (isAddCountRun)
			{
				PlayerPrefs.SetInt("CountRunCompany", PlayerPrefs.GetInt("CountRunCompany", 0) + 1);
			}
		}
		else if (GameConnect.isHunger)
		{
			int int3 = PlayerPrefs.GetInt("CountRunHunger", 0);
			Resources.Load("NoteLoadings/note_Deadly_Games_" + int3 % countNoteCaptureHunger);
			if (isAddCountRun)
			{
				PlayerPrefs.SetInt("CountRunHunger", PlayerPrefs.GetInt("CountRunHunger", 0) + 1);
			}
		}
		else if (GameConnect.isFlag)
		{
			int int4 = PlayerPrefs.GetInt("CountRunFlag", 0);
			Resources.Load("NoteLoadings/note_Flag_Capture_" + int4 % countNoteCaptureFlag);
			if (isAddCountRun)
			{
				PlayerPrefs.SetInt("CountRunFlag", PlayerPrefs.GetInt("CountRunFlag", 0) + 1);
			}
		}
		else
		{
			int int5 = PlayerPrefs.GetInt("CountRunDeadmath", 0);
			Resources.Load("NoteLoadings/note_Deathmatch_" + int5 % countNoteCaptureDeadmatch);
			if (isAddCountRun)
			{
				PlayerPrefs.SetInt("CountRunDeadmath", PlayerPrefs.GetInt("CountRunDeadmath", 0) + 1);
			}
		}
		LoadConnectScene.textureToShow = Resources.Load("LevelLoadings" + (Device.isRetinaAndStrong ? "/Hi" : "") + "/Loading_" + _mapName) as Texture2D;
		LoadingInAfterGame.loadingTexture = LoadConnectScene.textureToShow;
		LoadConnectScene.sceneToLoad = _mapName;
		LoadConnectScene.noteToShow = null;
		loadingToDraw.gameObject.SetActive(false);
		loadingToDraw.mainTexture = null;
	}

	private IEnumerator MoveToGameScene(string _goMapName)
	{
		UnityEngine.Debug.Log("MoveToGameScene=" + _goMapName);
		Defs.isGameFromFriends = false;
		Defs.isGameFromClans = false;
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset(Defs.filterMaps.ContainsKey(_goMapName) ? Defs.filterMaps[_goMapName] : 0);
		}
		GlobalGameController.countKillsBlue = 0;
		GlobalGameController.countKillsRed = 0;
		while (PhotonNetwork.room == null)
		{
			yield return 0;
		}
		PlayerPrefs.SetString("RoomName", PhotonNetwork.room.name);
		PhotonNetwork.isMessageQueueRunning = false;
		mapPreview.Clear();
		yield return null;
		yield return Resources.UnloadUnusedAssets();
		yield return StartCoroutine(SetFonLoadingWaitForReset(_goMapName, true));
		loadingMapPanel.SetActive(true);
		isGoInPhotonGame = true;
		AsyncOperation asyncOperation = Singleton<SceneLoader>.Instance.LoadSceneAsync(Defs.PromSceneName);
		if (FriendsController.sharedController != null)
		{
			FriendsController.sharedController.GetFriendsData();
		}
		yield return asyncOperation;
		for (int i = 0; i < grid.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(grid.transform.GetChild(i).gameObject);
		}
		mapPreview.Clear();
	}

	private void ConnectToPhoton()
	{
		if (FriendsController.sharedController != null && FriendsController.sharedController.Banned == 1)
		{
			return;
		}
		if (PhotonNetwork.connectionState == ConnectionState.Connecting || PhotonNetwork.connectionState == ConnectionState.Connected)
		{
			UnityEngine.Debug.Log("ConnectToPhoton return");
			return;
		}
		UnityEngine.Debug.Log("ConnectToPhoton");
		if (FriendsController.sharedController != null && FriendsController.sharedController.Banned == 1)
		{
			timerShowBan = 3f;
			return;
		}
		isConnectingToPhoton = true;
		isCancelConnectingToPhoton = false;
		GameConnect.ConnectToPhoton(GameConnect.GetTierForRoom());
	}

	private void StartSearchLocalServers()
	{
		lanScan.StartSearchBroadCasting(SeachServer);
	}

	private void SeachServer(string ipServerSeaches)
	{
		bool flag = false;
		if (servers.Count > 0)
		{
			foreach (infoServer server in servers)
			{
				if (server.ipAddress.Equals(ipServerSeaches))
				{
					flag = true;
				}
			}
		}
		if (!flag)
		{
			infoServer item = default(infoServer);
			item.ipAddress = ipServerSeaches;
			servers.Add(item);
		}
	}

	private int LocalServerComparison(LANBroadcastService.ReceivedMessage msg1, LANBroadcastService.ReceivedMessage msg2)
	{
		return msg1.ipAddress.CompareTo(msg2.ipAddress);
	}

	private void SetLocalRoomInfo(GameInfo _gameInfo, LANBroadcastService.ReceivedMessage _roomInfo)
	{
		string text = _roomInfo.name;
		if (string.IsNullOrEmpty(text))
		{
			text = LocalizationStore.Get("Key_0948");
		}
		_gameInfo.serverNameLabel.text = text;
		_gameInfo.countPlayersLabel.text = _roomInfo.connectedPlayers + "/" + _roomInfo.playerLimit;
		_gameInfo.openSprite.SetActive(true);
		_gameInfo.closeSprite.SetActive(false);
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(_roomInfo.map);
		string text2 = infoScene.TranslatePreviewName.ToUpper();
		_gameInfo.mapNameLabel.SetText(text2);
		_gameInfo.roomInfoLocal = _roomInfo;
		_gameInfo.mapTexture.mainTexture = GetMapTexture(infoScene.NameScene);
		_gameInfo.SizeMapNameLbl[0].SetActive(infoScene.sizeMap == InfoSizeMap.small);
		_gameInfo.SizeMapNameLbl[1].SetActive(infoScene.sizeMap == InfoSizeMap.normal);
		_gameInfo.SizeMapNameLbl[2].SetActive(infoScene.sizeMap == InfoSizeMap.big || infoScene.sizeMap == InfoSizeMap.veryBig);
	}

	public void UpdateLocalServersList()
	{
		List<LANBroadcastService.ReceivedMessage> list = new List<LANBroadcastService.ReceivedMessage>();
		for (int i = 0; i < lanScan.lstReceivedMessages.Count; i++)
		{
			bool flag = Defs.filterMaps.ContainsKey(lanScan.lstReceivedMessages[i].map) && Defs.filterMaps[lanScan.lstReceivedMessages[i].map] == 3;
			if (((GameConnect.isDaterRegim && flag) || (!GameConnect.isDaterRegim && !flag)) && lanScan.lstReceivedMessages[i].regim == (int)GameConnect.gameMode)
			{
				list.Add(lanScan.lstReceivedMessages[i]);
			}
		}
		countNote = 50;
		if (list.Count < countNote)
		{
			countNote = list.Count;
		}
		while (countNote < gamesInfo.Count)
		{
			UnityEngine.Object.Destroy(gamesInfo[gamesInfo.Count - 1]);
			gamesInfo.RemoveAt(gamesInfo.Count - 1);
		}
		if (countNote > gamesInfo.Count)
		{
			countColumn = (((double)((float)Screen.width / (float)Screen.height) < 1.5) ? 3 : 4);
			_widthCell = (CustomPanelConnectScene.Instance.fonGames.localSize.x - (float)(countColumn * 10)) / (float)countColumn;
			if (countNote > gamesInfo.Count)
			{
				CustomPanelConnectScene.Instance.gameInfoItemPrefab.SetActive(true);
			}
			while (countNote > gamesInfo.Count)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(CustomPanelConnectScene.Instance.gameInfoItemPrefab);
				gameObject.name = "GameInfo_" + gamesInfo.Count;
				gameObject.transform.SetParent(CustomPanelConnectScene.Instance.gridGamesTransform, false);
				_scale = _widthCell / gameObject.GetComponent<GameInfo>().mapTexture.localSize.x;
				_heightCell = gameObject.GetComponent<GameInfo>().mapTexture.localSize.y * _scale;
				gameObject.transform.GetChild(0).localScale = new Vector3(_scale, _scale, _scale);
				gamesInfo.Add(gameObject);
				CustomPanelConnectScene.Instance.gameInfoItemPrefab.SetActive(false);
			}
			if (CustomPanelConnectScene.Instance.gameInfoItemPrefab.activeSelf)
			{
				CustomPanelConnectScene.Instance.gameInfoItemPrefab.SetActive(false);
			}
			CustomPanelConnectScene.Instance.gridGames.GetComponent<UIGrid>().cellWidth = _widthCell + borderWidth;
			CustomPanelConnectScene.Instance.gridGames.GetComponent<UIGrid>().cellHeight = _heightCell + borderWidth;
			CustomPanelConnectScene.Instance.gridGames.GetComponent<UIGrid>().maxPerLine = countColumn;
		}
		float num = CustomPanelConnectScene.Instance.scrollGames.bounds.extents.y * 2f;
		float y = CustomPanelConnectScene.Instance.scrollGames.GetComponent<UIPanel>().GetViewSize().y;
		CustomPanelConnectScene.Instance.gridGames.Reposition();
		if (!isFirstGamesReposition || num < y)
		{
			CustomPanelConnectScene.Instance.gridGames.transform.localPosition = new Vector3((0f - (_widthCell + borderWidth)) * ((float)countColumn * 0.5f - 0.5f), CustomPanelConnectScene.Instance.gridGames.transform.localPosition.y, CustomPanelConnectScene.Instance.gridGames.transform.localPosition.z);
			CustomPanelConnectScene.Instance.scrollGames.ResetPosition();
			isFirstGamesReposition = true;
		}
		for (int j = 0; j < countNote; j++)
		{
			SetLocalRoomInfo(gamesInfo[j].GetComponent<GameInfo>(), list[j]);
		}
	}

	public void JoinToLocalRoom(LANBroadcastService.ReceivedMessage _roomInfo)
	{
		if (_roomInfo.connectedPlayers == _roomInfo.playerLimit)
		{
			CustomPanelConnectScene.Instance.gameIsfullLabel.timer = 3f;
			CustomPanelConnectScene.Instance.gameIsfullLabel.gameObject.SetActive(true);
			CustomPanelConnectScene.Instance.incorrectPasswordLabel.timer = -1f;
			CustomPanelConnectScene.Instance.incorrectPasswordLabel.gameObject.SetActive(false);
			return;
		}
		GlobalGameController.countKillsBlue = 0;
		GlobalGameController.countKillsRed = 0;
		Defs.ServerIp = _roomInfo.ipAddress;
		PlayerPrefs.SetString("MaxKill", _roomInfo.comment);
		PlayerPrefs.SetString("MapName", _roomInfo.map);
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset(Defs.filterMaps.ContainsKey(_roomInfo.map) ? Defs.filterMaps[_roomInfo.map] : 0);
		}
		Initializer.isLocalServer = false;
		StartCoroutine(SetFonLoadingWaitForReset(_roomInfo.map));
		Invoke("goGame", 0.1f);
	}

	public void JoinToRoomPhoton(RoomInfo _roomInfo)
	{
		if (_roomInfo.playerCount == _roomInfo.maxPlayers)
		{
			CustomPanelConnectScene.Instance.gameIsfullLabel.timer = 3f;
			CustomPanelConnectScene.Instance.gameIsfullLabel.gameObject.SetActive(true);
			CustomPanelConnectScene.Instance.incorrectPasswordLabel.timer = -1f;
			CustomPanelConnectScene.Instance.incorrectPasswordLabel.gameObject.SetActive(false);
			return;
		}
		joinRoomInfoFromCustom = _roomInfo;
		if (string.IsNullOrEmpty(_roomInfo.customProperties[GameConnect.passwordProperty].ToString()))
		{
			JoinToRoomPhotonAfterCheck();
			return;
		}
		CustomPanelConnectScene.Instance.gameIsfullLabel.timer = -1f;
		CustomPanelConnectScene.Instance.gameIsfullLabel.gameObject.SetActive(false);
		CustomPanelConnectScene.Instance.incorrectPasswordLabel.timer = 3f;
		CustomPanelConnectScene.Instance.incorrectPasswordLabel.gameObject.SetActive(true);
		CustomPanelConnectScene.Instance.enterPasswordInput.value = string.Empty;
		CustomPanelConnectScene.Instance.enterPasswordPanel.SetActive(true);
		CustomPanelConnectScene.Instance.enterPasswordInput.isSelected = false;
		CustomPanelConnectScene.Instance.enterPasswordInput.isSelected = true;
		CustomPanelConnectScene.Instance.customPanel.SetActive(false);
	}

	private void EnterPassInputSubmit()
	{
		CustomPanelConnectScene.Instance.enterPasswordInput.RemoveFocus();
		CustomPanelConnectScene.Instance.enterPasswordInput.isSelected = false;
		Invoke("EnterPassInput", 0.1f);
	}

	private void EnterPassInput()
	{
		HandleJoinRoomFromEnterPasswordBtnClicked(null, null);
	}

	public void JoinToRoomPhotonAfterCheck()
	{
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(joinRoomInfoFromCustom.customProperties[GameConnect.mapProperty].ToString()));
		StartCoroutine(SetFonLoadingWaitForReset(infoScene.NameScene));
		loadingMapPanel.SetActive(true);
		PhotonNetwork.JoinRoom(joinRoomInfoFromCustom.name);
		ActivityIndicator.IsActiveIndicator = true;
	}

	private void SetPosSelectMapPanelInMainMenu()
	{
		if (!GameConnect.isDaterRegim)
		{
			if (posSelectMapPanelInMainMenu.y < 9000f)
			{
				selectMapPanelTransform.localPosition = posSelectMapPanelInMainMenu;
			}
		}
		else
		{
			selectMapPanelTransform.localPosition = Vector3.zero;
		}
	}

	private void SetPosSelectMapPanelInCreatePanel()
	{
		posSelectMapPanelInMainMenu = selectMapPanelTransform.localPosition;
		selectMapPanelTransform.localPosition = Vector3.zero;
		if (GameConnect.isDaterRegim)
		{
			selectMapPanelTransform.localPosition = new Vector3(0f, -90f, 0f);
		}
	}

	private void goGame()
	{
		WeaponManager.sharedManager.Reset(Defs.filterMaps.ContainsKey(PlayerPrefs.GetString("MapName")) ? Defs.filterMaps[PlayerPrefs.GetString("MapName")] : 0);
		Singleton<SceneLoader>.Instance.LoadScene(PlayerPrefs.GetString("MapName"));
	}

	private void OnDestroy()
	{
		UnityEngine.Debug.Log("OnDestroy ConnectSceneController");
		if (base.gameObject.activeSelf && (!Defs.isInet || (!isGoInPhotonGame && PhotonNetwork.connectionState == ConnectionState.Connected) || PhotonNetwork.connectionState == ConnectionState.Connecting))
		{
			GameConnect.Disconnect();
			UnityEngine.Debug.Log("PhotonNetwork.Disconnect()");
		}
		if (ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.isShowRanks = false;
			ExperienceController.sharedController.isMenu = false;
			ExperienceController.sharedController.isConnectScene = false;
		}
		lanScan.StopBroadCasting();
		sharedController = null;
	}

	public void HandleShopClicked()
	{
		if (!ShopNGUIController.GuiActive && !MainMenuController.IsLevelUpOrBannerShown() && (!(connectToPhotonPanel != null) || !connectToPhotonPanel.activeInHierarchy))
		{
			ShopNGUIController.sharedShop.SetInGame(false);
			ShopNGUIController.GuiActive = true;
			ShopNGUIController.sharedShop.resumeAction = HandleResumeFromShop;
		}
	}

	public void HandleResumeFromShop()
	{
		ShopNGUIController.GuiActive = false;
		ShopNGUIController.sharedShop.resumeAction = delegate
		{
		};
		if (ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.isShowRanks = true;
		}
	}
}
