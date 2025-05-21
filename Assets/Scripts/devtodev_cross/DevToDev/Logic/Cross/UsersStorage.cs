using System;
using System.Collections.Generic;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Helpers;
using DevToDev.Data.Metrics;

namespace DevToDev.Logic.Cross
{
	internal class UsersStorage : IStorageable<UsersStorage>
	{
		public new static int SerialVersionId = 1;

		private string activeUserId;

		private Dictionary<string, User> users;

		private Device device;

		private long installDate;

		private string applicationVersion;

		public PeopleLogic ActivePeople
		{
			get
			{
				return ActiveUser.ActivePeople;
			}
		}

		public string ActiveUserId
		{
			get
			{
				return activeUserId;
			}
			set
			{
				if (value != null && !value.Equals(activeUserId))
				{
					if (device != null)
					{
						device.CheckDeviceChanges();
					}
					if (users.ContainsKey(value))
					{
						ActiveUser.MetricStorage.ClearUnfinishedProgressionEvent();
						activeUserId = value;
						users[value].CheckDeviceChanges(device);
					}
					else if (string.IsNullOrEmpty(activeUserId))
					{
						User activeUser = ActiveUser;
						users.Remove(activeUserId);
						users.Add(value, activeUser);
						users[value].CheckDeviceChanges(device);
						activeUserId = value;
					}
					else if (users.ContainsKey(string.Empty))
					{
						ActiveUser.MetricStorage.ClearUnfinishedProgressionEvent();
						User value2 = users[string.Empty];
						users.Remove(string.Empty);
						users.Add(value, value2);
						users[value].CheckDeviceChanges(device);
						activeUserId = value;
					}
					else
					{
						ActiveUser.MetricStorage.ClearUnfinishedProgressionEvent();
						activeUserId = value;
						users.Add(value, new User(string.IsNullOrEmpty(value) ? device.DeviceId : value));
						users[value].CheckDeviceChanges(device);
					}
					if (!string.IsNullOrEmpty(value))
					{
						ActiveUser.RegistredTime = DeviceHelper.Instance.GetUnixTime() / 1000;
					}
					users[value].ReplaceId(string.IsNullOrEmpty(value) ? device.DeviceId : value);
					Log.R(string.Format("Changing UserId to {0}, registered time {1}", activeUserId, ActiveUser.RegistredTime));
				}
			}
		}

		public Device Device
		{
			get
			{
				return device;
			}
		}

		public long InstallDate
		{
			get
			{
				return installDate;
			}
		}

		public string ApplicationVersion
		{
			get
			{
				return applicationVersion;
			}
			set
			{
				if (value != null)
				{
					applicationVersion = value;
				}
			}
		}

		public User ActiveUser
		{
			get
			{
				return users[activeUserId];
			}
		}

		public override string StorageName()
		{
			return "usersStorage.dat";
		}

		public override ISaveable GetBlankObject()
		{
			return new UsersStorage();
		}

		public override ISaveable GetObject(byte[] data)
		{
			return new Formatter<UsersStorage>().Load(data);
		}

		public override byte[] SaveObject(ISaveable obj)
		{
			return new Formatter<UsersStorage>().Save(obj as UsersStorage);
		}

		public UsersStorage()
		{
			device = new Device();
			activeUserId = "";
			users = new Dictionary<string, User>();
			users.Add(activeUserId, new User(Device.DeviceId));
			installDate = DeviceHelper.Instance.GetUnixTime() / 1000;
		}

		public void LoadNative()
		{
			new NativeDataLoader().Load(this);
		}

		public UsersStorage(ObjectInfo info)
		{
			try
			{
				activeUserId = info.GetValue("activeUserId", typeof(string)) as string;
				users = info.GetValue("users", typeof(Dictionary<string, User>)) as Dictionary<string, User>;
				device = info.GetValue("device", typeof(Device)) as Device;
				installDate = (long)info.GetValue("installDate", typeof(long));
				applicationVersion = info.GetValue("applicationVersion", typeof(string)) as string;
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
				info.AddValue("activeUserId", activeUserId);
				info.AddValue("users", users);
				info.AddValue("device", device);
				info.AddValue("installDate", installDate);
				info.AddValue("applicationVersion", applicationVersion);
			}
			catch (Exception ex)
			{
				Log.D("Error in sealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public void ForceSendEvents()
		{
			ActiveUser.ForceSendEvents();
		}

		public void OnInitialized(List<Event> futureEvents)
		{
			if (device.CheckDeviceChanges())
			{
				ActiveUser.CheckDeviceChanges(device);
			}
			ActiveUser.OnInitialized(futureEvents);
			futureEvents.Clear();
		}

		public void AddEvents(List<Event> events)
		{
			ActiveUser.AddEvents(events);
		}

		public void AddEvent(Event eventData)
		{
			ActiveUser.AddEvent(eventData);
		}

		public void ReplaceUserId(string fromUserId, string toUserId)
		{
			if (users.ContainsKey(fromUserId))
			{
				device.CheckDeviceChanges();
				User user = users[fromUserId];
				users.Remove(fromUserId);
				if (activeUserId == fromUserId)
				{
					activeUserId = toUserId;
				}
				if (users.ContainsKey(toUserId))
				{
					users.Remove(toUserId);
				}
				users.Add(toUserId, user);
				if (!string.IsNullOrEmpty(toUserId))
				{
					ActiveUser.RegistredTime = DeviceHelper.Instance.GetUnixTime() / 1000;
				}
				user.ReplaceId(string.IsNullOrEmpty(toUserId) ? device.DeviceId : toUserId);
				Log.R(string.Format("Replacing UserId from {0} to {1}", fromUserId, toUserId));
			}
		}

		public void SessionOpen()
		{
			ActiveUser.SessionOpen();
		}

		public void SessionClose(long timestamp, bool inActive = false)
		{
			ActiveUser.SessionClose(timestamp, inActive);
		}

		public void OnPeriodicSend(object sender, EventArgs e)
		{
			ActiveUser.OnPeriodicSend(sender, e);
		}

		public void LoadNativeData(JSONNode data)
		{
			Device.LoadNativeData(data);
			if (data["installDate"] != null)
			{
				string value = data["installDate"].Value;
				long result = 0L;
				if (long.TryParse(value, out result))
				{
					installDate = result;
				}
			}
			if (data["dataStorage"] != null)
			{
				foreach (KeyValuePair<string, JSONNode> item in data["dataStorage"] as JSONClass)
				{
					string key = item.Key;
					Log.D("Adding user: " + key);
					User user = new User(Device.DeviceId);
					user.LoadNativeData(item.Value);
					if (users.ContainsKey(key))
					{
						users.Remove(key);
					}
					users.Add(key, user);
					if (key != null && key.Length > 0)
					{
						user.ReplaceIdSilent(key);
					}
				}
			}
			if (data["userId"] != null)
			{
				string value2 = data["userId"].Value;
				Log.D("Setting user to: " + value2);
				activeUserId = value2;
			}
		}
	}
}
