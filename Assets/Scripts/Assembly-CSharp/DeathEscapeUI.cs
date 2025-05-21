using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DeathEscapeUI : MonoBehaviour
{
	public UILabel timeLabel;

	public UILabel bestTimeLabel;

	public UILabel recordTimeLabel;

	public UIButton saveCheckpointButton;

	public UILabel labelFreeSave;

	public UILabel labelSavePrice;

	private bool firstChange;

	private static readonly StringBuilder _sharedStringBuilder = new StringBuilder();

	private static KeyValuePair<int, string> _countdownMemo = new KeyValuePair<int, string>(0, "0:00:00");

	private void Awake()
	{
		saveCheckpointButton.onClick.Add(new EventDelegate(SaveCheckpointButtonClick));
	}

	private void SaveCheckpointButtonClick()
	{
		CheckpointController.instance.TrySaveOnCheckpoint();
	}

	private void Update()
	{
		bool flag = CheckpointController.instance.canSaveCheckpoint && WeaponManager.sharedManager.myPlayer != null;
		if (saveCheckpointButton.gameObject.activeSelf != flag || !firstChange)
		{
			firstChange = true;
			saveCheckpointButton.gameObject.SetActive(CheckpointController.instance.canSaveCheckpoint);
			if (CheckpointController.instance.canSaveCheckpoint)
			{
				int freeCheckpointsCount = CheckpointController.instance.GetFreeCheckpointsCount();
				bool flag2 = freeCheckpointsCount > 0;
				labelFreeSave.gameObject.SetActive(flag2);
				labelSavePrice.gameObject.SetActive(!flag2);
				if (flag2)
				{
					labelFreeSave.text = LocalizationStore.Get("Key_3175") + " " + freeCheckpointsCount;
				}
				else
				{
					labelSavePrice.text = CheckpointController.instance.saveCheckpointPrice.ToString();
				}
			}
		}
		timeLabel.text = FormatTime(CheckpointController.instance.lapTimeMili);
		bestTimeLabel.text = FormatTime(CheckpointController.instance.bestLapTimeMili);
		recordTimeLabel.text = FormatTime(CheckpointController.instance.lapRecordTimeMili);
	}

	public static string FormatTime(float timeDown)
	{
		return FormatTime(Mathf.FloorToInt(timeDown * 100f));
	}

	public static string FormatTime(int totalMiliSeconds)
	{
		if (totalMiliSeconds != _countdownMemo.Key)
		{
			int value = totalMiliSeconds / 6000;
			int num = totalMiliSeconds % 6000 / 100;
			int num2 = totalMiliSeconds % 100;
			_sharedStringBuilder.Length = 0;
			string value2 = ((num < 10) ? ":0" : ":");
			string value3 = ((num2 < 10) ? ":0" : ":");
			_sharedStringBuilder.Append(value).Append(value2).Append(num)
				.Append(value3)
				.Append(num2);
			string value4 = _sharedStringBuilder.ToString();
			_sharedStringBuilder.Length = 0;
			_countdownMemo = new KeyValuePair<int, string>(totalMiliSeconds, value4);
		}
		return _countdownMemo.Value;
	}
}
