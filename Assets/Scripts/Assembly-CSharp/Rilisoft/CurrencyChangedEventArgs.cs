using System;
using System.Collections.Generic;
using Rilisoft.MiniJson;

namespace Rilisoft
{
	public sealed class CurrencyChangedEventArgs : EventArgs
	{
		public string Currency { get; set; }

		public int NewValue { get; set; }

		public int AddedValue { get; set; }

		public Dictionary<string, object> ToJson()
		{
			return new Dictionary<string, object>
			{
				{ "currency", Currency },
				{ "newValue", NewValue },
				{ "addedValue", AddedValue }
			};
		}

		public override string ToString()
		{
			return Json.Serialize(ToJson());
		}
	}
}
