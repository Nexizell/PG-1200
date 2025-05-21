namespace Rilisoft
{
	public abstract class StoragerCachedPropertyBase<T> : CachedPropertyWithKeyBase<T>
	{
		protected StoragerCachedPropertyBase(string prefsKey)
			: base(prefsKey)
		{
		}
	}
}
