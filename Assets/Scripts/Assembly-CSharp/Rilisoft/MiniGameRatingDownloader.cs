using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class MiniGameRatingDownloader
	{
		[CompilerGenerated]
		internal sealed class _003CGetMiniGameRatingLoop_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public MiniGameRatingDownloader _003C_003E4__this;

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
			public _003CGetMiniGameRatingLoop_003Ed__10(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				float realtimeSinceStartup;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					goto IL_003f;
				case 1:
					_003C_003E1__state = -1;
					goto IL_003f;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = new WaitForSecondsRealtime(1200f);
					_003C_003E1__state = 3;
					return true;
				case 3:
					{
						_003C_003E1__state = -1;
						goto IL_004e;
					}
					IL_003f:
					if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.None)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_004e;
					IL_004e:
					realtimeSinceStartup = Time.realtimeSinceStartup;
					_003C_003E2__current = CoroutineRunner.Instance.StartCoroutine(_003C_003E4__this.GetMiniGameRatingoOnce());
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

		[CompilerGenerated]
		internal sealed class _003CGetMiniGameRatingoOnce_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			private WWW _003Cresponse_003E5__1;

			public MiniGameRatingDownloader _003C_003E4__this;

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
			public _003CGetMiniGameRatingoOnce_003Ed__12(int _003C_003E1__state)
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
				{
					_003C_003E1__state = -1;
					string miniGameRatingUrl = URLs.MiniGameRatingUrl;
					_003Cresponse_003E5__1 = Tools.CreateWww(miniGameRatingUrl);
					_003C_003E2__current = _003Cresponse_003E5__1;
					_003C_003E1__state = 1;
					return true;
				}
				case 1:
				{
					_003C_003E1__state = -1;
					if (!string.IsNullOrEmpty(_003Cresponse_003E5__1.error))
					{
						UnityEngine.Debug.LogWarningFormat("Minigame rating response error: {0}", (_003Cresponse_003E5__1 != null) ? _003Cresponse_003E5__1.error : "null");
						return false;
					}
					object obj = Json.Deserialize(URLs.Sanitize(_003Cresponse_003E5__1));
					if (obj == null)
					{
						return false;
					}
					Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
					if (dictionary == null)
					{
						return false;
					}
					if (dictionary.Count == 0)
					{
						return false;
					}
					foreach (KeyValuePair<string, object> item in dictionary)
					{
						int result = 0;
						if (int.TryParse(item.Key, out result))
						{
							double value = Convert.ToDouble(item.Value);
							_003C_003E4__this.MiniGameRating[(GameConnect.GameMode)result] = value;
						}
					}
					EventHandler loaded = _003C_003E4__this.Loaded;
					if (loaded != null)
					{
						loaded(_003C_003E4__this, EventArgs.Empty);
					}
					return false;
				}
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

		private static readonly Lazy<MiniGameRatingDownloader> s_instance = new Lazy<MiniGameRatingDownloader>(() => new MiniGameRatingDownloader());

		private readonly Dictionary<GameConnect.GameMode, double> _miniGameRating = new Dictionary<GameConnect.GameMode, double>();

		internal static MiniGameRatingDownloader Instance
		{
			get
			{
				return s_instance.Value;
			}
		}

		internal Dictionary<GameConnect.GameMode, double> MiniGameRating
		{
			get
			{
				return _miniGameRating;
			}
		}

		internal event EventHandler Loaded;

		internal double GetRatingOrDefault(GameConnect.GameMode gameMode)
		{
			double value = 0.0;
			MiniGameRating.TryGetValue(gameMode, out value);
			return value;
		}

		internal IEnumerator GetMiniGameRatingLoop()
		{
			while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.None)
			{
				yield return null;
			}
			while (true)
			{
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				yield return CoroutineRunner.Instance.StartCoroutine(GetMiniGameRatingoOnce());
				yield return new WaitForSecondsRealtime(1200f);
			}
		}

		private MiniGameRatingDownloader()
		{
		}

		private IEnumerator GetMiniGameRatingoOnce()
		{
			string miniGameRatingUrl = URLs.MiniGameRatingUrl;
			WWW response = Tools.CreateWww(miniGameRatingUrl);
			yield return response;
			if (!string.IsNullOrEmpty(response.error))
			{
				UnityEngine.Debug.LogWarningFormat("Minigame rating response error: {0}", (response != null) ? response.error : "null");
				yield break;
			}
			object obj = Json.Deserialize(URLs.Sanitize(response));
			if (obj == null)
			{
				yield break;
			}
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null || dictionary.Count == 0)
			{
				yield break;
			}
			foreach (KeyValuePair<string, object> item in dictionary)
			{
				int result = 0;
				if (int.TryParse(item.Key, out result))
				{
					double value = Convert.ToDouble(item.Value);
					MiniGameRating[(GameConnect.GameMode)result] = value;
				}
			}
			EventHandler loaded = this.Loaded;
			if (loaded != null)
			{
				loaded(this, EventArgs.Empty);
			}
		}
	}
}
