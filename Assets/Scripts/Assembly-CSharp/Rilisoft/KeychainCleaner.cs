using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	[DisallowMultipleComponent]
	public sealed class KeychainCleaner : MonoBehaviour
	{
		[CompilerGenerated]
		internal sealed class _003CQuitFromEditorCoroutine_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			private int _003Ci_003E5__1;

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
			public _003CQuitFromEditorCoroutine_003Ed__5(int _003C_003E1__state)
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
					if (!Application.isEditor)
					{
						return false;
					}
					_003Ci_003E5__1 = 0;
					break;
				case 1:
				{
					_003C_003E1__state = -1;
					int num = _003Ci_003E5__1 + 1;
					_003Ci_003E5__1 = num;
					break;
				}
				}
				if (_003Ci_003E5__1 != 2)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
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
		protected internal GUIStyle _resetKeychainButtonStyle;

		private bool _lock;

		internal void AcquireLock()
		{
			_lock = true;
		}

		internal bool LockAcquired()
		{
			return _lock;
		}

		internal void ReleaseLock()
		{
			_lock = false;
		}

		private IEnumerator QuitFromEditorCoroutine()
		{
			if (Application.isEditor)
			{
				int i = 0;
				while (i != 2)
				{
					yield return null;
					int num = i + 1;
					i = num;
				}
			}
		}

		private void Quit()
		{
			if (!Application.isEditor)
			{
				Application.Quit();
			}
			else
			{
				StartCoroutine(QuitFromEditorCoroutine());
			}
		}

		private void Clear()
		{
			PlayerPrefs.DeleteAll();
			PlayerPrefs.Save();
			bool isEditor = Application.isEditor;
		}

		private void OnResetButtonClicked()
		{
		}

		private void DrawResetKeychainButton()
		{
			Rect position = new Rect((float)Screen.width * 0.7f, 0f, (float)Screen.width * 0.3f, (float)Screen.height * 0.2f);
			_resetKeychainButtonStyle.fontSize = Mathf.RoundToInt((float)Screen.height * 0.05f);
			if (GUI.Button(position, "Начать заново", _resetKeychainButtonStyle))
			{
				OnResetButtonClicked();
			}
		}

		private void OnApplicationQuit()
		{
		}
	}
}
