using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Data.Metrics;
using DevToDev.Data.Metrics.Simple;

namespace DevToDev.Push.Data.Metrics.Simple
{
	internal class TokenSendEvent : SimpleEvent
	{
		private static readonly string TOKEN = "token";

		public TokenSendEvent()
		{
		}

		public TokenSendEvent(string token)
			: base(EventType.PushToken)
		{
			parameters.Remove(Event.TIMESTAMP);
			parameters.Add(TOKEN, token);
		}

		public TokenSendEvent(ObjectInfo info)
			: base(info)
		{
		}

		public static TokenSendEvent CreateFromJSON(JSONNode elem)
		{
			if (elem[TOKEN] == null)
			{
				return null;
			}
			return new TokenSendEvent(elem[TOKEN]);
		}
	}
}
