using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RaitingPanelController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CAnimateRaitingPanel_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public RaitingPanelController _003C_003E4__this;

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
		public _003CAnimateRaitingPanel_003Ed__12(int _003C_003E1__state)
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
				_003Ci_003E5__1 = 0;
				goto IL_00ab;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this.newRating.gameObject.SetActive(true);
				_003C_003E2__current = new WaitForSeconds(0.15f);
				_003C_003E1__state = 2;
				return true;
			case 2:
			{
				_003C_003E1__state = -1;
				int num = _003Ci_003E5__1 + 1;
				_003Ci_003E5__1 = num;
				goto IL_00ab;
			}
			case 3:
				{
					_003C_003E1__state = -1;
					_003C_003E4__this.oldRaiting.localScale = new Vector3((float)RatingSystem.instance.currentRating / (float)_003C_003E4__this.maxRating, 1f, 1f);
					return false;
				}
				IL_00ab:
				if (_003Ci_003E5__1 != 4)
				{
					_003C_003E4__this.newRating.gameObject.SetActive(false);
					_003C_003E2__current = new WaitForSeconds(0.15f);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = new WaitForSeconds(0.15f);
				_003C_003E1__state = 3;
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

	[SerializeField]
	protected internal Transform oldRaiting;

	[SerializeField]
	protected internal Transform newRating;

	[SerializeField]
	protected internal UILabel raitingLabel;

	private string raitingText;

	private int maxRating
	{
		get
		{
			return RatingSystem.instance.MaxRatingInLeague(RatingSystem.instance.currentLeague);
		}
	}

	private int league
	{
		get
		{
			return (int)RatingSystem.instance.currentLeague;
		}
	}

	private void Start()
	{
		RatingSystem.OnRatingUpdate += OnRatingUpdated;
		if (league != 5)
		{
			raitingText = string.Format("{0}/{1}", new object[2]
			{
				RatingSystem.instance.currentRating,
				maxRating
			});
			oldRaiting.localScale = new Vector3((float)RatingSystem.instance.currentRating / (float)maxRating, 1f, 1f);
			newRating.localScale = new Vector3((float)RatingSystem.instance.currentRating / (float)maxRating, 1f, 1f);
		}
		else
		{
			raitingText = RatingSystem.instance.currentRating.ToString();
			oldRaiting.localScale = Vector3.one;
		}
		raitingLabel.text = raitingText;
	}

	private void OnEnable()
	{
		if (league != 5)
		{
			raitingText = string.Format("{0}/{1}", new object[2]
			{
				RatingSystem.instance.currentRating,
				maxRating
			});
			oldRaiting.localScale = new Vector3((float)RatingSystem.instance.currentRating / (float)maxRating, 1f, 1f);
			newRating.localScale = new Vector3((float)RatingSystem.instance.currentRating / (float)maxRating, 1f, 1f);
		}
		else
		{
			raitingText = RatingSystem.instance.currentRating.ToString();
			oldRaiting.localScale = Vector3.one;
		}
		raitingLabel.text = raitingText;
	}

	private void OnRatingUpdated()
	{
		if (league != 5)
		{
			raitingText = string.Format("{0}/{1}", new object[2]
			{
				RatingSystem.instance.currentRating,
				maxRating
			});
			newRating.localScale = new Vector3((float)RatingSystem.instance.currentRating / (float)maxRating, 1f, 1f);
			CoroutineRunner.Instance.StartCoroutine(AnimateRaitingPanel());
		}
		else
		{
			raitingText = RatingSystem.instance.currentRating.ToString();
			oldRaiting.localScale = Vector3.one;
		}
		raitingLabel.text = raitingText;
	}

	private void OnDestroy()
	{
		RatingSystem.OnRatingUpdate -= OnRatingUpdated;
	}

	private IEnumerator AnimateRaitingPanel()
	{
		int i = 0;
		while (i != 4)
		{
			newRating.gameObject.SetActive(false);
			yield return new WaitForSeconds(0.15f);
			newRating.gameObject.SetActive(true);
			yield return new WaitForSeconds(0.15f);
			int num = i + 1;
			i = num;
		}
		yield return new WaitForSeconds(0.15f);
		oldRaiting.localScale = new Vector3((float)RatingSystem.instance.currentRating / (float)maxRating, 1f, 1f);
	}
}
