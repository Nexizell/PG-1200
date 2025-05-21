using System;
using UnityEngine;

namespace Rilisoft
{
	public class AchievementInfoView : MonoBehaviour
	{
		[SerializeField]
		protected internal UITexture _textureAchievementsBg;

		[SerializeField]
		protected internal UISprite _spriteIcon;

		[SerializeField]
		protected internal TextGroup _textName;

		[SerializeField]
		protected internal UILabel _labelDesc;

		private IDisposable _backSubscription;

		private void OnEnable()
		{
			if (_backSubscription != null)
			{
				_backSubscription.Dispose();
			}
			_backSubscription = BackSystem.Instance.Register(Hide, GetType().Name);
		}

		private void OnDisable()
		{
			if (_backSubscription != null)
			{
				_backSubscription.Dispose();
				_backSubscription = null;
			}
		}

		public void Show(Achievement ach)
		{
			base.gameObject.SetActive(true);
			_textureAchievementsBg.mainTexture = AchievementView.BackgroundTextureFor(ach);
			_spriteIcon.spriteName = ach.Data.Icon;
			_textName.Text = LocalizationStore.Get(ach.Data.LKeyName);
			string text = string.Empty;
			if (ach.Type == AchievementType.Common || ach.Type == AchievementType.Openable)
			{
				text = ((ach.Stage >= ach.MaxStage) ? string.Format("[00ff00]{0}", new object[1] { ach.Points }) : string.Format("{0}/{1}", new object[2] { ach.Points, ach.ToNextStagePointsTotal }));
			}
			if (ach.Type == AchievementType.Openable && ach.IsCompleted && ach.Data.Thresholds[0] == 1)
			{
				text = string.Empty;
			}
			_labelDesc.text = (text.IsNullOrEmpty() ? LocalizationStore.Get(ach.Data.LKeyDesc) : string.Format("{0}{1}{2}", new object[3]
			{
				LocalizationStore.Get(ach.Data.LKeyDesc),
				Environment.NewLine,
				text
			}));
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}
	}
}
