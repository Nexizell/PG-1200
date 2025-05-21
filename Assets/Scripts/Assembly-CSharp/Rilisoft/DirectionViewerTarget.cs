using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class DirectionViewerTarget : MonoBehaviour
	{
		[CompilerGenerated]
		internal sealed class _003CRocketMonitorCoroutine_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public DirectionViewerTarget _003C_003E4__this;

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
			public _003CRocketMonitorCoroutine_003Ed__8(int _003C_003E1__state)
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
					goto IL_005f;
				case 1:
					_003C_003E1__state = -1;
					goto IL_005f;
				case 2:
					_003C_003E1__state = -1;
					goto IL_008b;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_005f:
					if (_003C_003E4__this._rocketComponent == null)
					{
						_003C_003E4__this._rocketComponent = _003C_003E4__this.transform.root.GetComponentInParent<Rocket>();
						_003C_003E2__current = null;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_008b;
					IL_008b:
					if (!_003C_003E4__this._rocketComponent.isRun)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003E4__this.ShowPointer();
					break;
				}
				if (_003C_003E4__this._rocketComponent.isRun)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				}
				_003C_003E4__this.HidePointer();
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
		internal sealed class _003CPetMonitorCoroutine_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public DirectionViewerTarget _003C_003E4__this;

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
			public _003CPetMonitorCoroutine_003Ed__9(int _003C_003E1__state)
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
					_003C_003E4__this.ShowPointer();
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
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

		[SerializeField]
		protected internal DirectionViewTargetType _Type;

		[SerializeField]
		[ReadOnly]
		protected internal Rocket _rocketComponent;

		public DirectionViewTargetType Type
		{
			get
			{
				return _Type;
			}
		}

		public Transform Transform
		{
			get
			{
				return base.gameObject.transform;
			}
		}

		private void OnEnable()
		{
			switch (Type)
			{
			case DirectionViewTargetType.Grenade:
				StartCoroutine(RocketMonitorCoroutine());
				break;
			case DirectionViewTargetType.Pet:
				StartCoroutine(PetMonitorCoroutine());
				break;
			}
		}

		private void OnDisable()
		{
			HidePointer();
		}

		private IEnumerator RocketMonitorCoroutine()
		{
			while (_rocketComponent == null)
			{
				_rocketComponent = transform.root.GetComponentInParent<Rocket>();
				yield return null;
			}
			while (!_rocketComponent.isRun)
			{
				yield return null;
			}
			ShowPointer();
			while (_rocketComponent.isRun)
			{
				yield return null;
			}
			HidePointer();
		}

		private IEnumerator PetMonitorCoroutine()
		{
			ShowPointer();
			yield return null;
		}

		private void ShowPointer()
		{
			CoroutineRunner.WaitUntil(() => DirectionViewer.Instance != null, delegate
			{
				DirectionViewer.Instance.LookToMe(this);
			});
		}

		private void HidePointer()
		{
			if (DirectionViewer.Instance != null)
			{
				DirectionViewer.Instance.ForgetMe(this);
			}
		}
	}
}
