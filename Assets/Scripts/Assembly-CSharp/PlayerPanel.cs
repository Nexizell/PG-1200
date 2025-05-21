using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class PlayerPanel : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CStartExpAnim_003Ed__42 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PlayerPanel _003C_003E4__this;

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
		public _003CStartExpAnim_003Ed__42(int _003C_003E1__state)
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
				goto IL_00a1;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this.currentExp.enabled = true;
				_003C_003E2__current = new WaitForSeconds(0.15f);
				_003C_003E1__state = 2;
				return true;
			case 2:
			{
				_003C_003E1__state = -1;
				int num = _003Ci_003E5__1 + 1;
				_003Ci_003E5__1 = num;
				goto IL_00a1;
			}
			case 3:
				{
					_003C_003E1__state = -1;
					_003C_003E4__this.oldExp.transform.localScale = _003C_003E4__this.currentExp.transform.localScale;
					return false;
				}
				IL_00a1:
				if (_003Ci_003E5__1 != 4)
				{
					_003C_003E4__this.currentExp.enabled = false;
					_003C_003E2__current = new WaitForSeconds(0.15f);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = null;
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

	public static PlayerPanel instance;

	[SerializeField]
	protected internal UILabel raitingLabel;

	private string raitingText;

	[SerializeField]
	public UILabel experienceLabel;

	[SerializeField]
	public UILabel likesLabel;

	[SerializeField]
	public UISprite currentExp;

	[SerializeField]
	public UISprite oldExp;

	[SerializeField]
	public UISprite rankSprite;

	[SerializeField]
	public GameObject trophys;

	[SerializeField]
	public GameObject like;

	private int curentExp;

	private int currentLevel = 1;

	[SerializeField]
	protected internal UILabel playerName;

	[SerializeField]
	protected internal UITexture clanIcon;

	[SerializeField]
	protected internal UILabel clanName;

	private Vector3 playerNameStartPos;

	private GameObject panelContainer;

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

	public string ExperienceLabel
	{
		get
		{
			if (!(experienceLabel != null))
			{
				return string.Empty;
			}
			return experienceLabel.text;
		}
		set
		{
			if (experienceLabel != null)
			{
				experienceLabel.text = value ?? string.Empty;
			}
		}
	}

	public float CurrentProgress
	{
		get
		{
			if (!(currentExp != null))
			{
				return 0f;
			}
			return currentExp.transform.localScale.x;
		}
		set
		{
			if (currentExp != null)
			{
				currentExp.transform.localScale = new Vector3(Mathf.Clamp(value, 0f, 1f), 1f, 1f);
			}
		}
	}

	public float OldProgress
	{
		get
		{
			if (!(oldExp != null))
			{
				return 0f;
			}
			return oldExp.transform.localScale.x;
		}
		set
		{
			if (oldExp != null)
			{
				oldExp.transform.localScale = new Vector3(Mathf.Clamp(value, 0f, 1f), base.transform.localScale.y, base.transform.localScale.z);
			}
		}
	}

	public int RankSprite
	{
		get
		{
			return currentLevel;
		}
		set
		{
			if (rankSprite != null)
			{
				string spriteName = string.Format("Rank_{0}", new object[1] { value });
				rankSprite.spriteName = spriteName;
				currentLevel = value;
			}
		}
	}

	private void Awake()
	{
		instance = this;
		RatingSystem.OnRatingUpdate += OnRatingUpdated;
		playerNameStartPos = playerName.transform.localPosition;
		panelContainer = base.transform.GetChild(0).gameObject;
		panelContainer.SetActive(AskNameManager.isComplete);
	}

	private void OnEnable()
	{
		updateLabelState(true);
		if (FriendsController.sharedController.clanName != "")
		{
			string text = FriendsController.sharedController.clanName;
			clanName.text = text;
			int num = 0;
			while (clanName.width > 168)
			{
				num++;
				clanName.text = text.Remove(text.Length - num);
				clanName.ProcessText();
			}
			byte[] data = Convert.FromBase64String(FriendsController.sharedController.clanLogo);
			Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoWidth);
			texture2D.LoadImage(data);
			texture2D.filterMode = FilterMode.Point;
			texture2D.Apply();
			clanIcon.mainTexture = texture2D;
			playerName.transform.localPosition = playerNameStartPos;
		}
		else
		{
			clanName.enabled = false;
			clanIcon.enabled = false;
			playerName.transform.localPosition = playerNameStartPos - Vector3.down * -16f;
		}
		UpdateNickPlayer();
		UpdateRating();
		UpdateExp();
	}

	public void UpdateRating()
	{
		raitingText = RatingSystem.instance.currentRating.ToString();
		raitingLabel.text = raitingText;
	}

	public void UpdateNickPlayer()
	{
		string text = FilterBadWorld.FilterString(ProfileController.GetPlayerNameOrDefault());
		playerName.text = text;
	}

	public void UpdateExp()
	{
		int num = ExperienceController.sharedController.currentLevel;
		curentExp = ExperienceController.sharedController.CurrentExperience;
		int num2 = ExperienceController.MaxExpLevelsDefault[num];
		RankSprite = num;
		if (num != 36)
		{
			OldProgress = (float)curentExp / (float)num2;
			CurrentProgress = (float)curentExp / (float)num2;
			ExperienceLabel = curentExp + "/" + num2;
		}
		else
		{
			ExperienceLabel = LocalizationStore.Get("Key_0928");
			CurrentProgress = 1f;
		}
	}

	private void OnDisable()
	{
		oldExp.enabled = true;
		oldExp.transform.localScale = currentExp.transform.localScale;
		StopAllCoroutines();
	}

	private void Update()
	{
		if (curentExp != ExperienceController.sharedController.CurrentExperience || currentLevel != ExperienceController.sharedController.currentLevel)
		{
			OnExpUpdate();
		}
		updateLabelState();
		if (likesLabel.gameObject.activeSelf)
		{
			likesLabel.text = Singleton<LobbyItemsController>.Instance.GetLikeCount().ToString();
		}
		if (panelContainer.activeSelf != AskNameManager.isComplete)
		{
			panelContainer.SetActive(AskNameManager.isComplete);
		}
	}

	private void updateLabelState(bool isForce = false)
	{
		bool flag = LobbyCraftController.Instance != null && LobbyCraftController.Instance.InterfaceEnabled;
		if (trophys.activeSelf == flag || isForce)
		{
			trophys.SetActive(!flag);
			raitingLabel.gameObject.SetActive(!flag);
			likesLabel.gameObject.SetActive(flag);
			like.SetActive(flag);
		}
	}

	private void OnExpUpdate()
	{
		int num = ExperienceController.sharedController.currentLevel;
		curentExp = ExperienceController.sharedController.CurrentExperience;
		int num2 = ExperienceController.MaxExpLevelsDefault[num];
		RankSprite = num;
		if (num != 36)
		{
			CurrentProgress = (float)curentExp / (float)num2;
			ExperienceLabel = curentExp + "/" + num2;
			if (oldExp.transform.localScale.x > currentExp.transform.localScale.x)
			{
				oldExp.transform.localScale = Vector3.zero;
			}
			StartCoroutine(StartExpAnim());
		}
		else
		{
			ExperienceLabel = LocalizationStore.Get("Key_0928");
			CurrentProgress = 1f;
			oldExp.transform.localScale = Vector3.one;
		}
	}

	private IEnumerator StartExpAnim()
	{
		int i = 0;
		while (i != 4)
		{
			currentExp.enabled = false;
			yield return new WaitForSeconds(0.15f);
			currentExp.enabled = true;
			yield return new WaitForSeconds(0.15f);
			int num = i + 1;
			i = num;
		}
		yield return null;
		oldExp.transform.localScale = currentExp.transform.localScale;
	}

	public void HandleOpenProfile()
	{
		if (MainMenuController.sharedController != null)
		{
			MainMenuController.sharedController.GoToProfile();
		}
		if (ProfileController.Instance != null)
		{
			ProfileController.Instance.SetStaticticTab(ProfileStatTabType.Leagues);
		}
	}

	private void OnRatingUpdated()
	{
		raitingText = RatingSystem.instance.currentRating.ToString();
		raitingLabel.text = raitingText;
	}

	private void OnDestroy()
	{
		instance = null;
		RatingSystem.OnRatingUpdate -= OnRatingUpdated;
	}
}
