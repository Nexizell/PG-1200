using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	public class LobbyCraftLikeGui : MonoBehaviour
	{
		public class CellData
		{
			public string PlayerId;

			public string PlayerName;

			public int LikesCount;

			public int Rank;
		}

		[SerializeField]
		protected internal BetterWrapContent _wrap;

		private List<CellData> _currentDataCollection;

		private void OnEnable()
		{
			UpdateData();
			FriendsController.UpdatedLastLikeLobbyPlayers = (Action)Delegate.Combine(FriendsController.UpdatedLastLikeLobbyPlayers, new Action(UpdateData));
		}

		private void OnDisable()
		{
			FriendsController.UpdatedLastLikeLobbyPlayers = (Action)Delegate.Remove(FriendsController.UpdatedLastLikeLobbyPlayers, new Action(UpdateData));
		}

		private void UpdateData()
		{
			List<CellData> list = new List<CellData>();
			foreach (string lastLikesPlayer in FriendsController.lastLikesPlayers)
			{
				if (lastLikesPlayer.IsNullOrEmpty())
				{
					continue;
				}
				Dictionary<string, object> fullPlayerDataById = FriendsController.GetFullPlayerDataById(lastLikesPlayer);
				if (fullPlayerDataById == null)
				{
					continue;
				}
				Dictionary<string, object> value = null;
				fullPlayerDataById.TryGetValue<Dictionary<string, object>>("player", out value);
				if (value != null)
				{
					string value2 = null;
					value.TryGetValue<string>("nick", out value2);
					if (!value2.IsNullOrEmpty())
					{
						CellData item = new CellData
						{
							PlayerId = lastLikesPlayer,
							PlayerName = value2
						};
						list.Add(item);
					}
				}
			}
			FillGrid(list);
		}

		private void FillGrid(List<CellData> dataCollection)
		{
			_currentDataCollection = dataCollection;
			_wrap.Initialize(() => _currentDataCollection.Count - 1, OnUpdateCell);
		}

		private void OnUpdateCell(GameObject cellObj, int cellIndex)
		{
			cellObj.GetComponent<LikeCell>().Setup(_currentDataCollection[cellIndex], ClickToCell);
		}

		public void ClickToCell(LikeCell cell)
		{
			if (FriendsController.sharedController == null || FriendsController.sharedController.id.IsNullOrEmpty() || cell.Data.PlayerId.IsNullOrEmpty() || cell.Data.PlayerId == FriendsController.sharedController.id)
			{
				return;
			}
			try
			{
				base.gameObject.SetActive(false);
				Action<bool> onCloseEvent = delegate
				{
					base.gameObject.SetActive(true);
				};
				FriendsController.ShowProfile(cell.Data.PlayerId, ProfileWindowType.other, onCloseEvent);
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Format("LobbyLikePanel: fail show profile with exception: {0}", new object[1] { ex.ToString() }));
				base.gameObject.SetActive(true);
			}
		}
	}
}
