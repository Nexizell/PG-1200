using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class DummySavedGameClient
	{
		[CompilerGenerated]
		internal sealed class _003CWaitAndExecuteCoroutine_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public Action action;

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
			public _003CWaitAndExecuteCoroutine_003Ed__9(int _003C_003E1__state)
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
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					action();
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

		private int _conflictCounter;

		private readonly string _filename;

		private readonly DummySavedGameMetadata _dummySavedGameMetadata;

		public string Filename
		{
			get
			{
				return _filename;
			}
		}

		public DummySavedGameClient(string filename)
		{
			_filename = filename ?? string.Empty;
			_dummySavedGameMetadata = new DummySavedGameMetadata(_filename);
		}

		public void Delete(ISavedGameMetadata metadata)
		{
			UnityEngine.Debug.LogFormat("{0}('{1}').Delete()", GetType().Name, Filename);
		}

		public void FetchAllSavedGames(DataSource source, Action<SavedGameRequestStatus, List<ISavedGameMetadata>> callback)
		{
			UnityEngine.Debug.LogFormat("{0}('{1}').FetchAllSavedGames()", GetType().Name, Filename);
			if (callback != null)
			{
				Action action = delegate
				{
					callback(SavedGameRequestStatus.Success, new List<ISavedGameMetadata>());
				};
				CoroutineRunner.Instance.StartCoroutine(WaitAndExecuteCoroutine(action));
			}
		}

		public void OpenWithAutomaticConflictResolution(string filename, DataSource source, ConflictResolutionStrategy resolutionStrategy, Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
		{
			UnityEngine.Debug.LogFormat("{0}('{1}').OpenWithAutomaticConflictResolution()", GetType().Name, Filename);
			if (callback != null)
			{
				Action action = delegate
				{
					callback(SavedGameRequestStatus.Success, _dummySavedGameMetadata);
				};
				CoroutineRunner.Instance.StartCoroutine(WaitAndExecuteCoroutine(action));
			}
		}

		public void OpenWithManualConflictResolution(string filename, DataSource source, bool prefetchDataOnConflict, ConflictCallback conflictCallback, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback)
		{
			bool flag = _conflictCounter % 2 == 0;
			UnityEngine.Debug.LogFormat("{0}('{1}', {2}).OpenWithManualConflictResolution()", GetType().Name, Filename, flag);
			if (flag)
			{
				if (conflictCallback == null)
				{
					return;
				}
				byte[] data = Encoding.UTF8.GetBytes("{}");
				Action action = delegate
				{
					conflictCallback(DummyConflictResolver.Instance, _dummySavedGameMetadata, data, _dummySavedGameMetadata, data);
				};
				CoroutineRunner.Instance.StartCoroutine(WaitAndExecuteCoroutine(action));
			}
			else
			{
				if (completedCallback == null)
				{
					return;
				}
				Action action2 = delegate
				{
					completedCallback(SavedGameRequestStatus.Success, _dummySavedGameMetadata);
				};
				CoroutineRunner.Instance.StartCoroutine(WaitAndExecuteCoroutine(action2));
			}
			_conflictCounter++;
		}

		public void ReadBinaryData(ISavedGameMetadata metadata, Action<SavedGameRequestStatus, byte[]> completedCallback)
		{
			UnityEngine.Debug.LogFormat("{0}('{1}').ReadBinaryData()", GetType().Name, Filename);
			if (completedCallback != null)
			{
				byte[] binaryData = Encoding.UTF8.GetBytes("{}");
				Action action = delegate
				{
					completedCallback(SavedGameRequestStatus.Success, binaryData);
				};
				CoroutineRunner.Instance.StartCoroutine(WaitAndExecuteCoroutine(action));
			}
		}

		public void ShowSelectSavedGameUI(string uiTitle, uint maxDisplayedSavedGames, bool showCreateSaveUI, bool showDeleteSaveUI, Action<SelectUIStatus, ISavedGameMetadata> callback)
		{
			UnityEngine.Debug.LogFormat("{0}('{1}').ShowSelectSavedGameUI()", GetType().Name, Filename);
			if (callback != null)
			{
				Action action = delegate
				{
					callback(SelectUIStatus.SavedGameSelected, _dummySavedGameMetadata);
				};
				CoroutineRunner.Instance.StartCoroutine(WaitAndExecuteCoroutine(action));
			}
		}

		private IEnumerator WaitAndExecuteCoroutine(Action action)
		{
			yield return null;
			action();
		}
	}
}
