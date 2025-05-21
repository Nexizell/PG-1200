using System;
using DevToDev.Core.Network;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Builders;
using DevToDev.Core.Utils.Helpers;
using DevToDev.Data.Consts;
using DevToDev.Logic;

namespace DevToDev.Cheat
{
	internal class TimeManager
	{
		private static readonly string TIME = "time";

		private static TimeManager instance;

		private long savedTimestamp;

		private long serverTimestamp;

		private long currentTimestamp;

		private long deltaTime;

		public long SavedTimestamp
		{
			get
			{
				return savedTimestamp;
			}
		}

		public long ServerTimestamp
		{
			get
			{
				return serverTimestamp;
			}
			set
			{
				serverTimestamp = value;
			}
		}

		public long CurrentTimestamp
		{
			get
			{
				return currentTimestamp;
			}
		}

		public long DeltaTime
		{
			get
			{
				return deltaTime;
			}
			set
			{
				deltaTime = value;
			}
		}

		public static TimeManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new TimeManager();
				}
				return instance;
			}
		}

		private TimeManager()
		{
			currentTimestamp = DeviceHelper.Instance.GetUnixTime() / 1000;
		}

		public void CheckTime(OnTimeVerifyCallback callback)
		{
			savedTimestamp = LoadSavedTimestamp();
			SaveTimestamp();
			if (callback == null)
			{
				return;
			}
			if (serverTimestamp == 0)
			{
				LoadServerTimestamp(callback);
				return;
			}
			SDKClient.Instance.AsyncOperationDispatcher.DispatchOnMainThread(delegate
			{
				callback(GetTimeStatus());
			});
		}

		private void OnTimeGot(Response response, object state)
		{
			if (response.ResponseState != 0)
			{
				return;
			}
			try
			{
				JSONClass jSONClass = JSON.Parse(response.ResposeString) as JSONClass;
				if (jSONClass == null)
				{
					Log.E("Server error occured in time request.");
					return;
				}
				long asLong = jSONClass[TIME].AsLong;
				serverTimestamp = asLong;
			}
			catch (Exception)
			{
				Log.E("Server error occured in time request.");
			}
			CheckTime(state as OnTimeVerifyCallback);
		}

		private void LoadServerTimestamp(OnTimeVerifyCallback callback)
		{
			Request request = new RequestBuilder().Url(NetworkConsts.MAIN_SERVER + NetworkConsts.WEB).AddParameter(RequestParam.ID, SDKClient.Instance.AppKey).AddParameter(RequestParam.UID, SDKClient.Instance.UsersStorage.Device.DeviceId)
				.AddParameter(RequestParam.FUNCTION, CheatNetworkConst.CHECK_TIMESTAMP)
				.Secret(SDKClient.Instance.AppSecret)
				.NeedSigned(true)
				.Build();
			Log.D("Send: " + request.Url);
			NetworkClient networkClient = new NetworkClient(OnTimeGot);
			networkClient.Send(request, callback);
		}

		private long LoadSavedTimestamp()
		{
			CheatData cheatData = new CheatData().Load() as CheatData;
			return cheatData.LocalTime;
		}

		private void SaveTimestamp()
		{
			new CheatData().Save(new CheatData
			{
				LocalTime = savedTimestamp
			});
		}

		private TimeVerificationStatus GetTimeStatus()
		{
			currentTimestamp = DeviceHelper.Instance.GetUnixTime() / 1000;
			bool flag = savedTimestamp > currentTimestamp + 3600;
			bool flag2 = serverTimestamp > currentTimestamp + 3600;
			if (flag || flag2)
			{
				return TimeVerificationStatus.TimeRewind;
			}
			if (serverTimestamp + 3600 < currentTimestamp)
			{
				return TimeVerificationStatus.TimeForward;
			}
			return TimeVerificationStatus.TimeValid;
		}
	}
}
