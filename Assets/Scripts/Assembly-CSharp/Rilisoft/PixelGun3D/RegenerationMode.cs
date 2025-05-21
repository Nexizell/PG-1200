using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft.PixelGun3D
{
	public sealed class RegenerationMode : MonoBehaviour
	{
		[CompilerGenerated]
		internal sealed class _003CIncrementHealth_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public RegenerationMode _003C_003E4__this;

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
			public _003CIncrementHealth_003Ed__1(int _003C_003E1__state)
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
					if (_003C_003E4__this._playerController != null && _003C_003E4__this._playerController.CurHealth < _003C_003E4__this._playerController.MaxHealth)
					{
						_003C_003E4__this._playerController.CurHealth = _003C_003E4__this._playerController.CurHealth + 1f;
					}
				}
				else
				{
					_003C_003E1__state = -1;
				}
				_003C_003E2__current = new WaitForSeconds(1f);
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

		private Player_move_c _playerController;

		private void Start()
		{
		}

		private IEnumerator IncrementHealth()
		{
			while (true)
			{
				yield return new WaitForSeconds(1f);
				if (_playerController != null && _playerController.CurHealth < _playerController.MaxHealth)
				{
					_playerController.CurHealth += 1f;
				}
			}
		}
	}
}
