using System;

namespace Rilisoft
{
	internal sealed class GpgFacade
	{
		private static readonly Lazy<GpgFacade> s_instance = new Lazy<GpgFacade>(() => new GpgFacade());

		public static GpgFacade Instance
		{
			get
			{
				return s_instance.Value;
			}
		}

		public event EventHandler SignedOut;

		public void Initialize()
		{
		}

		public void Authenticate(Action<bool> callback, bool silent)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
		}

		public void IncrementAchievement(string achievementId, int steps, Action<bool> callback)
		{
			if (achievementId == null)
			{
				throw new ArgumentNullException("achievementId");
			}
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
		}

		public bool IsAuthenticated()
		{
			return false;
		}

		public void SignOut()
		{
			EventHandler signedOut = this.SignedOut;
			if (signedOut != null)
			{
				signedOut(this, EventArgs.Empty);
			}
		}

		private GpgFacade()
		{
		}
	}
}
