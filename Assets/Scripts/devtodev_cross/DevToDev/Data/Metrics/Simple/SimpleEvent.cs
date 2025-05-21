using System;
using System.Collections.Generic;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;

namespace DevToDev.Data.Metrics.Simple
{
	internal abstract class SimpleEvent : Event
	{
		public SimpleEvent()
		{
		}

		protected SimpleEvent(EventType type)
			: base(type)
		{
		}

		public SimpleEvent(ObjectInfo info)
			: base(info)
		{
		}

		public override JSONNode GetAdditionalDataJson()
		{
			JSONClass jSONClass = new JSONClass();
			foreach (KeyValuePair<string, object> parameter in parameters)
			{
				if (!parameter.Key.Equals(Event.IN_PROGRESS))
				{
					if (parameter.Value is JSONNode)
					{
						jSONClass.Add(parameter.Key, parameter.Value as JSONNode);
					}
					else
					{
						if (parameter.Value == null)
						{
							continue;
						}
						if (parameter.Value is string)
						{
							if (!parameter.Key.Equals("token") && !parameter.Key.Equals("receipt"))
							{
								jSONClass.Add(parameter.Key, new JSONData(Uri.EscapeDataString(parameter.Value.ToString())));
							}
							else
							{
								jSONClass.Add(parameter.Key, new JSONData(parameter.Value.ToString()));
							}
						}
						else
						{
							jSONClass.Add(parameter.Key, new JSONData(parameter.Value));
						}
					}
				}
				else
				{
					AddPendingToJSON(jSONClass);
				}
			}
			return jSONClass;
		}

		protected void addParameterIfNotNull(string key, object value)
		{
			if (value != null)
			{
				parameters.Add(key, value);
			}
		}
	}
}
