using Rilisoft;

public class BankExchangeItemData
{
	private SaltedInt _currencyCount;

	private SaltedInt _gemsPrice;

	public int InAppId { get; set; }

	public int CurrencyCount
	{
		get
		{
			return _currencyCount.Value;
		}
		set
		{
			_currencyCount = new SaltedInt(SaltedInt._prng.Next(), value);
		}
	}

	public int GemsPrice
	{
		get
		{
			return _gemsPrice.Value;
		}
		set
		{
			_gemsPrice = new SaltedInt(SaltedInt._prng.Next(), value);
		}
	}
}
