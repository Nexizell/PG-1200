using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public class NewLobbyLevelAnimator : MonoBehaviour
{
	public enum CondtionsForShow
	{
		None = 0,
		PromoOffers = 1,
		Premium = 2
	}

	[CompilerGenerated]
	internal sealed class _003CStart_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public NewLobbyLevelAnimator _003C_003E4__this;

		private UISprite _003Csprite_003E5__1;

		private int _003Ci_003E5__2;

		private float _003CstartTm_003E5__3;

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
		public _003CStart_003Ed__12(int _003C_003E1__state)
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
				goto IL_0047;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0047;
			case 2:
				_003C_003E1__state = -1;
				goto IL_013b;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E4__this.tips[_003Ci_003E5__2].SetActive(true);
				_003CstartTm_003E5__3 = Time.realtimeSinceStartup;
				_003C_003E4__this._tapped = false;
				goto IL_01db;
			case 4:
				_003C_003E1__state = -1;
				goto IL_01db;
			case 5:
				{
					_003C_003E1__state = -1;
					UnityEngine.Object.Destroy(_003C_003E4__this.gameObject);
					return false;
				}
				IL_01db:
				if (Time.realtimeSinceStartup - _003CstartTm_003E5__3 < _003C_003E4__this.timeTipShown && !_003C_003E4__this._tapped)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 4;
					return true;
				}
				_003C_003E4__this._tapped = false;
				_003C_003E4__this.tips[_003Ci_003E5__2].SetActive(false);
				_003C_003E4__this.shines[_003Ci_003E5__2].SetActive(false);
				_003Csprite_003E5__1 = null;
				goto IL_024c;
				IL_013b:
				if (_003Csprite_003E5__1.alpha < 1f)
				{
					_003Csprite_003E5__1.alpha += 0.04f;
					_003C_003E2__current = _003C_003E4__this.StartCoroutine(FriendsController.sharedController.MyWaitForSeconds(_003C_003E4__this.buttonAlphaTime / 25f));
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003E4__this.shines[_003Ci_003E5__2].SetActive(true);
				_003C_003E2__current = new WaitForSeconds(_003C_003E4__this.timeBetweenShineAndTip);
				_003C_003E1__state = 3;
				return true;
				IL_025c:
				if (_003Ci_003E5__2 < _003C_003E4__this.buttons.Count)
				{
					bool flag = true;
					if (_003C_003E4__this.conditions[_003Ci_003E5__2] == CondtionsForShow.Premium)
					{
						flag = Storager.getInt(Defs.PremiumEnabledFromServer) == 1;
					}
					else if (_003C_003E4__this.conditions[_003Ci_003E5__2] == CondtionsForShow.PromoOffers)
					{
						flag = MainMenuController.sharedController != null && MainMenuController.sharedController.PromoOffersPanelShouldBeShown();
					}
					if (flag)
					{
						_003Csprite_003E5__1 = _003C_003E4__this.buttons[_003Ci_003E5__2].GetComponent<UISprite>();
						goto IL_013b;
					}
					goto IL_024c;
				}
				Storager.setInt(Defs.ShownLobbyLevelSN, Storager.getInt(Defs.ShownLobbyLevelSN) + 1);
				try
				{
					string text = "[]";
					if (Storager.hasKey("AppsFlyer.TutorialStepsLogged"))
					{
						text = Storager.getString("AppsFlyer.TutorialStepsLogged");
						if (string.IsNullOrEmpty(text))
						{
							text = "[]";
						}
					}
					int @int = Storager.getInt(Defs.ShownLobbyLevelSN);
					List<object> list = Json.Deserialize(text) as List<object>;
					List<int> list2 = ((list != null) ? list.Select(Convert.ToInt32).ToList() : new List<int>(2));
					if (!list2.Contains(@int))
					{
						AnalyticsFacade.SendCustomEventToAppsFlyer("af_gui_tutorial_completion", new Dictionary<string, string> { 
						{
							"step",
							@int.ToString()
						} });
						list2.Add(@int);
						Storager.setString("AppsFlyer.TutorialStepsLogged", Json.Serialize(list2));
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogWarning(ex.ToString());
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 5;
				return true;
				IL_0047:
				if (FriendsController.sharedController == null)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003Ci_003E5__2 = 0;
				goto IL_025c;
				IL_024c:
				_003Ci_003E5__2++;
				goto IL_025c;
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

	public float buttonAlphaTime = 1f;

	public float timeBetweenShineAndTip = 0.3f;

	public float timeTipShown = 5f;

	public List<GameObject> buttons;

	public List<GameObject> tips;

	public List<GameObject> shines;

	public List<CondtionsForShow> conditions;

	private const int numOFStepsWhenAppearingButton = 25;

	private bool _tapped;

	private void Awake()
	{
		foreach (GameObject button in buttons)
		{
			button.GetComponent<UISprite>().alpha = 0f;
		}
		foreach (GameObject shine in shines)
		{
			shine.SetActive(false);
		}
		foreach (GameObject tip in tips)
		{
			tip.SetActive(false);
		}
	}

	public void OnMouseDown()
	{
		_tapped = true;
	}

	private IEnumerator Start()
	{
		while (FriendsController.sharedController == null)
		{
			yield return null;
		}
		for (int i = 0; i < buttons.Count; i++)
		{
			bool flag = true;
			if (conditions[i] == CondtionsForShow.Premium)
			{
				flag = Storager.getInt(Defs.PremiumEnabledFromServer) == 1;
			}
			else if (conditions[i] == CondtionsForShow.PromoOffers)
			{
				flag = MainMenuController.sharedController != null && MainMenuController.sharedController.PromoOffersPanelShouldBeShown();
			}
			if (flag)
			{
				UISprite sprite = buttons[i].GetComponent<UISprite>();
				while (sprite.alpha < 1f)
				{
					sprite.alpha += 0.04f;
					yield return StartCoroutine(FriendsController.sharedController.MyWaitForSeconds(buttonAlphaTime / 25f));
				}
				shines[i].SetActive(true);
				yield return new WaitForSeconds(timeBetweenShineAndTip);
				tips[i].SetActive(true);
				float startTm = Time.realtimeSinceStartup;
				_tapped = false;
				while (Time.realtimeSinceStartup - startTm < timeTipShown && !_tapped)
				{
					yield return null;
				}
				_tapped = false;
				tips[i].SetActive(false);
				shines[i].SetActive(false);
			}
		}
		Storager.setInt(Defs.ShownLobbyLevelSN, Storager.getInt(Defs.ShownLobbyLevelSN) + 1);
		try
		{
			string text = "[]";
			if (Storager.hasKey("AppsFlyer.TutorialStepsLogged"))
			{
				text = Storager.getString("AppsFlyer.TutorialStepsLogged");
				if (string.IsNullOrEmpty(text))
				{
					text = "[]";
				}
			}
			int @int = Storager.getInt(Defs.ShownLobbyLevelSN);
			List<object> list = Json.Deserialize(text) as List<object>;
			List<int> list2 = ((list != null) ? list.Select(Convert.ToInt32).ToList() : new List<int>(2));
			if (!list2.Contains(@int))
			{
				AnalyticsFacade.SendCustomEventToAppsFlyer("af_gui_tutorial_completion", new Dictionary<string, string> { 
				{
					"step",
					@int.ToString()
				} });
				list2.Add(@int);
				Storager.setString("AppsFlyer.TutorialStepsLogged", Json.Serialize(list2));
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogWarning(ex.ToString());
		}
		yield return null;
		UnityEngine.Object.Destroy(gameObject);
	}
}
