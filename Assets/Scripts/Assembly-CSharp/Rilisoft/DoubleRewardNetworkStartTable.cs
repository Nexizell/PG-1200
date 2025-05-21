using System;
using System.Globalization;
using UnityEngine;

namespace Rilisoft
{
	public sealed class DoubleRewardNetworkStartTable : MonoBehaviour
	{
		[SerializeField]
		protected internal UILabel _doubleRewardLabel;

		public UILabel DoubleRewardLabel
		{
			get
			{
				return _doubleRewardLabel;
			}
		}

		internal void RefreshDoubleRewardLabel(bool needDoubleReward)
		{
			if (_doubleRewardLabel == null)
			{
				return;
			}
			try
			{
				_doubleRewardLabel.gameObject.SetActive(needDoubleReward);
				string format = LocalizationStore.Get("Key_3147");
				_doubleRewardLabel.text = string.Format(CultureInfo.InvariantCulture, format, DoubleReward.Instance.RewardFactor);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				_doubleRewardLabel.gameObject.SetActive(false);
			}
		}
	}
}
