using Rilisoft;
using UnityEngine;

public sealed class CoinConfigurator : MonoBehaviour
{
	[SerializeField]
	protected internal VirtualCurrencyBonusType bonusType;

	public bool CoinIsPresent = true;

	public Vector3 pos;

	public Transform coinCreatePoint;

	public VirtualCurrencyBonusType BonusType
	{
		get
		{
			return bonusType;
		}
	}
}
