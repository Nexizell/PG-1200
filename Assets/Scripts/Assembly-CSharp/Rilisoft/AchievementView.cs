using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	public class AchievementView : MonoBehaviour
	{
		[SerializeField]
		protected internal UILabel _textName;

		[SerializeField]
		protected internal UITexture _textureBackground;

		[SerializeField]
		protected internal UISprite _spriteIcon;

		[SerializeField]
		protected internal UILabel _textProgress;

		[SerializeField]
		protected internal UISprite _spriteProgress;

		private Achievement _achievement;

		private static Dictionary<string, Texture> _loadedBgTextures = new Dictionary<string, Texture>();

		public Achievement Achievement
		{
			get
			{
				return _achievement;
			}
			set
			{
				if (_achievement != null)
				{
					_achievement.OnProgressChanged -= UpdateUI;
				}
				if (value == null)
				{
					Achievement.LogMsg("achievement is null");
					return;
				}
				_achievement = value;
				_textName.text = LocalizationStore.Get(_achievement.Data.LKeyName);
				if (_textName.text == _achievement.Data.LKeyName)
				{
					_textName.text = string.Format("[{0}]", new object[1] { _achievement.GetType().Name.Replace("Achievement", "") });
				}
				UpdateUI(true, true);
				_achievement.OnProgressChanged += UpdateUI;
			}
		}

		public static event Action<AchievementView> OnClicked;

		public static Texture BackgroundTextureFor(Achievement ach)
		{
			string text;
			switch (ach.Type)
			{
			case AchievementType.Common:
				text = string.Format("Achievements/Achievment_base_common_{0}", new object[1] { ach.Stage });
				break;
			case AchievementType.Hidden:
				text = "Achievements/Achievment_base_hidden_1";
				break;
			case AchievementType.Openable:
				text = string.Format("Achievements/Achievment_base_openable_{0}", new object[1] { ach.Stage });
				break;
			default:
				text = string.Empty;
				break;
			}
			if (text.IsNullOrEmpty())
			{
				return null;
			}
			if (_loadedBgTextures.ContainsKey(text))
			{
				return _loadedBgTextures[text];
			}
			bool isDebugBuild = Debug.isDebugBuild;
			Texture texture = Resources.Load<Texture>(text);
			bool isDebugBuild2 = Debug.isDebugBuild;
			if (texture != null)
			{
				_loadedBgTextures.Add(text, texture);
			}
			return texture;
		}

		private void Awake()
		{
			LocalizationStore.AddEventCallAfterLocalize(OnLocalize);
		}

		private void OnLocalize()
		{
			_textName.text = LocalizationStore.Get(_achievement.Data.LKeyName);
		}

		private void UpdateUI(bool pointsChanged, bool stageChanged)
		{
			_textureBackground.mainTexture = BackgroundTextureFor(_achievement);
			_textureBackground.fixedAspect = true;
			_spriteIcon.spriteName = _achievement.Data.Icon;
			if (_achievement.IsCompleted)
			{
				_spriteProgress.gameObject.SetActive(false);
				if (_achievement.Type == AchievementType.Common)
				{
					_textProgress.gameObject.SetActive(true);
					_textProgress.text = string.Format("{0}", new object[1] { _achievement.Points });
				}
				else
				{
					_textProgress.gameObject.SetActive(false);
				}
			}
			else
			{
				_textProgress.gameObject.SetActive(true);
				_spriteProgress.gameObject.SetActive(true);
				_textProgress.text = string.Format("{0}/{1}", new object[2] { _achievement.Points, _achievement.ToNextStagePointsTotal });
				_spriteProgress.fillAmount = (float)_achievement.Points / (float)_achievement.ToNextStagePointsTotal;
			}
		}

		private void OnClick()
		{
			if (AchievementView.OnClicked != null)
			{
				AchievementView.OnClicked(this);
			}
		}

		private void OnDestroy()
		{
			_achievement.OnProgressChanged -= UpdateUI;
			LocalizationStore.DelEventCallAfterLocalize(OnLocalize);
		}
	}
}
