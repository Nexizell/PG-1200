using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class DummyCloudApplyer : CloudApplyer
	{
		[CompilerGenerated]
		internal sealed class _003CApply_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public DummyCloudApplyer _003C_003E4__this;

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
			public _003CApply_003Ed__1(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				if (_003C_003E1__state != 0)
				{
					return false;
				}
				_003C_003E1__state = -1;
				UnityEngine.Debug.LogErrorFormat("DummyCloudApplyer.Apply, slot = {0}", _003C_003E4__this.SlotSynchronizer.ToString());
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

		public DummyCloudApplyer(CloudSlotSynchronizer synchronizer)
			: base(synchronizer)
		{
		}

		public override IEnumerator Apply(bool skipApplyingToLocalState)
		{
			UnityEngine.Debug.LogErrorFormat("DummyCloudApplyer.Apply, slot = {0}", SlotSynchronizer.ToString());
			yield break;
		}
	}
}
