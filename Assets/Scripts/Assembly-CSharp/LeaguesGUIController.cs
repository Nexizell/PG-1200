using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public class LeaguesGUIController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CPositionToCurrentLeagueCoroutine_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LeaguesGUIController _003C_003E4__this;

		private ProfileCup _003Cto_003E5__1;

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
		public _003CPositionToCurrentLeagueCoroutine_003Ed__13(int _003C_003E1__state)
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
				_003Cto_003E5__1 = _003C_003E4__this._cups.FirstOrDefault((ProfileCup c) => c.League == RatingSystem.instance.currentLeague);
				_003C_003E4__this._centerOnChild.CenterOn(_003Cto_003E5__1.gameObject.transform);
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E4__this.SetInfoFromLeague(_003Cto_003E5__1.League);
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
	protected internal UICenterOnChild _centerOnChild;

	[SerializeField]
	protected internal UILabel _lblLeagueName;

	[SerializeField]
	protected internal UILabel _lblLeagueNameOutline;

	[SerializeField]
	protected internal UILabel _lblScore;

	[SerializeField]
	protected internal UISprite _sprScoreBar;

	[SerializeField]
	protected internal GameObject _progressGO;

	[SerializeField]
	protected internal UILabel _progressTextLabel;

	[ReadOnly]
	[SerializeField]
	protected internal List<ProfileCup> _cups;

	[ReadOnly]
	[SerializeField]
	protected internal LeagueItemsView _itemsView;

	private ProfileCup _selectedCup;

	private readonly Dictionary<RatingSystem.RatingLeague, string> _leaguesLKeys = new Dictionary<RatingSystem.RatingLeague, string>(6, RatingSystem.RatingLeagueComparer.Instance)
	{
		{
			RatingSystem.RatingLeague.Wood,
			"Key_1953"
		},
		{
			RatingSystem.RatingLeague.Steel,
			"Key_1954"
		},
		{
			RatingSystem.RatingLeague.Gold,
			"Key_1955"
		},
		{
			RatingSystem.RatingLeague.Crystal,
			"Key_1956"
		},
		{
			RatingSystem.RatingLeague.Ruby,
			"Key_1957"
		},
		{
			RatingSystem.RatingLeague.Adamant,
			"Key_1958"
		}
	};

	private void OnEnable()
	{
		_cups = GetComponentsInChildren<ProfileCup>(true).ToList();
		_itemsView = GetComponentInChildren<LeagueItemsView>(true);
		Reposition();
	}

	private void Reposition()
	{
		_selectedCup = _cups.FirstOrDefault((ProfileCup c) => c.League == RatingSystem.instance.currentLeague);
		StartCoroutine(PositionToCurrentLeagueCoroutine());
	}

	private IEnumerator PositionToCurrentLeagueCoroutine()
	{
		yield return null;
		ProfileCup to = _cups.FirstOrDefault((ProfileCup c) => c.League == RatingSystem.instance.currentLeague);
		_centerOnChild.CenterOn(to.gameObject.transform);
		yield return null;
		SetInfoFromLeague(to.League);
	}

	public void CupCentered(ProfileCup cup)
	{
		_selectedCup = cup;
		SetInfoFromLeague(cup.League);
		_itemsView.Repaint(cup.League);
	}

	private void SetInfoFromLeague(RatingSystem.RatingLeague league)
	{
		string text = LocalizationStore.Get(_leaguesLKeys[league]);
		_lblLeagueName.text = text;
		_lblLeagueNameOutline.text = text;
		if (league < RatingSystem.instance.currentLeague)
		{
			_progressGO.SetActive(false);
			_progressTextLabel.gameObject.SetActive(true);
			_progressTextLabel.text = LocalizationStore.Get("Key_2173");
		}
		else if (league > RatingSystem.instance.currentLeague)
		{
			_progressGO.SetActive(false);
			_progressTextLabel.gameObject.SetActive(true);
			int num = RatingSystem.instance.MaxRatingInLeague(league - 1) - RatingSystem.instance.currentRating;
			_progressTextLabel.text = string.Format(LocalizationStore.Get("Key_2172"), new object[1] { num });
		}
		else if (league == (RatingSystem.RatingLeague)RiliExtensions.EnumNumbers<RatingSystem.RatingLeague>().Max())
		{
			_progressGO.SetActive(false);
			_progressTextLabel.gameObject.SetActive(true);
			_progressTextLabel.text = LocalizationStore.Get("Key_2249");
		}
		else
		{
			_progressGO.SetActive(true);
			_progressTextLabel.gameObject.SetActive(false);
			int num2 = RatingSystem.instance.MaxRatingInLeague(league);
			_lblScore.text = string.Format("{0}/{1}", new object[2]
			{
				RatingSystem.instance.currentRating,
				num2
			});
			_sprScoreBar.fillAmount = (float)RatingSystem.instance.currentRating / (float)num2;
		}
	}
}
