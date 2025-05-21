using System;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	public sealed class DoubleRewardLabelMainMenu : MonoBehaviour
	{
		[SerializeField]
		protected internal TextGroup _doubleRewardLabel;

		private NewDayWatcher _newDayWatcher;

		private NewDayWatcher NewDayWatcher
		{
			get
			{
				if (_newDayWatcher == null)
				{
					NewDayWatcher newDayWatcher = base.gameObject.GetComponent<NewDayWatcher>() ?? base.gameObject.AddComponent<NewDayWatcher>();
					_newDayWatcher = newDayWatcher;
				}
				return _newDayWatcher;
			}
		}

		private void OnEnable()
		{
			RefreshDoubleRewardLabel();
			if (NewDayWatcher != null)
			{
				NewDayWatcher.NewDay += HandleNewDay;
			}
		}

		private void OnDisable()
		{
			if (NewDayWatcher != null)
			{
				NewDayWatcher.NewDay -= HandleNewDay;
			}
		}

		private void RefreshDoubleRewardLabel()
		{
			if (_doubleRewardLabel == null)
			{
				return;
			}
			try
			{
				bool active = GameModeLocker.Instance.SupportedModes.Any(DoubleReward.Instance.NeedDoubleReward);
				_doubleRewardLabel.gameObject.SetActive(active);
				string format = LocalizationStore.Get("Key_3146");
				_doubleRewardLabel.Text = string.Format(CultureInfo.InvariantCulture, format, DoubleReward.Instance.RewardFactor);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				_doubleRewardLabel.gameObject.SetActive(false);
			}
		}

		private void HandleNewDay(object sender, EventArgs e)
		{
			RefreshDoubleRewardLabel();
		}
	}
}
