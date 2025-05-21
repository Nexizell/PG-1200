using System;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class QuestMediator
	{
		internal sealed class QuestEventSource : QuestEvents
		{
			internal new void RaiseWin(WinEventArgs e)
			{
				base.RaiseWin(e);
			}

			internal new void RaiseKillOtherPlayer(KillOtherPlayerEventArgs e)
			{
				base.RaiseKillOtherPlayer(e);
			}

			internal new void RaiseKillOtherPlayerWithFlag(EventArgs e)
			{
				base.RaiseKillOtherPlayerWithFlag(e);
			}

			internal new void RaiseCapture(CaptureEventArgs e)
			{
				base.RaiseCapture(e);
			}

			internal new void RaiseKillMonster(KillMonsterEventArgs e)
			{
				base.RaiseKillMonster(e);
			}

			internal new void RaiseBreakSeries(EventArgs e)
			{
				base.RaiseBreakSeries(e);
			}

			internal new void RaiseMakeSeries(EventArgs e)
			{
				base.RaiseMakeSeries(e);
			}

			internal new void RaiseSurviveWaveInArena(SurviveWaveInArenaEventArgs e)
			{
				base.RaiseSurviveWaveInArena(e);
			}

			internal new void RaiseGetGotcha(EventArgs e)
			{
				base.RaiseGetGotcha(e);
			}

			internal new void RaiseSocialInteraction(SocialInteractionEventArgs e)
			{
				base.RaiseSocialInteraction(e);
			}

			internal new void RaiseJump(EventArgs e)
			{
				base.RaiseJump(e);
			}

			internal new void RaiseTurretKill(EventArgs e)
			{
				base.RaiseTurretKill(e);
			}

			internal new void RaiseKillOtherPlayerOnFly(KillOtherPlayerOnFlyEventArgs e)
			{
				base.RaiseKillOtherPlayerOnFly(e);
			}
		}

		private static readonly QuestEventSource _eventSource = new QuestEventSource();

		public static QuestEvents Events
		{
			get
			{
				return _eventSource;
			}
		}

		public static void NotifyWin(GameConnect.GameMode mode, string map)
		{
			WinEventArgs winEventArgs = new WinEventArgs
			{
				Mode = mode,
				Map = (map ?? string.Empty)
			};
			try
			{
				_eventSource.RaiseWin(winEventArgs);
			}
			catch (Exception exception)
			{
				Debug.LogErrorFormat("Caught exception in NotifyWin: {0}", winEventArgs);
				Debug.LogException(exception);
			}
		}

		public static void NotifyKillOtherPlayer(GameConnect.GameMode mode, ShopNGUIController.CategoryNames weaponSlot, bool headshot = false, bool grenade = false, bool revenge = false, bool isInvisible = false, bool turretKill = false)
		{
			KillOtherPlayerEventArgs killOtherPlayerEventArgs = new KillOtherPlayerEventArgs
			{
				Mode = mode,
				WeaponSlot = weaponSlot,
				Headshot = headshot,
				Grenade = grenade,
				Revenge = revenge,
				IsInvisible = isInvisible
			};
			try
			{
				_eventSource.RaiseKillOtherPlayer(killOtherPlayerEventArgs);
			}
			catch (Exception exception)
			{
				Debug.LogErrorFormat("Caught exception in NotifyKillOtherPlayer: {0}", killOtherPlayerEventArgs);
				Debug.LogException(exception);
			}
		}

		public static void NotifyKillOtherPlayerWithFlag()
		{
			try
			{
				_eventSource.RaiseKillOtherPlayerWithFlag(EventArgs.Empty);
			}
			catch (Exception exception)
			{
				Debug.LogError("Caught exception in NotifyKillOtherPlayerWithFlag.");
				Debug.LogException(exception);
			}
		}

		public static void NotifyCapture(GameConnect.GameMode mode)
		{
			CaptureEventArgs captureEventArgs = new CaptureEventArgs
			{
				Mode = mode
			};
			try
			{
				_eventSource.RaiseCapture(captureEventArgs);
			}
			catch (Exception exception)
			{
				Debug.LogErrorFormat("Caught exception in NotifyCapture: {0}", captureEventArgs);
				Debug.LogException(exception);
			}
		}

		public static void NotifyKillMonster(ShopNGUIController.CategoryNames weaponSlot, bool campaign = false)
		{
			KillMonsterEventArgs killMonsterEventArgs = new KillMonsterEventArgs
			{
				WeaponSlot = weaponSlot,
				Campaign = campaign
			};
			try
			{
				_eventSource.RaiseKillMonster(killMonsterEventArgs);
			}
			catch (Exception exception)
			{
				Debug.LogErrorFormat("Caught exception in NotifyKillMonster: {0}", killMonsterEventArgs);
				Debug.LogException(exception);
			}
		}

		public static void NotifyBreakSeries()
		{
			try
			{
				_eventSource.RaiseBreakSeries(EventArgs.Empty);
			}
			catch (Exception exception)
			{
				Debug.LogError("Caught exception in NotifyBreakSeries.");
				Debug.LogException(exception);
			}
		}

		public static void NotifyMakeSeries()
		{
			try
			{
				_eventSource.RaiseMakeSeries(EventArgs.Empty);
			}
			catch (Exception exception)
			{
				Debug.LogError("Caught exception in NotifyMakeSeries.");
				Debug.LogException(exception);
			}
		}

		public static void NotifySurviveWaveInArena(int currentWave)
		{
			SurviveWaveInArenaEventArgs surviveWaveInArenaEventArgs = new SurviveWaveInArenaEventArgs
			{
				WaveNumber = currentWave
			};
			try
			{
				_eventSource.RaiseSurviveWaveInArena(surviveWaveInArenaEventArgs);
			}
			catch (Exception exception)
			{
				Debug.LogErrorFormat("Caught exception in NotifySurviveWaveInArena: {0}", surviveWaveInArenaEventArgs);
				Debug.LogException(exception);
			}
		}

		public static void NotifyGetGotcha()
		{
			try
			{
				_eventSource.RaiseGetGotcha(EventArgs.Empty);
			}
			catch (Exception exception)
			{
				Debug.LogError("Caught exception in NotifyGetGotcha.");
				Debug.LogException(exception);
			}
		}

		public static void NotifySocialInteraction(string kind)
		{
			SocialInteractionEventArgs socialInteractionEventArgs = new SocialInteractionEventArgs
			{
				Kind = (kind ?? string.Empty)
			};
			try
			{
				_eventSource.RaiseSocialInteraction(socialInteractionEventArgs);
			}
			catch (Exception exception)
			{
				Debug.LogErrorFormat("Caught exception in NotifySocialInteraction: {0}", socialInteractionEventArgs);
				Debug.LogException(exception);
			}
		}

		public static void NotifyJump()
		{
			try
			{
				_eventSource.RaiseJump(EventArgs.Empty);
			}
			catch (Exception exception)
			{
				Debug.LogError("Caught exception in NotifyJump");
				Debug.LogException(exception);
			}
		}

		public static void NotifyTurretKill()
		{
			try
			{
				_eventSource.RaiseTurretKill(EventArgs.Empty);
			}
			catch (Exception exception)
			{
				Debug.LogError("Caught exception in NotifyTurretKill");
				Debug.LogException(exception);
			}
		}

		public static void NotifyKillOtherPlayerOnFly(bool iamFly, bool killedFly)
		{
			KillOtherPlayerOnFlyEventArgs killOtherPlayerOnFlyEventArgs = new KillOtherPlayerOnFlyEventArgs
			{
				IamFly = iamFly,
				KilledPlayerFly = killedFly
			};
			try
			{
				_eventSource.RaiseKillOtherPlayerOnFly(killOtherPlayerOnFlyEventArgs);
			}
			catch (Exception exception)
			{
				Debug.LogErrorFormat("Caught exception in NotifyKillOtherPlayerOnFly: {0}", killOtherPlayerOnFlyEventArgs);
				Debug.LogException(exception);
			}
		}
	}
}
