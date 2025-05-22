using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

internal sealed class MobileAdManager
{
	public enum Type
	{
		Image = 0,
		Video = 1
	}

	public enum State
	{
		None = 0,
		Idle = 1,
		Loaded = 2
	}

	internal enum SampleGroup
	{
		Unknown = 0,
		Video = 1,
		Image = 2
	}

	private static byte[] _guid = new byte[0];

	private int _imageAdUnitIdIndex;

	private int _imageIdGroupIndex;

	private int _videoAdUnitIdIndex;

	private int _videoIdGroupIndex;

	internal const string TextInterstitialUnitId = "ca-app-pub-5590536419057381/7885668153";

	internal const string DefaultImageInterstitialUnitId = "ca-app-pub-5590536419057381/1950086558";

	internal const string DefaultVideoInterstitialUnitId = "ca-app-pub-5590536419057381/2096360557";

	private static readonly Rilisoft.Lazy<MobileAdManager> _instance = new Rilisoft.Lazy<MobileAdManager>(() => new MobileAdManager());

	public static MobileAdManager Instance
	{
		get
		{
			return _instance.Value;
		}
	}

	public State VideoInterstitialState
	{
		get
		{
			return State.None;
		}
	}

	public string ImageAdFailedToLoadMessage { get; private set; }

	public string VideoAdFailedToLoadMessage { get; private set; }

	internal bool SuppressShowOnReturnFromPause { get; set; }

	internal static byte[] GuidBytes
	{
		get
		{
			if (_guid != null && _guid.Length != 0)
			{
				return _guid;
			}
			if (PlayerPrefs.HasKey("Guid"))
			{
				try
				{
					_guid = new Guid(PlayerPrefs.GetString("Guid")).ToByteArray();
				}
				catch
				{
					Guid guid = Guid.NewGuid();
					_guid = guid.ToByteArray();
					PlayerPrefs.SetString("Guid", guid.ToString("D"));
					PlayerPrefs.Save();
				}
			}
			else
			{
				Guid guid2 = Guid.NewGuid();
				_guid = guid2.ToByteArray();
				PlayerPrefs.SetString("Guid", guid2.ToString("D"));
				PlayerPrefs.Save();
			}
			return _guid;
		}
	}

	private string ImageInterstitialUnitId
	{
		get
		{
			if (PromoActionsManager.MobileAdvert == null || PromoActionsManager.MobileAdvert.AdmobImageAdUnitIds.Count == 0)
			{
				return "ca-app-pub-5590536419057381/1950086558";
			}
			return AdmobImageAdUnitIds[_imageAdUnitIdIndex % AdmobImageAdUnitIds.Count];
		}
	}

	private string VideoInterstitialUnitId
	{
		get
		{
			if (PromoActionsManager.MobileAdvert == null)
			{
				return "ca-app-pub-5590536419057381/2096360557";
			}
			if (AdmobVideoAdUnitIds.Count == 0)
			{
				if (!string.IsNullOrEmpty(PromoActionsManager.MobileAdvert.AdmobVideoAdUnitId))
				{
					return PromoActionsManager.MobileAdvert.AdmobVideoAdUnitId;
				}
				return "ca-app-pub-5590536419057381/2096360557";
			}
			return AdmobVideoAdUnitIds[_videoAdUnitIdIndex % AdmobVideoAdUnitIds.Count];
		}
	}

	private List<string> AdmobVideoAdUnitIds
	{
		get
		{
			if (PromoActionsManager.MobileAdvert.AdmobVideoIdGroups.Count == 0)
			{
				return PromoActionsManager.MobileAdvert.AdmobVideoAdUnitIds;
			}
			return PromoActionsManager.MobileAdvert.AdmobVideoIdGroups[_videoIdGroupIndex % PromoActionsManager.MobileAdvert.AdmobVideoIdGroups.Count];
		}
	}

	private List<string> AdmobImageAdUnitIds
	{
		get
		{
			if (PromoActionsManager.MobileAdvert.AdmobImageIdGroups.Count == 0)
			{
				return PromoActionsManager.MobileAdvert.AdmobImageAdUnitIds;
			}
			return PromoActionsManager.MobileAdvert.AdmobImageIdGroups[_imageIdGroupIndex % PromoActionsManager.MobileAdvert.AdmobImageIdGroups.Count];
		}
	}

	internal int ImageAdUnitIndexClamped
	{
		get
		{
			if (AdmobImageAdUnitIds.Count == 0)
			{
				return -1;
			}
			return _imageAdUnitIdIndex % AdmobImageAdUnitIds.Count;
		}
	}

	internal int VideoAdUnitIndexClamped
	{
		get
		{
			if (AdmobVideoAdUnitIds.Count == 0)
			{
				return -1;
			}
			return _videoAdUnitIdIndex % AdmobVideoAdUnitIds.Count;
		}
	}

	private MobileAdManager()
	{
		ImageAdFailedToLoadMessage = string.Empty;
		VideoAdFailedToLoadMessage = string.Empty;
	}

	public static string GetReasonToDismissVideoChestInLobby()
	{
		AdsConfigMemento lastLoadedConfig = AdsConfigManager.Instance.LastLoadedConfig;
		if (lastLoadedConfig == null)
		{
			return "Ads config is `null`.";
		}
		if (lastLoadedConfig.Exception != null)
		{
			return lastLoadedConfig.Exception.Message;
		}
		string videoDisabledReason = AdsConfigManager.GetVideoDisabledReason(lastLoadedConfig);
		if (!string.IsNullOrEmpty(videoDisabledReason))
		{
			return videoDisabledReason;
		}
		ChestInLobbyPointMemento chestInLobby = lastLoadedConfig.AdPointsConfig.ChestInLobby;
		if (chestInLobby == null)
		{
			return string.Format("`{0}` config is `null`", new object[1] { chestInLobby.Id });
		}
		string playerCategory = AdsConfigManager.GetPlayerCategory(lastLoadedConfig);
		string disabledReason = chestInLobby.GetDisabledReason(playerCategory);
		if (!string.IsNullOrEmpty(disabledReason))
		{
			return disabledReason;
		}
		int lobbyLevel = ExpController.LobbyLevel;
		if (lobbyLevel < 3)
		{
			return string.Format(CultureInfo.InvariantCulture, "lobbyLevel: {0} < 3", lobbyLevel);
		}
		return string.Empty;
	}

	public void DestroyImageInterstitial()
	{
	}

	public void DestroyVideoInterstitial()
	{
	}

	public static bool UserPredicate(Type adType, bool verbose, bool showToPaying = false, bool showToNew = false)
	{
		bool flag = IsNewUser();
		bool flag2 = IsPayingUser();
		bool flag8;
		if (adType == Type.Video)
		{
			int lobbyLevel = ExpController.LobbyLevel;
			bool flag3 = lobbyLevel >= 3;
			bool num = PromoActionsManager.MobileAdvert != null && PromoActionsManager.MobileAdvert.VideoEnabled;
			bool flag4 = PromoActionsManager.MobileAdvert != null && PromoActionsManager.MobileAdvert.VideoShowPaying;
			bool flag5 = PromoActionsManager.MobileAdvert != null && PromoActionsManager.MobileAdvert.VideoShowNonpaying;
			bool flag6 = (flag2 && flag4) || (!flag2 && flag5);
			bool flag7 = PlayerPrefs.GetInt("CountRunMenu", 0) >= 3;
			flag8 = num && flag7 && flag6 && flag3;
			if (verbose)
			{
				Debug.LogFormat("AdIsApplicable ({0}): {1}    Paying: {2},  Need to show: {3},  Session count satisfied: {4},  Lobby level: {5}", adType, flag8, flag2, flag2 ? flag4 : flag5, flag7, lobbyLevel);
			}
		}
		else
		{
			bool flag9 = IsLongTimeShowBaner();
			flag8 = PromoActionsManager.MobileAdvert != null && PromoActionsManager.MobileAdvert.ImageEnabled && (!flag || showToNew) && (!flag2 || showToPaying) && flag9;
			if (verbose)
			{
				Dictionary<string, bool> obj = new Dictionary<string, bool>(6)
				{
					{
						"ImageEnabled",
						PromoActionsManager.MobileAdvert != null && PromoActionsManager.MobileAdvert.ImageEnabled
					},
					{ "isNewUser", flag },
					{ "showToNew", showToNew },
					{ "isPayingUser", flag2 },
					{ "showToPaying", showToPaying },
					{ "longTimeShowBanner", flag9 }
				};
				Debug.Log(string.Format("AdIsApplicable ({0}): {1}    Details: {2}", new object[3]
				{
					adType,
					flag8,
					Json.Serialize(obj)
				}));
			}
		}
		return flag8;
	}

	internal static void RefreshBytes()
	{
		PlayerPrefs.SetString("Guid", new Guid(_guid).ToString("D"));
		PlayerPrefs.Save();
	}

	internal static SampleGroup GetSempleGroup()
	{
		if (GuidBytes[0] % 2 != 0)
		{
			return SampleGroup.Video;
		}
		return SampleGroup.Image;
	}

	public static bool IsNewUserOldMetod()
	{
		string @string = PlayerPrefs.GetString("First Launch (Advertisement)", string.Empty);
		DateTimeOffset result;
		if (!string.IsNullOrEmpty(@string) && DateTimeOffset.TryParse(@string, out result))
		{
			return (DateTimeOffset.Now - result).TotalDays < 7.0;
		}
		return true;
	}

	private static bool IsLongTimeShowBaner()
	{
		string @string = PlayerPrefs.GetString(Defs.LastTimeShowBanerKey, string.Empty);
		if (string.IsNullOrEmpty(@string))
		{
			return true;
		}
		DateTime result;
		if (!DateTime.TryParse(@string, out result))
		{
			return false;
		}
		DateTime? serverTime = FriendsController.GetServerTime();
		if (!serverTime.HasValue)
		{
			return false;
		}
		return (serverTime.Value - result).TotalSeconds > (double)PromoActionsManager.MobileAdvert.TimeoutBetweenShowInterstitial;
	}

	private static bool IsNewUser()
	{
		if (PlayerPrefs.GetInt(Defs.SessionDayNumberKey, 1) > PromoActionsManager.MobileAdvert.CountSessionNewPlayer)
		{
			return false;
		}
		return true;
	}

	public static bool IsPayingUser()
	{
		return StoreKitEventListener.IsPayingUser();
	}

	internal bool SwitchImageAdUnitId()
	{
		int imageAdUnitIdIndex = _imageAdUnitIdIndex;
		string imageInterstitialUnitId = ImageInterstitialUnitId;
		_imageAdUnitIdIndex++;
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log(string.Format("Switching image ad unit id from {0} ({1}) to {2} ({3})", imageAdUnitIdIndex, RemovePrefix(imageInterstitialUnitId), _imageAdUnitIdIndex, RemovePrefix(ImageInterstitialUnitId)));
		}
		if (PromoActionsManager.MobileAdvert.AdmobImageAdUnitIds.Count != 0)
		{
			return _imageAdUnitIdIndex % PromoActionsManager.MobileAdvert.AdmobImageAdUnitIds.Count == 0;
		}
		return true;
	}

	internal bool SwitchVideoAdUnitId()
	{
		int videoAdUnitIdIndex = _videoAdUnitIdIndex;
		string videoInterstitialUnitId = VideoInterstitialUnitId;
		_videoAdUnitIdIndex++;
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log(string.Format("Switching video ad unit id from {0} ({1}) to {2} ({3}); group index {4}", videoAdUnitIdIndex, RemovePrefix(videoInterstitialUnitId), _videoAdUnitIdIndex, RemovePrefix(VideoInterstitialUnitId), _videoIdGroupIndex));
		}
		if (AdmobVideoAdUnitIds.Count != 0)
		{
			return _videoAdUnitIdIndex % AdmobVideoAdUnitIds.Count == 0;
		}
		return true;
	}

	internal bool SwitchImageIdGroup()
	{
		int imageIdGroupIndex = _imageIdGroupIndex;
		string text = Json.Serialize(AdmobImageAdUnitIds.Select(RemovePrefix).ToList());
		_imageIdGroupIndex++;
		_imageAdUnitIdIndex = 0;
		string text2 = Json.Serialize(AdmobImageAdUnitIds.Select(RemovePrefix).ToList());
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log(string.Format("Switching image id group from {0} ({1}) to {2} ({3})", imageIdGroupIndex, text, _imageIdGroupIndex, text2));
		}
		if (PromoActionsManager.MobileAdvert.AdmobImageIdGroups.Count != 0)
		{
			return _imageIdGroupIndex % PromoActionsManager.MobileAdvert.AdmobImageIdGroups.Count == 0;
		}
		return true;
	}

	internal bool SwitchVideoIdGroup()
	{
		int videoIdGroupIndex = _videoIdGroupIndex;
		string text = Json.Serialize(AdmobVideoAdUnitIds.Select(RemovePrefix).ToList());
		_videoIdGroupIndex++;
		_videoAdUnitIdIndex = 0;
		string text2 = Json.Serialize(AdmobVideoAdUnitIds.Select(RemovePrefix).ToList());
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log(string.Format("Switching video id group from {0} ({1}) to {2} ({3})", videoIdGroupIndex, text, _videoIdGroupIndex, text2));
		}
		if (PromoActionsManager.MobileAdvert.AdmobVideoIdGroups.Count != 0)
		{
			return _videoIdGroupIndex % PromoActionsManager.MobileAdvert.AdmobVideoIdGroups.Count == 0;
		}
		return true;
	}

	internal static string RemovePrefix(string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return string.Empty;
		}
		int num = s.IndexOf('/');
		if (num <= 0)
		{
			return s;
		}
		return s.Remove(0, num);
	}

	internal bool ResetVideoAdUnitId()
	{
		int videoAdUnitIdIndex = _videoAdUnitIdIndex;
		string videoInterstitialUnitId = VideoInterstitialUnitId;
		int videoIdGroupIndex = _videoIdGroupIndex;
		_videoAdUnitIdIndex = 0;
		_videoIdGroupIndex = 0;
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log(string.Format("Resetting video group from {0} to {1}", new object[2] { videoIdGroupIndex, _videoIdGroupIndex }));
		}
		return true;
	}

	internal bool ResetImageAdUnitId()
	{
		int imageAdUnitIdIndex = _imageAdUnitIdIndex;
		string imageInterstitialUnitId = ImageInterstitialUnitId;
		int imageIdGroupIndex = _imageIdGroupIndex;
		_imageAdUnitIdIndex = 0;
		_imageIdGroupIndex = 0;
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log(string.Format("Resetting image ad unit id from {0} to {1}; group index from {2} to 0", new object[3] { imageAdUnitIdIndex, _imageAdUnitIdIndex, imageIdGroupIndex }));
		}
		return true;
	}
}
