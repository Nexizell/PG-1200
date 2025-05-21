using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using Unity.Linq;
using UnityEngine;

namespace RilisoftBot
{
	public class BotHpIndicator : MonoBehaviour
	{
		internal class HealthProvider
		{
			private readonly Func<float> _healthGetter;

			private readonly Func<float> _baseHealthGetter;

			public float Health
			{
				get
				{
					return _healthGetter();
				}
			}

			public float BaseHealth
			{
				get
				{
					return _baseHealthGetter();
				}
			}

			public HealthProvider(Func<float> healthGetter, Func<float> baseHealthGetter)
			{
				_healthGetter = healthGetter ?? ((Func<float>)(() => 0f));
				_baseHealthGetter = baseHealthGetter ?? ((Func<float>)(() => 0f));
			}
		}

		[CompilerGenerated]
		internal sealed class _003CStart_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public BotHpIndicator _003C_003E4__this;

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
			public _003CStart_003Ed__6(int _003C_003E1__state)
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
					_003C_003E4__this._frame.SetActive(false);
					_003C_003E2__current = new WaitForSeconds(0.2f);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E2__current = _003C_003E4__this.WaitHpOwner();
					_003C_003E1__state = 2;
					return true;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = _003C_003E4__this.UpdateIndicator();
					_003C_003E1__state = 3;
					return true;
				case 3:
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

		[CompilerGenerated]
		internal sealed class _003CUpdateIndicator_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public BotHpIndicator _003C_003E4__this;

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
			public _003CUpdateIndicator_003Ed__7(int _003C_003E1__state)
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
					goto IL_0026;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0079;
				case 2:
					_003C_003E1__state = -1;
					break;
				case 3:
					{
						_003C_003E1__state = -1;
						goto IL_0026;
					}
					IL_0026:
					if (_003C_003E4__this._hp == null || _003C_003E4__this._healthBar == null || Math.Abs(_003C_003E4__this._hp.BaseHealth) < 0.0001f)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0079;
					IL_0079:
					if (_003C_003E4__this._hp.Health <= 0f && _003C_003E4__this._frame.activeInHierarchy)
					{
						_003C_003E4__this._frame.SetActive(false);
						_003C_003E2__current = null;
						_003C_003E1__state = 2;
						return true;
					}
					break;
				}
				_003C_003E4__this._currentScale = _003C_003E4__this._hp.Health / _003C_003E4__this._hp.BaseHealth;
				if (Math.Abs(_003C_003E4__this._currentScale - _003C_003E4__this._prevScale) > 0.0001f)
				{
					_003C_003E4__this._frame.SetActive(true);
					_003C_003E4__this._currShowTime = 2f;
					_003C_003E4__this._healthBar.localScale = new Vector3(_003C_003E4__this._currentScale, _003C_003E4__this._healthBar.localScale.y, _003C_003E4__this._healthBar.localScale.z);
				}
				_003C_003E4__this._prevScale = _003C_003E4__this._currentScale;
				if (_003C_003E4__this._currShowTime > 0f)
				{
					_003C_003E4__this._currShowTime -= Time.deltaTime;
				}
				else
				{
					_003C_003E4__this._currShowTime = 0f;
					_003C_003E4__this._frame.SetActive(false);
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 3;
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

		[CompilerGenerated]
		internal sealed class _003C_003Ec__DisplayClass8_0
		{
			public BaseBot bot;

			internal float _003CWaitHpOwner_003Eb__0()
			{
				return bot.health;
			}

			internal float _003CWaitHpOwner_003Eb__1()
			{
				return bot.baseHealth;
			}
		}

		[CompilerGenerated]
		internal sealed class _003C_003Ec__DisplayClass8_1
		{
			public TrainingEnemy dummy;

			internal float _003CWaitHpOwner_003Eb__2()
			{
				return dummy.hitPoints;
			}

			internal float _003CWaitHpOwner_003Eb__3()
			{
				return dummy.baseHitPoints;
			}
		}

		[CompilerGenerated]
		internal sealed class _003CWaitHpOwner_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public BotHpIndicator _003C_003E4__this;

			private bool _003Csetted_003E5__1;

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
			public _003CWaitHpOwner_003Ed__8(int _003C_003E1__state)
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
					_003Csetted_003E5__1 = false;
					break;
				case 1:
					_003C_003E1__state = -1;
					break;
				}
				if (!_003Csetted_003E5__1)
				{
					foreach (GameObject item in _003C_003E4__this.gameObject.AncestorsAndSelf())
					{
						_003C_003Ec__DisplayClass8_0 CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass8_0
						{
							bot = item.GetComponent<BaseBot>()
						};
						if (CS_0024_003C_003E8__locals0.bot != null)
						{
							_003C_003E4__this._hp = new HealthProvider(() => CS_0024_003C_003E8__locals0.bot.health, () => CS_0024_003C_003E8__locals0.bot.baseHealth);
							_003Csetted_003E5__1 = true;
							break;
						}
					}
					foreach (GameObject item2 in _003C_003E4__this.gameObject.AncestorsAndSelf())
					{
						_003C_003Ec__DisplayClass8_1 CS_0024_003C_003E8__locals1 = new _003C_003Ec__DisplayClass8_1
						{
							dummy = item2.GetComponent<TrainingEnemy>()
						};
						if (CS_0024_003C_003E8__locals1.dummy != null)
						{
							_003C_003E4__this._hp = new HealthProvider(() => CS_0024_003C_003E8__locals1.dummy.hitPoints, () => CS_0024_003C_003E8__locals1.dummy.baseHitPoints);
							_003Csetted_003E5__1 = true;
							break;
						}
					}
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
		protected internal GameObject _frame;

		private float _currShowTime;

		[SerializeField]
		protected internal Transform _healthBar;

		[ReadOnly]
		[SerializeField]
		protected internal float _currentScale;

		private float _prevScale = 1f;

		private HealthProvider _hp;

		private IEnumerator Start()
		{
			_frame.SetActive(false);
			yield return new WaitForSeconds(0.2f);
			yield return WaitHpOwner();
			yield return UpdateIndicator();
		}

		private IEnumerator UpdateIndicator()
		{
			while (true)
			{
				if (_hp == null || _healthBar == null || Math.Abs(_hp.BaseHealth) < 0.0001f)
				{
					yield return null;
				}
				if (_hp.Health <= 0f && _frame.activeInHierarchy)
				{
					_frame.SetActive(false);
					yield return null;
				}
				_currentScale = _hp.Health / _hp.BaseHealth;
				if (Math.Abs(_currentScale - _prevScale) > 0.0001f)
				{
					_frame.SetActive(true);
					_currShowTime = 2f;
					_healthBar.localScale = new Vector3(_currentScale, _healthBar.localScale.y, _healthBar.localScale.z);
				}
				_prevScale = _currentScale;
				if (_currShowTime > 0f)
				{
					_currShowTime -= Time.deltaTime;
				}
				else
				{
					_currShowTime = 0f;
					_frame.SetActive(false);
				}
				yield return null;
			}
		}

		private IEnumerator WaitHpOwner()
		{
			bool setted = false;
			while (!setted)
			{
				foreach (GameObject item in gameObject.AncestorsAndSelf())
				{
					BaseBot bot = item.GetComponent<BaseBot>();
					if (bot != null)
					{
						_hp = new HealthProvider(() => bot.health, () => bot.baseHealth);
						setted = true;
						break;
					}
				}
				foreach (GameObject item2 in gameObject.AncestorsAndSelf())
				{
					TrainingEnemy dummy = item2.GetComponent<TrainingEnemy>();
					if (dummy != null)
					{
						_hp = new HealthProvider(() => dummy.hitPoints, () => dummy.baseHitPoints);
						setted = true;
						break;
					}
				}
				yield return null;
			}
		}
	}
}
