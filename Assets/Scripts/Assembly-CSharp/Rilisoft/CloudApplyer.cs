using System.Collections;

namespace Rilisoft
{
	public abstract class CloudApplyer
	{
		protected CloudSlotSynchronizer SlotSynchronizer { get; set; }

		protected CloudApplyer(CloudSlotSynchronizer synchronizer)
		{
			SlotSynchronizer = synchronizer;
		}

		public abstract IEnumerator Apply(bool skipApplyingToLocalState);
	}
}
