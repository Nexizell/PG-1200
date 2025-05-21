using System;

namespace DevToDev.Core.Serialization
{
	public abstract class ISaveable
	{
		public static int SerialVersionId = 1;

		public abstract void GetObjectData(ObjectInfo info);

		public ISaveable()
		{
		}

		public ISaveable(ObjectInfo info)
		{
			throw new NotImplementedException("Override this method in your storage");
		}
	}
}
