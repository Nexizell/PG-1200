using UnityEngine;

namespace Rilisoft
{
	public class StoragerStringCachedProperty : StoragerCachedPropertyBase<string>
	{
		public StoragerStringCachedProperty(string prefsKey)
			: base(prefsKey)
		{
		}

		protected override string GetValue()
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer && !Storager.hasKey(base.PrefsKey))
			{
				Storager.setString(base.PrefsKey, string.Empty);
			}
			return Storager.getString(base.PrefsKey);
		}

		protected override void SetValue(string value)
		{
			Storager.setString(base.PrefsKey, value);
		}
	}
}
