using System;
using System.Collections.Generic;
using Rilisoft.MiniJson;

namespace Rilisoft
{
	public sealed class SocialInteractionEventArgs : EventArgs
	{
		public string Kind { get; set; }

		public Dictionary<string, object> ToJson()
		{
			return new Dictionary<string, object> { { "kind", Kind } };
		}

		public override string ToString()
		{
			return Json.Serialize(ToJson());
		}
	}
}
