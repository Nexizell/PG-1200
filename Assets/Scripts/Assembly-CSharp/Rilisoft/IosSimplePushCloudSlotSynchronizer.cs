using UnityEngine;

namespace Rilisoft
{
	public abstract class IosSimplePushCloudSlotSynchronizer : IosCloudSlotSynchronizer
	{
		public string CloudKey { get; private set; }

		protected IosSimplePushCloudSlotSynchronizer(string cloudKey)
		{
			CloudKey = cloudKey;
		}

		public override void Push(string data)
		{
			if (data == null)
			{
				Debug.LogErrorFormat("IosCloudSlotSynchronizer.Push: data == null , slotName = {0}", CloudKey ?? "null");
			}
			else
			{
				bool icloudAvailable = IcloudService.IcloudAvailable;
			}
		}
	}
}
