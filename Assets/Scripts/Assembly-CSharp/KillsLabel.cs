using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class KillsLabel : MonoBehaviour
{
	private UILabel _label;

	private InGameGUI _inGameGUI;

	private KeyValuePair<int, string> _killCountMemo = new KeyValuePair<int, string>(0, "0");

	private void Start()
	{
		base.gameObject.SetActive(Defs.isMulti && (GameConnect.gameMode == GameConnect.GameMode.DeathEscape || GameConnect.gameMode == GameConnect.GameMode.Duel || GameConnect.gameMode == GameConnect.GameMode.Deathmatch || GameConnect.isDaterRegim));
		_label = GetComponent<UILabel>();
		_inGameGUI = InGameGUI.sharedInGameGUI;
	}

	private void Update()
	{
		if ((bool)_inGameGUI && (bool)_label)
		{
			if (GameConnect.isDaterRegim || GameConnect.gameMode == GameConnect.GameMode.Duel)
			{
				_label.text = GetKillCountString(GlobalGameController.CountKills);
			}
			else if (_inGameGUI != null)
			{
				_label.text = _inGameGUI.killsToMaxKills();
			}
		}
	}

	private string GetKillCountString(int killCount)
	{
		if (killCount != _killCountMemo.Key)
		{
			string value = killCount.ToString();
			_killCountMemo = new KeyValuePair<int, string>(killCount, value);
		}
		return _killCountMemo.Value;
	}
}
