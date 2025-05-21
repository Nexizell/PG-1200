using System;

public sealed class PurchaseEventArgs : EventArgs
{
	public enum PurchaseType
	{
		Coins = 0,
		Gems = 1,
		Offer = 2
	}

	public int Index { get; private set; }

	public int Count { get; set; }

	public decimal CurrencyAmount { get; set; }

	public PurchaseType Type { get; private set; }

	public int Discount { get; private set; }

	public PurchaseEventArgs(int index, int count, decimal currencyAmount, PurchaseType type, int discount)
	{
		Index = index;
		Count = count;
		CurrencyAmount = currencyAmount;
		Type = type;
		Discount = discount;
	}

	public PurchaseEventArgs(PurchaseEventArgs other)
	{
		if (other != null)
		{
			Index = other.Index;
			Count = other.Count;
			CurrencyAmount = other.CurrencyAmount;
			Type = other.Type;
			Discount = other.Discount;
		}
	}

	public override string ToString()
	{
		return string.Format("{{ Index: {0}, Count: {1}, CurrencyAmount: {2}, Type: {3}, Discount: {4} }}", Index, Count, CurrencyAmount, Type, Discount);
	}
}
