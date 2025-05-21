namespace Rilisoft
{
	public class StoragerIntCachedProperty : StoragerCachedPropertyBase<int>
	{
		public StoragerIntCachedProperty(string prefsKey)
			: base(prefsKey)
		{
		}

		protected override int GetValue()
		{
			return Storager.getInt(base.PrefsKey);
		}

		protected override void SetValue(int value)
		{
			Storager.setInt(base.PrefsKey, value);
		}
	}
}
