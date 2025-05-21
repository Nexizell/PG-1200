namespace Rilisoft
{
	public class StoragerBoolCachedProperty : StoragerCachedPropertyBase<bool>
	{
		public StoragerBoolCachedProperty(string prefsKey)
			: base(prefsKey)
		{
		}

		protected override bool GetValue()
		{
			return Storager.getInt(base.PrefsKey).ToBool();
		}

		protected override void SetValue(bool value)
		{
			Storager.setInt(base.PrefsKey, value.ToInt());
		}
	}
}
