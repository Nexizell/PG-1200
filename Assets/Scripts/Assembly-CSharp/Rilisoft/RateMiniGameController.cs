using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public sealed class RateMiniGameController : MonoBehaviour
	{
		[CompilerGenerated]
		internal sealed class _003CWaitForResponse_003Ed__14 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public WWW response;

			public string action;

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
			public _003CWaitForResponse_003Ed__14(int _003C_003E1__state)
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
					if (!Defs.IsDeveloperBuild)
					{
						return false;
					}
					break;
				case 1:
					_003C_003E1__state = -1;
					break;
				}
				if (!response.isDone)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				if (!string.IsNullOrEmpty(response.error))
				{
					UnityEngine.Debug.LogErrorFormat("{0}: {1}", action, response.error);
					return false;
				}
				UnityEngine.Debug.LogFormat("{0}: {1}", action, response.text);
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

		[SerializeField]
		protected internal GameObject _rateGameWindow;

		[SerializeField]
		protected internal StarView[] _starViews;

		[SerializeField]
		protected internal GameObject _gameTitle;

		private GameConnect.GameMode _gameMode;

		private string _miniGameRatingKey;

		private int _currentRatingInGui;

		private readonly Dictionary<GameConnect.GameMode, DateTime> _lastSentTimestamps = new Dictionary<GameConnect.GameMode, DateTime>(6);

		private readonly List<UILabel> _gameTitleLabels = new List<UILabel>(3);

		private IDisposable _backSubscription;

		internal GameConnect.GameMode GameMode
		{
			get
			{
				return _gameMode;
			}
			set
			{
				if (_gameMode != value)
				{
					_gameMode = value;
					RefreshTitle();
					_miniGameRatingKey = null;
					_currentRatingInGui = PlayerPrefs.GetInt(MiniGameRatingKey, 0);
					RefreshStars(_currentRatingInGui);
				}
			}
		}

		private string MiniGameRatingKey
		{
			get
			{
				if (string.IsNullOrEmpty(_miniGameRatingKey))
				{
					_miniGameRatingKey = "MiniGameRating." + GameMode;
				}
				return _miniGameRatingKey;
			}
		}

		public void HandleSubmitRating()
		{
			try
			{
				if (_currentRatingInGui == 0)
				{
					return;
				}
				PlayerPrefs.SetInt(MiniGameRatingKey, _currentRatingInGui);
				if (FriendsController.sharedController == null || string.IsNullOrEmpty(FriendsController.sharedController.id))
				{
					return;
				}
				DateTime utcNow = DateTime.UtcNow;
				DateTime value;
				_lastSentTimestamps.TryGetValue(GameMode, out value);
				TimeSpan timeSpan = utcNow - value;
				TimeSpan timeSpan2 = (Defs.IsDeveloperBuild ? TimeSpan.FromSeconds(10.0) : TimeSpan.FromMinutes(10.0));
				if (timeSpan > timeSpan2)
				{
					UnityEngine.Debug.LogFormat("Sending rating {0} ({1}): {2}", GameMode, (int)GameMode, _currentRatingInGui);
					WWWForm wWWForm = new WWWForm();
					wWWForm.AddField("action", "set_mode_rating");
					wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
					wWWForm.AddField("uniq_id", FriendsController.sharedController.id);
					wWWForm.AddField("auth", FriendsController.Hash("set_mode_rating"));
					wWWForm.AddField("mode", (int)GameMode);
					wWWForm.AddField("rating", _currentRatingInGui);
					WWW wWW = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, wWWForm);
					if (wWW != null)
					{
						StartCoroutine(WaitForResponse("set_mode_rating", wWW));
						_lastSentTimestamps[GameMode] = utcNow;
						AnalyticsStuff.MiniGamesRating(_currentRatingInGui, GameMode);
					}
				}
				else
				{
					UnityEngine.Debug.LogFormat("Skipping sending rating {0} ({1}): {2}; elapsed: {3}", GameMode, (int)GameMode, _currentRatingInGui, timeSpan);
				}
			}
			finally
			{
				SetVisibility(false);
			}
		}

		internal void SetVisibility(bool visible)
		{
			if (_rateGameWindow == null)
			{
				UnityEngine.Debug.LogError("_rateGameWindow should not be null.");
				return;
			}
			if (visible)
			{
				RefreshTitle();
				if (PlayerPrefs.HasKey(MiniGameRatingKey))
				{
					_currentRatingInGui = PlayerPrefs.GetInt(MiniGameRatingKey);
				}
				RefreshStars(_currentRatingInGui);
				_backSubscription = BackSystem.Instance.Register(delegate
				{
					SetVisibility(false);
				});
			}
			else if (_backSubscription != null)
			{
				_backSubscription.Dispose();
				_backSubscription = null;
			}
			_rateGameWindow.SetActive(visible);
		}

		private IEnumerator WaitForResponse(string action, WWW response)
		{
			if (Defs.IsDeveloperBuild)
			{
				while (!response.isDone)
				{
					yield return null;
				}
				if (!string.IsNullOrEmpty(response.error))
				{
					UnityEngine.Debug.LogErrorFormat("{0}: {1}", action, response.error);
				}
				else
				{
					UnityEngine.Debug.LogFormat("{0}: {1}", action, response.text);
				}
			}
		}

		private void Awake()
		{
			if (_starViews == null)
			{
				return;
			}
			int num = _starViews.Length;
			for (int i = 0; i != num; i++)
			{
				int index = i;
				_starViews[i].Clicked += delegate
				{
					HandleStarClicked(index);
				};
			}
		}

		private void HandleStarClicked(int index)
		{
			if (Application.isEditor)
			{
				UnityEngine.Debug.LogFormat("HandleStarClicked({0})", index);
			}
			_currentRatingInGui = index + 1;
			RefreshStars(_currentRatingInGui);
			HandleSubmitRating();
		}

		private void RefreshStars(int rating)
		{
			if (_starViews != null)
			{
				int num = _starViews.Length;
				for (int i = 0; i != num; i++)
				{
					_starViews[i].SetVisibility(i < rating);
				}
			}
		}

		private void RefreshTitle()
		{
			if (_gameTitle == null)
			{
				return;
			}
			string value = string.Empty;
			if (GameMode == GameConnect.GameMode.Arena)
			{
				value = "Key_0456";
			}
			else if (!GameConnect.gameModesLocalizeKey.TryGetValue((int)GameMode, out value))
			{
				value = string.Empty;
			}
			string text = LocalizationStore.Get(value);
			if (_gameTitleLabels.Count == 0)
			{
				_gameTitle.GetComponentsInChildren(true, _gameTitleLabels);
			}
			foreach (UILabel gameTitleLabel in _gameTitleLabels)
			{
				gameTitleLabel.text = text;
			}
		}
	}
}
