using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentLinkMover : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CManage_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public AgentLinkMover _003C_003E4__this;

		private NavMeshAgent _003Cagent_003E5__1;

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
		public _003CManage_003Ed__5(int _003C_003E1__state)
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
				_003Cagent_003E5__1 = _003C_003E4__this.GetComponent<NavMeshAgent>();
				_003Cagent_003E5__1.autoTraverseOffMeshLink = false;
				goto IL_0047;
			case 1:
				_003C_003E1__state = -1;
				goto IL_014a;
			case 2:
				_003C_003E1__state = -1;
				goto IL_014a;
			case 3:
				_003C_003E1__state = -1;
				goto IL_014a;
			case 4:
				{
					_003C_003E1__state = -1;
					goto IL_0047;
				}
				IL_0047:
				if (!_003Cagent_003E5__1.isOnOffMeshLink)
				{
					break;
				}
				if (_003C_003E4__this._moveMethod == OffMeshLinkMoveMethod.NormalSpeed)
				{
					_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.NormalSpeed(_003Cagent_003E5__1));
					_003C_003E1__state = 1;
					return true;
				}
				if (_003C_003E4__this._moveMethod == OffMeshLinkMoveMethod.Parabola)
				{
					_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.Parabola(_003Cagent_003E5__1, _003C_003E4__this._parabolaHeight, _003C_003E4__this._duration));
					_003C_003E1__state = 2;
					return true;
				}
				if (_003C_003E4__this._moveMethod == OffMeshLinkMoveMethod.Curve)
				{
					_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.Curve(_003Cagent_003E5__1, _003C_003E4__this._duration, _003C_003E4__this._curve));
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_014a;
				IL_014a:
				_003Cagent_003E5__1.CompleteOffMeshLink();
				break;
			}
			_003C_003E2__current = null;
			_003C_003E1__state = 4;
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
	internal sealed class _003CNormalSpeed_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public NavMeshAgent agent;

		private Vector3 _003CendPos_003E5__1;

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
		public _003CNormalSpeed_003Ed__6(int _003C_003E1__state)
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
				_003CendPos_003E5__1 = agent.currentOffMeshLinkData.endPos + Vector3.up * agent.baseOffset;
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (agent.transform.position != _003CendPos_003E5__1)
			{
				agent.transform.position = Vector3.MoveTowards(agent.transform.position, _003CendPos_003E5__1, agent.speed * Time.deltaTime);
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

	[CompilerGenerated]
	internal sealed class _003CParabola_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public NavMeshAgent agent;

		public float height;

		private float _003CnormalizedTime_003E5__1;

		private Vector3 _003CstartPos_003E5__2;

		private Vector3 _003CendPos_003E5__3;

		public float duration;

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
		public _003CParabola_003Ed__7(int _003C_003E1__state)
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
				OffMeshLinkData currentOffMeshLinkData = agent.currentOffMeshLinkData;
				_003CstartPos_003E5__2 = agent.transform.position;
				_003CendPos_003E5__3 = currentOffMeshLinkData.endPos + Vector3.up * agent.baseOffset;
				_003CnormalizedTime_003E5__1 = 0f;
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (_003CnormalizedTime_003E5__1 < 1f)
			{
				float num = height * 4f * (_003CnormalizedTime_003E5__1 - _003CnormalizedTime_003E5__1 * _003CnormalizedTime_003E5__1);
				agent.transform.position = Vector3.Lerp(_003CstartPos_003E5__2, _003CendPos_003E5__3, _003CnormalizedTime_003E5__1) + num * Vector3.up;
				_003CnormalizedTime_003E5__1 += Time.deltaTime / duration;
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

	[CompilerGenerated]
	internal sealed class _003CCurve_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public NavMeshAgent agent;

		public AnimationCurve curve;

		private float _003CnormalizedTime_003E5__1;

		private Vector3 _003CstartPos_003E5__2;

		private Vector3 _003CendPos_003E5__3;

		public float duration;

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
		public _003CCurve_003Ed__8(int _003C_003E1__state)
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
				OffMeshLinkData currentOffMeshLinkData = agent.currentOffMeshLinkData;
				_003CstartPos_003E5__2 = agent.transform.position;
				_003CendPos_003E5__3 = currentOffMeshLinkData.endPos + Vector3.up * agent.baseOffset;
				_003CnormalizedTime_003E5__1 = 0f;
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (_003CnormalizedTime_003E5__1 < 1f)
			{
				float num = curve.Evaluate(_003CnormalizedTime_003E5__1);
				agent.transform.position = Vector3.Lerp(_003CstartPos_003E5__2, _003CendPos_003E5__3, _003CnormalizedTime_003E5__1) + num * Vector3.up;
				_003CnormalizedTime_003E5__1 += Time.deltaTime / duration;
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
	protected internal OffMeshLinkMoveMethod _moveMethod = OffMeshLinkMoveMethod.Parabola;

	[SerializeField]
	protected internal float _duration = 0.5f;

	[Header("special parameters")]
	[SerializeField]
	protected internal float _parabolaHeight = 2f;

	[SerializeField]
	protected internal AnimationCurve _curve = new AnimationCurve();

	private void OnEnable()
	{
		StartCoroutine(Manage());
	}

	private IEnumerator Manage()
	{
		NavMeshAgent agent = GetComponent<NavMeshAgent>();
		agent.autoTraverseOffMeshLink = false;
		while (true)
		{
			if (agent.isOnOffMeshLink)
			{
				if (_moveMethod == OffMeshLinkMoveMethod.NormalSpeed)
				{
					yield return StartCoroutine(NormalSpeed(agent));
				}
				else if (_moveMethod == OffMeshLinkMoveMethod.Parabola)
				{
					yield return StartCoroutine(Parabola(agent, _parabolaHeight, _duration));
				}
				else if (_moveMethod == OffMeshLinkMoveMethod.Curve)
				{
					yield return StartCoroutine(Curve(agent, _duration, _curve));
				}
				agent.CompleteOffMeshLink();
			}
			yield return null;
		}
	}

	private IEnumerator NormalSpeed(NavMeshAgent agent)
	{
		Vector3 endPos = agent.currentOffMeshLinkData.endPos + Vector3.up * agent.baseOffset;
		while (agent.transform.position != endPos)
		{
			agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);
			yield return null;
		}
	}

	private IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
	{
		OffMeshLinkData currentOffMeshLinkData = agent.currentOffMeshLinkData;
		Vector3 startPos = agent.transform.position;
		Vector3 endPos = currentOffMeshLinkData.endPos + Vector3.up * agent.baseOffset;
		float normalizedTime = 0f;
		while (normalizedTime < 1f)
		{
			float num = height * 4f * (normalizedTime - normalizedTime * normalizedTime);
			agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + num * Vector3.up;
			normalizedTime += Time.deltaTime / duration;
			yield return null;
		}
	}

	private IEnumerator Curve(NavMeshAgent agent, float duration, AnimationCurve curve)
	{
		OffMeshLinkData currentOffMeshLinkData = agent.currentOffMeshLinkData;
		Vector3 startPos = agent.transform.position;
		Vector3 endPos = currentOffMeshLinkData.endPos + Vector3.up * agent.baseOffset;
		float normalizedTime = 0f;
		while (normalizedTime < 1f)
		{
			float num = curve.Evaluate(normalizedTime);
			agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + num * Vector3.up;
			normalizedTime += Time.deltaTime / duration;
			yield return null;
		}
	}
}
