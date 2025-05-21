namespace Rilisoft
{
	public abstract class IosCloudSlotSynchronizer : CloudSlotSynchronizer
	{
		private string m_result;

		private bool m_pulled;

		public override string CurrentResult
		{
			get
			{
				return m_result;
			}
		}

		public override bool Pulled
		{
			get
			{
				return m_pulled;
			}
		}

		protected void SetResult(string result)
		{
			m_result = result;
		}

		protected void SetPulled(bool pulled)
		{
			m_pulled = pulled;
		}
	}
}
