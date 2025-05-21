using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FyberPlugin;
using UnityEngine;

namespace Rilisoft
{
	internal abstract class FyberInterstitialRunnerBase : IDisposable, AdCallback
	{
		[CompilerGenerated]
		internal sealed class _003CSimulateRequestCoroutine_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public FyberInterstitialRunnerBase _003C_003E4__this;

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
			public _003CSimulateRequestCoroutine_003Ed__12(int _003C_003E1__state)
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
					_003C_003E4__this.OnAdNotAvailable(AdFormat.INTERSTITIAL);
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
		internal sealed class _003CSimulateStartCoroutine_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public FyberInterstitialRunnerBase _003C_003E4__this;

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
			public _003CSimulateStartCoroutine_003Ed__13(int _003C_003E1__state)
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
					_003C_003E4__this._adFinishedPromise.TrySetException((Exception)new NotSupportedException());
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

		private readonly TaskCompletionSource<AdResult> _adFinishedPromise = new TaskCompletionSource<AdResult>();

		private bool _disposed;

		public Task<AdResult> AdFinishedFuture
		{
			get
			{
				return _adFinishedPromise.Task;
			}
		}

		public void Run()
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=magenta>{0}.Run</color>" : "{0}.Run", GetType().Name);
			}
			if (Application.isEditor || BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				CoroutineRunner.Instance.StartCoroutine(SimulateRequestCoroutine());
			}
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				Unsubscribe();
				Dispose(true);
				_disposed = true;
			}
		}

		public void OnAdFinished(AdResult result)
		{
			if (!_disposed)
			{
				if (Defs.IsDeveloperBuild)
				{
					string format = (Application.isEditor ? "<color=magenta>{0}.OnAdFinished: {1}</color>" : "{0}.OnAdFinished: {1}");
					string text = string.Format(CultureInfo.InvariantCulture, "{{ status: {0}, message: '{1}' }}", result.Status, result.Message);
					UnityEngine.Debug.LogFormat(format, GetType().Name, text);
				}
				_adFinishedPromise.TrySetResult(result);
			}
		}

		public void OnAdStarted(Ad ad)
		{
			if (!_disposed && Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=magenta>{0}.OnAdStarted: {1}</color>" : "{0}.OnAdStarted: {1}", GetType().Name, ad);
			}
		}

		protected abstract string GetReasonToSkip();

		protected virtual void Dispose(bool disposing)
		{
		}

		private void OnAdAvailable(Ad ad)
		{
			if (_disposed || ad.AdFormat != AdFormat.INTERSTITIAL)
			{
				return;
			}
			Unsubscribe();
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=magenta>{0}.OnAdAvailable: {1}</color>" : "{0}.OnAdAvailable: {1}", GetType().Name, ad);
			}
			DateTime? serverTime = FriendsController.GetServerTime();
			if (!serverTime.HasValue)
			{
				UnityEngine.Debug.Log("Skipping showing interstitial: server time is not received.");
				return;
			}
			string reasonToSkip = GetReasonToSkip();
			if (!string.IsNullOrEmpty(reasonToSkip))
			{
				UnityEngine.Debug.LogFormat("Skipping showing interstitial: {0}", reasonToSkip);
				return;
			}
			UnityEngine.Debug.Log("Trying to show interstitial.");
			PlayerPrefs.SetString(Defs.LastTimeShowBanerKey, serverTime.Value.ToString("s"));
			int countWithinCurrentDay = FyberFacade.Instance.IncrementCurrentDailyInterstitialCount(serverTime.Value);
			if (Application.isEditor || BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				CoroutineRunner.Instance.StartCoroutine(SimulateStartCoroutine());
				return;
			}
			AnalyticsStuff.LogInterstitialStarted(countWithinCurrentDay);
			ad.WithCallback(this).Start();
		}

		private void OnAdNotAvailable(AdFormat adFormat)
		{
			if (!_disposed && adFormat == AdFormat.INTERSTITIAL)
			{
				Unsubscribe();
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=magenta>{0}.OnAdNotAvailable: {1}</color>" : "{0}.OnAdNotAvailable: {1}", GetType().Name, adFormat);
				}
				string message = "Interstitial is not available.";
				_adFinishedPromise.TrySetException((Exception)new InvalidOperationException(message));
			}
		}

		private void OnRequestFail(RequestError requestError)
		{
			if (!_disposed)
			{
				Unsubscribe();
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=magenta>{0}.OnRequestFail: {1}</color>" : "{0}.OnRequestFail: {1}", GetType().Name, requestError.Description);
				}
				_adFinishedPromise.TrySetException((Exception)new InvalidOperationException(requestError.Description));
			}
		}

		private void Unsubscribe()
		{
		}

		private IEnumerator SimulateRequestCoroutine()
		{
			yield return new WaitForSeconds(1f);
			OnAdNotAvailable(AdFormat.INTERSTITIAL);
		}

		private IEnumerator SimulateStartCoroutine()
		{
			yield return new WaitForSeconds(1f);
			_adFinishedPromise.TrySetException((Exception)new NotSupportedException());
		}
	}
}
