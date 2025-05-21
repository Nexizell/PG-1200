using UnityEngine;

namespace Rilisoft
{
	public class LeaderboardTournamentFooter : MonoBehaviour
	{
		[SerializeField]
		protected internal UILabel _textLabel;

		[SerializeField]
		protected internal UILabel _countLabel;

		[SerializeField]
		protected internal GameObject _coinObject;

		[SerializeField]
		protected internal GameObject _gemObject;

		private void OnEnable()
		{
			_textLabel.text = string.Format(LocalizationStore.Get("Key_2813"), new object[1] { BalanceController.countPlaceAwardInCompetion });
			if (_textLabel.text.Length > 47)
			{
				_textLabel.overflowMethod = UILabel.Overflow.ShrinkContent;
				_textLabel.width = 690;
			}
			if (BalanceController.competitionAward != null)
			{
				_countLabel.text = BalanceController.competitionAward.Price + "!";
				if (BalanceController.competitionAward.Currency == "Coins")
				{
					_coinObject.SetActiveSafe(true);
					_gemObject.SetActiveSafe(false);
				}
				else
				{
					_coinObject.SetActiveSafe(false);
					_gemObject.SetActiveSafe(true);
				}
			}
		}
	}
}
