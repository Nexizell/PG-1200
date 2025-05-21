using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Rilisoft
{
	internal sealed class AmazonGenericCloudSlotSynchronizer : AmazonCloudSlotSynchronizer
	{
		private readonly string _syncableStringName;

		public AmazonGenericCloudSlotSynchronizer(string syncableStringName)
		{
			if (syncableStringName == null)
			{
				throw new ArgumentNullException("syncableStringName");
			}
			_syncableStringName = syncableStringName;
		}

		protected override Task<CloudPullResult> PullCore(AGSGameDataMap dataMap)
		{
			using (AGSSyncableString aGSSyncableString = dataMap.GetLatestString(_syncableStringName))
			{
				string value = aGSSyncableString.GetValue();
				base.CurrentResultCore = value;
				base.PulledCore = true;
			}
			return Task.FromResult<CloudPullResult>(CloudPullResult.Successful);
		}

		protected override void PushCore(AGSGameDataMap dataMap, string data)
		{
			if (data == null)
			{
				return;
			}
			using (AGSSyncableString aGSSyncableString = dataMap.GetLatestString(_syncableStringName))
			{
				aGSSyncableString.Set(data);
			}
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}(`{1}`)", GetType(), _syncableStringName);
		}
	}
}
