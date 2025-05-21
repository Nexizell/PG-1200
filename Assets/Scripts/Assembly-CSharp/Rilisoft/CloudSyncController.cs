using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Rilisoft.MiniJson;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Rilisoft
{
	public class CloudSyncController
	{
		public enum CurrentPulledPurchasesState
		{
			BadDataOrNoProgress = 0,
			HasProgress = 1,
			DelimitedRequired = 2
		}

		internal class CloudSlotSyncingPair
		{
			public CloudSlotSynchronizer SlotSynchronizer { get; private set; }

			public CloudApplyer SlotApplyer { get; private set; }

			public CloudSlotSyncingPair(CloudSlotSynchronizer slotSynchronizer, CloudApplyer slotApplyer)
			{
				SlotSynchronizer = slotSynchronizer;
				SlotApplyer = slotApplyer;
			}

			public override string ToString()
			{
				string text = ((SlotSynchronizer == null) ? typeof(CloudSlotSynchronizer).Name : SlotSynchronizer.GetType().Name);
				string text2 = ((SlotApplyer == null) ? typeof(CloudApplyer).Name : SlotApplyer.GetType().Name);
				return string.Format("({0},{1})", new object[2] { text, text2 });
			}
		}

		[CompilerGenerated]
		internal sealed class _003C_003Ec__DisplayClass13_0
		{
			public IDisposable hardwareBackSubscription;

			internal void _003CSynchronizeWithCloud_DataAlreadyPulled_003Eb__1()
			{
				IsSynchronizingWithCloud = false;
				InfoWindowController.HideCurrentWindow();
				ActivityIndicator.IsActiveIndicator = false;
				if (hardwareBackSubscription != null)
				{
					hardwareBackSubscription.Dispose();
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _003CSynchronizeWithCloud_DataAlreadyPulled_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			private _003C_003Ec__DisplayClass13_0 _003C_003E8__1;

			public bool doPull;

			private TaskCompletionSource<bool> _003CamazonLoginPromise_003E5__2;

			private Action _003ConReturn_003E5__3;

			private TaskCompletionSource<bool> _003CpullPromise_003E5__4;

			public bool suppressRestoreWindow;

			public bool setupSyncRelatedAppData;

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
			public _003CSynchronizeWithCloud_DataAlreadyPulled_003Ed__13(int _003C_003E1__state)
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
					_003C_003E8__1 = new _003C_003Ec__DisplayClass13_0();
					if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
					{
						return false;
					}
					IsSynchronizingWithCloud = true;
					InfoWindowController.BlockAllClick();
					_003C_003E8__1.hardwareBackSubscription = BackSystem.Instance.Register(delegate
					{
					}, "SynchronizeWithCloud_DataAlreadyPulled");
					_003C_003E2__current = CoroutineRunner.Instance.StartCoroutine(WaitForActivityIndicator());
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					ActivityIndicator.IsActiveIndicator = true;
					_003ConReturn_003E5__3 = delegate
					{
						IsSynchronizingWithCloud = false;
						InfoWindowController.HideCurrentWindow();
						ActivityIndicator.IsActiveIndicator = false;
						if (_003C_003E8__1.hardwareBackSubscription != null)
						{
							_003C_003E8__1.hardwareBackSubscription.Dispose();
						}
					};
					if (doPull)
					{
						_003CamazonLoginPromise_003E5__2 = new TaskCompletionSource<bool>();
						_003C_003E2__current = CoroutineRunner.Instance.StartCoroutine(LoginToAmazonIfNeeded(_003CamazonLoginPromise_003E5__2));
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_01e1;
				case 2:
					_003C_003E1__state = -1;
					if (!_003CamazonLoginPromise_003E5__2.Task.Result)
					{
						_003ConReturn_003E5__3();
						return false;
					}
					_003CpullPromise_003E5__4 = new TaskCompletionSource<bool>();
					_003C_003E2__current = CoroutineRunner.Instance.StartCoroutine(PullCore(_003CpullPromise_003E5__4, false));
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -1;
					if (!((Task)_003CpullPromise_003E5__4.Task).IsCompleted || ((Task)_003CpullPromise_003E5__4.Task).IsFaulted || ((Task)_003CpullPromise_003E5__4.Task).IsCanceled)
					{
						UnityEngine.Debug.LogErrorFormat("PullCore not completed the task");
						_003ConReturn_003E5__3();
						return false;
					}
					if (!_003CpullPromise_003E5__4.Task.Result)
					{
						_003ConReturn_003E5__3();
						return false;
					}
					_003CamazonLoginPromise_003E5__2 = null;
					_003CpullPromise_003E5__4 = null;
					goto IL_01e1;
				case 4:
					_003C_003E1__state = -1;
					goto IL_025c;
				case 5:
					{
						_003C_003E1__state = -1;
						PostSyncAppSetup(setupSyncRelatedAppData);
						Instance.OnExplicitSyncCompleted();
						_003ConReturn_003E5__3();
						return false;
					}
					IL_025c:
					_003C_003E2__current = CoroutineRunner.Instance.StartCoroutine(Instance.ApplyChanges(Storager.getInt("HackDetected") > 0));
					_003C_003E1__state = 5;
					return true;
					IL_01e1:
					if (!AreProgressInCurrentPullResult())
					{
						_003ConReturn_003E5__3();
						return false;
					}
					if (ConnectScene.isEnable)
					{
						_003ConReturn_003E5__3();
						return false;
					}
					if (LobbyCraftController.Instance != null && LobbyCraftController.Instance.InterfaceEnabled)
					{
						_003ConReturn_003E5__3();
						return false;
					}
					if (!suppressRestoreWindow)
					{
						_003C_003E2__current = CoroutineRunner.Instance.StartCoroutine(ShowRestoreWindow());
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_025c;
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
		internal sealed class _003CSynchronizeWithCloud_NotAuthenticated_003Ed__14 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			private TaskCompletionSource<bool> _003CpullPromise_003E5__1;

			private Action _003ConReturn_003E5__2;

			private IDisposable _003ChardwareBackSubscription_003E5__3;

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
			public _003CSynchronizeWithCloud_NotAuthenticated_003Ed__14(int _003C_003E1__state)
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
					if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
					{
						return false;
					}
					if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
					{
						return false;
					}
					IsSynchronizingWithCloud = true;
					_003ConReturn_003E5__2 = delegate
					{
						IsSynchronizingWithCloud = false;
					};
					_003CpullPromise_003E5__1 = new TaskCompletionSource<bool>();
					_003C_003E2__current = CoroutineRunner.Instance.StartCoroutine(PullCore(_003CpullPromise_003E5__1, true));
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					if (!((Task)_003CpullPromise_003E5__1.Task).IsCompleted || ((Task)_003CpullPromise_003E5__1.Task).IsFaulted || ((Task)_003CpullPromise_003E5__1.Task).IsCanceled)
					{
						UnityEngine.Debug.LogErrorFormat("PullCore not completed the task");
						_003ConReturn_003E5__2();
						return false;
					}
					if (!_003CpullPromise_003E5__1.Task.Result)
					{
						_003ConReturn_003E5__2();
						return false;
					}
					if (!AreProgressInCurrentPullResult())
					{
						_003ConReturn_003E5__2();
						return false;
					}
					TrainingController.OnGetProgress();
					goto IL_014e;
				case 2:
					_003C_003E1__state = -1;
					goto IL_014e;
				case 3:
					_003C_003E1__state = -1;
					_003ChardwareBackSubscription_003E5__3 = BackSystem.Instance.Register(delegate
					{
					}, "SynchronizeWithCloud_NotAuthenticated");
					ActivityIndicator.IsActiveIndicator = true;
					InfoWindowController.BlockAllClick();
					_003C_003E2__current = CoroutineRunner.Instance.StartCoroutine(Instance.ApplyChanges(Storager.getInt("HackDetected") > 0));
					_003C_003E1__state = 4;
					return true;
				case 4:
					{
						_003C_003E1__state = -1;
						PostSyncAppSetup(true);
						Instance.OnExplicitSyncCompleted();
						_003ConReturn_003E5__2();
						InfoWindowController.HideCurrentWindow();
						ActivityIndicator.IsActiveIndicator = false;
						if (_003ChardwareBackSubscription_003E5__3 != null)
						{
							_003ChardwareBackSubscription_003E5__3.Dispose();
						}
						return false;
					}
					IL_014e:
					if (MainMenuController.sharedController != null && MainMenuController.sharedController.IsSomeWindowOpenExceptSettings())
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003E2__current = CoroutineRunner.Instance.StartCoroutine(ShowRestoreWindow());
					_003C_003E1__state = 3;
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

		[CompilerGenerated]
		internal sealed class _003CSynchronizeWithCloud_NotAuthenticated_SecondLaunch_003Ed__15 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			private TaskCompletionSource<bool> _003CpullPromise_003E5__1;

			private Action _003ConReturn_003E5__2;

			private UnityAction<Scene, Scene> _003CstopSyncing_003E5__3;

			private IDisposable _003ChardwareBackSubscription_003E5__4;

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
			public _003CSynchronizeWithCloud_NotAuthenticated_SecondLaunch_003Ed__15(int _003C_003E1__state)
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
					if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
					{
						return false;
					}
					if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
					{
						return false;
					}
					IsSynchronizingWithCloud = true;
					_003ConReturn_003E5__2 = delegate
					{
						IsSynchronizingWithCloud = false;
					};
					_003CpullPromise_003E5__1 = new TaskCompletionSource<bool>();
					_003C_003E2__current = CoroutineRunner.Instance.StartCoroutine(PullCore(_003CpullPromise_003E5__1, false));
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					if (!((Task)_003CpullPromise_003E5__1.Task).IsCompleted || ((Task)_003CpullPromise_003E5__1.Task).IsFaulted || ((Task)_003CpullPromise_003E5__1.Task).IsCanceled)
					{
						UnityEngine.Debug.LogErrorFormat("PullCore not completed the task");
						_003ConReturn_003E5__2();
						return false;
					}
					if (!_003CpullPromise_003E5__1.Task.Result)
					{
						_003ConReturn_003E5__2();
						return false;
					}
					if (!AreProgressInCurrentPullResult())
					{
						_003ConReturn_003E5__2();
						return false;
					}
					if (ConnectScene.isEnable)
					{
						_003ConReturn_003E5__2();
						return false;
					}
					if (LobbyCraftController.Instance != null && LobbyCraftController.Instance.InterfaceEnabled)
					{
						_003ConReturn_003E5__2();
						return false;
					}
					if (SceneManager.GetActiveScene().name != Defs.MainMenuScene)
					{
						_003ConReturn_003E5__2();
						return false;
					}
					_003CstopSyncing_003E5__3 = delegate
					{
						IsSynchronizingWithCloud = false;
					};
					SceneManager.activeSceneChanged += _003CstopSyncing_003E5__3;
					goto IL_01d9;
				case 2:
					_003C_003E1__state = -1;
					goto IL_01d9;
				case 3:
					_003C_003E1__state = -1;
					_003ChardwareBackSubscription_003E5__4 = BackSystem.Instance.Register(delegate
					{
					}, "SynchronizeWithCloud_NotAuthenticated_SecondLaunch");
					ActivityIndicator.IsActiveIndicator = true;
					InfoWindowController.BlockAllClick();
					_003C_003E2__current = CoroutineRunner.Instance.StartCoroutine(Instance.ApplyChanges(Storager.getInt("HackDetected") > 0));
					_003C_003E1__state = 4;
					return true;
				case 4:
					{
						_003C_003E1__state = -1;
						PostSyncAppSetup(true);
						Instance.OnExplicitSyncCompleted();
						_003ConReturn_003E5__2();
						InfoWindowController.HideCurrentWindow();
						ActivityIndicator.IsActiveIndicator = false;
						if (_003ChardwareBackSubscription_003E5__4 != null)
						{
							_003ChardwareBackSubscription_003E5__4.Dispose();
						}
						return false;
					}
					IL_01d9:
					if (IsSynchronizingWithCloud && MainMenuController.sharedController != null && MainMenuController.sharedController.IsSomeWindowOpenExceptSettings() && !ConnectScene.isEnable && (LobbyCraftController.Instance == null || !LobbyCraftController.Instance.InterfaceEnabled))
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 2;
						return true;
					}
					SceneManager.activeSceneChanged -= _003CstopSyncing_003E5__3;
					if (!IsSynchronizingWithCloud)
					{
						_003ConReturn_003E5__2();
						return false;
					}
					if (ConnectScene.isEnable)
					{
						_003ConReturn_003E5__2();
						return false;
					}
					if (LobbyCraftController.Instance != null && LobbyCraftController.Instance.InterfaceEnabled)
					{
						_003ConReturn_003E5__2();
						return false;
					}
					_003C_003E2__current = CoroutineRunner.Instance.StartCoroutine(ShowRestoreWindow());
					_003C_003E1__state = 3;
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

		[CompilerGenerated]
		internal sealed class _003C_003Ec__DisplayClass18_0
		{
			public float enterBackgroundTime;

			public float elapsedTime;

			internal void _003CPullCore_003Eb__2(bool pause)
			{
				if (pause)
				{
					enterBackgroundTime = Time.realtimeSinceStartup;
					return;
				}
				float num = Time.realtimeSinceStartup - enterBackgroundTime;
				elapsedTime -= num;
			}
		}

		[CompilerGenerated]
		internal sealed class _003CPullCore_003Ed__18 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public TaskCompletionSource<bool> pullPromise;

			private _003C_003Ec__DisplayClass18_0 _003C_003E8__1;

			public bool stopOnEnterShop;

			private IEnumerable<Task<CloudPullResult>> _003Cfutures_003E5__2;

			private Action<bool> _003CremoveBackgroundTimeFromElapsed_003E5__3;

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
			public _003CPullCore_003Ed__18(int _003C_003E1__state)
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
					_003C_003E8__1.elapsedTime += Time.unscaledDeltaTime;
					if (_003C_003E8__1.elapsedTime >= 50f || (stopOnEnterShop && ShopNGUIController.GuiActive))
					{
						goto IL_01ef;
					}
				}
				else
				{
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass18_0();
					_003Cfutures_003E5__2 = null;
					try
					{
						_003Cfutures_003E5__2 = Instance.Pull(false);
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogErrorFormat("Exception in PullCore CloudSyncController.Instance.Pull: {0}", ex);
					}
					if (_003Cfutures_003E5__2 == null)
					{
						UnityEngine.Debug.LogErrorFormat("PullCore: futures == null");
						pullPromise.SetResult(false);
						return false;
					}
					if ((Defs.IsDeveloperBuild || Application.isEditor) && _003Cfutures_003E5__2.Any((Task<CloudPullResult> f) => f == null))
					{
						UnityEngine.Debug.LogErrorFormat("PullCore: null futures!");
					}
					_003Cfutures_003E5__2 = _003Cfutures_003E5__2.Where((Task<CloudPullResult> f) => f != null).ToList();
					if (_003Cfutures_003E5__2.Count() == 0)
					{
						UnityEngine.Debug.LogErrorFormat("PullCore: all futres are nulls");
						pullPromise.SetResult(false);
						return false;
					}
					_003C_003E8__1.elapsedTime = 0f;
					_003C_003E8__1.enterBackgroundTime = float.MaxValue;
					_003CremoveBackgroundTimeFromElapsed_003E5__3 = delegate(bool pause)
					{
						if (pause)
						{
							_003C_003E8__1.enterBackgroundTime = Time.realtimeSinceStartup;
						}
						else
						{
							float num2 = Time.realtimeSinceStartup - _003C_003E8__1.enterBackgroundTime;
							_003C_003E8__1.elapsedTime -= num2;
						}
					};
					LifetimeEventsRaiser.Instance.ApplicationPause += _003CremoveBackgroundTimeFromElapsed_003E5__3;
				}
				if (_003Cfutures_003E5__2.Any((Task<CloudPullResult> f) => !((Task)f).IsCompleted))
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_01ef;
				IL_01ef:
				LifetimeEventsRaiser.Instance.ApplicationPause -= _003CremoveBackgroundTimeFromElapsed_003E5__3;
				if (stopOnEnterShop && ShopNGUIController.GuiActive)
				{
					UnityEngine.Debug.LogErrorFormat("PullCore: ShopNGUIController.GuiActive");
					pullPromise.SetResult(false);
					return false;
				}
				if (_003Cfutures_003E5__2.All((Task<CloudPullResult> f) => !((Task)f).IsCompleted))
				{
					UnityEngine.Debug.LogErrorFormat("PullCore: all pulls not completed within timeout");
					pullPromise.SetResult(false);
					return false;
				}
				List<Task<CloudPullResult>> list = _003Cfutures_003E5__2.Where((Task<CloudPullResult> f) => ((Task)f).IsCompleted && !((Task)f).IsFaulted && !((Task)f).IsCanceled).ToList();
				if (list == null || list.Count == 0)
				{
					UnityEngine.Debug.LogErrorFormat("PullCore: all futures timed out or failed or canceled");
					pullPromise.SetResult(false);
					return false;
				}
				if (list.Any((Task<CloudPullResult> f) => f.Result == CloudPullResult.LoginFailed))
				{
					UnityEngine.Debug.LogWarningFormat("PullCore: login failed");
					pullPromise.SetResult(false);
					return false;
				}
				if (list.All((Task<CloudPullResult> f) => f.Result != CloudPullResult.Successful))
				{
					string format = "PullCore: all pulls failed (futures completed)";
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.LogErrorFormat(format);
					}
					else
					{
						UnityEngine.Debug.LogWarningFormat(format);
					}
					pullPromise.SetResult(false);
					return false;
				}
				pullPromise.SetResult(true);
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
		internal sealed class _003C_003Ec__DisplayClass19_0
		{
			public int startWaitAcitvivtyIndicatorFrame;

			internal bool _003CWaitForActivityIndicator_003Eb__0()
			{
				if (ActivityIndicator.IsActiveIndicator)
				{
					return Time.frameCount - startWaitAcitvivtyIndicatorFrame >= 60;
				}
				return true;
			}
		}

		[CompilerGenerated]
		internal sealed class _003CWaitForActivityIndicator_003Ed__19 : IEnumerator<object>, IEnumerator, IDisposable
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
			public _003CWaitForActivityIndicator_003Ed__19(int _003C_003E1__state)
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
					_003C_003E2__current = new WaitUntil(new _003C_003Ec__DisplayClass19_0
					{
						startWaitAcitvivtyIndicatorFrame = Time.frameCount
					}._003CWaitForActivityIndicator_003Eb__0);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
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
		internal sealed class _003C_003Ec__DisplayClass20_0
		{
			public int startWaitSharedMainMenuFrame;

			public bool restoreWindowShown;

			internal bool _003CShowRestoreWindow_003Eb__0()
			{
				if (!(MainMenuController.sharedController != null))
				{
					return Time.frameCount - startWaitSharedMainMenuFrame >= 60;
				}
				return true;
			}

			internal void _003CShowRestoreWindow_003Eb__1()
			{
				restoreWindowShown = false;
			}
		}

		[CompilerGenerated]
		internal sealed class _003CShowRestoreWindow_003Ed__20 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			private _003C_003Ec__DisplayClass20_0 _003C_003E8__1;

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
			public _003CShowRestoreWindow_003Ed__20(int _003C_003E1__state)
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
					_003C_003E8__1 = new _003C_003Ec__DisplayClass20_0();
					_003C_003E8__1.startWaitSharedMainMenuFrame = Time.frameCount;
					_003C_003E2__current = new WaitUntil(() => MainMenuController.sharedController != null || Time.frameCount - _003C_003E8__1.startWaitSharedMainMenuFrame >= 60);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0086;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0086;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0086:
					if (MainMenuController.sharedController != null && MainMenuController.sharedController.stubLoading.activeInHierarchy)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 2;
						return true;
					}
					ActivityIndicator.IsActiveIndicator = false;
					InfoWindowController.HideCurrentWindow();
					_003C_003E8__1.restoreWindowShown = true;
					InfoWindowController.ShowRestorePanel(delegate
					{
						_003C_003E8__1.restoreWindowShown = false;
					});
					break;
				}
				if (_003C_003E8__1.restoreWindowShown)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				}
				InfoWindowController.BlockAllClick();
				ActivityIndicator.IsActiveIndicator = true;
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
		internal sealed class _003C_003Ec__DisplayClass22_0
		{
			public bool initialDataComes;

			internal void _003CLoginToAmazonIfNeeded_003Eb__0()
			{
				initialDataComes = true;
			}
		}

		[CompilerGenerated]
		internal sealed class _003CLoginToAmazonIfNeeded_003Ed__22 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			private float _003CendTime_003E5__1;

			public TaskCompletionSource<bool> promise;

			private _003C_003Ec__DisplayClass22_0 _003C_003E8__2;

			private float _003CstartWaitingForInitialDownloadTime_003E5__3;

			private Action _003CinitialDataDownloaded_003E5__4;

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
			public _003CLoginToAmazonIfNeeded_003Ed__22(int _003C_003E1__state)
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
					if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon && !Instance.IsAuthenticatedToCloud())
					{
						if (!GameCircleSocial.Instance.localUser.authenticated)
						{
							UnityEngine.Debug.LogFormat("[Rilisoft] SynchronizeWithCloud_DataAlreadyPulled Sign in to GameCircle");
							AGSClient.ShowSignInPage();
						}
						_003CendTime_003E5__1 = Time.realtimeSinceStartup + 60f;
						goto IL_009e;
					}
					promise.SetResult(true);
					break;
				case 1:
					_003C_003E1__state = -1;
					goto IL_009e;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0147;
				case 3:
					{
						_003C_003E1__state = -1;
						goto IL_0167;
					}
					IL_009e:
					if (!GameCircleSocial.Instance.localUser.authenticated && Time.realtimeSinceStartup < _003CendTime_003E5__1)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 1;
						return true;
					}
					promise.SetResult(GameCircleSocial.Instance.localUser.authenticated);
					if (!GameCircleSocial.Instance.localUser.authenticated)
					{
						break;
					}
					_003C_003E8__2 = new _003C_003Ec__DisplayClass22_0();
					_003C_003E8__2.initialDataComes = false;
					_003CinitialDataDownloaded_003E5__4 = delegate
					{
						_003C_003E8__2.initialDataComes = true;
					};
					AGSWhispersyncClient.OnNewCloudDataEvent += _003CinitialDataDownloaded_003E5__4;
					_003CstartWaitingForInitialDownloadTime_003E5__3 = Time.realtimeSinceStartup;
					goto IL_0147;
					IL_0167:
					if (!_003C_003E8__2.initialDataComes && Time.realtimeSinceStartup - _003CstartWaitingForInitialDownloadTime_003E5__3 < 60f)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 3;
						return true;
					}
					AGSWhispersyncClient.OnNewCloudDataEvent -= _003CinitialDataDownloaded_003E5__4;
					_003C_003E8__2 = null;
					_003CinitialDataDownloaded_003E5__4 = null;
					break;
					IL_0147:
					if (!AGSClient.IsServiceReady())
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0167;
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
		internal sealed class _003CApplyChanges_003Ed__30 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public CloudSyncController _003C_003E4__this;

			public bool skipApplyingToLocalState;

			private CloudSlotSyncingPair _003CsyncPair_003E5__1;

			private Dictionary<string, CloudSlotSyncingPair>.ValueCollection.Enumerator _003C_003E7__wrap1;

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
			public _003CApplyChanges_003Ed__30(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int num = _003C_003E1__state;
				if (num == -3 || num == 1 || num == 2)
				{
					try
					{
					}
					finally
					{
						_003C_003Em__Finally1();
					}
				}
			}

			private bool MoveNext()
			{
				try
				{
					switch (_003C_003E1__state)
					{
					default:
						return false;
					case 0:
						_003C_003E1__state = -1;
						if (AreProgressInCurrentPullResultCore() == CurrentPulledPurchasesState.DelimitedRequired)
						{
							return false;
						}
						_003C_003E7__wrap1 = _003C_003E4__this.m_syncingPairs.Values.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_010d;
					case 1:
						_003C_003E1__state = -3;
						if (!skipApplyingToLocalState)
						{
							_003C_003E2__current = null;
							_003C_003E1__state = 2;
							return true;
						}
						goto IL_0106;
					case 2:
						_003C_003E1__state = -3;
						goto IL_0106;
					case 3:
						{
							_003C_003E1__state = -1;
							break;
						}
						IL_010d:
						if (_003C_003E7__wrap1.MoveNext())
						{
							_003CsyncPair_003E5__1 = _003C_003E7__wrap1.Current;
							if (_003CsyncPair_003E5__1.SlotSynchronizer.Pulled)
							{
								_003C_003E2__current = CoroutineRunner.Instance.StartCoroutine(_003CsyncPair_003E5__1.SlotApplyer.Apply(skipApplyingToLocalState));
								_003C_003E1__state = 1;
								return true;
							}
							UnityEngine.Debug.LogErrorFormat("CloudSyncController: Trying to merge and push synchronizer that never pulled: {0}", _003CsyncPair_003E5__1.ToString());
							goto IL_0106;
						}
						_003C_003Em__Finally1();
						_003C_003E7__wrap1 = default(Dictionary<string, CloudSlotSyncingPair>.ValueCollection.Enumerator);
						PostApply();
						if (!skipApplyingToLocalState && WeaponManager.sharedManager != null)
						{
							_003C_003E2__current = CoroutineRunner.Instance.StartCoroutine(WeaponManager.sharedManager.ResetCoroutine(WeaponManager.sharedManager.CurrentFilterMap));
							_003C_003E1__state = 3;
							return true;
						}
						UnityEngine.Debug.LogErrorFormat("ApplyChanges WeaponManager.sharedManager == null");
						break;
						IL_0106:
						_003CsyncPair_003E5__1 = null;
						goto IL_010d;
					}
					_003C_003E4__this.m_lastTimeAppliedInApplyPoint = Time.realtimeSinceStartup;
					Action applyCompleted = CloudSyncController.ApplyCompleted;
					if (applyCompleted != null)
					{
						applyCompleted();
					}
					return false;
				}
				catch
				{
					//try-fault
					((IDisposable)this).Dispose();
					throw;
				}
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			private void _003C_003Em__Finally1()
			{
				_003C_003E1__state = -1;
				((IDisposable)_003C_003E7__wrap1).Dispose();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		[CompilerGenerated]
		internal sealed class _003CPullAndApplyChangesIfNeeded_003Ed__31 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public CloudSyncController _003C_003E4__this;

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
			public _003CPullAndApplyChangesIfNeeded_003Ed__31(int _003C_003E1__state)
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
					if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
					{
						return false;
					}
					if (Time.realtimeSinceStartup - _003C_003E4__this.IntervalBetweenApplyInApplyPoints < _003C_003E4__this.m_lastTimeAppliedInApplyPoint)
					{
						return false;
					}
					try
					{
						Instance.Pull(true);
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogErrorFormat("CloudSyncController: Exception in PullAndApplyChangesIfNeeded: {0}", ex);
					}
					_003C_003E2__current = CoroutineRunner.Instance.StartCoroutine(Instance.ApplyChanges(Storager.getInt("HackDetected") > 0));
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
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

		private float m_lastTimeAppliedInApplyPoint = float.MinValue;

		private Dictionary<string, CloudSlotSyncingPair> m_syncingPairs;

		private static CloudSyncController s_instance;

		public static bool CheckSettingsOfLobbyBackground;

		public static CloudSyncController Instance
		{
			get
			{
				if (s_instance == null)
				{
					s_instance = new CloudSyncController();
				}
				return s_instance;
			}
		}

		public static bool IsSynchronizingWithCloud { get; private set; }

		private float IntervalBetweenApplyInApplyPoints
		{
			get
			{
				return 300f;
			}
		}

		public static event Action ApplyCompleted;

		public static event Action ExplicitSyncCompleted;

		public static IEnumerator SynchronizeWithCloud_DataAlreadyPulled(bool doPull, bool setupSyncRelatedAppData, bool suppressRestoreWindow = false)
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				yield break;
			}
			IsSynchronizingWithCloud = true;
			InfoWindowController.BlockAllClick();
			IDisposable hardwareBackSubscription = BackSystem.Instance.Register(delegate
			{
			}, "SynchronizeWithCloud_DataAlreadyPulled");
			yield return CoroutineRunner.Instance.StartCoroutine(WaitForActivityIndicator());
			ActivityIndicator.IsActiveIndicator = true;
			Action onReturn = delegate
			{
				IsSynchronizingWithCloud = false;
				InfoWindowController.HideCurrentWindow();
				ActivityIndicator.IsActiveIndicator = false;
				if (hardwareBackSubscription != null)
				{
					hardwareBackSubscription.Dispose();
				}
			};
			if (doPull)
			{
				TaskCompletionSource<bool> amazonLoginPromise = new TaskCompletionSource<bool>();
				yield return CoroutineRunner.Instance.StartCoroutine(LoginToAmazonIfNeeded(amazonLoginPromise));
				if (!amazonLoginPromise.Task.Result)
				{
					onReturn();
					yield break;
				}
				TaskCompletionSource<bool> pullPromise = new TaskCompletionSource<bool>();
				yield return CoroutineRunner.Instance.StartCoroutine(PullCore(pullPromise, false));
				if (!((Task)pullPromise.Task).IsCompleted || ((Task)pullPromise.Task).IsFaulted || ((Task)pullPromise.Task).IsCanceled)
				{
					UnityEngine.Debug.LogErrorFormat("PullCore not completed the task");
					onReturn();
					yield break;
				}
				if (!pullPromise.Task.Result)
				{
					onReturn();
					yield break;
				}
			}
			if (!AreProgressInCurrentPullResult())
			{
				onReturn();
				yield break;
			}
			if (ConnectScene.isEnable)
			{
				onReturn();
				yield break;
			}
			if (LobbyCraftController.Instance != null && LobbyCraftController.Instance.InterfaceEnabled)
			{
				onReturn();
				yield break;
			}
			if (!suppressRestoreWindow)
			{
				yield return CoroutineRunner.Instance.StartCoroutine(ShowRestoreWindow());
			}
			yield return CoroutineRunner.Instance.StartCoroutine(Instance.ApplyChanges(Storager.getInt("HackDetected") > 0));
			PostSyncAppSetup(setupSyncRelatedAppData);
			Instance.OnExplicitSyncCompleted();
			onReturn();
		}

		public static IEnumerator SynchronizeWithCloud_NotAuthenticated()
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64 || (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon))
			{
				yield break;
			}
			IsSynchronizingWithCloud = true;
			Action onReturn = delegate
			{
				IsSynchronizingWithCloud = false;
			};
			TaskCompletionSource<bool> pullPromise = new TaskCompletionSource<bool>();
			yield return CoroutineRunner.Instance.StartCoroutine(PullCore(pullPromise, true));
			if (!((Task)pullPromise.Task).IsCompleted || ((Task)pullPromise.Task).IsFaulted || ((Task)pullPromise.Task).IsCanceled)
			{
				UnityEngine.Debug.LogErrorFormat("PullCore not completed the task");
				onReturn();
				yield break;
			}
			if (!pullPromise.Task.Result)
			{
				onReturn();
				yield break;
			}
			if (!AreProgressInCurrentPullResult())
			{
				onReturn();
				yield break;
			}
			TrainingController.OnGetProgress();
			while (MainMenuController.sharedController != null && MainMenuController.sharedController.IsSomeWindowOpenExceptSettings())
			{
				yield return null;
			}
			yield return CoroutineRunner.Instance.StartCoroutine(ShowRestoreWindow());
			IDisposable hardwareBackSubscription = BackSystem.Instance.Register(delegate
			{
			}, "SynchronizeWithCloud_NotAuthenticated");
			ActivityIndicator.IsActiveIndicator = true;
			InfoWindowController.BlockAllClick();
			yield return CoroutineRunner.Instance.StartCoroutine(Instance.ApplyChanges(Storager.getInt("HackDetected") > 0));
			PostSyncAppSetup(true);
			Instance.OnExplicitSyncCompleted();
			onReturn();
			InfoWindowController.HideCurrentWindow();
			ActivityIndicator.IsActiveIndicator = false;
			if (hardwareBackSubscription != null)
			{
				hardwareBackSubscription.Dispose();
			}
		}

		public static IEnumerator SynchronizeWithCloud_NotAuthenticated_SecondLaunch()
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64 || (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon))
			{
				yield break;
			}
			IsSynchronizingWithCloud = true;
			Action onReturn = delegate
			{
				IsSynchronizingWithCloud = false;
			};
			TaskCompletionSource<bool> pullPromise = new TaskCompletionSource<bool>();
			yield return CoroutineRunner.Instance.StartCoroutine(PullCore(pullPromise, false));
			if (!((Task)pullPromise.Task).IsCompleted || ((Task)pullPromise.Task).IsFaulted || ((Task)pullPromise.Task).IsCanceled)
			{
				UnityEngine.Debug.LogErrorFormat("PullCore not completed the task");
				onReturn();
				yield break;
			}
			if (!pullPromise.Task.Result)
			{
				onReturn();
				yield break;
			}
			if (!AreProgressInCurrentPullResult())
			{
				onReturn();
				yield break;
			}
			if (ConnectScene.isEnable)
			{
				onReturn();
				yield break;
			}
			if (LobbyCraftController.Instance != null && LobbyCraftController.Instance.InterfaceEnabled)
			{
				onReturn();
				yield break;
			}
			if (SceneManager.GetActiveScene().name != Defs.MainMenuScene)
			{
				onReturn();
				yield break;
			}
			UnityAction<Scene, Scene> stopSyncing = delegate
			{
				IsSynchronizingWithCloud = false;
			};
			SceneManager.activeSceneChanged += stopSyncing;
			while (IsSynchronizingWithCloud && MainMenuController.sharedController != null && MainMenuController.sharedController.IsSomeWindowOpenExceptSettings() && !ConnectScene.isEnable && (LobbyCraftController.Instance == null || !LobbyCraftController.Instance.InterfaceEnabled))
			{
				yield return null;
			}
			SceneManager.activeSceneChanged -= stopSyncing;
			if (!IsSynchronizingWithCloud)
			{
				onReturn();
				yield break;
			}
			if (ConnectScene.isEnable)
			{
				onReturn();
				yield break;
			}
			if (LobbyCraftController.Instance != null && LobbyCraftController.Instance.InterfaceEnabled)
			{
				onReturn();
				yield break;
			}
			yield return CoroutineRunner.Instance.StartCoroutine(ShowRestoreWindow());
			IDisposable hardwareBackSubscription = BackSystem.Instance.Register(delegate
			{
			}, "SynchronizeWithCloud_NotAuthenticated_SecondLaunch");
			ActivityIndicator.IsActiveIndicator = true;
			InfoWindowController.BlockAllClick();
			yield return CoroutineRunner.Instance.StartCoroutine(Instance.ApplyChanges(Storager.getInt("HackDetected") > 0));
			PostSyncAppSetup(true);
			Instance.OnExplicitSyncCompleted();
			onReturn();
			InfoWindowController.HideCurrentWindow();
			ActivityIndicator.IsActiveIndicator = false;
			if (hardwareBackSubscription != null)
			{
				hardwareBackSubscription.Dispose();
			}
		}

		public static bool AreProgressInCurrentPullResult()
		{
			return AreProgressInCurrentPullResultCore() == CurrentPulledPurchasesState.HasProgress;
		}

		public static CurrentPulledPurchasesState AreProgressInCurrentPullResultCore()
		{
			string text = Instance.CurrentPulledPurchases();
			if (text.IsNullOrEmpty())
			{
				return CurrentPulledPurchasesState.BadDataOrNoProgress;
			}
			object obj = null;
			try
			{
				obj = Json.Deserialize(text);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in AreProgressInCurrentPullResult Deserialize: {0}", ex);
			}
			if (obj == null)
			{
				return CurrentPulledPurchasesState.BadDataOrNoProgress;
			}
			List<object> list = obj as List<object>;
			if (list == null)
			{
				UnityEngine.Debug.LogErrorFormat("AreProgressInCurrentPullResult: not a json list: {0}", text);
				return CurrentPulledPurchasesState.BadDataOrNoProgress;
			}
			List<string> list2 = list.OfType<string>().ToList();
			if (list2 == null || list2.Count == 0)
			{
				return CurrentPulledPurchasesState.BadDataOrNoProgress;
			}
			if (list2.Contains("Environment.Terrains.SpecificalTerrainsMarkedFlag"))
			{
				return CurrentPulledPurchasesState.DelimitedRequired;
			}
			if (!list2.Contains("Armor_Army_1") && !list2.Contains("currentLevel2"))
			{
				return CurrentPulledPurchasesState.BadDataOrNoProgress;
			}
			return CurrentPulledPurchasesState.HasProgress;
		}

		private static IEnumerator PullCore(TaskCompletionSource<bool> pullPromise, bool stopOnEnterShop)
		{
			IEnumerable<Task<CloudPullResult>> futures = null;
			try
			{
				futures = Instance.Pull(false);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in PullCore CloudSyncController.Instance.Pull: {0}", ex);
			}
			if (futures == null)
			{
				UnityEngine.Debug.LogErrorFormat("PullCore: futures == null");
				pullPromise.SetResult(false);
				yield break;
			}
			if ((Defs.IsDeveloperBuild || Application.isEditor) && futures.Any((Task<CloudPullResult> f) => f == null))
			{
				UnityEngine.Debug.LogErrorFormat("PullCore: null futures!");
			}
			futures = futures.Where((Task<CloudPullResult> f) => f != null).ToList();
			if (futures.Count() == 0)
			{
				UnityEngine.Debug.LogErrorFormat("PullCore: all futres are nulls");
				pullPromise.SetResult(false);
				yield break;
			}
			float elapsedTime = 0f;
			float enterBackgroundTime = float.MaxValue;
			Action<bool> removeBackgroundTimeFromElapsed = delegate(bool pause)
			{
				if (pause)
				{
					enterBackgroundTime = Time.realtimeSinceStartup;
				}
				else
				{
					float num = Time.realtimeSinceStartup - enterBackgroundTime;
					elapsedTime -= num;
				}
			};
			LifetimeEventsRaiser.Instance.ApplicationPause += removeBackgroundTimeFromElapsed;
			while (futures.Any((Task<CloudPullResult> f) => !((Task)f).IsCompleted))
			{
				yield return null;
				elapsedTime += Time.unscaledDeltaTime;
				if (elapsedTime >= 50f || (stopOnEnterShop && ShopNGUIController.GuiActive))
				{
					break;
				}
			}
			LifetimeEventsRaiser.Instance.ApplicationPause -= removeBackgroundTimeFromElapsed;
			if (stopOnEnterShop && ShopNGUIController.GuiActive)
			{
				UnityEngine.Debug.LogErrorFormat("PullCore: ShopNGUIController.GuiActive");
				pullPromise.SetResult(false);
				yield break;
			}
			if (futures.All((Task<CloudPullResult> f) => !((Task)f).IsCompleted))
			{
				UnityEngine.Debug.LogErrorFormat("PullCore: all pulls not completed within timeout");
				pullPromise.SetResult(false);
				yield break;
			}
			List<Task<CloudPullResult>> list = futures.Where((Task<CloudPullResult> f) => ((Task)f).IsCompleted && !((Task)f).IsFaulted && !((Task)f).IsCanceled).ToList();
			if (list == null || list.Count == 0)
			{
				UnityEngine.Debug.LogErrorFormat("PullCore: all futures timed out or failed or canceled");
				pullPromise.SetResult(false);
			}
			else if (list.Any((Task<CloudPullResult> f) => f.Result == CloudPullResult.LoginFailed))
			{
				UnityEngine.Debug.LogWarningFormat("PullCore: login failed");
				pullPromise.SetResult(false);
			}
			else if (list.All((Task<CloudPullResult> f) => f.Result != CloudPullResult.Successful))
			{
				string format = "PullCore: all pulls failed (futures completed)";
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.LogErrorFormat(format);
				}
				else
				{
					UnityEngine.Debug.LogWarningFormat(format);
				}
				pullPromise.SetResult(false);
			}
			else
			{
				pullPromise.SetResult(true);
			}
		}

		private static IEnumerator WaitForActivityIndicator()
		{
			int startWaitAcitvivtyIndicatorFrame = Time.frameCount;
			yield return new WaitUntil(() => !ActivityIndicator.IsActiveIndicator || Time.frameCount - startWaitAcitvivtyIndicatorFrame >= 60);
		}

		private static IEnumerator ShowRestoreWindow()
		{
			int startWaitSharedMainMenuFrame = Time.frameCount;
			yield return new WaitUntil(() => MainMenuController.sharedController != null || Time.frameCount - startWaitSharedMainMenuFrame >= 60);
			while (MainMenuController.sharedController != null && MainMenuController.sharedController.stubLoading.activeInHierarchy)
			{
				yield return null;
			}
			ActivityIndicator.IsActiveIndicator = false;
			InfoWindowController.HideCurrentWindow();
			bool restoreWindowShown = true;
			InfoWindowController.ShowRestorePanel(delegate
			{
				restoreWindowShown = false;
			});
			while (restoreWindowShown)
			{
				yield return null;
			}
			InfoWindowController.BlockAllClick();
			ActivityIndicator.IsActiveIndicator = true;
		}

		private static void PostSyncAppSetup(bool setupSyncRelatedAppData)
		{
			UpdateNickname();
			if (setupSyncRelatedAppData)
			{
				SetupDifferentSubsystems();
			}
			SendNativeAchievemnts();
			if (ExpController.Instance != null)
			{
				ExpController.Instance.InterfaceEnabled = !ExpController.Instance.InterfaceEnabled;
				ExpController.Instance.InterfaceEnabled = !ExpController.Instance.InterfaceEnabled;
			}
		}

		private static IEnumerator LoginToAmazonIfNeeded(TaskCompletionSource<bool> promise)
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon && !Instance.IsAuthenticatedToCloud())
			{
				if (!GameCircleSocial.Instance.localUser.authenticated)
				{
					UnityEngine.Debug.LogFormat("[Rilisoft] SynchronizeWithCloud_DataAlreadyPulled Sign in to GameCircle");
					AGSClient.ShowSignInPage();
				}
				float endTime = Time.realtimeSinceStartup + 60f;
				while (!GameCircleSocial.Instance.localUser.authenticated && Time.realtimeSinceStartup < endTime)
				{
					yield return null;
				}
				promise.SetResult(GameCircleSocial.Instance.localUser.authenticated);
				if (GameCircleSocial.Instance.localUser.authenticated)
				{
					bool initialDataComes = false;
					Action initialDataDownloaded = delegate
					{
						initialDataComes = true;
					};
					AGSWhispersyncClient.OnNewCloudDataEvent += initialDataDownloaded;
					float startWaitingForInitialDownloadTime = Time.realtimeSinceStartup;
					while (!AGSClient.IsServiceReady())
					{
						yield return null;
					}
					while (!initialDataComes && Time.realtimeSinceStartup - startWaitingForInitialDownloadTime < 60f)
					{
						yield return null;
					}
					AGSWhispersyncClient.OnNewCloudDataEvent -= initialDataDownloaded;
				}
			}
			else
			{
				promise.SetResult(true);
			}
		}

		private static void SetupDifferentSubsystems()
		{
			try
			{
				if (QuestSystem.Instance != null && QuestSystem.Instance.QuestProgress != null)
				{
					QuestSystem.Instance.QuestProgress.FilterFulfilledTutorialQuests();
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in SynchronizeWithCloud FilterFulfilledTutorialQuests: {0}", ex);
			}
			WeaponManager.FillWeaponSlotAfterSync();
			try
			{
				if (MainMenuController.sharedController != null)
				{
					MainMenuController.sharedController.RefreshSettingsButton();
				}
			}
			catch (Exception ex2)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in SynchronizeWithCloud RefreshSettingsButton: {0}", ex2);
			}
			try
			{
				if ((bool)GiftController.Instance)
				{
					GiftController.Instance.ReCreateSlots();
				}
			}
			catch (Exception ex3)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in SynchronizeWithCloud ReCreateSlots: {0}", ex3);
			}
		}

		private static void UpdateNickname()
		{
			try
			{
				if (PlayerPanel.instance != null)
				{
					PlayerPanel.instance.UpdateNickPlayer();
					PlayerPanel.instance.UpdateExp();
					PlayerPanel.instance.UpdateRating();
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in SynchronizeWithCloud update nickname: {0}", ex);
			}
		}

		private static void SendNativeAchievemnts()
		{
			try
			{
				GameServicesController gameServicesController = UnityEngine.Object.FindObjectOfType<GameServicesController>();
				if (gameServicesController == null)
				{
					gameServicesController = new GameObject("Rilisoft.GameServicesController").AddComponent<GameServicesController>();
				}
				gameServicesController.WaitAuthenticationAndIncrementBeginnerAchievement();
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in SynchronizeWithCloud WaitAuthenticationAndIncrementBeginnerAchievement: {0}", ex);
			}
		}

		public void OnExplicitSyncCompleted()
		{
			Action explicitSyncCompleted = CloudSyncController.ExplicitSyncCompleted;
			if (explicitSyncCompleted != null)
			{
				explicitSyncCompleted();
			}
		}

		public bool IsAuthenticatedToCloud()
		{
			try
			{
				if (Application.platform == RuntimePlatform.IPhonePlayer)
				{
					return true;
				}
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
				{
					if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
					{
						return GpgFacade.Instance != null && GpgFacade.Instance.IsAuthenticated();
					}
					if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
					{
						return GameCircleSocial.Instance.localUser != null && GameCircleSocial.Instance.localUser.authenticated;
					}
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in IsAuthenticatedToCloud: {0}", ex);
			}
			return false;
		}

		public string CurrentPulledPurchases()
		{
			try
			{
				CloudSlotSyncingPair value;
				if (m_syncingPairs != null && m_syncingPairs.TryGetValue("CloudSynchronizationConstants.PURCHASES_SYNCHRONIZATION_SLOT", out value) && value != null && value.SlotSynchronizer != null)
				{
					if (Application.platform == RuntimePlatform.IPhonePlayer || (!Application.isEditor && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon))
					{
						Pull(true);
					}
					return value.SlotSynchronizer.CurrentResult;
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in CurrentPulledPurchases: {0}", ex);
			}
			return string.Empty;
		}

		public IEnumerable<Task<CloudPullResult>> Pull(bool silent)
		{
			List<Task<CloudPullResult>> list = new List<Task<CloudPullResult>>();
			foreach (CloudSlotSyncingPair value in m_syncingPairs.Values)
			{
				Task<CloudPullResult> item = value.SlotSynchronizer.Pull(silent);
				list.Add(item);
			}
			return list;
		}

		public IEnumerator ApplyChanges(bool skipApplyingToLocalState)
		{
			if (AreProgressInCurrentPullResultCore() == CurrentPulledPurchasesState.DelimitedRequired)
			{
				yield break;
			}
			foreach (CloudSlotSyncingPair syncPair in m_syncingPairs.Values)
			{
				if (syncPair.SlotSynchronizer.Pulled)
				{
					yield return CoroutineRunner.Instance.StartCoroutine(syncPair.SlotApplyer.Apply(skipApplyingToLocalState));
					if (!skipApplyingToLocalState)
					{
						yield return null;
					}
				}
				else
				{
					UnityEngine.Debug.LogErrorFormat("CloudSyncController: Trying to merge and push synchronizer that never pulled: {0}", syncPair.ToString());
				}
			}
			PostApply();
			if (!skipApplyingToLocalState && WeaponManager.sharedManager != null)
			{
				yield return CoroutineRunner.Instance.StartCoroutine(WeaponManager.sharedManager.ResetCoroutine(WeaponManager.sharedManager.CurrentFilterMap));
			}
			else
			{
				UnityEngine.Debug.LogErrorFormat("ApplyChanges WeaponManager.sharedManager == null");
			}
			m_lastTimeAppliedInApplyPoint = Time.realtimeSinceStartup;
			Action applyCompleted = CloudSyncController.ApplyCompleted;
			if (applyCompleted != null)
			{
				applyCompleted();
			}
		}

		public IEnumerator PullAndApplyChangesIfNeeded()
		{
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64 && !(Time.realtimeSinceStartup - IntervalBetweenApplyInApplyPoints < m_lastTimeAppliedInApplyPoint))
			{
				try
				{
					Instance.Pull(true);
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogErrorFormat("CloudSyncController: Exception in PullAndApplyChangesIfNeeded: {0}", ex);
				}
				yield return CoroutineRunner.Instance.StartCoroutine(Instance.ApplyChanges(Storager.getInt("HackDetected") > 0));
			}
		}

		protected CloudSyncController()
		{
			m_syncingPairs = new Dictionary<string, CloudSlotSyncingPair>();
			CloudSlotSynchronizer cloudSlotSynchronizer = CloudSlotSynchronizerFactory.Create("CloudSynchronizationConstants.PURCHASES_SYNCHRONIZATION_SLOT");
			PurchasesCloudApplyer slotApplyer = new PurchasesCloudApplyer(cloudSlotSynchronizer);
			m_syncingPairs.Add("CloudSynchronizationConstants.PURCHASES_SYNCHRONIZATION_SLOT", new CloudSlotSyncingPair(cloudSlotSynchronizer, slotApplyer));
			CloudSlotSynchronizer cloudSlotSynchronizer2 = CloudSlotSynchronizerFactory.Create("CloudSynchronizationConstants.CAMPAIGN_PROGRESS_SYNCHRONIZATION_SLOT");
			CampaignProgressCloudApplyer slotApplyer2 = new CampaignProgressCloudApplyer(cloudSlotSynchronizer2);
			m_syncingPairs.Add("CloudSynchronizationConstants.CAMPAIGN_PROGRESS_SYNCHRONIZATION_SLOT", new CloudSlotSyncingPair(cloudSlotSynchronizer2, slotApplyer2));
			CloudSlotSynchronizer cloudSlotSynchronizer3 = CloudSlotSynchronizerFactory.Create("CloudSynchronizationConstants.CAMPAIGN_SECRETS_SYNCHRONIZATION_SLOT");
			CampaignSecretsCloudApplyer slotApplyer3 = new CampaignSecretsCloudApplyer(cloudSlotSynchronizer3);
			m_syncingPairs.Add("CloudSynchronizationConstants.CAMPAIGN_SECRETS_SYNCHRONIZATION_SLOT", new CloudSlotSyncingPair(cloudSlotSynchronizer3, slotApplyer3));
			CloudSlotSynchronizer cloudSlotSynchronizer4 = CloudSlotSynchronizerFactory.Create("CloudSynchronizationConstants.TROPHIES_SYNCHRONIZATION_SLOT");
			TrophiesCloudApplyer slotApplyer4 = new TrophiesCloudApplyer(cloudSlotSynchronizer4);
			m_syncingPairs.Add("CloudSynchronizationConstants.TROPHIES_SYNCHRONIZATION_SLOT", new CloudSlotSyncingPair(cloudSlotSynchronizer4, slotApplyer4));
			CloudSlotSynchronizer cloudSlotSynchronizer5 = CloudSlotSynchronizerFactory.Create("CloudSynchronizationConstants.ACHIEVEMENTS_SYNCHRONIZATION_SLOT");
			AchievementsCloudApplyer slotApplyer5 = new AchievementsCloudApplyer(cloudSlotSynchronizer5);
			m_syncingPairs.Add("CloudSynchronizationConstants.ACHIEVEMENTS_SYNCHRONIZATION_SLOT", new CloudSlotSyncingPair(cloudSlotSynchronizer5, slotApplyer5));
			CloudSlotSynchronizer cloudSlotSynchronizer6 = CloudSlotSynchronizerFactory.Create("CloudSynchronizationConstants.SKINS_SYNCHRONIZATION_SLOT");
			SkinsCloudApplyer slotApplyer6 = new SkinsCloudApplyer(cloudSlotSynchronizer6);
			m_syncingPairs.Add("CloudSynchronizationConstants.SKINS_SYNCHRONIZATION_SLOT", new CloudSlotSyncingPair(cloudSlotSynchronizer6, slotApplyer6));
			CloudSlotSynchronizer cloudSlotSynchronizer7 = CloudSlotSynchronizerFactory.Create("CloudSynchronizationConstants.PETS_SYNCHRONIZATION_SLOT");
			PetsCloudApplyer slotApplyer7 = new PetsCloudApplyer(cloudSlotSynchronizer7);
			m_syncingPairs.Add("CloudSynchronizationConstants.PETS_SYNCHRONIZATION_SLOT", new CloudSlotSyncingPair(cloudSlotSynchronizer7, slotApplyer7));
			CloudSlotSynchronizer cloudSlotSynchronizer8 = CloudSlotSynchronizerFactory.Create("CloudSynchronizationConstants.TECHNICAL_SYNCHRONIZATION_SLOT");
			TechnicalCloudApplyer slotApplyer8 = new TechnicalCloudApplyer(cloudSlotSynchronizer8);
			m_syncingPairs.Add("CloudSynchronizationConstants.TECHNICAL_SYNCHRONIZATION_SLOT", new CloudSlotSyncingPair(cloudSlotSynchronizer8, slotApplyer8));
			CloudSlotSynchronizer cloudSlotSynchronizer9 = CloudSlotSynchronizerFactory.Create("CloudSynchronizationConstants.LOBBY_ITEMS_SYNCHRONIZATION_SLOT");
			LobbyItemsCloudApplyer slotApplyer9 = new LobbyItemsCloudApplyer(cloudSlotSynchronizer9);
			m_syncingPairs.Add("CloudSynchronizationConstants.LOBBY_ITEMS_SYNCHRONIZATION_SLOT", new CloudSlotSyncingPair(cloudSlotSynchronizer9, slotApplyer9));
		}

		private static void PostApply()
		{
			RuntimePlatform platform = Application.platform;
			int num = 8;
		}
	}
}
