using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Facebook.Unity;
using Rilisoft;
using UnityEngine;

public sealed class NetworkStartTableNGUIController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CMatchFinishedInterface_003Ed__142 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public NetworkStartTableNGUIController _003C_003E4__this;

		public int blueTotal;

		public int redTotal;

		public bool showAward;

		public RatingSystem.RatingChange ratingChange;

		public int _addCoin;

		public int _addExpierence;

		public bool firstPlace;

		public int _winnerCommand;

		public bool iAmWinnerInTeam;

		public string _winner;

		public bool _isCustom;

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
		public _003CMatchFinishedInterface_003Ed__142(int _003C_003E1__state)
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
				for (int i = 0; i < _003C_003E4__this.totalBlue.Length; i++)
				{
					_003C_003E4__this.totalBlue[i].text = blueTotal.ToString();
				}
				for (int j = 0; j < _003C_003E4__this.totalRed.Length; j++)
				{
					_003C_003E4__this.totalRed[j].text = redTotal.ToString();
				}
				_003C_003E4__this.ranksTable.totalBlue = blueTotal;
				_003C_003E4__this.ranksTable.totalRed = redTotal;
				bool flag = GameConnect.gameMode == GameConnect.GameMode.TeamFight || GameConnect.gameMode == GameConnect.GameMode.FlagCapture || GameConnect.gameMode == GameConnect.GameMode.CapturePoints;
				_003C_003E4__this.interfaceAnimator.ResetTrigger("RewardTaken");
				_003C_003E4__this.interfaceAnimator.ResetTrigger("GetReward");
				_003C_003E4__this.interfaceAnimator.SetBool("IsTwoTeams", flag);
				_003C_003E4__this.interfaceAnimator.SetBool("isRewarded", showAward || ratingChange.addRating != 0);
				_003C_003E4__this.interfaceAnimator.SetBool("isExpOnly", _addCoin == 0 || (ExperienceController.sharedController.currentLevel == 36 && _addCoin > 0));
				_003C_003E4__this.interfaceAnimator.SetBool("isHunger", GameConnect.isMiniGame);
				_003C_003E4__this.interfaceAnimator.SetBool("isDater", GameConnect.isDaterRegim);
				_003C_003E4__this.interfaceAnimator.SetBool("IsTrophyUp", ratingChange.isUp);
				_003C_003E4__this.interfaceAnimator.SetBool("IsTrophyDown", ratingChange.isDown);
				_003C_003E4__this.interfaceAnimator.SetBool("IsTrophyAdd", ratingChange.addRating != 0 && !ratingChange.isUp && !ratingChange.isDown);
				_003C_003E4__this.interfaceAnimator.SetBool("NoRaiting", ratingChange.addRating == 0);
				_003C_003E4__this.interfaceAnimator.SetBool("isTrophyOnly", ratingChange.addRating != 0 && _addCoin == 0 && _addExpierence == 0);
				_003C_003E4__this.interfaceAnimator.SetBool("isTrophyAdded", false);
				_003C_003E4__this.interfaceAnimator.SetBool("isRatingSystem", true);
				_003C_003E4__this.interfaceAnimator.SetBool("interfaceAnimationDone", false);
				_003C_003E4__this.isUsed = false;
				_003C_003E4__this.trophiAddIcon.SetActive(ratingChange.addRating > 0);
				_003C_003E4__this.trophiMinusIcon.SetActive(ratingChange.addRating < 0);
				_003C_003E4__this.trophyPanel.SetActive(ratingChange.addRating != 0);
				_003C_003E4__this.currentCup.spriteName = ratingChange.oldLeague.ToString() + " 1";
				_003C_003E4__this.NewCup.spriteName = ratingChange.newLeague.ToString() + " 1";
				if (ratingChange.addRating > 0)
				{
					_003C_003E4__this.currentBarFillAmount = ratingChange.oldRatingAmount;
					_003C_003E4__this.nextBarFillAmount = ratingChange.newRatingAmount;
					_003C_003E4__this.currentBar.fillAmount = _003C_003E4__this.currentBarFillAmount;
					_003C_003E4__this.nextBar.fillAmount = _003C_003E4__this.currentBarFillAmount;
					_003C_003E4__this.nextBar.color = Color.yellow;
				}
				else
				{
					_003C_003E4__this.currentBarFillAmount = ratingChange.oldRatingAmount;
					_003C_003E4__this.nextBarFillAmount = ratingChange.newRatingAmount;
					_003C_003E4__this.currentBar.fillAmount = _003C_003E4__this.nextBarFillAmount;
					_003C_003E4__this.nextBar.fillAmount = _003C_003E4__this.nextBarFillAmount;
					_003C_003E4__this.nextBar.color = Color.red;
				}
				_003C_003E4__this.leagueUp = ratingChange.newLeague > ratingChange.oldLeague;
				if (ratingChange.maxRating == int.MaxValue)
				{
					_003C_003E4__this.trophyPoints.text = ratingChange.newRating.ToString();
				}
				else
				{
					_003C_003E4__this.trophyPoints.text = ratingChange.newRating + "/" + ratingChange.maxRating;
				}
				_003C_003E4__this.trophyShine.SetActive(ratingChange.isUp);
				_003C_003E4__this.trophyRewardValue = ratingChange.addRating;
				string text = string.Format((ratingChange.addRating > 0) ? "+{0}" : "{0}", new object[1] { _003C_003E4__this.trophyRewardValue });
				UILabel[] rewardTrophy = _003C_003E4__this.rewardTrophy;
				foreach (UILabel obj in rewardTrophy)
				{
					obj.gameObject.SetActive(ratingChange.addRating != 0);
					obj.text = text;
				}
				string text2 = string.Format(LocalizationStore.Get(RatingSystem.leagueChangeLocalizations[(int)ratingChange.newLeague]), new object[1] { "" });
				rewardTrophy = _003C_003E4__this.textLeagueUp;
				for (int k = 0; k < rewardTrophy.Length; k++)
				{
					rewardTrophy[k].text = text2;
				}
				rewardTrophy = _003C_003E4__this.textLeagueDown;
				for (int k = 0; k < rewardTrophy.Length; k++)
				{
					rewardTrophy[k].text = text2;
				}
				_003C_003E4__this.shareToggle.value = (firstPlace & showAward) && (FB.IsLoggedIn || TwitterController.IsLoggedIn);
				_003C_003E4__this.shareToggle.gameObject.SetActive(_003C_003E4__this.shareToggle.value);
				if (_003C_003E4__this.defaultTeamOneState == Vector3.zero)
				{
					_003C_003E4__this.defaultTeamOneState = _003C_003E4__this.teamOneLabel.transform.localPosition;
				}
				if (_003C_003E4__this.defaultTeamTwoState == Vector3.zero)
				{
					_003C_003E4__this.defaultTeamTwoState = _003C_003E4__this.teamTwoLabel.transform.localPosition;
				}
				if (!flag || _winnerCommand == 0)
				{
					_003C_003E4__this.teamOneLabel.transform.localPosition = _003C_003E4__this.defaultTeamOneState;
					_003C_003E4__this.teamTwoLabel.transform.localPosition = _003C_003E4__this.defaultTeamTwoState;
				}
				else
				{
					_003C_003E4__this.teamOneLabel.transform.localPosition = _003C_003E4__this.defaultTeamOneState + Vector3.right * 55f;
					_003C_003E4__this.teamTwoLabel.transform.localPosition = _003C_003E4__this.defaultTeamTwoState + Vector3.left * 55f;
				}
				if (GameConnect.isMiniGame)
				{
					if (GameConnect.isHunger)
					{
						_003C_003E4__this.EndSpectatorMode();
					}
					_003C_003E4__this.HideTable();
				}
				if (WeaponManager.sharedManager.myPlayerMoveC != null)
				{
					_003C_003E4__this.FreezePlayer();
					_003C_003E4__this.interfaceAnimator.SetTrigger("MatchEnd");
					_003C_003E4__this.BankShopGUI.InteractionEnabled = false;
					_003C_003E4__this.BankShopGUI.UpdateView();
					_003C_003E4__this.ShowFinishedInterface(iAmWinnerInTeam, (GameConnect.isCompany || GameConnect.isFlag || GameConnect.isCapturePoints) && _winnerCommand == 0);
					_003C_003E4__this.waitForAnimationDone = true;
					goto IL_08a6;
				}
				goto IL_08b3;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_08a6;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_08a6:
				if (_003C_003E4__this.waitForAnimationDone)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_08b3;
				IL_08b3:
				_003C_003E4__this.rewardCoinsObject.gameObject.SetActive(false);
				_003C_003E4__this.rewardExpObject.gameObject.SetActive(false);
				if (showAward || ratingChange.addRating != 0)
				{
					_003C_003E4__this.interfaceAnimator.SetTrigger("Reward");
					if (ExperienceController.sharedController.currentLevel == 36 && _addCoin > 0)
					{
						_003C_003E4__this.expRewardValue = 0;
					}
					_003C_003E4__this.expRewardValue = _addExpierence;
					_003C_003E4__this.coinsRewardValue = _addCoin;
				}
				else
				{
					_003C_003E4__this.expRewardValue = 0;
					_003C_003E4__this.coinsRewardValue = 0;
				}
				_003C_003E4__this.isRewardShow = showAward || ratingChange.addRating != 0;
				if (GameConnect.isDaterRegim)
				{
					for (int l = 0; l < _003C_003E4__this.finishedInterfaceLabels.Length; l++)
					{
						_003C_003E4__this.finishedInterfaceLabels[l].text = _winner;
					}
				}
				ExperienceController.sharedController.isShowRanks = true;
				WeaponManager.sharedManager.myNetworkStartTable.DestroyPlayer();
				if (showAward)
				{
					_003C_003E4__this.ShowAwardEndInterface(_winner, _addCoin, _addExpierence, _isCustom, firstPlace, _winnerCommand);
				}
				else
				{
					_003C_003E4__this.ShowEndInterface(_winner, _winnerCommand);
				}
				if (GameConnect.isMiniGame)
				{
					_003C_003E4__this.backButtonInHunger.SetActive(false);
					_003C_003E4__this.randomBtn.SetActive(false);
					_003C_003E4__this.MapSelectPanel.SetActive(false);
					_003C_003E4__this.questsButton.SetActive(false);
					_003C_003E4__this.spectatorModeBtnPnl.SetActive(false);
				}
				if (_003C_003E4__this.leagueUp)
				{
					_003C_003E4__this.rewardButton.SetActive(true);
					string newLeagueEggID = _003C_003E4__this.GiveLeagueEggAndReturnId();
					List<Texture> leagueItems = _003C_003E4__this.GetLeagueItems(newLeagueEggID);
					for (int m = 0; m < _003C_003E4__this.trophyItems.Length; m++)
					{
						if (m >= leagueItems.Count)
						{
							_003C_003E4__this.trophyItems[m].gameObject.SetActive(false);
							continue;
						}
						_003C_003E4__this.trophyItems[m].gameObject.SetActive(true);
						_003C_003E4__this.trophyItems[m].mainTexture = leagueItems[m];
					}
					_003C_003E4__this.rewardPanel.SetActive(_003C_003E4__this.trophyItems[0].gameObject.activeSelf);
					_003C_003E4__this.labelNewItems.SetActive(_003C_003E4__this.trophyItems[0].gameObject.activeSelf);
					_003C_003E4__this.rewardFrame.ResizeFrame();
					_003C_003E4__this.Invoke("OnTrophyOkButtonPress", 60f);
				}
				else
				{
					_003C_003E4__this.rewardButton.SetActive(false);
					_003C_003E4__this.rewardPanel.SetActive(false);
					_003C_003E4__this.labelNewItems.SetActive(false);
				}
				_003C_003E4__this.waitForTrophyAnimationDone = ratingChange.addRating != 0;
				break;
			}
			if (_003C_003E4__this.waitForTrophyAnimationDone)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			}
			if (GameConnect.isMiniGame)
			{
				_003C_003E4__this.backButtonInHunger.SetActive(true);
				_003C_003E4__this.randomBtn.SetActive(true);
				if (GameConnect.isHunger)
				{
					_003C_003E4__this.spectatorModeBtnPnl.SetActive(true);
				}
			}
			for (int n = 0; n < _003C_003E4__this.trophyItems.Length; n++)
			{
				_003C_003E4__this.trophyItems[n].gameObject.SetActive(false);
			}
			if (showAward)
			{
				_003C_003E4__this.rewardExpObject.color = Color.white;
				_003C_003E4__this.rewardCoinsObject.color = Color.white;
				bool needDoubleReward = DoubleReward.Instance.NeedDoubleReward(GameConnect.gameMode) && _addCoin > 0;
				DoubleRewardNetworkStartTable componentInChildren = _003C_003E4__this.GetComponentInChildren<DoubleRewardNetworkStartTable>(true);
				if (componentInChildren != null)
				{
					componentInChildren.RefreshDoubleRewardLabel(needDoubleReward);
				}
				_003C_003E4__this.rewardCoinsObject.gameObject.SetActive(_addCoin > 0);
				_003C_003E4__this.rewardExpObject.gameObject.SetActive(ExperienceController.sharedController.currentLevel < 36);
				_003C_003E4__this.rewardPanel.SetActive(_003C_003E4__this.rewardCoinsObject.gameObject.activeSelf || _003C_003E4__this.rewardExpObject.gameObject.activeSelf);
				_003C_003E4__this.rewardFrame.ResizeFrame();
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
	internal sealed class _003CMatchFinishedInDuelInterface_003Ed__150 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public NetworkStartTableNGUIController _003C_003E4__this;

		public bool showAward;

		public RatingSystem.RatingChange ratingChange;

		public int _addCoin;

		public int _addExpierence;

		public bool firstPlace;

		public bool deadheat;

		public bool _isCustom;

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
		public _003CMatchFinishedInDuelInterface_003Ed__150(int _003C_003E1__state)
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
				_003C_003E4__this.interfaceAnimator.ResetTrigger("RewardTaken");
				_003C_003E4__this.interfaceAnimator.ResetTrigger("GetReward");
				_003C_003E4__this.interfaceAnimator.ResetTrigger("AnimationEnds");
				_003C_003E4__this.interfaceAnimator.SetBool("isRewarded", showAward || ratingChange.addRating != 0);
				_003C_003E4__this.interfaceAnimator.SetBool("isExpOnly", _addCoin == 0 || (ExperienceController.sharedController.currentLevel == 36 && _addCoin > 0));
				_003C_003E4__this.interfaceAnimator.SetBool("IsTrophyUp", ratingChange.isUp);
				_003C_003E4__this.interfaceAnimator.SetBool("IsTrophyDown", ratingChange.isDown);
				_003C_003E4__this.interfaceAnimator.SetBool("IsTrophyAdd", ratingChange.addRating != 0 && !ratingChange.isUp && !ratingChange.isDown);
				_003C_003E4__this.interfaceAnimator.SetBool("NoRaiting", ratingChange.addRating == 0);
				_003C_003E4__this.interfaceAnimator.SetBool("isTrophyOnly", ratingChange.addRating != 0 && _addCoin == 0 && _addExpierence == 0);
				_003C_003E4__this.interfaceAnimator.SetBool("isTrophyAdded", false);
				_003C_003E4__this.interfaceAnimator.SetBool("interfaceAnimationDone", false);
				_003C_003E4__this.isUsed = false;
				_003C_003E4__this.trophiAddIcon.SetActive(ratingChange.addRating > 0);
				_003C_003E4__this.trophiMinusIcon.SetActive(ratingChange.addRating < 0);
				_003C_003E4__this.trophyPanel.SetActive(ratingChange.addRating != 0);
				_003C_003E4__this.currentCup.spriteName = ratingChange.oldLeague.ToString() + " 1";
				_003C_003E4__this.NewCup.spriteName = ratingChange.newLeague.ToString() + " 1";
				if (ratingChange.addRating > 0)
				{
					_003C_003E4__this.currentBarFillAmount = ratingChange.oldRatingAmount;
					_003C_003E4__this.nextBarFillAmount = ratingChange.newRatingAmount;
					_003C_003E4__this.currentBar.fillAmount = _003C_003E4__this.currentBarFillAmount;
					_003C_003E4__this.nextBar.fillAmount = _003C_003E4__this.currentBarFillAmount;
					_003C_003E4__this.nextBar.color = Color.yellow;
				}
				else
				{
					_003C_003E4__this.currentBarFillAmount = ratingChange.oldRatingAmount;
					_003C_003E4__this.nextBarFillAmount = ratingChange.newRatingAmount;
					_003C_003E4__this.currentBar.fillAmount = _003C_003E4__this.nextBarFillAmount;
					_003C_003E4__this.nextBar.fillAmount = _003C_003E4__this.nextBarFillAmount;
					_003C_003E4__this.nextBar.color = Color.red;
				}
				_003C_003E4__this.leagueUp = ratingChange.newLeague > ratingChange.oldLeague;
				if (ratingChange.maxRating == int.MaxValue)
				{
					_003C_003E4__this.trophyPoints.text = ratingChange.newRating.ToString();
				}
				else
				{
					_003C_003E4__this.trophyPoints.text = ratingChange.newRating + "/" + ratingChange.maxRating;
				}
				_003C_003E4__this.trophyShine.SetActive(ratingChange.isUp);
				_003C_003E4__this.trophyRewardValue = ratingChange.addRating;
				string text = string.Format((ratingChange.addRating > 0) ? "+{0}" : "{0}", new object[1] { _003C_003E4__this.trophyRewardValue });
				UILabel[] rewardTrophy = _003C_003E4__this.rewardTrophy;
				foreach (UILabel obj in rewardTrophy)
				{
					obj.gameObject.SetActive(ratingChange.addRating != 0);
					obj.text = text;
				}
				string text2 = string.Format(LocalizationStore.Get(RatingSystem.leagueChangeLocalizations[(int)ratingChange.newLeague]), new object[1] { "" });
				rewardTrophy = _003C_003E4__this.textLeagueUp;
				for (int i = 0; i < rewardTrophy.Length; i++)
				{
					rewardTrophy[i].text = text2;
				}
				rewardTrophy = _003C_003E4__this.textLeagueDown;
				for (int i = 0; i < rewardTrophy.Length; i++)
				{
					rewardTrophy[i].text = text2;
				}
				_003C_003E4__this.shareToggle.value = false;
				if (WeaponManager.sharedManager.myPlayerMoveC != null)
				{
					_003C_003E4__this.FreezePlayer();
					_003C_003E4__this.interfaceAnimator.SetTrigger("MatchEnd");
					_003C_003E4__this.BankShopGUI.InteractionEnabled = false;
					_003C_003E4__this.BankShopGUI.UpdateView();
					_003C_003E4__this.ShowFinishedInterface(firstPlace, deadheat);
					_003C_003E4__this.waitForAnimationDone = true;
					goto IL_05e8;
				}
				goto IL_05f5;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_05e8;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_05f5:
				_003C_003E4__this.rewardCoinsObject.gameObject.SetActive(false);
				_003C_003E4__this.rewardExpObject.gameObject.SetActive(false);
				if (showAward || ratingChange.addRating != 0)
				{
					_003C_003E4__this.interfaceAnimator.SetTrigger("Reward");
					if (ExperienceController.sharedController.currentLevel == 36 && _addCoin > 0)
					{
						_003C_003E4__this.expRewardValue = 0;
					}
					_003C_003E4__this.expRewardValue = _addExpierence;
					_003C_003E4__this.coinsRewardValue = _addCoin;
				}
				else
				{
					_003C_003E4__this.expRewardValue = 0;
					_003C_003E4__this.coinsRewardValue = 0;
				}
				_003C_003E4__this.isRewardShow = showAward || ratingChange.addRating != 0;
				ExperienceController.sharedController.isShowRanks = true;
				WeaponManager.sharedManager.myNetworkStartTable.DestroyPlayer();
				if (showAward)
				{
					_003C_003E4__this.ShowAwardEndInterface("", _addCoin, _addExpierence, _isCustom, firstPlace, 0);
				}
				else
				{
					_003C_003E4__this.ShowEndInterface("", 0);
				}
				_003C_003E4__this.duelUI.ShowFinishedInterface(ratingChange, showAward, _addCoin, _addExpierence, firstPlace, deadheat);
				if (_003C_003E4__this.leagueUp)
				{
					_003C_003E4__this.rewardButton.SetActive(true);
					string newLeagueEggID = _003C_003E4__this.GiveLeagueEggAndReturnId();
					List<Texture> leagueItems = _003C_003E4__this.GetLeagueItems(newLeagueEggID);
					for (int j = 0; j < _003C_003E4__this.trophyItems.Length; j++)
					{
						if (j >= leagueItems.Count)
						{
							_003C_003E4__this.trophyItems[j].gameObject.SetActive(false);
							continue;
						}
						_003C_003E4__this.trophyItems[j].gameObject.SetActive(true);
						_003C_003E4__this.trophyItems[j].mainTexture = leagueItems[j];
					}
					_003C_003E4__this.rewardPanel.SetActive(_003C_003E4__this.trophyItems[0].gameObject.activeSelf);
					_003C_003E4__this.labelNewItems.SetActive(_003C_003E4__this.trophyItems[0].gameObject.activeSelf);
					_003C_003E4__this.rewardFrame.ResizeFrame();
					_003C_003E4__this.Invoke("OnTrophyOkButtonPress", 60f);
				}
				else
				{
					_003C_003E4__this.rewardButton.SetActive(false);
					_003C_003E4__this.rewardPanel.SetActive(false);
					_003C_003E4__this.labelNewItems.SetActive(false);
				}
				_003C_003E4__this.waitForTrophyAnimationDone = ratingChange.addRating != 0;
				break;
				IL_05e8:
				if (_003C_003E4__this.waitForAnimationDone)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_05f5;
			}
			if (_003C_003E4__this.waitForTrophyAnimationDone)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			}
			for (int k = 0; k < _003C_003E4__this.trophyItems.Length; k++)
			{
				_003C_003E4__this.trophyItems[k].gameObject.SetActive(false);
			}
			if (showAward)
			{
				_003C_003E4__this.rewardExpObject.color = Color.white;
				_003C_003E4__this.rewardCoinsObject.color = Color.white;
				bool needDoubleReward = DoubleReward.Instance.NeedDoubleReward(GameConnect.gameMode) && _addCoin > 0;
				DoubleRewardNetworkStartTable componentInChildren = _003C_003E4__this.GetComponentInChildren<DoubleRewardNetworkStartTable>(true);
				if (componentInChildren != null)
				{
					componentInChildren.RefreshDoubleRewardLabel(needDoubleReward);
				}
				_003C_003E4__this.rewardCoinsObject.gameObject.SetActive(_addCoin > 0);
				_003C_003E4__this.rewardExpObject.gameObject.SetActive(ExperienceController.sharedController.currentLevel < 36);
				_003C_003E4__this.rewardPanel.SetActive(_003C_003E4__this.rewardCoinsObject.gameObject.activeSelf || _003C_003E4__this.rewardExpObject.gameObject.activeSelf);
				_003C_003E4__this.rewardFrame.ResizeFrame();
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
	internal sealed class _003CStartRewardAnimation_003Ed__153 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public NetworkStartTableNGUIController _003C_003E4__this;

		private Vector3 _003CexpStart_003E5__1;

		private float _003CanimTime_003E5__2;

		private bool _003CneedDoubleReward_003E5__3;

		private Vector3 _003CcoinsStart_003E5__4;

		private GameConnect.GameMode _003CcurrentMode_003E5__5;

		private DateTime? _003Cnow_003E5__6;

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
		public _003CStartRewardAnimation_003Ed__153(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int incrementExperience;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E4__this.rewardFrame.ResizeFrame();
				_003CanimTime_003E5__2 = 0f;
				goto IL_005a;
			case 1:
				_003C_003E1__state = -1;
				goto IL_005a;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0265;
			case 3:
				{
					_003C_003E1__state = -1;
					goto IL_03dd;
				}
				IL_005a:
				if (ShopNGUIController.GuiActive || (BankController.Instance != null && BankController.Instance.InterfaceEnabled))
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003CcurrentMode_003E5__5 = GameConnect.gameMode;
				_003Cnow_003E5__6 = FriendsController.GetServerTime();
				_003CneedDoubleReward_003E5__3 = _003Cnow_003E5__6.HasValue && DoubleReward.Instance.NeedDoubleReward(_003CcurrentMode_003E5__5) && _003C_003E4__this.coinsRewardValue > 0;
				if (_003CneedDoubleReward_003E5__3 && Application.isEditor)
				{
					DateTime? doubleRewardDate = DoubleReward.Instance.GetDoubleRewardDate(_003CcurrentMode_003E5__5);
					UnityEngine.Debug.LogFormat("Need double reward: {0}, {1:s}, {2}x{4}, {3}x{4}", _003CcurrentMode_003E5__5, doubleRewardDate.HasValue ? doubleRewardDate.Value : default(DateTime), _003C_003E4__this.addExperience, _003C_003E4__this.addCoins, DoubleReward.Instance.RewardFactor);
				}
				if (_003C_003E4__this.expRewardValue > 0)
				{
					if (ExperienceController.sharedController.currentLevel == 1)
					{
						DoubleReward.Instance.InitializeDoubleRewardDate(_003Cnow_003E5__6);
					}
					_003CexpStart_003E5__1 = _003C_003E4__this.rewardExpObject.transform.localPosition;
					goto IL_0265;
				}
				goto IL_02d9;
				IL_0265:
				if (_003CanimTime_003E5__2 < 1f)
				{
					_003C_003E4__this.rewardExpObject.transform.localPosition = Vector3.Lerp(_003CexpStart_003E5__1, _003C_003E4__this.rewardExpAnimPoint.localPosition, Mathf.Min(_003CanimTime_003E5__2, 1f));
					_003C_003E4__this.rewardExpObject.color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), Mathf.Min(_003CanimTime_003E5__2, 1f));
					_003CanimTime_003E5__2 += Time.deltaTime / 0.4f;
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
				incrementExperience = (_003CneedDoubleReward_003E5__3 ? (_003C_003E4__this.expRewardValue * DoubleReward.Instance.RewardFactor) : _003C_003E4__this.expRewardValue);
				ExperienceController.sharedController.AddExperience(incrementExperience);
				if (WeaponManager.sharedManager.myNetworkStartTable != null)
				{
					WeaponManager.sharedManager.myNetworkStartTable.UpdateRanks();
				}
				_003C_003E4__this.expRewardValue = 0;
				goto IL_02d9;
				IL_02d9:
				_003C_003E4__this.rewardExpObject.gameObject.SetActive(false);
				_003CanimTime_003E5__2 = 0f;
				if (_003C_003E4__this.coinsRewardValue <= 0)
				{
					break;
				}
				_003CcoinsStart_003E5__4 = _003C_003E4__this.rewardCoinsObject.transform.localPosition;
				goto IL_03dd;
				IL_03dd:
				if (_003CanimTime_003E5__2 < 1f)
				{
					_003C_003E4__this.rewardCoinsObject.transform.localPosition = Vector3.Lerp(_003CcoinsStart_003E5__4, _003C_003E4__this.rewardCoinsAnimPoint.localPosition, Mathf.Min(_003CanimTime_003E5__2, 1f));
					_003C_003E4__this.rewardCoinsObject.color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), Mathf.Min(_003CanimTime_003E5__2, 1f));
					_003CanimTime_003E5__2 += Time.deltaTime / 0.4f;
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				}
				BankController.AddCoins(_003CneedDoubleReward_003E5__3 ? (_003C_003E4__this.coinsRewardValue * DoubleReward.Instance.RewardFactor) : _003C_003E4__this.coinsRewardValue);
				_003C_003E4__this.coinsRewardValue = 0;
				break;
			}
			if (_003CneedDoubleReward_003E5__3)
			{
				DoubleReward.Instance.SaveDoubleRewardDate(_003CcurrentMode_003E5__5, _003Cnow_003E5__6);
				AnalyticsStuff.FirstWinOfTheDayGameModesCompleted(GameConnect.gameMode);
			}
			_003C_003E4__this.rewardCoinsObject.gameObject.SetActive(false);
			_003CanimTime_003E5__2 = 0f;
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
	internal sealed class _003CTrophyFillAnimation_003Ed__155 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public NetworkStartTableNGUIController _003C_003E4__this;

		private float _003CanimTime_003E5__1;

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
		public _003CTrophyFillAnimation_003Ed__155(int _003C_003E1__state)
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
			}
			else
			{
				_003C_003E1__state = -1;
				_003CanimTime_003E5__1 = 0f;
				if (_003C_003E4__this.trophyRewardValue == 0)
				{
					goto IL_00a5;
				}
			}
			if (_003CanimTime_003E5__1 < 1f)
			{
				_003C_003E4__this.nextBar.fillAmount = Mathf.Lerp(_003C_003E4__this.currentBarFillAmount, _003C_003E4__this.nextBarFillAmount, Mathf.Min(_003CanimTime_003E5__1, 1f));
				_003CanimTime_003E5__1 += Time.deltaTime;
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			goto IL_00a5;
			IL_00a5:
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
	internal sealed class _003CWaitAndRemoveInterfaceOnReconnect_003Ed__184 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

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
		public _003CWaitAndRemoveInterfaceOnReconnect_003Ed__184(int _003C_003E1__state)
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
				UnityEngine.Object.Destroy(sharedController.gameObject);
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
	internal sealed class _003CTryToShowExpiredBanner_003Ed__185 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public NetworkStartTableNGUIController _003C_003E4__this;

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
		public _003CTryToShowExpiredBanner_003Ed__185(int _003C_003E1__state)
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
				goto IL_003b;
			case 1:
				_003C_003E1__state = -1;
				goto IL_003b;
			case 2:
				{
					_003C_003E1__state = -1;
					try
					{
						if (ShopNGUIController.GuiActive || (BankController.Instance != null && BankController.Instance.InterfaceEnabled) || (ExpController.Instance != null && ExpController.Instance.WaitingForLevelUpView) || (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown) || _003C_003E4__this.waitForAnimationDone || _003C_003E4__this.waitForTrophyAnimationDone || ExchangeWindow.IsOpened || _003C_003E4__this.rentScreenPoint.childCount != 0)
						{
							break;
						}
						if (BuffSystem.instance != null && BuffSystem.instance.haveAllInteractons && Storager.getInt("Training.ShouldRemoveNoviceArmorInShopKey") == 1)
						{
							GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("NguiWindows/WeRemoveNoviceArmorBanner"));
							gameObject.transform.parent = _003C_003E4__this.rentScreenPoint;
							Player_move_c.SetLayerRecursively(gameObject, LayerMask.NameToLayer("NGUITable"));
							gameObject.transform.localPosition = new Vector3(0f, 0f, -130f);
							gameObject.transform.localRotation = Quaternion.identity;
							gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
							_003C_003E4__this.GetComponent<HintController>().HideHintByName("shop_remove_novice_armor");
							try
							{
								ShopNGUIController.EquipWearInCategoryIfNotEquiped("Armor_Army_1", ShopNGUIController.CategoryNames.ArmorCategory, false);
							}
							catch (Exception ex)
							{
								UnityEngine.Debug.LogError("Exception in NetworkStartTableNguiController: ShopNGUIController.EquipWearInCategoryIfNotEquiped: " + ex);
							}
						}
						else
						{
							ShopNGUIController.ShowTryGunIfPossible(_003C_003E4__this.startInterfacePanel.activeSelf, _003C_003E4__this.rentScreenPoint, "NGUITable");
						}
					}
					catch (Exception ex2)
					{
						UnityEngine.Debug.LogWarning("exception in NetworkTableNGUI  TryToShowExpiredBanner: " + ex2);
					}
					break;
				}
				IL_003b:
				if (FriendsController.sharedController == null || TempItemsController.sharedController == null)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				break;
			}
			_003C_003E2__current = _003C_003E4__this.StartCoroutine(FriendsController.sharedController.MyWaitForSeconds(1f));
			_003C_003E1__state = 2;
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

	public static NetworkStartTableNGUIController sharedController;

	public GameObject miniGamesNextMatchPriceContainer;

	public UILabel[] miniGamesNextMatchPriceLabels;

	public Transform freeTicketsTimerPoint;

	public GameObject allInterfaceContainer;

	public GameObject facebookButton;

	public GameObject twitterButton;

	public Transform rentScreenPoint;

	public GameObject ranksInterface;

	public RanksTable ranksTable;

	public GameObject shopAnchor;

	public GameObject finishedInterface;

	public UILabel[] finishedInterfaceLabels;

	public GameObject startInterfacePanel;

	public GameObject winnerPanelCom1;

	public GameObject winnerPanelCom2;

	public GameObject endInterfacePanel;

	public Animator interfaceAnimator;

	public GameObject allInterfacePanel;

	public GameObject randomBtn;

	public GameObject socialPnl;

	public GameObject spectratorModePnl;

	public GameObject spectatorModeBtnPnl;

	public GameObject spectatorModeOnBtn;

	public GameObject spectatorModeOffBtn;

	public GameObject MapSelectPanel;

	public string winner;

	public int winnerCommand;

	public UILabel HungerStartLabel;

	private int addCoins;

	private int addExperience;

	private bool isCancelHideAvardPanel;

	private bool updateRealTableAfterActionPanel = true;

	public GameObject SexualButton;

	public GameObject InAppropriateActButton;

	public GameObject OtherButton;

	public GameObject ReasonsPanel;

	public GameObject ActionPanel;

	public GameObject AddButton;

	public GameObject ReportButton;

	public GameObject questsButton;

	public GameObject hideOldRanksButton;

	public GameObject rewardButton;

	public GameObject labelNewItems;

	public BankShopViewGuiElement BankShopGUI;

	public UILabel[] actionPanelNicklabel;

	public GameObject trophiAddIcon;

	public GameObject trophiMinusIcon;

	public string pixelbookID;

	public string nick;

	public GoMapInEndGame[] goMapInEndGameButtons = new GoMapInEndGame[3];

	public GoMapInEndGame[] goMapInEndGameButtonsDuel = new GoMapInEndGame[2];

	public int CountAddFriens;

	public UILabel[] totalBlue;

	public UILabel[] totalRed;

	private GameObject cameraObj;

	public GameObject changeMapLabel;

	public GameObject rewardPanel;

	public GameObject listOfPlayers;

	public GameObject backButtonInHunger;

	public GameObject goBattleLabel;

	public GameObject daterButtonLabel;

	public UITexture rewardCoinsObject;

	public UITexture rewardExpObject;

	public UISprite rewardTrophysObject;

	public UITexture[] trophyItems;

	public UISprite currentCup;

	public UISprite NewCup;

	public GameObject trophyPanel;

	public GameObject trophyShine;

	public UISprite currentBar;

	public UISprite nextBar;

	public UILabel trophyPoints;

	public Transform rewardCoinsAnimPoint;

	public Transform rewardExpAnimPoint;

	public UILabel[] rewardCoins;

	public UILabel[] rewardExperience;

	public UILabel[] gameModeLabel;

	public UILabel[] rewardTrophy;

	public GameObject[] finishWin;

	public GameObject[] finishDefeat;

	public GameObject[] finishDraw;

	public UILabel teamOneLabel;

	public UILabel teamTwoLabel;

	private Vector3 defaultTeamOneState;

	private Vector3 defaultTeamTwoState;

	public UIToggle shareToggle;

	public UILabel[] textLeagueUp;

	public UILabel[] textLeagueDown;

	public GameObject duelPanel;

	[HideInInspector]
	public DuelUIController duelUI;

	public FrameResizer rewardFrame;

	public bool isRewardShow;

	public RuntimeAnimatorController standartAnimController;

	public RuntimeAnimatorController DuelAnimController;

	private readonly Lazy<string> _versionString = new Lazy<string>(() => "12.0.0");

	private IDisposable _backSubscription;

	private bool waitForAnimationDone;

	private bool leagueUp;

	private int expRewardValue;

	private int coinsRewardValue;

	private int trophyRewardValue;

	private float currentBarFillAmount;

	private float nextBarFillAmount;

	private bool isUsed;

	private bool waitForTrophyAnimationDone;

	private bool oldRanksIsActive;

	private FacebookController.StoryPriority _facebookPriority;

	private FacebookController.StoryPriority _twiiterPriority;

	public Action shareAction;

	public Action customHide;

	public RewardWindowBase rewardWindow { get; set; }

	public FacebookController.StoryPriority facebookPriority
	{
		get
		{
			return _facebookPriority;
		}
		set
		{
			_facebookPriority = value;
		}
	}

	public FacebookController.StoryPriority twitterPriority
	{
		get
		{
			return _twiiterPriority;
		}
		set
		{
			_twiiterPriority = value;
		}
	}

	public FacebookController.StoryPriority faceBookPriority
	{
		set
		{
			facebookPriority = value;
			twitterPriority = value;
		}
	}

	public string EventTitle { get; set; }

	public Func<string> twitterStatus { get; set; }

	private void Awake()
	{
		if (GameConnect.isMiniGame || GameConnect.isDaterRegim)
		{
			ChooseMiniGameController.AddFreeTicketsTimerToPoint(freeTicketsTimerPoint);
		}
		interfaceAnimator.runtimeAnimatorController = (GameConnect.isDuel ? DuelAnimController : standartAnimController);
		sharedController = this;
	}

	private void OnDestroy()
	{
		sharedController = null;
	}

	private void Start()
	{
		if (BuffSystem.instance != null && !BuffSystem.instance.haveAllInteractons && Storager.getInt("Training.ShouldRemoveNoviceArmorInShopKey") == 1 && HintController.instance != null)
		{
			HintController.instance.ShowHintByName("shop_remove_novice_armor");
		}
		cameraObj = base.transform.GetChild(0).gameObject;
		if (SexualButton != null)
		{
			ButtonHandler component = SexualButton.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += SexualButtonHandler;
			}
		}
		if (InAppropriateActButton != null)
		{
			ButtonHandler component2 = InAppropriateActButton.GetComponent<ButtonHandler>();
			if (component2 != null)
			{
				component2.Clicked += InAppropriateActButtonHandler;
			}
		}
		if (OtherButton != null)
		{
			ButtonHandler component3 = OtherButton.GetComponent<ButtonHandler>();
			if (component3 != null)
			{
				component3.Clicked += OtherButtonHandler;
			}
		}
		if (ReportButton != null)
		{
			ButtonHandler component4 = ReportButton.GetComponent<ButtonHandler>();
			if (component4 != null)
			{
				component4.Clicked += ShowReasonPanel;
			}
		}
		if (AddButton != null)
		{
			ButtonHandler component5 = AddButton.GetComponent<ButtonHandler>();
			if (component5 != null)
			{
				component5.Clicked += AddButtonHandler;
			}
		}
		if (GameConnect.gameMode == GameConnect.GameMode.TeamFight || GameConnect.gameMode == GameConnect.GameMode.FlagCapture || GameConnect.gameMode == GameConnect.GameMode.CapturePoints)
		{
			listOfPlayers.transform.localPosition -= 50f * Vector3.up;
			if (NetworkStartTable.LocalOrPasswordRoom())
			{
				MapSelectPanel.transform.localPosition += 80f * Vector3.up;
			}
		}
		if (duelPanel != null)
		{
			duelUI = duelPanel.GetComponent<DuelUIController>();
		}
		if (GameConnect.isMiniGame || GameConnect.isDaterRegim)
		{
			BankShopGUI.ViewType = BankShopViewGuiElement.BankShopViewType.Bank;
			BankShopGUI.ShowTickets = true;
			BankShopGUI.UpdateView();
			BankShopGUI.Clicked += BankShopGUI_Clicked;
		}
		if (InGameGUI.sharedInGameGUI != null)
		{
			shopAnchor.SetActive(false);
		}
	}

	private void BankShopGUI_Clicked(object sender, EventArgs e)
	{
		ShowBankWindow();
	}

	public void ShowBankWindow()
	{
		if (ShopNGUIController.GuiActive)
		{
			UnityEngine.Debug.LogWarning("ShowBankWindow ShopNGUIController.GuiActive != null");
			return;
		}
		if (BankController.Instance == null)
		{
			UnityEngine.Debug.LogWarning("ShowBankWindow bankController == null");
			return;
		}
		if (BankController.Instance.InterfaceEnabledCoroutineLocked)
		{
			UnityEngine.Debug.LogWarning("ShowBankWindow InterfaceEnabledCoroutineLocked");
			return;
		}
		BankController.Instance.BackRequested += HandleBackFromBankClicked;
		allInterfaceContainer.SetActiveSafeSelf(false);
		BankController.Instance.SetInterfaceEnabledWithDesiredCurrency(true, "TicketsCurrency");
	}

	private void HandleBackFromBankClicked(object sender, EventArgs e)
	{
		if (ShopNGUIController.GuiActive)
		{
			UnityEngine.Debug.LogWarning("HandleBackFromBankClicked ShopNGUIController.GuiActive  != null");
			return;
		}
		if (BankController.Instance == null)
		{
			UnityEngine.Debug.LogWarning("HandleBackFromBankClicked bankController == null");
			return;
		}
		if (BankController.Instance.InterfaceEnabledCoroutineLocked)
		{
			UnityEngine.Debug.LogWarning("HandleBackFromBankClicked InterfaceEnabledCoroutineLocked");
			return;
		}
		BankController.Instance.BackRequested -= HandleBackFromBankClicked;
		BankController.Instance.SetInterfaceEnabledWithDesiredCurrency(false, null);
		allInterfaceContainer.SetActiveSafeSelf(true);
	}

	private void Update()
	{
		cameraObj.SetActiveSafeSelf((ExpController.Instance == null || !ExpController.Instance.LevelUpPanelOpened) && (BankController.Instance == null || !BankController.Instance.InterfaceEnabled));
		if (GameConnect.isMiniGame && Initializer.players.Count == 0 && (GameConnect.isHunger || Defs.isRegimVidosDebug) && spectatorModeBtnPnl.activeSelf)
		{
			spectatorModeBtnPnl.SetActive(false);
			spectratorModePnl.SetActive(false);
		}
		facebookButton.SetActive(FacebookController.FacebookSupported && FacebookController.FacebookPost_Old_Supported && FB.IsLoggedIn);
		twitterButton.SetActive(TwitterController.TwitterSupported && TwitterController.TwitterSupported_OldPosts && TwitterController.IsLoggedIn);
		bool flag = facebookButton.activeSelf || twitterButton.activeSelf;
		if (socialPnl.activeSelf != flag)
		{
			socialPnl.SetActive(flag);
		}
	}

	private void OnEnable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(HandleEscape, "Network Start Table GUI");
	}

	private void OnDisable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
	}

	public void HandleEscape()
	{
		if (ReasonsPanel != null && ReasonsPanel.activeInHierarchy)
		{
			BackFromReasonPanel();
		}
		else if (ActionPanel != null && ActionPanel.activeInHierarchy)
		{
			CancelFromActionPanel();
		}
		else if (ShopNGUIController.GuiActive)
		{
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myTable != null)
			{
				WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().HandleResumeFromShop();
			}
		}
		else if (hideOldRanksButton.activeInHierarchy)
		{
			EventDelegate.Execute(hideOldRanksButton.GetComponent<UIButton>().onClick);
		}
	}

	public void ShowActionPanel(string _pixelbookID, string _nick)
	{
		pixelbookID = _pixelbookID;
		nick = _nick;
		HideTable();
		for (int i = 0; i < actionPanelNicklabel.Length; i++)
		{
			actionPanelNicklabel[i].text = nick;
		}
		ActionPanel.SetActive(true);
		spectatorModeBtnPnl.SetActive(false);
		if (FriendsController.sharedController.IsShowAdd(pixelbookID) && CountAddFriens < 3)
		{
			AddButton.GetComponent<UIButton>().isEnabled = true;
		}
		else
		{
			AddButton.GetComponent<UIButton>().isEnabled = false;
		}
	}

	public void HideActionPanel()
	{
		ActionPanel.SetActive(false);
		ShowTable(updateRealTableAfterActionPanel);
		if ((GameConnect.isHunger || Defs.isRegimVidosDebug) && Initializer.players.Count > 0)
		{
			spectatorModeBtnPnl.SetActive(Initializer.players.Count != 0);
		}
	}

	public void ShowReasonPanel(object sender, EventArgs e)
	{
		if ((!(ExpController.Instance != null) || !ExpController.Instance.IsLevelUpShown) && !ShopNGUIController.GuiActive && !ExperienceController.sharedController.isShowNextPlashka)
		{
			UnityEngine.Debug.Log("ShowReasonPanel");
			ReasonsPanel.SetActive(true);
			ActionPanel.SetActive(false);
		}
	}

	public void HideReasonPanel()
	{
		if ((!(ExpController.Instance != null) || !ExpController.Instance.IsLevelUpShown) && !ShopNGUIController.GuiActive && !ExperienceController.sharedController.isShowNextPlashka)
		{
			ReasonsPanel.SetActive(false);
			ActionPanel.SetActive(true);
		}
	}

	public bool CheckHideInternalPanel()
	{
		if (ActionPanel.activeInHierarchy)
		{
			CancelFromActionPanel();
			return true;
		}
		if (ReasonsPanel.activeInHierarchy)
		{
			BackFromReasonPanel();
			return true;
		}
		return false;
	}

	public void AddButtonHandler(object sender, EventArgs e)
	{
		if ((!(ExpController.Instance != null) || !ExpController.Instance.IsLevelUpShown) && !ShopNGUIController.GuiActive && !ExperienceController.sharedController.isShowNextPlashka)
		{
			UnityEngine.Debug.Log("[Add] " + pixelbookID);
			CountAddFriens++;
			string value = (GameConnect.isDaterRegim ? "Sandbox (Dating)" : "Multiplayer Battle");
			Dictionary<string, object> socialEventParameters = new Dictionary<string, object>
			{
				{ "Added Friends", value },
				{ "Deleted Friends", "Add" }
			};
			FriendsController.sharedController.SendInvitation(pixelbookID, socialEventParameters);
			if (!FriendsController.sharedController.notShowAddIds.Contains(pixelbookID))
			{
				FriendsController.sharedController.notShowAddIds.Add(pixelbookID);
			}
			AddButton.GetComponent<UIButton>().isEnabled = false;
		}
	}

	public void CancelFromActionPanel()
	{
		if ((!(ExpController.Instance != null) || !ExpController.Instance.IsLevelUpShown) && !ShopNGUIController.GuiActive && !ExperienceController.sharedController.isShowNextPlashka)
		{
			HideActionPanel();
		}
	}

	public void BackFromReasonPanel()
	{
		if ((!(ExpController.Instance != null) || !ExpController.Instance.IsLevelUpShown) && !ShopNGUIController.GuiActive && !ExperienceController.sharedController.isShowNextPlashka)
		{
			HideReasonPanel();
		}
	}

	public void InAppropriateActButtonHandler(object sender, EventArgs e)
	{
		if ((!(ExpController.Instance != null) || !ExpController.Instance.IsLevelUpShown) && !ShopNGUIController.GuiActive && !ExperienceController.sharedController.isShowNextPlashka)
		{
			FeedbackMenuController.ShowDialogWithCompletion(delegate
			{
				string value = _versionString.Value;
				Application.OpenURL(string.Concat("mailto:", Defs.SupportMail, "?subject=INAPPROPRIATE ACT ", nick, "(", pixelbookID, ")&body=%0D%0A%0D%0A%0D%0A%0D%0A%0D%0A------------%20DO NOT DELETE%20------------%0D%0AUTC%20Time:%20", DateTime.Now.ToString(), "%0D%0AGame:%20PixelGun3D%0D%0AVersion:%20", value, "%0D%0APlayerID:%20", FriendsController.sharedController.id, "%0D%0ACategory:%20INAPPROPRIATE ACT ", nick, "(", pixelbookID, ")%0D%0ADevice%20Type:%20", SystemInfo.deviceType, "%20", SystemInfo.deviceModel, "%0D%0AOS%20Version:%20", SystemInfo.operatingSystem, "%0D%0A------------------------").Replace(" ", "%20"));
			});
		}
	}

	public void SexualButtonHandler(object sender, EventArgs e)
	{
		if ((!(ExpController.Instance != null) || !ExpController.Instance.IsLevelUpShown) && !ShopNGUIController.GuiActive && !ExperienceController.sharedController.isShowNextPlashka)
		{
			FeedbackMenuController.ShowDialogWithCompletion(delegate
			{
				string value = _versionString.Value;
				Application.OpenURL(string.Concat("mailto:", Defs.SupportMail, "?subject=CHEATING ", nick, "(", pixelbookID, ")&body=%0D%0A%0D%0A%0D%0A%0D%0A%0D%0A------------%20DO NOT DELETE%20------------%0D%0AUTC%20Time:%20", DateTime.Now.ToString(), "%0D%0AGame:%20PixelGun3D%0D%0AVersion:%20", value, "%0D%0APlayerID:%20", FriendsController.sharedController.id, "%0D%0ACategory:%20CHEATING ", nick, "(", pixelbookID, ")%0D%0ADevice%20Type:%20", SystemInfo.deviceType, "%20", SystemInfo.deviceModel, "%0D%0AOS%20Version:%20", SystemInfo.operatingSystem, "%0D%0A------------------------").Replace(" ", "%20"));
			});
		}
	}

	public void OtherButtonHandler(object sender, EventArgs e)
	{
		if ((!(ExpController.Instance != null) || !ExpController.Instance.IsLevelUpShown) && !ShopNGUIController.GuiActive && !ExperienceController.sharedController.isShowNextPlashka)
		{
			FeedbackMenuController.ShowDialogWithCompletion(delegate
			{
				string value = _versionString.Value;
				Application.OpenURL(string.Concat("mailto:", Defs.SupportMail, "?subject=Report ", nick, "(", pixelbookID, ")&body=%0D%0A%0D%0A%0D%0A%0D%0A%0D%0A------------%20DO NOT DELETE%20------------%0D%0AUTC%20Time:%20", DateTime.Now.ToString(), "%0D%0AGame:%20PixelGun3D%0D%0AVersion:%20", value, "%0D%0APlayerID:%20", FriendsController.sharedController.id, "%0D%0ACategory:%20Report ", nick, "(", pixelbookID, ")%0D%0ADevice%20Type:%20", SystemInfo.deviceType, "%20", SystemInfo.deviceModel, "%0D%0AOS%20Version:%20", SystemInfo.operatingSystem, "%0D%0A------------------------").Replace(" ", "%20"));
			});
		}
	}

	public void StartSpectatorMode()
	{
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.aimPanel.SetActive(true);
		}
		spectatorModeOnBtn.SetActive(true);
		spectatorModeOffBtn.SetActive(false);
		spectratorModePnl.SetActive(true);
		socialPnl.SetActive(false);
		MapSelectPanel.SetActive(false);
		HideTable();
		if (WeaponManager.sharedManager.myTable != null)
		{
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().isRegimVidos = true;
		}
	}

	public void EndSpectatorMode()
	{
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.aimPanel.SetActive(false);
		}
		spectatorModeOnBtn.SetActive(false);
		spectatorModeOffBtn.SetActive(true);
		spectratorModePnl.SetActive(false);
		MapSelectPanel.SetActive(true);
		if (WeaponManager.sharedManager.myTable != null)
		{
			if (WeaponManager.sharedManager.myNetworkStartTable.currentGameObjectPlayer != null)
			{
				Player_move_c.SetLayerRecursively(WeaponManager.sharedManager.myNetworkStartTable.currentGameObjectPlayer.transform.GetChild(0).gameObject, 0);
			}
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().isRegimVidos = false;
		}
		ShowTable();
	}

	public void HideAvardPanel()
	{
		if (!isCancelHideAvardPanel)
		{
			rewardWindow = null;
			ShowEndInterface(winner, winnerCommand);
			if (WeaponManager.sharedManager.myTable != null)
			{
				WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().isShowAvard = false;
			}
			else
			{
				UnityEngine.Object.Destroy(sharedController.gameObject);
			}
			isCancelHideAvardPanel = true;
		}
	}

	public static RewardWindowBase ShowRewardWindow(bool win, Transform par)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("NguiWindows/WinWindowNGUI"));
		RewardWindowBase component = gameObject.GetComponent<RewardWindowBase>();
		FacebookController.StoryPriority priority = FacebookController.StoryPriority.Red;
		component.priority = priority;
		component.twitterPriority = FacebookController.StoryPriority.MultyWinLimit;
		component.shareAction = delegate
		{
			FacebookController.PostOpenGraphStory("win", "battle", priority, new Dictionary<string, string> { 
			{
				"battle",
				GameConnect.gameMode.ToString().ToLower()
			} });
		};
		component.HasReward = true;
		component.CollectOnlyNoShare = !win;
		component.twitterStatus = () => "I've won a battle in @PixelGun3D! Join the fight now! #pixelgun3d #pixelgun #3d #pg3d #mobile #fps #shooter http://goo.gl/8fzL9u";
		component.EventTitle = "Won Batlle";
		gameObject.transform.parent = par;
		Player_move_c.SetLayerRecursively(gameObject, LayerMask.NameToLayer("NGUITable"));
		gameObject.transform.localPosition = new Vector3(0f, 0f, -130f);
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		return component;
	}

	public void ShowFinishedInterface(bool isWinner, bool deadHeat)
	{
		bool flag = GameConnect.gameMode == GameConnect.GameMode.TeamFight || GameConnect.gameMode == GameConnect.GameMode.FlagCapture || GameConnect.gameMode == GameConnect.GameMode.CapturePoints;
		finishedInterface.SetActive(true);
		string text = LocalizationStore.Get(GameConnect.isDaterRegim ? "Key_1987" : ((!deadHeat) ? (isWinner ? "Key_1115" : (flag ? "Key_1116" : "Key_1976")) : ((GameConnect.isDuel && Initializer.networkTables.Count < 2) ? "Key_2436" : "Key_0571")));
		GameObject[] array = finishDraw;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive((deadHeat || (!isWinner && !flag)) && !GameConnect.isDaterRegim);
		}
		array = finishWin;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(isWinner && !deadHeat && !GameConnect.isDaterRegim);
		}
		array = finishDefeat;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(flag && !isWinner && !deadHeat && !GameConnect.isDaterRegim);
		}
		for (int j = 0; j < finishedInterfaceLabels.Length; j++)
		{
			finishedInterfaceLabels[j].text = text;
		}
		finishedInterfaceLabels[0].gameObject.SetActive(true);
	}

	public void ShowFinishedDuelInterface(RatingSystem.RatingChange ratingChange, bool showAward, int _addCoin, int _addExpierence, bool isWinner, bool deadheat)
	{
	}

	public void ShowStartDuelInterface()
	{
		duelUI = duelPanel.GetComponent<DuelUIController>();
		startInterfacePanel.SetActive(false);
		allInterfacePanel.SetActive(false);
		HideTable();
		duelPanel.SetActive(true);
		duelUI.ShowStartInterface();
	}

	public void ShowStartInterface()
	{
		if (GameConnect.isDuel)
		{
			ShowStartDuelInterface();
			return;
		}
		questsButton.SetActive(TrainingController.TrainingCompleted && !GameConnect.isHunger);
		MapSelectPanel.SetActive(false);
		goBattleLabel.SetActive(!GameConnect.isDaterRegim);
		daterButtonLabel.SetActive(GameConnect.isDaterRegim);
		allInterfacePanel.SetActive(true);
		startInterfacePanel.SetActive(true);
		rewardPanel.SetActive(false);
		isRewardShow = false;
		ShowTable();
		StartCoroutine("TryToShowExpiredBanner");
	}

	public void ShowNewMatchInterface()
	{
		isRewardShow = false;
		rewardPanel.SetActive(false);
		allInterfacePanel.SetActive(true);
		startInterfacePanel.SetActive(true);
		ShowTable();
	}

	public void HideStartInterface()
	{
		isRewardShow = false;
		rewardPanel.SetActive(false);
		UnityEngine.Debug.Log("HideStartInterface");
		finishedInterface.SetActive(false);
		allInterfacePanel.SetActive(false);
		startInterfacePanel.SetActive(false);
		ReasonsPanel.SetActive(false);
		ActionPanel.SetActive(false);
		updateRealTableAfterActionPanel = true;
		HideTable();
		StopCoroutine("TryToShowExpiredBanner");
	}

	public void ShowEndInterfaceDeadInHunger(string _winner, RatingSystem.RatingChange ratingChange)
	{
		StartCoroutine(MatchFinishedInterface(_winner, ratingChange, false, 0, 0, false, false, false, 0, 0, 0));
	}

	public void MathFinishedDeadInHunger()
	{
		if (spectratorModePnl.activeSelf)
		{
			EndSpectatorMode();
			return;
		}
		spectatorModeOnBtn.SetActive(false);
		spectatorModeOffBtn.SetActive(true);
		spectratorModePnl.SetActive(false);
	}

	public void FreezePlayer()
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.BlockPlayerInEnd();
			InGameGUI.sharedInGameGUI.gameObject.SetActive(false);
			if (ChatViewrController.sharedController != null)
			{
				UnityEngine.Object.Destroy(ChatViewrController.sharedController.gameObject);
			}
		}
	}

	public IEnumerator MatchFinishedInterface(string _winner, RatingSystem.RatingChange ratingChange, bool showAward, int _addCoin, int _addExpierence, bool _isCustom, bool firstPlace, bool iAmWinnerInTeam, int _winnerCommand, int blueTotal, int redTotal)
	{
		for (int i = 0; i < totalBlue.Length; i++)
		{
			totalBlue[i].text = blueTotal.ToString();
		}
		for (int j = 0; j < totalRed.Length; j++)
		{
			totalRed[j].text = redTotal.ToString();
		}
		ranksTable.totalBlue = blueTotal;
		ranksTable.totalRed = redTotal;
		bool flag = GameConnect.gameMode == GameConnect.GameMode.TeamFight || GameConnect.gameMode == GameConnect.GameMode.FlagCapture || GameConnect.gameMode == GameConnect.GameMode.CapturePoints;
		interfaceAnimator.ResetTrigger("RewardTaken");
		interfaceAnimator.ResetTrigger("GetReward");
		interfaceAnimator.SetBool("IsTwoTeams", flag);
		interfaceAnimator.SetBool("isRewarded", showAward || ratingChange.addRating != 0);
		interfaceAnimator.SetBool("isExpOnly", _addCoin == 0 || (ExperienceController.sharedController.currentLevel == 36 && _addCoin > 0));
		interfaceAnimator.SetBool("isHunger", GameConnect.isMiniGame);
		interfaceAnimator.SetBool("isDater", GameConnect.isDaterRegim);
		interfaceAnimator.SetBool("IsTrophyUp", ratingChange.isUp);
		interfaceAnimator.SetBool("IsTrophyDown", ratingChange.isDown);
		interfaceAnimator.SetBool("IsTrophyAdd", ratingChange.addRating != 0 && !ratingChange.isUp && !ratingChange.isDown);
		interfaceAnimator.SetBool("NoRaiting", ratingChange.addRating == 0);
		interfaceAnimator.SetBool("isTrophyOnly", ratingChange.addRating != 0 && _addCoin == 0 && _addExpierence == 0);
		interfaceAnimator.SetBool("isTrophyAdded", false);
		interfaceAnimator.SetBool("isRatingSystem", true);
		interfaceAnimator.SetBool("interfaceAnimationDone", false);
		isUsed = false;
		trophiAddIcon.SetActive(ratingChange.addRating > 0);
		trophiMinusIcon.SetActive(ratingChange.addRating < 0);
		trophyPanel.SetActive(ratingChange.addRating != 0);
		currentCup.spriteName = ratingChange.oldLeague.ToString() + " 1";
		NewCup.spriteName = ratingChange.newLeague.ToString() + " 1";
		if (ratingChange.addRating > 0)
		{
			currentBarFillAmount = ratingChange.oldRatingAmount;
			nextBarFillAmount = ratingChange.newRatingAmount;
			currentBar.fillAmount = currentBarFillAmount;
			nextBar.fillAmount = currentBarFillAmount;
			nextBar.color = Color.yellow;
		}
		else
		{
			currentBarFillAmount = ratingChange.oldRatingAmount;
			nextBarFillAmount = ratingChange.newRatingAmount;
			currentBar.fillAmount = nextBarFillAmount;
			nextBar.fillAmount = nextBarFillAmount;
			nextBar.color = Color.red;
		}
		leagueUp = ratingChange.newLeague > ratingChange.oldLeague;
		if (ratingChange.maxRating == int.MaxValue)
		{
			trophyPoints.text = ratingChange.newRating.ToString();
		}
		else
		{
			trophyPoints.text = ratingChange.newRating + "/" + ratingChange.maxRating;
		}
		trophyShine.SetActive(ratingChange.isUp);
		trophyRewardValue = ratingChange.addRating;
		string text = string.Format((ratingChange.addRating > 0) ? "+{0}" : "{0}", new object[1] { trophyRewardValue });
		UILabel[] array = rewardTrophy;
		foreach (UILabel obj in array)
		{
			obj.gameObject.SetActive(ratingChange.addRating != 0);
			obj.text = text;
		}
		string text2 = string.Format(LocalizationStore.Get(RatingSystem.leagueChangeLocalizations[(int)ratingChange.newLeague]), new object[1] { "" });
		array = textLeagueUp;
		for (int k = 0; k < array.Length; k++)
		{
			array[k].text = text2;
		}
		array = textLeagueDown;
		for (int k = 0; k < array.Length; k++)
		{
			array[k].text = text2;
		}
		shareToggle.value = firstPlace && showAward && (FB.IsLoggedIn || TwitterController.IsLoggedIn);
		shareToggle.gameObject.SetActive(shareToggle.value);
		if (defaultTeamOneState == Vector3.zero)
		{
			defaultTeamOneState = teamOneLabel.transform.localPosition;
		}
		if (defaultTeamTwoState == Vector3.zero)
		{
			defaultTeamTwoState = teamTwoLabel.transform.localPosition;
		}
		if (!flag || _winnerCommand == 0)
		{
			teamOneLabel.transform.localPosition = defaultTeamOneState;
			teamTwoLabel.transform.localPosition = defaultTeamTwoState;
		}
		else
		{
			teamOneLabel.transform.localPosition = defaultTeamOneState + Vector3.right * 55f;
			teamTwoLabel.transform.localPosition = defaultTeamTwoState + Vector3.left * 55f;
		}
		if (GameConnect.isMiniGame)
		{
			if (GameConnect.isHunger)
			{
				EndSpectatorMode();
			}
			HideTable();
		}
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			FreezePlayer();
			interfaceAnimator.SetTrigger("MatchEnd");
			BankShopGUI.InteractionEnabled = false;
			BankShopGUI.UpdateView();
			ShowFinishedInterface(iAmWinnerInTeam, (GameConnect.isCompany || GameConnect.isFlag || GameConnect.isCapturePoints) && _winnerCommand == 0);
			waitForAnimationDone = true;
			while (waitForAnimationDone)
			{
				yield return null;
			}
		}
		rewardCoinsObject.gameObject.SetActive(false);
		rewardExpObject.gameObject.SetActive(false);
		if (showAward || ratingChange.addRating != 0)
		{
			interfaceAnimator.SetTrigger("Reward");
			if (ExperienceController.sharedController.currentLevel == 36 && _addCoin > 0)
			{
				expRewardValue = 0;
			}
			expRewardValue = _addExpierence;
			coinsRewardValue = _addCoin;
		}
		else
		{
			expRewardValue = 0;
			coinsRewardValue = 0;
		}
		isRewardShow = showAward || ratingChange.addRating != 0;
		if (GameConnect.isDaterRegim)
		{
			for (int l = 0; l < finishedInterfaceLabels.Length; l++)
			{
				finishedInterfaceLabels[l].text = _winner;
			}
		}
		ExperienceController.sharedController.isShowRanks = true;
		WeaponManager.sharedManager.myNetworkStartTable.DestroyPlayer();
		if (showAward)
		{
			ShowAwardEndInterface(_winner, _addCoin, _addExpierence, _isCustom, firstPlace, _winnerCommand);
		}
		else
		{
			ShowEndInterface(_winner, _winnerCommand);
		}
		if (GameConnect.isMiniGame)
		{
			backButtonInHunger.SetActive(false);
			randomBtn.SetActive(false);
			MapSelectPanel.SetActive(false);
			questsButton.SetActive(false);
			spectatorModeBtnPnl.SetActive(false);
		}
		if (leagueUp)
		{
			rewardButton.SetActive(true);
			string newLeagueEggID = GiveLeagueEggAndReturnId();
			List<Texture> leagueItems = GetLeagueItems(newLeagueEggID);
			for (int m = 0; m < trophyItems.Length; m++)
			{
				if (m >= leagueItems.Count)
				{
					trophyItems[m].gameObject.SetActive(false);
					continue;
				}
				trophyItems[m].gameObject.SetActive(true);
				trophyItems[m].mainTexture = leagueItems[m];
			}
			rewardPanel.SetActive(trophyItems[0].gameObject.activeSelf);
			labelNewItems.SetActive(trophyItems[0].gameObject.activeSelf);
			rewardFrame.ResizeFrame();
			Invoke("OnTrophyOkButtonPress", 60f);
		}
		else
		{
			rewardButton.SetActive(false);
			rewardPanel.SetActive(false);
			labelNewItems.SetActive(false);
		}
		waitForTrophyAnimationDone = ratingChange.addRating != 0;
		while (waitForTrophyAnimationDone)
		{
			yield return null;
		}
		if (GameConnect.isMiniGame)
		{
			backButtonInHunger.SetActive(true);
			randomBtn.SetActive(true);
			if (GameConnect.isHunger)
			{
				spectatorModeBtnPnl.SetActive(true);
			}
		}
		for (int n = 0; n < trophyItems.Length; n++)
		{
			trophyItems[n].gameObject.SetActive(false);
		}
		if (showAward)
		{
			rewardExpObject.color = Color.white;
			rewardCoinsObject.color = Color.white;
			bool needDoubleReward = DoubleReward.Instance.NeedDoubleReward(GameConnect.gameMode) && _addCoin > 0;
			DoubleRewardNetworkStartTable componentInChildren = GetComponentInChildren<DoubleRewardNetworkStartTable>(true);
			if (componentInChildren != null)
			{
				componentInChildren.RefreshDoubleRewardLabel(needDoubleReward);
			}
			rewardCoinsObject.gameObject.SetActive(_addCoin > 0);
			rewardExpObject.gameObject.SetActive(ExperienceController.sharedController.currentLevel < 36);
			rewardPanel.SetActive(rewardCoinsObject.gameObject.activeSelf || rewardExpObject.gameObject.activeSelf);
			rewardFrame.ResizeFrame();
		}
	}

	private string GiveLeagueEggAndReturnId()
	{
		string result = string.Empty;
		try
		{
			RatingSystem.RatingLeague nextLeague = RatingSystem.instance.currentLeague + 1;
			string key = EggGivenForLeagueKey(nextLeague);
			if (Storager.getInt(key) == 0)
			{
				EggData eggData2 = Singleton<EggsManager>.Instance.GetAllEggs().FirstOrDefault((EggData eggData) => eggData.League == nextLeague);
				if (eggData2 != null)
				{
					result = eggData2.Id;
					Singleton<EggsManager>.Instance.AddEgg(eggData2);
				}
				else
				{
					UnityEngine.Debug.LogErrorFormat("Giving league egg: No egg found for league {0}", nextLeague.ToString());
				}
				Storager.setInt(key, 1);
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in giving league egg: {0}", ex);
		}
		return result;
	}

	private string EggGivenForLeagueKey(RatingSystem.RatingLeague league)
	{
		return string.Format("{0}_egg_given", new object[1] { league.ToString() });
	}

	private List<Texture> GetLeagueItems(string newLeagueEggID)
	{
		RatingSystem.RatingLeague league = RatingSystem.instance.currentLeague;
		List<Texture> list = new List<Texture>();
		foreach (WeaponSkin item in WeaponSkinsManager.SkinsForLeague(league))
		{
			list.Add(ItemDb.GetItemIcon(item.Id, ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory));
		}
		List<string> list2 = Wear.UnboughtLeagueItemsByLeagues()[league];
		for (int i = 0; i < list2.Count; i++)
		{
			list.Add(ItemDb.GetTextureForShopItem(list2[i]));
		}
		foreach (SkinItem item2 in (from kvp in SkinsController.sharedController.skinItemsDict
			where kvp.Value.currentLeague == league
			select kvp.Value).ToList())
		{
			SkinItem skinItem = item2;
			list.Add(Resources.Load<Texture>(string.Format("LeagueSkinsProfileImages/league{0}_skin_profile", new object[1] { (int)(league + 1) })));
		}
		try
		{
			if (!newLeagueEggID.IsNullOrEmpty())
			{
				list.Add(ItemDb.GetItemIcon(newLeagueEggID, ShopNGUIController.CategoryNames.EggsCategory));
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in GetLEaguueItems, adding egg texture: {0}", ex);
		}
		return list;
	}

	public void OnTrophyAnimationDone()
	{
		if (!isUsed)
		{
			if (!leagueUp)
			{
				waitForTrophyAnimationDone = false;
				interfaceAnimator.SetBool("isTrophyAdded", true);
			}
			isUsed = true;
		}
	}

	public void OnMatchEndAnimationDone()
	{
		interfaceAnimator.SetBool("interfaceAnimationDone", true);
	}

	public IEnumerator MatchFinishedInDuelInterface(RatingSystem.RatingChange ratingChange, bool showAward, int _addCoin, int _addExpierence, bool _isCustom, bool firstPlace, bool deadheat)
	{
		interfaceAnimator.ResetTrigger("RewardTaken");
		interfaceAnimator.ResetTrigger("GetReward");
		interfaceAnimator.ResetTrigger("AnimationEnds");
		interfaceAnimator.SetBool("isRewarded", showAward || ratingChange.addRating != 0);
		interfaceAnimator.SetBool("isExpOnly", _addCoin == 0 || (ExperienceController.sharedController.currentLevel == 36 && _addCoin > 0));
		interfaceAnimator.SetBool("IsTrophyUp", ratingChange.isUp);
		interfaceAnimator.SetBool("IsTrophyDown", ratingChange.isDown);
		interfaceAnimator.SetBool("IsTrophyAdd", ratingChange.addRating != 0 && !ratingChange.isUp && !ratingChange.isDown);
		interfaceAnimator.SetBool("NoRaiting", ratingChange.addRating == 0);
		interfaceAnimator.SetBool("isTrophyOnly", ratingChange.addRating != 0 && _addCoin == 0 && _addExpierence == 0);
		interfaceAnimator.SetBool("isTrophyAdded", false);
		interfaceAnimator.SetBool("interfaceAnimationDone", false);
		isUsed = false;
		trophiAddIcon.SetActive(ratingChange.addRating > 0);
		trophiMinusIcon.SetActive(ratingChange.addRating < 0);
		trophyPanel.SetActive(ratingChange.addRating != 0);
		currentCup.spriteName = ratingChange.oldLeague.ToString() + " 1";
		NewCup.spriteName = ratingChange.newLeague.ToString() + " 1";
		if (ratingChange.addRating > 0)
		{
			currentBarFillAmount = ratingChange.oldRatingAmount;
			nextBarFillAmount = ratingChange.newRatingAmount;
			currentBar.fillAmount = currentBarFillAmount;
			nextBar.fillAmount = currentBarFillAmount;
			nextBar.color = Color.yellow;
		}
		else
		{
			currentBarFillAmount = ratingChange.oldRatingAmount;
			nextBarFillAmount = ratingChange.newRatingAmount;
			currentBar.fillAmount = nextBarFillAmount;
			nextBar.fillAmount = nextBarFillAmount;
			nextBar.color = Color.red;
		}
		leagueUp = ratingChange.newLeague > ratingChange.oldLeague;
		if (ratingChange.maxRating == int.MaxValue)
		{
			trophyPoints.text = ratingChange.newRating.ToString();
		}
		else
		{
			trophyPoints.text = ratingChange.newRating + "/" + ratingChange.maxRating;
		}
		trophyShine.SetActive(ratingChange.isUp);
		trophyRewardValue = ratingChange.addRating;
		string text = string.Format((ratingChange.addRating > 0) ? "+{0}" : "{0}", new object[1] { trophyRewardValue });
		UILabel[] array = rewardTrophy;
		foreach (UILabel obj in array)
		{
			obj.gameObject.SetActive(ratingChange.addRating != 0);
			obj.text = text;
		}
		string text2 = string.Format(LocalizationStore.Get(RatingSystem.leagueChangeLocalizations[(int)ratingChange.newLeague]), new object[1] { "" });
		array = textLeagueUp;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].text = text2;
		}
		array = textLeagueDown;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].text = text2;
		}
		shareToggle.value = false;
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			FreezePlayer();
			interfaceAnimator.SetTrigger("MatchEnd");
			BankShopGUI.InteractionEnabled = false;
			BankShopGUI.UpdateView();
			ShowFinishedInterface(firstPlace, deadheat);
			waitForAnimationDone = true;
			while (waitForAnimationDone)
			{
				yield return null;
			}
		}
		rewardCoinsObject.gameObject.SetActive(false);
		rewardExpObject.gameObject.SetActive(false);
		if (showAward || ratingChange.addRating != 0)
		{
			interfaceAnimator.SetTrigger("Reward");
			if (ExperienceController.sharedController.currentLevel == 36 && _addCoin > 0)
			{
				expRewardValue = 0;
			}
			expRewardValue = _addExpierence;
			coinsRewardValue = _addCoin;
		}
		else
		{
			expRewardValue = 0;
			coinsRewardValue = 0;
		}
		isRewardShow = showAward || ratingChange.addRating != 0;
		ExperienceController.sharedController.isShowRanks = true;
		WeaponManager.sharedManager.myNetworkStartTable.DestroyPlayer();
		if (showAward)
		{
			ShowAwardEndInterface("", _addCoin, _addExpierence, _isCustom, firstPlace, 0);
		}
		else
		{
			ShowEndInterface("", 0);
		}
		duelUI.ShowFinishedInterface(ratingChange, showAward, _addCoin, _addExpierence, firstPlace, deadheat);
		if (leagueUp)
		{
			rewardButton.SetActive(true);
			string newLeagueEggID = GiveLeagueEggAndReturnId();
			List<Texture> leagueItems = GetLeagueItems(newLeagueEggID);
			for (int j = 0; j < trophyItems.Length; j++)
			{
				if (j >= leagueItems.Count)
				{
					trophyItems[j].gameObject.SetActive(false);
					continue;
				}
				trophyItems[j].gameObject.SetActive(true);
				trophyItems[j].mainTexture = leagueItems[j];
			}
			rewardPanel.SetActive(trophyItems[0].gameObject.activeSelf);
			labelNewItems.SetActive(trophyItems[0].gameObject.activeSelf);
			rewardFrame.ResizeFrame();
			Invoke("OnTrophyOkButtonPress", 60f);
		}
		else
		{
			rewardButton.SetActive(false);
			rewardPanel.SetActive(false);
			labelNewItems.SetActive(false);
		}
		waitForTrophyAnimationDone = ratingChange.addRating != 0;
		while (waitForTrophyAnimationDone)
		{
			yield return null;
		}
		for (int k = 0; k < trophyItems.Length; k++)
		{
			trophyItems[k].gameObject.SetActive(false);
		}
		if (showAward)
		{
			rewardExpObject.color = Color.white;
			rewardCoinsObject.color = Color.white;
			bool needDoubleReward = DoubleReward.Instance.NeedDoubleReward(GameConnect.gameMode) && _addCoin > 0;
			DoubleRewardNetworkStartTable componentInChildren = GetComponentInChildren<DoubleRewardNetworkStartTable>(true);
			if (componentInChildren != null)
			{
				componentInChildren.RefreshDoubleRewardLabel(needDoubleReward);
			}
			rewardCoinsObject.gameObject.SetActive(_addCoin > 0);
			rewardExpObject.gameObject.SetActive(ExperienceController.sharedController.currentLevel < 36);
			rewardPanel.SetActive(rewardCoinsObject.gameObject.activeSelf || rewardExpObject.gameObject.activeSelf);
			rewardFrame.ResizeFrame();
		}
	}

	public void OnTablesShow()
	{
		waitForAnimationDone = false;
	}

	public void OnRewardShow()
	{
		StartCoroutine(StartRewardAnimation());
	}

	public IEnumerator StartRewardAnimation()
	{
		rewardFrame.ResizeFrame();
		float animTime2 = 0f;
		while (ShopNGUIController.GuiActive || (BankController.Instance != null && BankController.Instance.InterfaceEnabled))
		{
			yield return null;
		}
		GameConnect.GameMode currentMode = GameConnect.gameMode;
		DateTime? now = FriendsController.GetServerTime();
		bool needDoubleReward = now.HasValue && DoubleReward.Instance.NeedDoubleReward(currentMode) && coinsRewardValue > 0;
		if (needDoubleReward && Application.isEditor)
		{
			DateTime? doubleRewardDate = DoubleReward.Instance.GetDoubleRewardDate(currentMode);
			UnityEngine.Debug.LogFormat("Need double reward: {0}, {1:s}, {2}x{4}, {3}x{4}", currentMode, doubleRewardDate.HasValue ? doubleRewardDate.Value : default(DateTime), addExperience, addCoins, DoubleReward.Instance.RewardFactor);
		}
		if (expRewardValue > 0)
		{
			if (ExperienceController.sharedController.currentLevel == 1)
			{
				DoubleReward.Instance.InitializeDoubleRewardDate(now);
			}
			Vector3 expStart = rewardExpObject.transform.localPosition;
			while (animTime2 < 1f)
			{
				rewardExpObject.transform.localPosition = Vector3.Lerp(expStart, rewardExpAnimPoint.localPosition, Mathf.Min(animTime2, 1f));
				rewardExpObject.color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), Mathf.Min(animTime2, 1f));
				animTime2 += Time.deltaTime / 0.4f;
				yield return null;
			}
			int incrementExperience = (needDoubleReward ? (expRewardValue * DoubleReward.Instance.RewardFactor) : expRewardValue);
			ExperienceController.sharedController.AddExperience(incrementExperience);
			if (WeaponManager.sharedManager.myNetworkStartTable != null)
			{
				WeaponManager.sharedManager.myNetworkStartTable.UpdateRanks();
			}
			expRewardValue = 0;
		}
		rewardExpObject.gameObject.SetActive(false);
		animTime2 = 0f;
		if (coinsRewardValue > 0)
		{
			Vector3 coinsStart = rewardCoinsObject.transform.localPosition;
			while (animTime2 < 1f)
			{
				rewardCoinsObject.transform.localPosition = Vector3.Lerp(coinsStart, rewardCoinsAnimPoint.localPosition, Mathf.Min(animTime2, 1f));
				rewardCoinsObject.color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), Mathf.Min(animTime2, 1f));
				animTime2 += Time.deltaTime / 0.4f;
				yield return null;
			}
			BankController.AddCoins(needDoubleReward ? (coinsRewardValue * DoubleReward.Instance.RewardFactor) : coinsRewardValue);
			coinsRewardValue = 0;
		}
		if (needDoubleReward)
		{
			DoubleReward.Instance.SaveDoubleRewardDate(currentMode, now);
			AnalyticsStuff.FirstWinOfTheDayGameModesCompleted(GameConnect.gameMode);
		}
		rewardCoinsObject.gameObject.SetActive(false);
	}

	public void StartTrophyAnim()
	{
		StartCoroutine(TrophyFillAnimation());
	}

	private IEnumerator TrophyFillAnimation()
	{
		float animTime = 0f;
		if (trophyRewardValue != 0)
		{
			while (animTime < 1f)
			{
				nextBar.fillAmount = Mathf.Lerp(currentBarFillAmount, nextBarFillAmount, Mathf.Min(animTime, 1f));
				animTime += Time.deltaTime;
				yield return null;
			}
		}
	}

	public void ShowAwardEndInterface(string _winner, int _addCoin, int _addExpierence, bool _isCustom, bool firstPlace, int _winnerCommand)
	{
		if (_isCustom)
		{
			addCoins = 0;
			addExperience = 0;
		}
		else
		{
			addCoins = _addCoin;
			addExperience = _addExpierence;
		}
		GameConnect.GameMode gameMode = GameConnect.gameMode;
		PremiumAccountController instance = PremiumAccountController.Instance;
		bool flag = gameMode == GameConnect.GameMode.Deathmatch || gameMode == GameConnect.GameMode.FlagCapture || gameMode == GameConnect.GameMode.TeamFight || gameMode == GameConnect.GameMode.CapturePoints;
		bool flag2 = PromoActionsManager.sharedManager.IsDayOfValorEventActive && flag;
		if (instance.IsActiveOrWasActiveBeforeStartMatch() || flag2)
		{
			if (!GameConnect.isCOOP && !GameConnect.isHunger)
			{
				AdminSettingsController.GetMultiplyerRewardWithBoostEvent(false);
			}
			else
			{
				int rewardCoeff = PremiumAccountController.Instance.RewardCoeff;
			}
			if (!GameConnect.isCOOP && !GameConnect.isHunger)
			{
				AdminSettingsController.GetMultiplyerRewardWithBoostEvent(true);
			}
			else
			{
				int rewardCoeff2 = PremiumAccountController.Instance.RewardCoeff;
			}
		}
		bool flag3 = DoubleReward.Instance.NeedDoubleReward(gameMode) && _addCoin > 0;
		int num = (flag3 ? (_addCoin * DoubleReward.Instance.RewardFactor) : _addCoin);
		string term = (flag3 ? "Key_3151" : "Key_3148");
		try
		{
			string text = string.Format(LocalizationStore.Get(term), new object[1] { num });
			UILabel[] array = rewardCoins;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].text = text;
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
			string text2 = string.Format("+{0} {1}", new object[2]
			{
				num,
				LocalizationStore.Key_0275
			});
			UILabel[] array = rewardCoins;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].text = text2;
			}
		}
		int num2 = (flag3 ? (_addExpierence * DoubleReward.Instance.RewardFactor) : _addExpierence);
		string term2 = (flag3 ? "Key_3152" : "Key_3149");
		try
		{
			string text3 = string.Format(LocalizationStore.Get(term2), new object[1] { num2 });
			UILabel[] array = rewardExperience;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].text = text3;
			}
		}
		catch (Exception exception2)
		{
			UnityEngine.Debug.LogException(exception2);
			string text4 = string.Format("+{0} {1}", new object[2]
			{
				num2,
				LocalizationStore.Key_0204
			});
			UILabel[] array = rewardExperience;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].text = text4;
			}
		}
		ShowEndInterface(_winner, _winnerCommand);
	}

	public void ShowEndInterface(string _winner, int _winnerCommand)
	{
		if (!ShopNGUIController.NoviceArmorAvailable)
		{
			GetComponent<HintController>().HideHintByName("shop_remove_novice_armor");
		}
		NotificationController.instance.SaveTimeValues();
		if (ABTestController.useBuffSystem)
		{
			BuffSystem.instance.EndRound();
		}
		else
		{
			KillRateCheck.instance.CheckKillRate();
		}
		WeaponManager.sharedManager.myNetworkStartTable.ClearKillrate();
		startInterfacePanel.SetActive(GameConnect.isDaterRegim);
		endInterfacePanel.SetActive(!GameConnect.isDaterRegim);
		goBattleLabel.SetActive(!GameConnect.isDaterRegim);
		daterButtonLabel.SetActive(GameConnect.isDaterRegim);
		try
		{
			bool isMiniGame = GameConnect.isMiniGame;
			miniGamesNextMatchPriceContainer.SetActiveSafeSelf(isMiniGame);
			if (miniGamesNextMatchPriceContainer.activeSelf)
			{
				miniGamesNextMatchPriceLabels.ForEach(delegate(UILabel lab)
				{
					lab.text = BalanceController.ParametersForMiniGameType(GameConnect.gameMode).TicketsPrice.ToString();
				});
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in miniGamesNextMatchPriceContainer set price: {0}", ex);
			miniGamesNextMatchPriceContainer.SetActiveSafeSelf(false);
		}
		backButtonInHunger.SetActive(GameConnect.isMiniGame);
		if (GameConnect.isDuel)
		{
			return;
		}
		if (GameConnect.isCompany || GameConnect.isFlag || GameConnect.isCapturePoints)
		{
			winnerPanelCom1.SetActive(_winnerCommand == 1);
			winnerPanelCom2.SetActive(_winnerCommand == 2);
		}
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.aimPanel.SetActive(false);
		}
		socialPnl.SetActive(BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64);
		winner = _winner;
		allInterfacePanel.SetActive(true);
		if (WeaponManager.sharedManager.myNetworkStartTable.oldPlayersList != null)
		{
			ranksTable.UpdateRanksFromOldSpisok();
		}
		if (GameConnect.isMiniGame || Defs.isRegimVidosDebug)
		{
			if (GameConnect.isMiniGame)
			{
				randomBtn.SetActive(true);
				questsButton.SetActive(true);
			}
			if (GameConnect.isHunger)
			{
				spectatorModeBtnPnl.SetActive(true);
			}
			updateRealTableAfterActionPanel = true;
			if (!ActionPanel.activeSelf && !ReasonsPanel.activeSelf)
			{
				ShowTable();
			}
		}
		else
		{
			updateRealTableAfterActionPanel = false;
			ShowTable(false);
			MapSelectPanel.SetActive(false);
			questsButton.SetActive(false);
		}
		StartCoroutine("TryToShowExpiredBanner");
	}

	private void ShareResults()
	{
		faceBookPriority = FacebookController.StoryPriority.Red;
		twitterPriority = FacebookController.StoryPriority.MultyWinLimit;
		twitterStatus = () => "I've won a battle in @PixelGun3D! Join the fight now! #pixelgun3d #pixelgun #3d #pg3d #mobile #fps #shooter http://goo.gl/8fzL9u";
		EventTitle = "Won Batlle";
		if (TwitterController.TwitterSupported && TwitterController.IsLoggedIn && TwitterController.Instance.CanPostStatusUpdateWithPriority(twitterPriority))
		{
			TwitterController.Instance.PostStatusUpdate(twitterStatus(), twitterPriority);
			AnalyticsFacade.SendCustomEvent("Virality", new Dictionary<string, object>
			{
				{ "Post Twitter", EventTitle },
				{ "Total Twitter", "Posts" }
			});
		}
		if (FacebookController.FacebookSupported && FB.IsLoggedIn && FacebookController.sharedController.CanPostStoryWithPriority(facebookPriority))
		{
			FacebookController.PostOpenGraphStory("win", "battle", facebookPriority, new Dictionary<string, string> { 
			{
				"battle",
				GameConnect.gameMode.ToString().ToLower()
			} });
			AnalyticsFacade.SendCustomEvent("Virality", new Dictionary<string, object>
			{
				{ "Post Facebook", EventTitle },
				{ "Total Facebook", "Posts" }
			});
		}
	}

	public void OnTablesShown()
	{
		if (!GameConnect.isDaterRegim)
		{
			if (!GameConnect.isMiniGame || expRewardValue > 0 || coinsRewardValue > 0)
			{
				if (GameConnect.isDuel)
				{
					oldRanksIsActive = true;
					HideOldRanks();
				}
				else
				{
					Invoke("HideOldRanks", 60f);
					oldRanksIsActive = true;
				}
				hideOldRanksButton.SetActive(!GameConnect.isDuel);
				if (GameConnect.isMiniGame)
				{
					backButtonInHunger.SetActive(false);
					randomBtn.SetActive(false);
					MapSelectPanel.SetActive(false);
					questsButton.SetActive(false);
					spectatorModeBtnPnl.SetActive(false);
				}
			}
			else
			{
				hideOldRanksButton.SetActive(false);
				MapSelectPanel.SetActive(true);
				questsButton.SetActive(true);
				BankShopGUI.InteractionEnabled = true;
				BankShopGUI.UpdateView();
			}
		}
		else
		{
			BankShopGUI.InteractionEnabled = true;
			BankShopGUI.UpdateView();
		}
	}

	public void HideOldRanks()
	{
		if (oldRanksIsActive && (hideOldRanksButton.activeSelf || GameConnect.isDuel))
		{
			CancelInvoke("HideOldRanks");
			interfaceAnimator.SetTrigger("OkPressed");
			if (expRewardValue > 0 || coinsRewardValue > 0 || trophyRewardValue != 0)
			{
				interfaceAnimator.SetTrigger("GetReward");
			}
			hideOldRanksButton.SetActive(false);
		}
	}

	public void HandleHideOldRanksClick()
	{
		if (oldRanksIsActive)
		{
			if (shareToggle.value && (expRewardValue > 0 || coinsRewardValue > 0))
			{
				ShareResults();
			}
			HideOldRanks();
		}
	}

	public void FinishHideOldRanks()
	{
		BankShopGUI.InteractionEnabled = true;
		BankShopGUI.UpdateView();
		interfaceAnimator.SetTrigger("RewardTaken");
		if (!oldRanksIsActive && !GameConnect.isMiniGame)
		{
			return;
		}
		trophyRewardValue = 0;
		oldRanksIsActive = false;
		questsButton.SetActive(TrainingController.TrainingCompleted);
		isRewardShow = false;
		if (Defs.isMulti)
		{
			MapSelectPanel.SetActive(true);
		}
		if (GameConnect.isDuel)
		{
			duelUI.ShowEndInterface();
		}
		else if (!GameConnect.isMiniGame)
		{
			finishedInterface.SetActive(false);
			HideEndInterface();
			ShowNewMatchInterface();
			WeaponManager.sharedManager.myNetworkStartTable.ResetOldScore();
		}
		else
		{
			backButtonInHunger.SetActive(true);
			randomBtn.SetActive(true);
			if (GameConnect.isHunger)
			{
				spectatorModeBtnPnl.SetActive(true);
			}
		}
		if (WeaponManager.sharedManager.myTable != null)
		{
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().isShowAvard = false;
		}
		else
		{
			StartCoroutine(WaitAndRemoveInterfaceOnReconnect());
		}
	}

	private IEnumerator WaitAndRemoveInterfaceOnReconnect()
	{
		yield return null;
		UnityEngine.Object.Destroy(sharedController.gameObject);
	}

	private IEnumerator TryToShowExpiredBanner()
	{
		while (FriendsController.sharedController == null || TempItemsController.sharedController == null)
		{
			yield return null;
		}
		while (true)
		{
			yield return StartCoroutine(FriendsController.sharedController.MyWaitForSeconds(1f));
			try
			{
				if (ShopNGUIController.GuiActive || (BankController.Instance != null && BankController.Instance.InterfaceEnabled) || (ExpController.Instance != null && ExpController.Instance.WaitingForLevelUpView) || (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown) || waitForAnimationDone || waitForTrophyAnimationDone || ExchangeWindow.IsOpened || rentScreenPoint.childCount != 0)
				{
					continue;
				}
				if (BuffSystem.instance != null && BuffSystem.instance.haveAllInteractons && Storager.getInt("Training.ShouldRemoveNoviceArmorInShopKey") == 1)
				{
					GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("NguiWindows/WeRemoveNoviceArmorBanner"));
					obj.transform.parent = rentScreenPoint;
					Player_move_c.SetLayerRecursively(obj, LayerMask.NameToLayer("NGUITable"));
					obj.transform.localPosition = new Vector3(0f, 0f, -130f);
					obj.transform.localRotation = Quaternion.identity;
					obj.transform.localScale = new Vector3(1f, 1f, 1f);
					GetComponent<HintController>().HideHintByName("shop_remove_novice_armor");
					try
					{
						ShopNGUIController.EquipWearInCategoryIfNotEquiped("Armor_Army_1", ShopNGUIController.CategoryNames.ArmorCategory, false);
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogError("Exception in NetworkStartTableNguiController: ShopNGUIController.EquipWearInCategoryIfNotEquiped: " + ex);
					}
				}
				else
				{
					ShopNGUIController.ShowTryGunIfPossible(startInterfacePanel.activeSelf, rentScreenPoint, "NGUITable");
				}
			}
			catch (Exception ex2)
			{
				UnityEngine.Debug.LogWarning("exception in NetworkTableNGUI  TryToShowExpiredBanner: " + ex2);
			}
		}
	}

	public static bool IsStartInterfaceShown()
	{
		if (sharedController != null && sharedController.startInterfacePanel != null)
		{
			return sharedController.startInterfacePanel.activeSelf;
		}
		return false;
	}

	public static bool IsEndInterfaceShown()
	{
		if (sharedController != null && sharedController.endInterfacePanel != null)
		{
			return sharedController.endInterfacePanel.activeSelf;
		}
		return false;
	}

	public void HideEndInterface()
	{
		UnityEngine.Debug.Log("HideEndInterface");
		socialPnl.SetActive(false);
		allInterfacePanel.SetActive(false);
		endInterfacePanel.SetActive(false);
		winnerPanelCom1.SetActive(false);
		winnerPanelCom2.SetActive(false);
		if (defaultTeamOneState == Vector3.zero)
		{
			defaultTeamOneState = teamOneLabel.transform.localPosition;
		}
		if (defaultTeamTwoState == Vector3.zero)
		{
			defaultTeamTwoState = teamTwoLabel.transform.localPosition;
		}
		teamOneLabel.transform.localPosition = defaultTeamOneState;
		teamTwoLabel.transform.localPosition = defaultTeamTwoState;
		HideTable();
		ReasonsPanel.SetActive(false);
		ActionPanel.SetActive(false);
		updateRealTableAfterActionPanel = true;
		StopCoroutine("TryToShowExpiredBanner");
	}

	private void ShowTable(bool _isRealUpdate = true)
	{
		ranksTable.isShowRanks = _isRealUpdate;
		ranksTable.tekPanel.SetActive(true);
	}

	public void HideTable()
	{
		ranksTable.isShowRanks = false;
		ranksTable.tekPanel.SetActive(false);
	}

	public void ShowRanksTable()
	{
		ShowTable();
		ranksInterface.SetActive(true);
	}

	public void HideRanksTable(bool isHideTable = true)
	{
		if (isHideTable)
		{
			HideTable();
		}
		ranksInterface.SetActive(false);
	}

	public void BackPressFromRanksTable(bool isHideTable = true)
	{
		if (!CheckHideInternalPanel())
		{
			HideRanksTable(isHideTable);
			ReasonsPanel.SetActive(false);
			ActionPanel.SetActive(false);
			if (WeaponManager.sharedManager.myPlayerMoveC != null)
			{
				WeaponManager.sharedManager.myPlayerMoveC.BackRanksPressed();
			}
		}
	}

	public void UpdateGoMapButtonsInDuel(bool show = true)
	{
		int tierForRoom = GameConnect.GetTierForRoom();
		bool flag = !show || GameConnect.gameTier != tierForRoom;
		for (int i = 0; i < goMapInEndGameButtonsDuel.Length; i++)
		{
			goMapInEndGameButtonsDuel[i].gameObject.SetActive(!flag);
		}
		changeMapLabel.SetActive(!flag);
		if (flag)
		{
			return;
		}
		AllScenesForMode listScenesForMode = SceneInfoController.instance.GetListScenesForMode(ConnectScene.curSelectMode);
		SceneInfo[] array = new SceneInfo[goMapInEndGameButtonsDuel.Length];
		goMapInEndGameButtonsDuel[0].SetMap(null);
		for (int j = 1; j < array.Length; j++)
		{
			int num = 0;
			bool flag2 = true;
			int num2 = UnityEngine.Random.Range(0, listScenesForMode.avaliableScenes.Count);
			while (flag2)
			{
				flag2 = false;
				SceneInfo sceneInfo = listScenesForMode.avaliableScenes[num2];
				for (int k = 0; k < j; k++)
				{
					if (array[k] == sceneInfo)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2 && (sceneInfo.NameScene.Equals(Application.loadedLevelName) || sceneInfo.AvaliableWeapon == ModeWeapon.dater || (sceneInfo.isPremium && Storager.getInt(sceneInfo.NameScene + "Key") == 0 && !PremiumAccountController.MapAvailableDueToPremiumAccount(sceneInfo.NameScene))))
				{
					flag2 = true;
				}
				if (!flag2)
				{
					array[j] = sceneInfo;
				}
				else
				{
					num2++;
					num++;
					if (num2 > listScenesForMode.avaliableScenes.Count - 1)
					{
						num2 = 0;
					}
				}
				if (num > listScenesForMode.avaliableScenes.Count)
				{
					UnityEngine.Debug.LogWarning("no map");
					break;
				}
			}
			if (array[j] != null)
			{
				goMapInEndGameButtonsDuel[j].SetMap(array[j]);
			}
			else
			{
				goMapInEndGameButtonsDuel[j].gameObject.SetActive(false);
			}
		}
	}

	public void UpdateGoMapButtons(bool show = true)
	{
		if (GameConnect.isDuel)
		{
			UpdateGoMapButtonsInDuel(show);
			return;
		}
		int tierForRoom = GameConnect.GetTierForRoom();
		bool flag = !show || GameConnect.gameTier != tierForRoom || GameConnect.isMiniGame;
		for (int i = 0; i < goMapInEndGameButtons.Length; i++)
		{
			goMapInEndGameButtons[i].gameObject.SetActive(!flag);
		}
		changeMapLabel.SetActive(!flag);
		if (flag)
		{
			return;
		}
		AllScenesForMode listScenesForMode = SceneInfoController.instance.GetListScenesForMode(ConnectScene.curSelectMode);
		SceneInfo[] array = new SceneInfo[goMapInEndGameButtons.Length];
		goMapInEndGameButtons[0].SetMap(null);
		for (int j = 1; j < array.Length; j++)
		{
			int num = 0;
			bool flag2 = true;
			int num2 = UnityEngine.Random.Range(0, listScenesForMode.avaliableScenes.Count);
			while (flag2)
			{
				flag2 = false;
				SceneInfo sceneInfo = listScenesForMode.avaliableScenes[num2];
				for (int k = 0; k < j; k++)
				{
					if (array[k] == sceneInfo)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2 && (sceneInfo.NameScene.Equals(Application.loadedLevelName) || sceneInfo.AvaliableWeapon == ModeWeapon.dater || (sceneInfo.isPremium && Storager.getInt(sceneInfo.NameScene + "Key") == 0 && !PremiumAccountController.MapAvailableDueToPremiumAccount(sceneInfo.NameScene))))
				{
					flag2 = true;
				}
				if (!flag2)
				{
					array[j] = sceneInfo;
				}
				else
				{
					num2++;
					num++;
					if (num2 > listScenesForMode.avaliableScenes.Count - 1)
					{
						num2 = 0;
					}
				}
				if (num > listScenesForMode.avaliableScenes.Count)
				{
					UnityEngine.Debug.LogWarning("no map");
					break;
				}
			}
			goMapInEndGameButtons[j].SetMap(array[j]);
		}
	}

	public void OnRewardAnimationEnds()
	{
		interfaceAnimator.SetTrigger("AnimationEnds");
	}

	public void OnTrophyOkButtonPress()
	{
		CancelInvoke("OnTrophyOkButtonPress");
		waitForTrophyAnimationDone = false;
		interfaceAnimator.SetBool("isTrophyAdded", true);
	}
}
