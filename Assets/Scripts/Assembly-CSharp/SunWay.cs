using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SunWay : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CGetDistance_003Ed__11 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public SunWay _003C_003E4__this;

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
		public _003CGetDistance_003Ed__11(int _003C_003E1__state)
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
				_003C_003E2__current = new WaitForSeconds(1f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if ((bool)NickLabelController.currentCamera)
				{
					_003C_003E4__this.startDistance = Vector2.Distance(new Vector2(NickLabelController.currentCamera.transform.position.x, NickLabelController.currentCamera.transform.position.z), new Vector2(_003C_003E4__this.transform.position.x, _003C_003E4__this.transform.position.z));
					_003C_003E4__this.startScale = _003C_003E4__this.transform.GetChild(0).localScale.y;
				}
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

	public float waterLevel;

	public Transform sun;

	private Vector3 directionLoolAt;

	private float distance;

	private float startDistance;

	private float startScale;

	private float startScaleX;

	public float multiplier = 1f;

	private float alpha = 1f;

	private void Start()
	{
		StartCoroutine(GetDistance());
		startScaleX = base.transform.GetChild(0).localScale.x;
	}

	private void Update()
	{
		if ((bool)NickLabelController.currentCamera && startDistance > 0f)
		{
			distance = Vector2.Distance(new Vector2(NickLabelController.currentCamera.transform.position.x, NickLabelController.currentCamera.transform.position.z), new Vector2(base.transform.position.x, base.transform.position.z));
			base.transform.position = new Vector3(sun.position.x, waterLevel, sun.position.z);
			Vector3 worldPosition = NickLabelController.currentCamera.transform.position + NickLabelController.currentCamera.transform.forward * -0.5f;
			worldPosition.y = base.transform.position.y;
			base.transform.LookAt(worldPosition);
			base.transform.GetChild(0).localScale = new Vector3(startScaleX * (1f + Mathf.Clamp(sun.transform.position.y / 100f, 0f, 0.3f)), startScale * Mathf.Pow(distance / startDistance, multiplier), base.transform.GetChild(0).localScale.z);
			if (sun.position.y + 120f > waterLevel)
			{
				base.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 1f - Mathf.Clamp01((waterLevel + NickLabelController.currentCamera.transform.position.y + sun.transform.position.y / 100f) / (20f + waterLevel)));
				alpha = 1f;
			}
			else
			{
				alpha -= Time.deltaTime;
				base.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, Mathf.Clamp01(alpha));
			}
		}
	}

	private IEnumerator GetDistance()
	{
		yield return new WaitForSeconds(1f);
		if ((bool)NickLabelController.currentCamera)
		{
			startDistance = Vector2.Distance(new Vector2(NickLabelController.currentCamera.transform.position.x, NickLabelController.currentCamera.transform.position.z), new Vector2(transform.position.x, transform.position.z));
			startScale = transform.GetChild(0).localScale.y;
		}
	}
}
