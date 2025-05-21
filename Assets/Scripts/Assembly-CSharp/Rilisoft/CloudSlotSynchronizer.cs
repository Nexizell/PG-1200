using System.Threading.Tasks;

namespace Rilisoft
{
	public abstract class CloudSlotSynchronizer
	{
		public abstract string CurrentResult { get; }

		public abstract bool Pulled { get; }

		public abstract Task<CloudPullResult> Pull(bool silent = true);

		public abstract void Push(string data);
	}
}
