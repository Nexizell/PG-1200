using System;
using System.Collections;
using System.Collections.Generic;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Data.Metrics.Specific;

namespace DevToDev.Logic
{
	internal class PeopleLogic : ISaveable
	{
		public static readonly string GENDER_KEY = "gender";

		public static readonly string AGE_KEY = "age";

		public static readonly string CHEATER_KEY = "cheater";

		public static readonly string NAME_KEY = "name";

		public static readonly string EMAIL_KEY = "email";

		public static readonly string PHONE_KEY = "phone";

		public static readonly string PHOTO_KEY = "photo";

		public static readonly string DATA_KEY = "data";

		private Dictionary<string, object> data;

		private PeopleEvent peopleEvent;

		public PeopleEvent PeopleEvent
		{
			get
			{
				return peopleEvent;
			}
		}

		public Gender Gender
		{
			set
			{
				if (AddOrReplace(GENDER_KEY, (int)value))
				{
					peopleEvent.AddOrReplace(GENDER_KEY, (int)value);
					SaveForWeb();
					Log.R("Set " + GENDER_KEY + " with value " + value);
				}
			}
		}

		public int Age
		{
			set
			{
				if (AddOrReplace(AGE_KEY, value))
				{
					peopleEvent.AddOrReplace(AGE_KEY, value);
					SaveForWeb();
					Log.R("Set " + AGE_KEY + " with value " + value);
				}
			}
		}

		public bool Cheater
		{
			set
			{
				if (AddOrReplace(CHEATER_KEY, value))
				{
					peopleEvent.AddOrReplace(CHEATER_KEY, value);
					SDKClient.Instance.SaveAll();
					Log.R("Set " + CHEATER_KEY + " with value " + value);
				}
			}
		}

		public string Name
		{
			set
			{
				if (!string.IsNullOrEmpty(value) && value.Length > 128)
				{
					Log.R("Value must be no longer then 128 symbols");
				}
				else if (AddOrReplace(NAME_KEY, value))
				{
					peopleEvent.AddOrReplace(NAME_KEY, value);
					SaveForWeb();
					Log.R("Set " + NAME_KEY + " with value " + value);
				}
			}
		}

		public string Email
		{
			set
			{
				if (!string.IsNullOrEmpty(value) && value.Length > 128)
				{
					Log.R("Value must be no longer then 128 symbols");
				}
				else if (AddOrReplace(EMAIL_KEY, value))
				{
					peopleEvent.AddOrReplace(EMAIL_KEY, value);
					SaveForWeb();
					Log.R("Set " + EMAIL_KEY + " with value " + value);
				}
			}
		}

		public string Phone
		{
			set
			{
				if (!string.IsNullOrEmpty(value) && value.Length > 128)
				{
					Log.R("Value must be no longer then 128 symbols");
				}
				else if (AddOrReplace(PHONE_KEY, value))
				{
					peopleEvent.AddOrReplace(PHONE_KEY, value);
					SaveForWeb();
					Log.R("Set " + PHONE_KEY + " with value " + value);
				}
			}
		}

		public string Photo
		{
			set
			{
				if (!string.IsNullOrEmpty(value) && value.Length > 256)
				{
					Log.R("Value must be no longer then 256 symbols");
				}
				else if (AddOrReplace(PHOTO_KEY, value))
				{
					peopleEvent.AddOrReplace(PHOTO_KEY, value);
					SaveForWeb();
					Log.R("Set " + PHOTO_KEY + " with value " + value);
				}
			}
		}

		public PeopleLogic()
		{
			peopleEvent = new PeopleEvent();
			data = new Dictionary<string, object>();
		}

		public void PeopleEventSent()
		{
			peopleEvent = new PeopleEvent();
		}

		public PeopleLogic(ObjectInfo info)
		{
			try
			{
				data = info.GetValue("data", typeof(Dictionary<string, object>)) as Dictionary<string, object>;
				peopleEvent = info.GetValue("peopleEvent", typeof(PeopleEvent)) as PeopleEvent;
			}
			catch (Exception ex)
			{
				Log.D("Error in desealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public override void GetObjectData(ObjectInfo info)
		{
			try
			{
				info.AddValue("data", data);
				info.AddValue("peopleEvent", peopleEvent);
			}
			catch (Exception ex)
			{
				Log.D("Error in sealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		private bool isPrimitive(object obj)
		{
			if (obj is long)
			{
				return true;
			}
			if (obj is int)
			{
				return true;
			}
			if (obj is byte)
			{
				return true;
			}
			if (obj is short)
			{
				return true;
			}
			if (obj is double)
			{
				return true;
			}
			if (obj is float)
			{
				return true;
			}
			if (obj is Gender)
			{
				return true;
			}
			return false;
		}

		public bool AddOrReplace(string key, object value)
		{
			if (data.ContainsKey(key))
			{
				if (!data[key].Equals(value))
				{
					if (value is Gender)
					{
						data[key] = (int)(Gender)value;
					}
					else
					{
						data[key] = value;
					}
					return true;
				}
				return false;
			}
			if (value is Gender)
			{
				data.Add(key, (int)(Gender)value);
			}
			else
			{
				data.Add(key, value);
			}
			return true;
		}

		private void SaveForWeb()
		{
			if (UnityPlayerPlatform.isUnityWebPlatform())
			{
				SDKClient.Instance.SaveAll();
			}
		}

		private bool IsKeyAllowed(string key, object value)
		{
			if (string.IsNullOrEmpty(key))
			{
				Log.R("Key must not be null or empty string.");
				return false;
			}
			if (key.ToLower().Equals(GENDER_KEY))
			{
				if (value is Gender || (value is int && (int)value < 3 && (int)value >= 0))
				{
					return true;
				}
				Log.R("Gender value must be a Gender: Unknown, Male, Female or an int: 0, 1, 2 resp.");
				return false;
			}
			if (key.ToLower().Equals(AGE_KEY))
			{
				if (value is int)
				{
					return true;
				}
				Log.R("Age value must be an int.");
				return false;
			}
			if (key.ToLower().Equals(CHEATER_KEY))
			{
				if (value is bool)
				{
					return true;
				}
				Log.R("Cheater value must be a bool.");
				return false;
			}
			if (key.ToLower().Equals(EMAIL_KEY) || key.ToLower().Equals(PHONE_KEY) || key.ToLower().Equals(NAME_KEY))
			{
				if (value is string && (value as string).Length < 128)
				{
					return true;
				}
				Log.R("Value must have a stirng type and be no longer then 128 symbols.");
				return false;
			}
			if (key.ToLower().Equals(PHOTO_KEY))
			{
				if (value is string && (value as string).Length < 256)
				{
					return true;
				}
				Log.R("Value must have a stirng type and be no longer then 256 symbols.");
				return false;
			}
			return true;
		}

		private List<object> RebuildList(IList oldList)
		{
			List<object> list = new List<object>();
			foreach (object old in oldList)
			{
				if (isPrimitive(old))
				{
					list.Add(old);
				}
				else if (old is IList)
				{
					list.Add(RebuildList(old as IList));
				}
				else
				{
					list.Add(old.ToString());
				}
			}
			return list;
		}

		public void SetUserData(string key, object value, bool save = true)
		{
			if (!IsKeyAllowed(key, value))
			{
				return;
			}
			if (isPrimitive(value))
			{
				if (AddOrReplace(key, value))
				{
					peopleEvent.AddOrReplace(key, value);
				}
			}
			else if (value is IList)
			{
				List<object> value2 = RebuildList(value as IList);
				if (AddOrReplace(key, value2))
				{
					peopleEvent.AddOrReplace(key, value2);
				}
			}
			else if (AddOrReplace(key, value.ToString()))
			{
				peopleEvent.AddOrReplace(key, value.ToString());
			}
			if (save)
			{
				SaveForWeb();
			}
			Log.R(key + " property was set successfully.");
		}

		public void SetUserData(Dictionary<string, object> userData)
		{
			foreach (KeyValuePair<string, object> userDatum in userData)
			{
				SetUserData(userDatum.Key, userDatum.Value, false);
			}
			SaveForWeb();
		}

		public void ClearUserData()
		{
			peopleEvent.IsRemoved = true;
			data.Clear();
			peopleEvent.ClearParameters();
			SaveForWeb();
			Log.R("User profile was erased successfully.");
		}

		public void UnsetUserData(string key, bool save = true)
		{
			if (string.IsNullOrEmpty(key))
			{
				Log.R("Key must not be null or empty string.");
				return;
			}
			if (data.ContainsKey(key))
			{
				data.Remove(key);
			}
			peopleEvent.AddOrReplace(key, null);
			if (save)
			{
				SaveForWeb();
			}
			Log.R(key + " property was unset successfully.");
		}

		public void UnsetUserData(List<string> keysToRemove)
		{
			foreach (string item in keysToRemove)
			{
				UnsetUserData(item, false);
			}
			SaveForWeb();
		}

		public void AppendUserData(string key, object value, bool save = true)
		{
			if (!data.ContainsKey(key) || data[key] == null)
			{
				if (value is IList)
				{
					SetUserData(key, value);
					return;
				}
				List<object> list = new List<object>();
				list.Add(value);
				SetUserData(key, list);
				if (save)
				{
					SaveForWeb();
				}
				return;
			}
			object obj = data[key];
			if (!(obj is List<object>))
			{
				Log.R("Type mismatch. " + key + " must be of List type.");
				return;
			}
			if (value is IList)
			{
				List<object> collection = RebuildList(value as IList);
				(obj as List<object>).AddRange(collection);
			}
			else if (isPrimitive(value))
			{
				(obj as List<object>).Add(value);
			}
			else
			{
				(obj as List<object>).Add(value.ToString());
			}
			peopleEvent.AddOrReplace(key, data[key]);
			if (save)
			{
				SaveForWeb();
			}
			Log.R("Values were appended successfully to propetry " + key);
		}

		public void AppendUserData(Dictionary<string, object> userData)
		{
			foreach (KeyValuePair<string, object> userDatum in userData)
			{
				AppendUserData(userDatum.Key, userDatum.Value, false);
			}
			SaveForWeb();
		}

		public void UnionUserData(string key, object value, bool save = true)
		{
			if (!data.ContainsKey(key) || data[key] == null)
			{
				if (value is IList)
				{
					SetUserData(key, value);
					return;
				}
				List<object> list = new List<object>();
				list.Add(value);
				SetUserData(key, list);
				if (save)
				{
					SaveForWeb();
				}
				return;
			}
			bool flag = false;
			object obj = data[key];
			if (!(obj is IList))
			{
				Log.R("Type mismatch. " + key + " must be of List type.");
				return;
			}
			if (value is IList)
			{
				List<object> list2 = RebuildList(value as IList);
				foreach (object item in list2)
				{
					if (!(obj as List<object>).Contains(item))
					{
						(obj as List<object>).Add(item);
						flag = true;
					}
				}
			}
			else if (isPrimitive(value))
			{
				if (!(obj as List<object>).Contains(value))
				{
					(obj as List<object>).Add(value);
					flag = true;
				}
			}
			else if (!(obj as List<object>).Contains(value.ToString()))
			{
				(obj as List<object>).Add(value.ToString());
				flag = true;
			}
			if (flag)
			{
				peopleEvent.AddOrReplace(key, data[key]);
			}
			if (save)
			{
				SaveForWeb();
			}
			Log.R("Values were united successfully for propetry " + key);
		}

		public void UnionUserData(Dictionary<string, object> userData)
		{
			foreach (KeyValuePair<string, object> userDatum in userData)
			{
				UnionUserData(userDatum.Key, userDatum.Value, false);
			}
			SaveForWeb();
		}

		public void Increment(string key, object value, bool save = true)
		{
			if (string.IsNullOrEmpty(key))
			{
				Log.R("Key must not be null or empty string.");
				return;
			}
			if (!isPrimitive(value))
			{
				Log.R("Type mismatch. Value must be of Number(double, int, etc) type.");
				return;
			}
			if (!data.ContainsKey(key) || data[key] == null)
			{
				SetUserData(key, value);
				if (save)
				{
					SaveForWeb();
				}
				return;
			}
			if (data[key] is float || data[key] is double || value is float || value is double)
			{
				data[key] = double.Parse(data[key].ToString()) + double.Parse(value.ToString());
			}
			else
			{
				if (!isPrimitive(data[key]))
				{
					Log.R("Type mismatch. Value with name " + key + " must be of Number(double, int, etc) type.");
					return;
				}
				data[key] = long.Parse(data[key].ToString()) + long.Parse(value.ToString());
			}
			peopleEvent.AddOrReplace(key, data[key]);
			if (save)
			{
				SaveForWeb();
			}
			Log.R("Values were incremented successfully for propetry " + key);
		}

		public void Increment(Dictionary<string, object> userData)
		{
			foreach (KeyValuePair<string, object> userDatum in userData)
			{
				Increment(userDatum.Key, userDatum.Value, false);
			}
			SaveForWeb();
		}

		public bool NeedToSend(List<string> excluded)
		{
			return peopleEvent.NeedToSend(excluded);
		}

		private void Merge(PeopleEvent peopleEvent, bool ageGenderAndCheaterOnly)
		{
			foreach (KeyValuePair<string, object> item in this.peopleEvent.Merge(peopleEvent, ageGenderAndCheaterOnly))
			{
				if (!data.ContainsKey(item.Key))
				{
					data.Add(item.Key, item.Value);
				}
			}
		}

		public void Merge(List<PeopleEvent> peopleEvents)
		{
			bool flag = false;
			foreach (PeopleEvent peopleEvent in peopleEvents)
			{
				if (!flag)
				{
					Merge(peopleEvent, false);
				}
				else
				{
					Merge(peopleEvent, true);
				}
				if (peopleEvent.IsRemoved)
				{
					this.peopleEvent.IsRemoved = true;
					flag = true;
				}
			}
		}
	}
}
