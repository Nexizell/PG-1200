using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Rilisoft
{
	public sealed class NewDayWatcher : MonoBehaviour
	{
		private CancellationTokenSource _cancellationTokenSource;

		private DateTime? _timestamp;

		internal event EventHandler NewDay;

		private void Start()
		{
			_timestamp = FriendsController.GetServerTime();
		}

		private void OnEnable()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			if (_cancellationTokenSource != null)
			{
				_cancellationTokenSource.Cancel();
			}
			_cancellationTokenSource = new CancellationTokenSource();
			StartCoroutine(WatchTimeCoroutine(_cancellationTokenSource.Token));
		}

		private void OnDisable()
		{
			if (_cancellationTokenSource != null)
			{
				_cancellationTokenSource.Cancel();
				_cancellationTokenSource = null;
			}
		}

		private IEnumerator WatchTimeCoroutine(CancellationToken cancellationToken)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			float delaySeconds = (Defs.IsDeveloperBuild ? 1f : 60f);
			while (!_timestamp.HasValue)
			{
				if (((CancellationToken)(cancellationToken)).IsCancellationRequested)
				{
					yield break;
				}
				yield return new WaitForSecondsRealtime(delaySeconds);
				_timestamp = FriendsController.GetServerTime();
			}
			while (!((CancellationToken)(cancellationToken)).IsCancellationRequested)
			{
				yield return new WaitForSecondsRealtime(delaySeconds);
				DateTime? serverTime = FriendsController.GetServerTime();
				if (serverTime.HasValue && serverTime.Value.Date > _timestamp.Value.Date)
				{
					_timestamp = serverTime;
					EventHandler newDay = this.NewDay;
					if (newDay != null)
					{
						newDay(this, EventArgs.Empty);
					}
				}
			}
		}
	}
}
