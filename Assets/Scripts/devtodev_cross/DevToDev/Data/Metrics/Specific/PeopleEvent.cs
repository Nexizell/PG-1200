using System;
using System.Collections;
using System.Collections.Generic;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Helpers;
using DevToDev.Data.Metrics.Simple;
using DevToDev.Logic;

namespace DevToDev.Data.Metrics.Specific
{
	internal class PeopleEvent : SimpleEvent
	{
		public bool IsRemoved { get; set; }

		public PeopleEvent()
			: base(EventType.UserCard)
		{
			IsRemoved = false;
			parameters.Remove(Event.TIMESTAMP);
		}

		public PeopleEvent(ObjectInfo info)
			: base(info)
		{
			try
			{
				IsRemoved = (bool)info.GetValue("isRemoved", typeof(bool));
			}
			catch (Exception ex)
			{
				Log.D("Error in desealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public override void GetObjectData(ObjectInfo info)
		{
			base.GetObjectData(info);
			try
			{
				info.AddValue("isRemoved", IsRemoved);
			}
			catch (Exception ex)
			{
				Log.D("Error in sealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public bool AddOrReplace(string key, object value)
		{
			if (parameters.ContainsKey(key))
			{
				if (parameters[key] == null)
				{
					if (value is Gender)
					{
						parameters[key] = (int)(Gender)value;
					}
					else
					{
						parameters[key] = value;
					}
					return true;
				}
				if (!parameters[key].Equals(value))
				{
					if (value is Gender)
					{
						parameters[key] = (int)(Gender)value;
					}
					else
					{
						parameters[key] = value;
					}
					return true;
				}
				return false;
			}
			if (value is Gender)
			{
				parameters.Add(key, (int)(Gender)value);
			}
			else
			{
				parameters.Add(key, value);
			}
			return true;
		}

		public void ClearParameters()
		{
			parameters.Clear();
		}

		private JSONArray ToJsonArray(List<object> value)
		{
			JSONArray jSONArray = new JSONArray();
			foreach (object item in value)
			{
				if (item is IList)
				{
					jSONArray.Add(ToJsonArray(item as List<object>));
				}
				else if (item is string)
				{
					jSONArray.Add(new JSONData(Uri.EscapeDataString(item.ToString())));
				}
				else
				{
					jSONArray.Add(new JSONData(item));
				}
			}
			return jSONArray;
		}

		public bool NeedToSend(List<string> excluded)
		{
			int num = 0;
			foreach (KeyValuePair<string, object> parameter in parameters)
			{
				if ((excluded == null || !excluded.Contains(parameter.Key)) && !parameter.Key.Equals(Event.IN_PROGRESS))
				{
					num++;
				}
			}
			if (num <= 0)
			{
				return IsRemoved;
			}
			return true;
		}

		public Dictionary<string, object> Merge(PeopleEvent peopleEvent, bool ageGenderAndCheaterOnly)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			foreach (KeyValuePair<string, object> parameter in peopleEvent.parameters)
			{
				if (!ageGenderAndCheaterOnly)
				{
					if (!parameters.ContainsKey(parameter.Key))
					{
						parameters.Add(parameter.Key, parameter.Value);
					}
					dictionary.Add(parameter.Key, parameter.Value);
				}
				else if (parameter.Key.Equals(PeopleLogic.CHEATER_KEY))
				{
					if (!parameters.ContainsKey(parameter.Key))
					{
						parameters.Add(parameter.Key, parameter.Value);
					}
					dictionary.Add(parameter.Key, parameter.Value);
				}
			}
			return dictionary;
		}

		public override JSONNode GetAdditionalDataJson()
		{
			List<string> excludedUserData = SDKClient.Instance.NetworkStorage.ExcludedUserData;
			JSONArray jSONArray = null;
			if (IsRemoved)
			{
				JSONClass jSONClass = new JSONClass();
				jSONClass.Add(Event.TIMESTAMP, new JSONData(DeviceHelper.Instance.GetUnixTime() / 1000));
				object aData = null;
				jSONClass.Add(PeopleLogic.DATA_KEY, new JSONData(aData));
				IsRemoved = false;
				AddPendingToJSON(jSONClass);
				if (!NeedToSend(excludedUserData))
				{
					return jSONClass;
				}
				jSONArray = new JSONArray();
				jSONArray.Add(jSONClass);
			}
			JSONClass jSONClass2 = new JSONClass();
			jSONClass2.Add(Event.TIMESTAMP, new JSONData(DeviceHelper.Instance.GetUnixTime() / 1000));
			JSONClass jSONClass3 = new JSONClass();
			jSONClass2.Add(PeopleLogic.DATA_KEY, jSONClass3);
			foreach (KeyValuePair<string, object> parameter in parameters)
			{
				if (!parameter.Key.Equals(Event.IN_PROGRESS))
				{
					if (excludedUserData == null || !excludedUserData.Contains(parameter.Key))
					{
						if (parameter.Value is IList)
						{
							jSONClass3.Add(Uri.EscapeDataString(parameter.Key), ToJsonArray(parameter.Value as List<object>));
						}
						else if (parameter.Value is string)
						{
							jSONClass3.Add(Uri.EscapeDataString(parameter.Key), new JSONData(Uri.EscapeDataString(parameter.Value.ToString())));
						}
						else
						{
							jSONClass3.Add(Uri.EscapeDataString(parameter.Key), new JSONData(parameter.Value));
						}
					}
				}
				else
				{
					AddPendingToJSON(jSONClass2);
				}
			}
			if (jSONArray != null)
			{
				jSONArray.Add(jSONClass2);
				return jSONArray;
			}
			return jSONClass2;
		}
	}
}
