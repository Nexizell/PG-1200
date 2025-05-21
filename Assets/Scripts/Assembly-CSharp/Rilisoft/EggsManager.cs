using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class EggsManager : Singleton<EggsManager>
	{
		[CompilerGenerated]
		internal sealed class _003CWaitRatingSystem_003Ed__18 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public EggsManager _003C_003E4__this;

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
			public _003CWaitRatingSystem_003Ed__18(int _003C_003E1__state)
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
				}
				if (RatingSystem.instance == null)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E4__this._prevPositiveRating = RatingSystem.instance.positiveRatingLocal;
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
		internal sealed class _003CUpdateEggsReadyCoroutine_003Ed__31 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public EggsManager _003C_003E4__this;

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
			public _003CUpdateEggsReadyCoroutine_003Ed__31(int _003C_003E1__state)
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
				}
				else
				{
					_003C_003E1__state = -1;
				}
				foreach (Egg egg in _003C_003E4__this._eggs)
				{
					if (!egg.PlayerEggData.IsReady && egg.CheckReady())
					{
						_003C_003E4__this.EggReady(egg);
					}
				}
				_003C_003E2__current = new WaitForRealSeconds(1f);
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

		private const float UPDATE_DELAY_SECS = 1f;

		public const string EGGS_PLAYER_DATA_KEY = "player_eggs";

		private EggsData _eggsData;

		private readonly List<Egg> _eggs = new List<Egg>();

		private int _prevPositiveRating = -1;

		public static long CurrentTime
		{
			get
			{
				return FriendsController.ServerTime;
			}
		}

		public static event Action<Egg> OnReadyToUse;

		public static event Action<Egg, PetInfo> OnEggHatched;

		private void OnInstanceCreated()
		{
			_eggsData = EggsData.Load();
			if (_eggsData == null || _eggsData.Eggs.IsNullOrEmpty())
			{
				UnityEngine.Debug.LogError("[EGGS] load static data fail");
				return;
			}
			if (Application.platform == RuntimePlatform.IPhonePlayer && !Storager.hasKey("player_eggs"))
			{
				Storager.setString("player_eggs", "");
			}
			string @string = Storager.getString("player_eggs");
			foreach (PlayerEgg pData in (@string.IsNullOrEmpty() ? new PlayerEggs() : PlayerEggs.Create(@string)).Eggs)
			{
				EggData eggData = _eggsData.Eggs.FirstOrDefault((EggData d) => d.Id == pData.DataId);
				if (eggData != null)
				{
					Egg item = new Egg(eggData, pData);
					_eggs.Add(item);
				}
				else
				{
					UnityEngine.Debug.LogErrorFormat("[EGGS] not found egg data: '{0}'", pData.DataId);
				}
			}
		}

		protected internal override void Awake()
		{
			base.Awake();
			RatingSystem.OnRatingUpdate += RatingSystemOnRatingUpdate;
		}

		private void Start()
		{
			CoroutineRunner.Instance.StartCoroutine(WaitRatingSystem());
		}

		private void OnEnable()
		{
			StartCoroutine(UpdateEggsReadyCoroutine());
		}

		public void OnMathEnded(bool isWinner)
		{
			if (!isWinner)
			{
				return;
			}
			foreach (Egg item in _eggs.Where((Egg e) => e.OnIncubator && !e.PlayerEggData.IsReady && e.HatchedType == EggHatchedType.Wins))
			{
				item.PlayerEggData.Wins++;
				if (item.CheckReady())
				{
					EggReady(item);
				}
				else
				{
					Save();
				}
			}
		}

		private IEnumerator WaitRatingSystem()
		{
			while (RatingSystem.instance == null)
			{
				yield return null;
			}
			_prevPositiveRating = RatingSystem.instance.positiveRatingLocal;
		}

		private void RatingSystemOnRatingUpdate()
		{
			if (_prevPositiveRating < 0)
			{
				return;
			}
			int num = RatingSystem.instance.positiveRatingLocal - _prevPositiveRating;
			if (num < 1)
			{
				return;
			}
			_prevPositiveRating = RatingSystem.instance.positiveRatingLocal;
			foreach (Egg item in _eggs.Where((Egg e) => e.OnIncubator && !e.PlayerEggData.IsReady && e.HatchedType == EggHatchedType.Rating))
			{
				item.PlayerEggData.Rating += num;
				if (item.CheckReady())
				{
					EggReady(item);
				}
				else
				{
					Save();
				}
			}
		}

		public void AddEggsForSuperIncubator()
		{
			EggData eggData = GetEggData("SI_simple");
			AddEgg(eggData);
			EggData eggData2 = GetEggData("SI_ancient");
			AddEgg(eggData2);
			EggData eggData3 = GetEggData("SI_magical");
			AddEgg(eggData3);
		}

		public List<EggData> GetAllEggs()
		{
			return _eggsData.Eggs.ToList();
		}

		public List<Egg> GetPlayerEggs()
		{
			return _eggs.ToList();
		}

		public List<Egg> GetPlayerEggsInIncubator()
		{
			return (from e in GetPlayerEggs()
				where !e.CheckReady()
				select e).ToList();
		}

		public bool ExistsEgg(string eggId)
		{
			return _eggs.Exists((Egg e) => e.Data.Id == eggId);
		}

		public List<Egg> ReadyEggs()
		{
			return _eggs.Where((Egg e) => e.CheckReady()).ToList();
		}

		public EggData GetEggData(string eggId)
		{
			EggData eggData = _eggsData.Eggs.FirstOrDefault((EggData e) => e.Id == eggId);
			if (eggData == null)
			{
				Egg.LogFormat("data not found id: '{0}'", eggId);
			}
			return eggData;
		}

		public Egg AddEgg(string eggId)
		{
			return AddEgg(GetEggData(eggId));
		}

		public Egg AddEgg(EggData data)
		{
			if (data == null)
			{
				Egg.LogErrorFormat("egg data is null");
				return null;
			}
			if (CurrentTime < 1 && data.Id != "egg_Training" && data.HatchedType != EggHatchedType.Champion)
			{
				Egg.LogErrorFormat("server time not setted");
				return null;
			}
			int thisId = ((!_eggs.Any()) ? 1 : (_eggs.Max((Egg e) => e.PlayerEggData.Id) + 1));
			Egg egg = new Egg(data, new PlayerEgg(data.Id, thisId));
			_eggs.Add(egg);
			Egg.LogFormat("egg added '{0}'", data.Id);
			if (data.Id == "egg_Training" || data.HatchedType == EggHatchedType.Champion)
			{
				egg.PlayerEggData.IncubationStart = RiliExtensions.SystemTime;
			}
			else if (CurrentTime > 0)
			{
				egg.PlayerEggData.IncubationStart = CurrentTime;
			}
			Save();
			return egg;
		}

		public Egg AddRandomEgg()
		{
			if (!_eggsData.Eggs.Any())
			{
				return null;
			}
			EggData[] array = (from e in _eggsData.Eggs
				where e.HatchedType == EggHatchedType.Time || e.HatchedType == EggHatchedType.Rating
				where e.Rare == EggRarity.Simple || e.Rare == EggRarity.Ancient || e.Rare == EggRarity.Magical
				where e.Id != "egg_Training" && e.Id != "egg_tournament_winner"
				select e).ToArray();
			int num = UnityEngine.Random.Range(0, array.Count());
			return AddEgg(array[num]);
		}

		public string Use(Egg egg)
		{
			if (egg == null)
			{
				Egg.LogErrorFormat("egg is null");
				return string.Empty;
			}
			int id = egg.Id;
			egg = _eggs.FirstOrDefault((Egg e) => e.Id == egg.Id);
			if (egg == null)
			{
				Egg.LogErrorFormat("unknown egg '{0}'", id);
				return string.Empty;
			}
			if (!egg.CheckReady())
			{
				Egg.LogErrorFormat("use fail, egg not ready", egg.Id);
				return string.Empty;
			}
			PetInfo petInfo = egg.DropPet();
			_eggs.Remove(egg);
			Save();
			if (petInfo != null)
			{
				Egg.LogFormat("[EGGS] pet dropped: '{0}'", petInfo.Id);
				if (EggsManager.OnEggHatched != null)
				{
					EggsManager.OnEggHatched(egg, petInfo);
				}
				return petInfo.Id;
			}
			Egg.LogErrorFormat("[EGGS] dropped null pet.");
			return null;
		}

		private IEnumerator UpdateEggsReadyCoroutine()
		{
			while (true)
			{
				foreach (Egg egg in _eggs)
				{
					if (!egg.PlayerEggData.IsReady && egg.CheckReady())
					{
						EggReady(egg);
					}
				}
				yield return new WaitForRealSeconds(1f);
			}
		}

		private void EggReady(Egg egg)
		{
			egg.PlayerEggData.IsReady = true;
			Save();
			if (EggsManager.OnReadyToUse != null)
			{
				EggsManager.OnReadyToUse(egg);
			}
		}

		private void Save()
		{
			string val = new PlayerEggs
			{
				Eggs = _eggs.Select((Egg e) => e.PlayerEggData).ToList()
			}.ToString();
			Storager.setString("player_eggs", val);
		}
	}
}
