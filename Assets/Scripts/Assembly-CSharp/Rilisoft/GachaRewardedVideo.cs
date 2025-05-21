using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FyberPlugin;
using UnityEngine;

namespace Rilisoft
{
	[DisallowMultipleComponent]
	public sealed class GachaRewardedVideo : MonoBehaviour
	{
		internal enum Input
		{
			None = 0,
			Start = 1,
			Update = 2,
			Proceed = 3,
			Close = 4
		}

		internal sealed class Initial : StateBase<Input>
		{
			private readonly GachaRewardedVideo _gachaRewardedVideo;

			public Initial(GachaRewardedVideo gachaRewardedVideo)
			{
				if (gachaRewardedVideo == null)
				{
					throw new ArgumentNullException("gachaRewardedVideo");
				}
				_gachaRewardedVideo = gachaRewardedVideo;
			}

			public override ReactionBase<Input> React(Input input)
			{
				return new TransitReaction<Idle, Input>(new Idle(_gachaRewardedVideo));
			}
		}

		internal sealed class Idle : StateBase<Input>
		{
			private readonly GachaRewardedVideo _gachaRewardedVideo;

			public Idle(GachaRewardedVideo gachaRewardedVideo)
			{
				if (gachaRewardedVideo == null)
				{
					throw new ArgumentNullException("gachaRewardedVideo");
				}
				_gachaRewardedVideo = gachaRewardedVideo;
			}

			public override void Enter(StateBase<Input> oldState, Input input)
			{
				DebugLog("Idle.Enter");
				_gachaRewardedVideo.RaiseEnterIdle(GetEnterEventArgs(oldState));
			}

			public override ReactionBase<Input> React(Input input)
			{
				if (input == Input.Proceed)
				{
					return new TransitReaction<Waiting, Input>(new Waiting(_gachaRewardedVideo));
				}
				return DiscardReaction<Input>.Default;
			}

			public override void Exit(StateBase<Input> newState, Input input)
			{
				DebugLog("Idle.Exit");
				_gachaRewardedVideo.RaiseExitIdle(EventArgs.Empty);
			}

			private FinishedEventArgs GetEnterEventArgs(StateBase<Input> oldState)
			{
				Watching watching = oldState as Watching;
				if (watching == null)
				{
					return FinishedEventArgs.Failure;
				}
				if (!((Task)watching.AdClosedFuture).IsCompleted)
				{
					return FinishedEventArgs.Failure;
				}
				if (((Task)watching.AdClosedFuture).IsFaulted)
				{
					return FinishedEventArgs.Failure;
				}
				return FinishedEventArgs.Success;
			}
		}

		internal sealed class Waiting : StateBase<Input>
		{
			private const float TimeoutInSeconds = 5f;

			private readonly GachaRewardedVideo _gachaRewardedVideo;

			private readonly TaskCompletionSource<Ad> _adPromise = new TaskCompletionSource<Ad>();

			private float _startTime = Time.realtimeSinceStartup;

			private Task<Ad> AdFuture
			{
				get
				{
					return _adPromise.Task;
				}
			}

			public Waiting(GachaRewardedVideo gachaRewardedVideo)
			{
				if (gachaRewardedVideo == null)
				{
					throw new ArgumentNullException("gachaRewardedVideo");
				}
				_gachaRewardedVideo = gachaRewardedVideo;
			}

			public override void Enter(StateBase<Input> oldState, Input input)
			{
				DebugLog("Waiting.Enter");
				_gachaRewardedVideo.waitingPanel.SetActive(true);
				if (_gachaRewardedVideo.simulateButton != null)
				{
					_gachaRewardedVideo.simulateButton.SetActive(Application.isEditor);
				}
				_startTime = Time.realtimeSinceStartup;
				FyberCallback.AdAvailable += OnAdAvailable;
				FyberCallback.AdNotAvailable += OnAdNotAvailable;
				FyberCallback.RequestFail += OnRequestFail;
			}

			public override ReactionBase<Input> React(Input input)
			{
				if (_gachaRewardedVideo.loadingSpinner != null)
				{
					float num = Time.realtimeSinceStartup - _startTime;
					int num2 = Mathf.FloorToInt(num);
					bool flag = num2 % 2 == 0;
					_gachaRewardedVideo.loadingSpinner.invert = flag;
					_gachaRewardedVideo.loadingSpinner.fillAmount = (flag ? (num - (float)num2) : (1f - num + (float)num2));
				}
				if (input == Input.Proceed && Application.isEditor)
				{
					_adPromise.TrySetResult((Ad)null);
					return DiscardReaction<Input>.Default;
				}
				if (input != Input.Update)
				{
					return DiscardReaction<Input>.Default;
				}
				if (((Task)AdFuture).IsCompleted)
				{
					if (((Task)AdFuture).IsFaulted)
					{
						Exception ex = ((Task)AdFuture).Exception.InnerExceptions.FirstOrDefault();
						string reason = ((ex != null) ? ex.Message : ((Exception)(object)((Task)AdFuture).Exception).Message);
						return new TransitReaction<Failure, Input>(new Failure(_gachaRewardedVideo, reason));
					}
					DateTime? serverTime = FriendsController.GetServerTime();
					if (!serverTime.HasValue)
					{
						return new TransitReaction<Failure, Input>(new Failure(_gachaRewardedVideo, "Server time not received."));
					}
					if (AdFuture.Result == null)
					{
						if (Application.isEditor)
						{
							return new TransitReaction<Watching, Input>(new Watching(_gachaRewardedVideo, AdFuture.Result, serverTime.Value));
						}
						return new TransitReaction<Failure, Input>(new Failure(_gachaRewardedVideo, "Ad is not available."));
					}
					return new TransitReaction<Watching, Input>(new Watching(_gachaRewardedVideo, AdFuture.Result, serverTime.Value));
				}
				if (5f <= Time.realtimeSinceStartup - _startTime)
				{
					string reason2 = string.Format(CultureInfo.InvariantCulture, "Timeout {0:f1} seconds.", 5f);
					return new TransitReaction<Failure, Input>(new Failure(_gachaRewardedVideo, reason2));
				}
				return DiscardReaction<Input>.Default;
			}

			public override void Exit(StateBase<Input> newState, Input input)
			{
				DebugLog("Waiting.Exit");
				FyberCallback.AdAvailable -= OnAdAvailable;
				FyberCallback.AdNotAvailable -= OnAdNotAvailable;
				FyberCallback.RequestFail -= OnRequestFail;
				if (_gachaRewardedVideo.simulateButton != null)
				{
					_gachaRewardedVideo.simulateButton.SetActive(false);
				}
				_gachaRewardedVideo.waitingPanel.SetActive(false);
			}

			private void OnAdAvailable(Ad ad)
			{
				_adPromise.TrySetResult(ad);
			}

			private void OnAdNotAvailable(AdFormat adFormat)
			{
				if (adFormat != AdFormat.REWARDED_VIDEO)
				{
					DebugLog("Unexpected ad format: " + adFormat);
				}
				else
				{
					_adPromise.TrySetResult((Ad)null);
				}
			}

			private void OnRequestFail(RequestError requestError)
			{
				_adPromise.TrySetException((Exception)new InvalidOperationException(requestError.Description));
			}
		}

		internal sealed class Failure : StateBase<Input>
		{
			private readonly GachaRewardedVideo _gachaRewardedVideo;

			private readonly string _reason;

			public Failure(GachaRewardedVideo gachaRewardedVideo, string reason)
			{
				if (gachaRewardedVideo == null)
				{
					throw new ArgumentNullException("gachaRewardedVideo");
				}
				_gachaRewardedVideo = gachaRewardedVideo;
				_reason = reason ?? string.Empty;
			}

			public override void Enter(StateBase<Input> oldState, Input input)
			{
				DebugLog("Failure.Enter: " + _reason);
				_gachaRewardedVideo.failurePanel.SetActive(true);
			}

			public override ReactionBase<Input> React(Input input)
			{
				if (input == Input.Close)
				{
					return new TransitReaction<Idle, Input>(new Idle(_gachaRewardedVideo));
				}
				return DiscardReaction<Input>.Default;
			}

			public override void Exit(StateBase<Input> newState, Input input)
			{
				DebugLog("Failure.Exit: " + _reason);
				_gachaRewardedVideo.failurePanel.SetActive(false);
			}
		}

		internal sealed class Watching : StateBase<Input>
		{
			[CompilerGenerated]
			internal sealed class _003C_003Ec__DisplayClass6_0
			{
				public Task<string> f;

				internal bool _003CWaitFutureThenContinue_003Eb__0()
				{
					return ((Task)f).IsCompleted;
				}
			}

			[CompilerGenerated]
			internal sealed class _003CWaitFutureThenContinue_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
			{
				private int _003C_003E1__state;

				private object _003C_003E2__current;

				public Watching _003C_003E4__this;

				private _003C_003Ec__DisplayClass6_0 _003C_003E8__1;

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
				public _003CWaitFutureThenContinue_003Ed__6(int _003C_003E1__state)
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
						_003C_003E8__1 = new _003C_003Ec__DisplayClass6_0();
						_003C_003E8__1.f = _003C_003E4__this.AdClosedFuture;
						_003C_003E2__current = new WaitUntil(() => ((Task)_003C_003E8__1.f).IsCompleted);
						_003C_003E1__state = 1;
						return true;
					case 1:
						_003C_003E1__state = -1;
						if (((Task)_003C_003E8__1.f).IsFaulted || ((Task)_003C_003E8__1.f).IsCanceled)
						{
							return false;
						}
						if (_003C_003E8__1.f.Result == "CLOSE_FINISHED")
						{
							_003C_003E4__this._gachaRewardedVideo.RaiseAdWatchedSuccessfully(EventArgs.Empty);
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
			internal sealed class _003CSimulateWatchingCoroutine_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
			{
				private int _003C_003E1__state;

				private object _003C_003E2__current;

				public Watching _003C_003E4__this;

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
				public _003CSimulateWatchingCoroutine_003Ed__7(int _003C_003E1__state)
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
						_003C_003E2__current = new WaitForSeconds(5f);
						_003C_003E1__state = 1;
						return true;
					case 1:
						_003C_003E1__state = -1;
						if (((Task)_003C_003E4__this.AdClosedFuture).IsCompleted)
						{
							return false;
						}
						_003C_003E4__this._adClosedPromise.TrySetResult("CLOSE_FINISHED");
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

			private readonly DateTime _startTime;

			private readonly Ad _ad;

			private readonly TaskCompletionSource<string> _adClosedPromise = new TaskCompletionSource<string>();

			private readonly GachaRewardedVideo _gachaRewardedVideo;

			internal Task<string> AdClosedFuture
			{
				get
				{
					return _adClosedPromise.Task;
				}
			}

			public Watching(GachaRewardedVideo gachaRewardedVideo, Ad ad, DateTime startTime)
			{
				if (gachaRewardedVideo == null)
				{
					throw new ArgumentNullException("gachaRewardedVideo");
				}
				if (!Application.isEditor && ad == null)
				{
					throw new ArgumentNullException("ad");
				}
				_startTime = startTime;
				_ad = ad;
				_gachaRewardedVideo = gachaRewardedVideo;
			}

			public override void Enter(StateBase<Input> oldState, Input input)
			{
				DebugLog("Watching.Enter");
				if (MenuBackgroundMusic.sharedMusic != null)
				{
					MenuBackgroundMusic.sharedMusic.Stop();
				}
				GachaRewardedVideoController.SavePendingFreeSpins(GachaRewardedVideoController.Instance.Reward);
				GachaRewardedVideoController.SaveShownTimestamp(_startTime);
				_gachaRewardedVideo.watchingPanel.SetActive(true);
				FyberCallback.AdFinished += OnAdFinished;
				CoroutineRunner.Instance.StartCoroutine(WaitFutureThenContinue());
				if (Application.isEditor)
				{
					CoroutineRunner.Instance.StartCoroutine(SimulateWatchingCoroutine());
				}
				else
				{
					_ad.Start();
				}
			}

			public override ReactionBase<Input> React(Input input)
			{
				switch (input)
				{
				case Input.Close:
				{
					string message = "Watching panel was closed manually.";
					_adClosedPromise.TrySetException((Exception)new InvalidOperationException(message));
					return new TransitReaction<Idle, Input>(new Idle(_gachaRewardedVideo));
				}
				default:
					return DiscardReaction<Input>.Default;
				case Input.Update:
					if (!((Task)AdClosedFuture).IsCompleted)
					{
						return DiscardReaction<Input>.Default;
					}
					return new TransitReaction<Idle, Input>(new Idle(_gachaRewardedVideo));
				}
			}

			public override void Exit(StateBase<Input> newState, Input input)
			{
				DebugLog("Watching.Exit");
				GachaRewardedVideoController.ResetPendingFreeSpins();
				if (MenuBackgroundMusic.sharedMusic != null)
				{
					MenuBackgroundMusic.sharedMusic.Play();
				}
				FyberCallback.AdFinished -= OnAdFinished;
				if (((Task)AdClosedFuture).IsFaulted)
				{
					Exception ex = ((Task)AdClosedFuture).Exception.InnerExceptions.FirstOrDefault();
					UnityEngine.Debug.LogWarning((ex != null) ? ex.Message : ((Exception)(object)((Task)AdClosedFuture).Exception).Message);
				}
				_gachaRewardedVideo.watchingPanel.SetActive(false);
			}

			private void OnAdFinished(AdResult adResult)
			{
				FyberCallback.AdFinished -= OnAdFinished;
				if (adResult.AdFormat != AdFormat.REWARDED_VIDEO)
				{
					DebugLog("Unexpected ad format: " + adResult.AdFormat);
				}
				else if (adResult.Status != 0)
				{
					string message = "Bad status: " + adResult.Status;
					_adClosedPromise.TrySetException((Exception)new InvalidOperationException(message));
				}
				else
				{
					_adClosedPromise.TrySetResult(adResult.Message);
				}
			}

			private IEnumerator WaitFutureThenContinue()
			{
				Task<string> f = AdClosedFuture;
				yield return new WaitUntil(() => ((Task)f).IsCompleted);
				if (!((Task)f).IsFaulted && !((Task)f).IsCanceled && f.Result == "CLOSE_FINISHED")
				{
					_gachaRewardedVideo.RaiseAdWatchedSuccessfully(EventArgs.Empty);
				}
			}

			private IEnumerator SimulateWatchingCoroutine()
			{
				yield return new WaitForSeconds(5f);
				if (!((Task)AdClosedFuture).IsCompleted)
				{
					_adClosedPromise.TrySetResult("CLOSE_FINISHED");
				}
			}
		}

		[Header("Internal")]
		[SerializeField]
		protected internal GameObject waitingPanel;

		[SerializeField]
		protected internal GameObject watchingPanel;

		[SerializeField]
		protected internal GameObject failurePanel;

		[SerializeField]
		protected internal GameObject simulateButton;

		[SerializeField]
		protected internal UITexture loadingSpinner;

		public GameObject windowBlocker;

		private IDisposable _backSubscription;

		private StateBase<Input> _currentState;

		private event EventHandler<FinishedEventArgs> EnterIdle;

		public event EventHandler ExitIdle;

		public event EventHandler AdWatchedSuccessfully;

		public void OnCloseFailurePanel()
		{
			Process(Input.Close);
		}

		public void OnCloseWatchingPanel()
		{
			Process(Input.Close);
		}

		public void OnSimulateButtonClicked()
		{
			Process(Input.Proceed);
		}

		public void OnWatchButtonClicked()
		{
			Process(Input.Proceed);
		}

		public GachaRewardedVideo()
		{
			_currentState = new Initial(this);
		}

		private void Awake()
		{
			Process(Input.Start);
		}

		private void OnEnable()
		{
			if (_backSubscription != null)
			{
				_backSubscription.Dispose();
			}
			_backSubscription = BackSystem.Instance.Register(OnBackRequested, GetType().Name);
		}

		private void OnDisable()
		{
			if (_backSubscription != null)
			{
				_backSubscription.Dispose();
				_backSubscription = null;
			}
		}

		private void Update()
		{
			if (windowBlocker != null)
			{
				windowBlocker.SetActive(waitingPanel.activeInHierarchy || watchingPanel.activeInHierarchy || failurePanel.activeInHierarchy);
			}
			Process(Input.Update);
		}

		private void OnBackRequested()
		{
			if (_currentState.React(Input.Close).GetNewState() != null)
			{
				OnCloseFailurePanel();
			}
			else
			{
				_backSubscription = BackSystem.Instance.Register(OnCloseFailurePanel, GetType().Name);
			}
		}

		private void RaiseEnterIdle(FinishedEventArgs e)
		{
			EventHandler<FinishedEventArgs> enterIdle = this.EnterIdle;
			if (enterIdle != null)
			{
				enterIdle(this, e);
			}
		}

		private void RaiseExitIdle(EventArgs e)
		{
			EventHandler exitIdle = this.ExitIdle;
			if (exitIdle != null)
			{
				exitIdle(this, e);
			}
		}

		private void RaiseAdWatchedSuccessfully(EventArgs e)
		{
			EventHandler adWatchedSuccessfully = this.AdWatchedSuccessfully;
			if (adWatchedSuccessfully != null)
			{
				adWatchedSuccessfully(this, e);
			}
		}

		private void Process(Input input)
		{
			StateBase<Input> newState = _currentState.React(input).GetNewState();
			if (newState != null)
			{
				StateBase<Input> currentState = _currentState;
				currentState.Exit(newState, input);
				newState.Enter(currentState, input);
				_currentState = newState;
				IDisposable disposable = currentState as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}

		private static void DebugLog(string message)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=cyan>[{0}] {1}</color>" : "[{0}] {1}", typeof(GachaRewardedVideo).Name, message);
			}
		}
	}
}
