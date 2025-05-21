using UnityEngine;

namespace Rilisoft
{
	public class PrefsEnumCachedProperty<T> : CachedPropertyWithKeyBase<T> where T : struct
	{
		public PrefsEnumCachedProperty(string prefsKey)
			: base(prefsKey)
		{
		}

		protected override T GetValue()
		{
			return (T)(object)PlayerPrefs.GetInt(base.PrefsKey, 0);
		}

		protected override void SetValue(T value)
		{
			int value2 = (int)(object)value;
			PlayerPrefs.SetInt(base.PrefsKey, value2);
		}
	}
}
