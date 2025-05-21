using System;
using System.Collections.Generic;

namespace Rilisoft
{
	internal sealed class FlurryFacade
	{
		public FlurryFacade(string apiKey, bool enableLogging)
		{
			if (apiKey == null)
			{
				throw new ArgumentNullException("apiKey");
			}
		}

		public void LogEvent(string eventName)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
		}

		public void LogEventWithParameters(string eventName, Dictionary<string, string> parameters)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
		}

		public void SetUserId(string userId)
		{
			if (userId == null)
			{
				throw new ArgumentNullException("userId");
			}
		}
	}
}
