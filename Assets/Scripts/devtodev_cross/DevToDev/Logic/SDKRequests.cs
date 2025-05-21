using System.Text;
using DevToDev.Core.Network;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Builders;
using DevToDev.Core.Utils.Helpers;
using DevToDev.Data.Consts;
using DevToDev.Data.Metrics.Simple;

namespace DevToDev.Logic
{
	internal static class SDKRequests
	{
		public static void GetServerNodeV3(OnRequestSend callback)
		{
			GetServerNodeEvent getServerNodeEvent = new GetServerNodeEvent();
			string text = getServerNodeEvent.GetAdditionalDataJson().ToString();
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			Request request = new RequestBuilder().Url(NetworkConsts.MAIN_SERVER + NetworkConsts.WEB).AddParameter(RequestParam.FUNCTION, NetworkConsts.NODE_REQUEST).AddParameter(RequestParam.ID, SDKClient.Instance.AppKey)
				.AddParameter(RequestParam.UID, DeviceHelper.Instance.GetDeviceId())
				.NeedSigned(false)
				.PostData(bytes)
				.Build();
			Log.D("Send: " + request.Url);
			Log.D("Post: " + text);
			NetworkClient networkClient = new NetworkClient(callback);
			networkClient.Send(request);
		}

		public static void SendStorage(MetricsStorage metricStorage, OnRequestSend callback)
		{
			Log.D("SendStorage");
			string text = metricStorage.PrepareToSend();
			if (string.IsNullOrEmpty(text))
			{
				Log.D("Nothing to send, exiting.");
				return;
			}
			if (string.IsNullOrEmpty(SDKClient.Instance.NetworkStorage.ActiveNodeUrl))
			{
				callback(null, metricStorage);
				return;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			RequestBuilder requestBuilder = new RequestBuilder().Url(SDKClient.Instance.NetworkStorage.ActiveNodeUrl + NetworkConsts.CORE).AddParameter(RequestParam.ID, SDKClient.Instance.AppKey).AddParameter(RequestParam.UID, SDKClient.Instance.UsersStorage.Device.DeviceId);
			if (SDKClient.Instance.UsersStorage.Device.DevicePrev != null)
			{
				requestBuilder.AddParameter(RequestParam.PREV, SDKClient.Instance.UsersStorage.Device.DevicePrev);
			}
			if (SDKClient.Instance.NetworkStorage.UseCustomUdid)
			{
				requestBuilder.AddParameter(RequestParam.USER_CUSTOM_UDID, SDKClient.Instance.UsersStorage.ActiveUser.UserId);
				if (SDKClient.Instance.UsersStorage.ActiveUser.PrevId != null)
				{
					requestBuilder.AddParameter(RequestParam.PREV_USER_CUSTOM_UDID, SDKClient.Instance.UsersStorage.ActiveUser.PrevId);
				}
			}
			Request request = requestBuilder.Secret(SDKClient.Instance.AppSecret).NeedSigned(true).PostData(bytes)
				.Build();
			Log.D("Send: " + request.Url);
			Log.D("Post: " + text);
			NetworkClient networkClient = new NetworkClient(callback);
			networkClient.Send(request, metricStorage);
		}
	}
}
