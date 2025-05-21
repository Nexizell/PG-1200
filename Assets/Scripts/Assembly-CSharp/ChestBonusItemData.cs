public class ChestBonusItemData
{
	public string tag;

	public int count;

	public int timeLife;

	public string GetTimeLabel(bool isShort = false)
	{
		int num = timeLife / 24;
		if (num > 0)
		{
			if (isShort)
			{
				return string.Format("{0}d.", new object[1] { num });
			}
			return string.Format("{0} {1}", new object[2]
			{
				LocalizationStore.Get("Key_1231"),
				num
			});
		}
		return string.Format("{0}h.", new object[1] { timeLife });
	}
}
