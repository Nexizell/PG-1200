using System;
using System.Threading;
using DevToDev.Core.Network;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Helpers;

namespace DevToDev.Logic
{
	internal class MetricsController
	{
		internal class WebTimer
		{
			private long currentTimerValue;

			private long interval;

			private object timerData;

			private Action<object> functionOnTick;

			public WebTimer(Action<object> functionOnTick, object timerData, long interval)
			{
				currentTimerValue = DeviceHelper.Instance.GetUnixTime();
				this.interval = interval;
				this.timerData = timerData;
				this.functionOnTick = functionOnTick;
			}

			public void WebTimerTick()
			{
				long unixTime = DeviceHelper.Instance.GetUnixTime();
				if (unixTime >= currentTimerValue + interval)
				{
					functionOnTick(timerData);
					currentTimerValue = unixTime;
				}
			}
		}

		public EventHandler PeriodicSendHandler;

		private Timer timer;

		private WebTimer webTimer;

		public void WebTimerTick()
		{
			if (webTimer != null)
			{
				webTimer.WebTimerTick();
			}
		}

		public void Resume()
		{
			int num = SDKClient.Instance.NetworkStorage.TimeForRequest * 1000;
			if (UnityPlayerPlatform.isUnityWebPlatform())
			{
				if (webTimer == null)
				{
					webTimer = new WebTimer(TimerTick, null, num);
				}
			}
			else if (timer == null)
			{
				timer = new Timer(TimerTick, null, num, num);
			}
			Log.Resume();
		}

		public void Suspend()
		{
			if (!UnityPlayerPlatform.isUnityWebPlatform() && timer != null)
			{
				timer.Dispose();
				timer = null;
			}
			Log.Suspend();
		}

		private void TimerTick(object state)
		{
			Log.D("timer send");
			try
			{
				SDKClient.Instance.AsyncOperationDispatcher.DispatchOnMainThread(delegate
				{
					try
					{
						PeriodicSendHandler(state, null);
					}
					catch
					{
					}
				});
			}
			catch
			{
			}
		}

		public void ProceedMetricsStorage(MetricsStorage metricStorage, OnRequestSend onSendCallback)
		{
			Log.D("Proceed");
			lock (metricStorage)
			{
				SDKClient.Instance.AsyncOperationDispatcher.DispatchOnMainThread(delegate
				{
					SDKRequests.SendStorage(metricStorage, onSendCallback);
				});
			}
		}
	}
}
