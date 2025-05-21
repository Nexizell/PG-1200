using System;
using UnityEngine;

namespace Rilisoft
{
	public class LikeCell : MonoBehaviour
	{
		[SerializeField]
		protected internal UILabel _playerNameLabel;

		private Action<LikeCell> _onClick;

		public LobbyCraftLikeGui.CellData Data { get; private set; }

		public void Setup(LobbyCraftLikeGui.CellData data, Action<LikeCell> onClick)
		{
			Data = data;
			_onClick = onClick;
			if (Data == null)
			{
				Debug.LogError("LikeCell setup error: data is null");
				_playerNameLabel.text = Data.PlayerName;
			}
			_playerNameLabel.text = Data.PlayerName;
		}

		public void U_Click()
		{
			if (_onClick != null)
			{
				_onClick(this);
			}
		}
	}
}
