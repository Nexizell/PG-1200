using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ExitGames.Client.Photon;
using UnityEngine;

public class PhotonPingManager
{
	[CompilerGenerated]
	internal sealed class _003CPingSocket_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Region region;

		public PhotonPingManager _003C_003E4__this;

		private PhotonPing _003Cping_003E5__1;

		private string _003CcleanIpOfRegion_003E5__2;

		private Stopwatch _003Csw_003E5__3;

		private int _003Ci_003E5__4;

		private bool _003Covertime_003E5__5;

		private float _003CrttSum_003E5__6;

		private int _003CreplyCount_003E5__7;

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
		public _003CPingSocket_003Ed__9(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num2;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				region.Ping = Attempts * MaxMilliseconsPerPing;
				_003C_003E4__this.PingsRunning++;
				if (PhotonHandler.PingImplementation == typeof(PingNativeDynamic))
				{
					UnityEngine.Debug.Log("Using constructor for new PingNativeDynamic()");
					_003Cping_003E5__1 = new PingNativeDynamic();
				}
				else if (PhotonHandler.PingImplementation == typeof(PingMono))
				{
					_003Cping_003E5__1 = new PingMono();
				}
				else
				{
					_003Cping_003E5__1 = (PhotonPing)Activator.CreateInstance(PhotonHandler.PingImplementation);
				}
				_003CrttSum_003E5__6 = 0f;
				_003CreplyCount_003E5__7 = 0;
				_003CcleanIpOfRegion_003E5__2 = region.HostAndPort;
				int num = _003CcleanIpOfRegion_003E5__2.LastIndexOf(':');
				if (num > 1)
				{
					_003CcleanIpOfRegion_003E5__2 = _003CcleanIpOfRegion_003E5__2.Substring(0, num);
				}
				_003CcleanIpOfRegion_003E5__2 = ResolveHost(_003CcleanIpOfRegion_003E5__2);
				_003Ci_003E5__4 = 0;
				goto IL_025d;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_01ab;
			case 2:
				_003C_003E1__state = -1;
				_003Csw_003E5__3 = null;
				_003Ci_003E5__4++;
				goto IL_025d;
			case 3:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_026d:
				_003C_003E4__this.PingsRunning--;
				_003C_003E2__current = null;
				_003C_003E1__state = 3;
				return true;
				IL_01ab:
				if (!_003Cping_003E5__1.Done())
				{
					if (_003Csw_003E5__3.ElapsedMilliseconds < MaxMilliseconsPerPing)
					{
						_003C_003E2__current = 0;
						_003C_003E1__state = 1;
						return true;
					}
					_003Covertime_003E5__5 = true;
				}
				num2 = (int)_003Csw_003E5__3.ElapsedMilliseconds;
				if ((!IgnoreInitialAttempt || _003Ci_003E5__4 != 0) && _003Cping_003E5__1.Successful && !_003Covertime_003E5__5)
				{
					_003CrttSum_003E5__6 += num2;
					_003CreplyCount_003E5__7++;
					region.Ping = (int)(_003CrttSum_003E5__6 / (float)_003CreplyCount_003E5__7);
				}
				_003C_003E2__current = new WaitForSeconds(0.1f);
				_003C_003E1__state = 2;
				return true;
				IL_025d:
				if (_003Ci_003E5__4 < Attempts)
				{
					_003Covertime_003E5__5 = false;
					_003Csw_003E5__3 = new Stopwatch();
					_003Csw_003E5__3.Start();
					try
					{
						_003Cping_003E5__1.StartPing(_003CcleanIpOfRegion_003E5__2);
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.Log("catched: " + ex);
						_003C_003E4__this.PingsRunning--;
						goto IL_026d;
					}
					goto IL_01ab;
				}
				goto IL_026d;
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

	public bool UseNative;

	public static int Attempts = 5;

	public static bool IgnoreInitialAttempt = true;

	public static int MaxMilliseconsPerPing = 800;

	private int PingsRunning;

	public Region BestRegion
	{
		get
		{
			Region result = null;
			int num = int.MaxValue;
			foreach (Region availableRegion in PhotonNetwork.networkingPeer.AvailableRegions)
			{
				UnityEngine.Debug.Log("BestRegion checks region: " + availableRegion);
				if (availableRegion.Ping != 0 && availableRegion.Ping < num)
				{
					num = availableRegion.Ping;
					result = availableRegion;
				}
			}
			return result;
		}
	}

	public bool Done
	{
		get
		{
			return PingsRunning == 0;
		}
	}

	public IEnumerator PingSocket(Region region)
	{
		region.Ping = Attempts * MaxMilliseconsPerPing;
		PingsRunning++;
		PhotonPing ping;
		if (PhotonHandler.PingImplementation != typeof(PingNativeDynamic))
		{
			ping = ((PhotonHandler.PingImplementation != typeof(PingMono)) ? ((PhotonPing)Activator.CreateInstance(PhotonHandler.PingImplementation)) : new PingMono());
		}
		else
		{
			UnityEngine.Debug.Log("Using constructor for new PingNativeDynamic()");
			ping = new PingNativeDynamic();
		}
		float rttSum = 0f;
		int replyCount = 0;
		string cleanIpOfRegion2 = region.HostAndPort;
		int num = cleanIpOfRegion2.LastIndexOf(':');
		if (num > 1)
		{
			cleanIpOfRegion2 = cleanIpOfRegion2.Substring(0, num);
		}
		cleanIpOfRegion2 = ResolveHost(cleanIpOfRegion2);
		for (int i = 0; i < Attempts; i++)
		{
			bool overtime = false;
			Stopwatch sw = new Stopwatch();
			sw.Start();
			try
			{
				ping.StartPing(cleanIpOfRegion2);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("catched: " + ex);
				PingsRunning--;
				break;
			}
			while (!ping.Done())
			{
				if (sw.ElapsedMilliseconds >= MaxMilliseconsPerPing)
				{
					overtime = true;
					break;
				}
				yield return 0;
			}
			int num2 = (int)sw.ElapsedMilliseconds;
			if ((!IgnoreInitialAttempt || i != 0) && ping.Successful && !overtime)
			{
				rttSum += (float)num2;
				replyCount++;
				region.Ping = (int)(rttSum / (float)replyCount);
			}
			yield return new WaitForSeconds(0.1f);
		}
		PingsRunning--;
		yield return null;
	}

	public static string ResolveHost(string hostName)
	{
		return hostName;
	}
}
