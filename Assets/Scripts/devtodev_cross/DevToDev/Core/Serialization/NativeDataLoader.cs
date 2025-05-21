using System;
using DevToDev.Core.Utils;
using DevToDev.Logic;
using DevToDev.Logic.Cross;

namespace DevToDev.Core.Serialization
{
	internal class NativeDataLoader
	{
		public void Load(UsersStorage usersStorage)
		{
			try
			{
				string text = NativeDataLoaderPlatform.Load(SDKClient.Instance.AppKey);
				if (text == null)
				{
					return;
				}
				Log.D("Native Json Data: " + text);
				JSONNode jSONNode = null;
				try
				{
					jSONNode = JSON.Parse(text);
					DecodeData(usersStorage, jSONNode);
				}
				catch (Exception ex)
				{
					Log.E("Error loading native data: \n" + ex.Message + "\n" + ex.StackTrace);
				}
			}
			catch (Exception ex2)
			{
				Log.E("Error loading native data: \n" + ex2.Message + "\n" + ex2.StackTrace);
			}
			RemoveNativeData();
		}

		private void DecodeData(UsersStorage usersStorage, JSONNode data)
		{
			usersStorage.LoadNativeData(data);
		}

		public void RemoveNativeData()
		{
			try
			{
				NativeDataLoaderPlatform.Clear(SDKClient.Instance.AppKey);
			}
			catch
			{
			}
		}
	}
}
