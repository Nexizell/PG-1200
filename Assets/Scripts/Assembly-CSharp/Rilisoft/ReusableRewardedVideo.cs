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
	public sealed class ReusableRewardedVideo : MonoBehaviour
	{
		internal enum Input
		{
			None = 0,
			Start = 1,
			Watch = 2,
			Update = 3,
			Proceed = 4,
			Close = 5
		}

		internal abstract class ContextStateBase : StateBase<Input>
		{
			private readonly ReusableRewardedVideo _context;

			protected ReusableRewardedVideo Context
			{
				get
				{
					return _context;
				}
			}

			protected ContextStateBase(ReusableRewardedVideo rewardedVideo)
			{
				if (rewardedVideo == null)
				{
					throw new ArgumentNullException("rewardedVideo");
				}
				_context = rewardedVideo;
			}
		}

		internal sealed class Initial : ContextStateBase
		{
			internal Initial(ReusableRewardedVideo rewardedVideo)
				: base(rewardedVideo)
			{
			}

			public override ReactionBase<Input> React(Input input)
			{
				return new TransitReaction<Idle, Input>(new Idle(base.Context));
			}
		}

		internal sealed class Idle : ContextStateBase
		{
			public Idle(ReusableRewardedVideo rewardedVideo)
				: base(rewardedVideo)
			{
			}

			public override void Enter(StateBase<Input> oldState, Input input)
			{
				DebugLog("Idle.Enter");
				if (base.Context._backSubscription != null)
				{
					base.Context._backSubscription.Dispose();
					base.Context._backSubscription = null;
				}
				base.Context.RaiseEnterIdle(GetEnterEventArgs(oldState));
			}

			public override ReactionBase<Input> React(Input input)
			{
				if (input == Input.Proceed)
				{
					return new TransitReaction<ReadyToWatch, Input>(new ReadyToWatch(base.Context));
				}
				return DiscardReaction<Input>.Default;
			}

			public override void Exit(StateBase<Input> newState, Input input)
			{
				DebugLog("Idle.Exit");
				if (base.Context._backSubscription != null)
				{
					base.Context._backSubscription.Dispose();
				}
				base.Context._backSubscription = BackSystem.Instance.Register(base.Context.OnBackRequested, "ReusableRewardedVideo.Idle.Exit()");
				base.Context.RaiseExitIdle(EventArgs.Empty);
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

		internal sealed class ReadyToWatch : ContextStateBase
		{
			internal ReadyToWatch(ReusableRewardedVideo rewardedVideo)
				: base(rewardedVideo)
			{
			}

			public override void Enter(StateBase<Input> oldState, Input input)
			{
				UnityEngine.Debug.Log("ReadyToWatch.Enter");
				base.Context._readyToWatchPanel.SetActive(true);
				base.Context._windowBlocker.SetActive(true);
			}

			public override void Exit(StateBase<Input> newState, Input input)
			{
				UnityEngine.Debug.Log("ReadyToWatch.Exit");
				base.Context._windowBlocker.SetActive(false);
				base.Context._readyToWatchPanel.SetActive(false);
			}

			public override ReactionBase<Input> React(Input input)
			{
				switch (input)
				{
				case Input.Watch:
					return new TransitReaction<Waiting, Input>(new Waiting(base.Context));
				case Input.Close:
					return new TransitReaction<Idle, Input>(new Idle(base.Context));
				default:
					return DiscardReaction<Input>.Default;
				}
			}
		}

		internal sealed class Waiting : ContextStateBase
		{
			private const float TimeoutInSeconds = 5f;

			private readonly TaskCompletionSource<Ad> _adPromise = new TaskCompletionSource<Ad>();

			private float _startTime = Time.realtimeSinceStartup;

			private Task<Ad> AdFuture
			{
				get
				{
					return _adPromise.Task;
				}
			}

			public Waiting(ReusableRewardedVideo rewardedVideo)
				: base(rewardedVideo)
			{
			}

			public override void Enter(StateBase<Input> oldState, Input input)
			{
				DebugLog("Waiting.Enter");
				base.Context._waitingPanel.SetActive(true);
				base.Context._windowBlocker.SetActive(true);
				if (base.Context._simulateButton != null)
				{
					base.Context._simulateButton.SetActive(Application.isEditor);
				}
				_startTime = Time.realtimeSinceStartup;
				FyberCallback.AdAvailable += OnAdAvailable;
				FyberCallback.AdNotAvailable += OnAdNotAvailable;
				FyberCallback.RequestFail += OnRequestFail;
			}

			public override ReactionBase<Input> React(Input input)
			{
				if (base.Context._loadingSpinner != null)
				{
					float num = Time.realtimeSinceStartup - _startTime;
					int num2 = Mathf.FloorToInt(num);
					bool flag = num2 % 2 == 0;
					base.Context._loadingSpinner.invert = flag;
					base.Context._loadingSpinner.fillAmount = (flag ? (num - (float)num2) : (1f - num + (float)num2));
				}
				switch (input)
				{
				case Input.Close:
					return new TransitReaction<Idle, Input>(new Idle(base.Context));
				case Input.Proceed:
					if (Application.isEditor)
					{
						_adPromise.TrySetResult((Ad)null);
						return DiscardReaction<Input>.Default;
					}
					break;
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
						return new TransitReaction<Failure, Input>(new Failure(base.Context, reason));
					}
					if (AdFuture.Result == null)
					{
						if (Application.isEditor)
						{
							return new TransitReaction<Watching, Input>(new Watching(base.Context, AdFuture.Result));
						}
						return new TransitReaction<Failure, Input>(new Failure(base.Context, "Ad is not available."));
					}
					return new TransitReaction<Watching, Input>(new Watching(base.Context, AdFuture.Result));
				}
				if (5f <= Time.realtimeSinceStartup - _startTime)
				{
					string reason2 = string.Format(CultureInfo.InvariantCulture, "Timeout {0:f1} seconds.", 5f);
					return new TransitReaction<Failure, Input>(new Failure(base.Context, reason2));
				}
				return DiscardReaction<Input>.Default;
			}

			public override void Exit(StateBase<Input> newState, Input input)
			{
				DebugLog("Waiting.Exit");
				FyberCallback.AdAvailable -= OnAdAvailable;
				FyberCallback.AdNotAvailable -= OnAdNotAvailable;
				FyberCallback.RequestFail -= OnRequestFail;
				base.Context._windowBlocker.SetActive(false);
				base.Context._waitingPanel.SetActive(false);
				if (base.Context._simulateButton != null)
				{
					base.Context._simulateButton.SetActive(false);
				}
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

		internal sealed class Failure : ContextStateBase
		{
			private readonly string _reason;

			public Failure(ReusableRewardedVideo rewardedVideo, string reason)
				: base(rewardedVideo)
			{
				_reason = reason ?? string.Empty;
			}

			public override void Enter(StateBase<Input> oldState, Input input)
			{
				DebugLog("Failure.Enter: " + _reason);
				base.Context._failurePanel.SetActive(true);
				base.Context._windowBlocker.SetActive(true);
			}

			public override ReactionBase<Input> React(Input input)
			{
				if (input == Input.Close)
				{
					return new TransitReaction<Idle, Input>(new Idle(base.Context));
				}
				return DiscardReaction<Input>.Default;
			}

			public override void Exit(StateBase<Input> newState, Input input)
			{
				DebugLog("Failure.Exit: " + _reason);
				base.Context._windowBlocker.SetActive(false);
				base.Context._failurePanel.SetActive(false);
			}
		}

		internal sealed class Watching : ContextStateBase
		{
			[CompilerGenerated]
			internal sealed class _003C_003Ec__DisplayClass7_0
			{
				public Task<string> f;

				internal bool _003CWaitFutureThenContinue_003Eb__0()
				{
					return ((Task)f).IsCompleted;
				}
			}

			[CompilerGenerated]
			internal sealed class _003CWaitFutureThenContinue_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
			{
				private int _003C_003E1__state;

				private object _003C_003E2__current;

				public Watching _003C_003E4__this;

				private _003C_003Ec__DisplayClass7_0 _003C_003E8__1;

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
				public _003CWaitFutureThenContinue_003Ed__7(int _003C_003E1__state)
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
						_003C_003E8__1 = new _003C_003Ec__DisplayClass7_0();
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
							_003C_003E4__this.Context.RaiseAdWatchedSuccessfully(EventArgs.Empty);
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
			internal sealed class _003CSimulateWatchingCoroutine_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
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
				public _003CSimulateWatchingCoroutine_003Ed__8(int _003C_003E1__state)
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

			private readonly Ad _ad;

			private readonly TaskCompletionSource<string> _adClosedPromise = new TaskCompletionSource<string>();

			internal Task<string> AdClosedFuture
			{
				get
				{
					return _adClosedPromise.Task;
				}
			}

			public Watching(ReusableRewardedVideo rewardedVideo, Ad ad)
				: base(rewardedVideo)
			{
				if (!Application.isEditor && ad == null)
				{
					throw new ArgumentNullException("ad");
				}
				_ad = ad;
			}

			public override void Enter(StateBase<Input> oldState, Input input)
			{
				DebugLog("Watching.Enter");
				base.Context.RaiseEnterWatching(EventArgs.Empty);
				base.Context._watchingPanel.SetActive(true);
				base.Context._windowBlocker.SetActive(true);
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
					string message2 = "Watching panel was closed manually.";
					_adClosedPromise.TrySetException((Exception)new InvalidOperationException(message2));
					return new TransitReaction<Idle, Input>(new Idle(base.Context));
				}
				default:
					return DiscardReaction<Input>.Default;
				case Input.Update:
					if (!((Task)AdClosedFuture).IsCompleted)
					{
						return DiscardReaction<Input>.Default;
					}
					if (((Task)AdClosedFuture).IsFaulted)
					{
						string message = ((Exception)(object)((Task)AdClosedFuture).Exception).InnerException.Message;
						return new TransitReaction<Failure, Input>(new Failure(base.Context, message));
					}
					if (base.Context.SkipRewardScreen)
					{
						return new TransitReaction<Idle, Input>(new Idle(base.Context));
					}
					if (AdClosedFuture.Result == "CLOSE_FINISHED")
					{
						return new TransitReaction<Reward, Input>(new Reward(base.Context));
					}
					return new TransitReaction<Idle, Input>(new Idle(base.Context));
				}
			}

			public override void Exit(StateBase<Input> newState, Input input)
			{
				DebugLog("Watching.Exit");
				base.Context.RaiseExitWatching(EventArgs.Empty);
				FyberCallback.AdFinished -= OnAdFinished;
				if (((Task)AdClosedFuture).IsFaulted)
				{
					Exception ex = ((Task)AdClosedFuture).Exception.InnerExceptions.FirstOrDefault();
					UnityEngine.Debug.LogWarning((ex != null) ? ex.Message : ((Exception)(object)((Task)AdClosedFuture).Exception).Message);
				}
				base.Context._windowBlocker.SetActive(false);
				base.Context._watchingPanel.SetActive(false);
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
					Context.RaiseAdWatchedSuccessfully(EventArgs.Empty);
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

		internal sealed class Reward : ContextStateBase
		{
			public Reward(ReusableRewardedVideo rewardedVideo)
				: base(rewardedVideo)
			{
			}

			public override void Enter(StateBase<Input> oldState, Input input)
			{
				DebugLog("Reward.Enter");
				base.Context._rewardPanel.SetActive(true);
				base.Context._windowBlocker.SetActive(true);
			}

			public override void Exit(StateBase<Input> newState, Input input)
			{
				DebugLog("Reward.Exit");
				base.Context.RaiseExitReward(EventArgs.Empty);
				base.Context._windowBlocker.SetActive(false);
				base.Context._rewardPanel.SetActive(false);
			}

			public override ReactionBase<Input> React(Input input)
			{
				switch (input)
				{
				case Input.Close:
					return new TransitReaction<Idle, Input>(new Idle(base.Context));
				case Input.Proceed:
					return new TransitReaction<Idle, Input>(new Idle(base.Context));
				default:
					return DiscardReaction<Input>.Default;
				}
			}
		}

		[SerializeField]
		protected internal GameObject _windowBlocker;

		[SerializeField]
		protected internal GameObject _readyToWatchPanel;

		[SerializeField]
		protected internal GameObject _waitingPanel;

		[SerializeField]
		protected internal GameObject _watchingPanel;

		[SerializeField]
		protected internal GameObject _failurePanel;

		[SerializeField]
		protected internal GameObject _rewardPanel;

		[SerializeField]
		protected internal GameObject _simulateButton;

		[SerializeField]
		protected internal UITexture _loadingSpinner;

		[SerializeField]
		protected internal UILabel[] _rewardCountCaption;

		[SerializeField]
		protected internal UILabel[] _getRewardCountCaption;

		[SerializeField]
		protected internal UILabel[] _readyToWatchTitle;

		private StateBase<Input> _currentState;

		private IDisposable _backSubscription;

		internal DateTime StartTime { get; private set; }

		private bool SkipRewardScreen { get; set; }

		public event EventHandler ExitIdle;

		public event EventHandler EnterWatching;

		public event EventHandler ExitWatching;

		public event EventHandler ExitReward;

		public event EventHandler AdWatchedSuccessfully;

		internal void SetRewardCountCaption(string value)
		{
			if (_rewardCountCaption != null)
			{
				UILabel[] rewardCountCaption = _rewardCountCaption;
				for (int i = 0; i < rewardCountCaption.Length; i++)
				{
					rewardCountCaption[i].text = value ?? string.Empty;
				}
			}
		}

		internal void SetClaimRewardCountCaption(string value)
		{
			if (_getRewardCountCaption != null)
			{
				UILabel[] getRewardCountCaption = _getRewardCountCaption;
				for (int i = 0; i < getRewardCountCaption.Length; i++)
				{
					getRewardCountCaption[i].text = value ?? string.Empty;
				}
			}
		}

		internal void SetReadyToWatchTitle(string value)
		{
			if (_readyToWatchTitle != null)
			{
				UILabel[] readyToWatchTitle = _readyToWatchTitle;
				for (int i = 0; i < readyToWatchTitle.Length; i++)
				{
					readyToWatchTitle[i].text = value ?? string.Empty;
				}
			}
		}

		internal void SetReadyToWatchState(DateTime startTime, bool skipReadyToWatchScreen, bool skipRewardScreen)
		{
			if (!(_currentState is Idle))
			{
				UnityEngine.Debug.LogWarning("!(_currentState is Idle)");
				return;
			}
			SkipRewardScreen = skipRewardScreen;
			StartTime = startTime;
			Process(Input.Proceed);
			if (skipReadyToWatchScreen)
			{
				Process(Input.Watch);
			}
			AnalyticsStuff.DeathEscapeAds(true);
		}

		public void OnClose()
		{
			Process(Input.Close);
		}

		public void OnSimulateButtonClicked()
		{
			Process(Input.Proceed);
		}

		public void OnWatchButtonClicked()
		{
			Process(Input.Watch);
		}

		public void OnGetRewardClicked()
		{
			Process(Input.Proceed);
		}

		public ReusableRewardedVideo()
		{
			_currentState = new Initial(this);
			SkipRewardScreen = true;
		}

		private void Awake()
		{
			Process(Input.Start);
		}

		private void Update()
		{
			Process(Input.Update);
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

		private void OnBackRequested()
		{
			Process(Input.Close);
		}

		private void RaiseEnterIdle(FinishedEventArgs e)
		{
		}

		private void RaiseExitIdle(EventArgs e)
		{
			EventHandler exitIdle = this.ExitIdle;
			if (exitIdle != null)
			{
				exitIdle(this, e);
			}
		}

		private void RaiseEnterWatching(EventArgs e)
		{
			EventHandler enterWatching = this.EnterWatching;
			if (enterWatching != null)
			{
				enterWatching(this, e);
			}
		}

		private void RaiseExitWatching(EventArgs e)
		{
			EventHandler exitWatching = this.ExitWatching;
			if (exitWatching != null)
			{
				exitWatching(this, e);
			}
		}

		private void RaiseExitReward(EventArgs e)
		{
			EventHandler exitReward = this.ExitReward;
			if (exitReward != null)
			{
				exitReward(this, e);
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

		private static void DebugLog(string message)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=cyan>[{0}] {1}</color>" : "[{0}] {1}", typeof(ReusableRewardedVideo).Name, message);
			}
		}
	}
}
