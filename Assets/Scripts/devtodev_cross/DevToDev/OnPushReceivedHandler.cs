using System.Collections.Generic;

namespace DevToDev
{
	public delegate void OnPushReceivedHandler(PushType type, IDictionary<string, string> pushAdditionalData);
}
