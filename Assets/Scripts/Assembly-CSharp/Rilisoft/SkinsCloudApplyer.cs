using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	public class SkinsCloudApplyer : CloudApplyer
	{
		[CompilerGenerated]
		internal sealed class _003CApply_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public SkinsCloudApplyer _003C_003E4__this;

			public bool skipApplyingToLocalState;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return _003C_003E2__current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return _003C_003E2__current;
				}
			}

			[DebuggerHidden]
			public _003CApply_003Ed__1(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				if (_003C_003E1__state != 0)
				{
					return false;
				}
				_003C_003E1__state = -1;
				if (_003C_003E4__this.SlotSynchronizer == null)
				{
					UnityEngine.Debug.LogErrorFormat("SkinsCloudApplyer.Apply: SlotSynchronizer == null");
					return false;
				}
				string currentResult = _003C_003E4__this.SlotSynchronizer.CurrentResult;
				if (currentResult == null)
				{
					UnityEngine.Debug.LogErrorFormat("SkinsCloudApplyer currentPullResult == null");
				}
				try
				{
					SkinsMemento cloudSkins = (currentResult.IsNullOrEmpty() ? default(SkinsMemento) : JsonUtility.FromJson<SkinsMemento>(currentResult));
					SkinsMemento localSkins = LoadLocalSkins();
					HashSet<string> hashSet = new HashSet<string>(cloudSkins.DeletedSkins);
					HashSet<string> hashSet2 = new HashSet<string>(localSkins.DeletedSkins);
					HashSet<string> hashSet3 = new HashSet<string>(hashSet);
					hashSet3.UnionWith(hashSet2);
					HashSet<string> hashSet4 = new HashSet<string>(cloudSkins.Skins.Select((SkinMemento s) => s.Id));
					HashSet<string> hashSet5 = new HashSet<string>(localSkins.Skins.Select((SkinMemento s) => s.Id));
					HashSet<string> hashSet6 = new HashSet<string>(hashSet4);
					hashSet6.UnionWith(hashSet5);
					CapeMemento capeMemento = CapeMemento.ChooseCape(localSkins.Cape, cloudSkins.Cape);
					bool flag = hashSet2.Count < hashSet3.Count || hashSet5.Count < hashSet6.Count || localSkins.Cape.Id < capeMemento.Id;
					if (flag && !skipApplyingToLocalState)
					{
						if (hashSet5.Count < hashSet6.Count && Storager.getInt(Defs.SkinsMakerInProfileBought) == 0)
						{
							Storager.setInt(Defs.SkinsMakerInProfileBought, 1);
						}
						OverwriteLocalSkins(localSkins, cloudSkins);
					}
					bool flag2 = hashSet.Count < hashSet3.Count || hashSet4.Count < hashSet6.Count || cloudSkins.Cape.Id < capeMemento.Id;
					if (flag2)
					{
						_003C_003E4__this.SlotSynchronizer.Push(JsonUtility.ToJson(LoadLocalSkins()));
					}
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.LogFormat("SkinsCloudApplyer: Succeeded to apply skins:\n'currentPullResult':{0},\n'cloudDeletedSkins':{1},\n'localDeletedSkins':{2},\n'cloudSkinIds':{3},\n'localSkinIds':{4},\n'localDirty':{5},\n'cloudDirty':{6}", currentResult, Json.Serialize(hashSet.ToList()), Json.Serialize(hashSet2.ToList()), Json.Serialize(hashSet4.ToList()), Json.Serialize(hashSet5.ToList()), flag, flag2);
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogErrorFormat("Exception in SkinsCloudApplyer.Apply: {0}", ex);
				}
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		public SkinsCloudApplyer(CloudSlotSynchronizer synchronizer)
			: base(synchronizer)
		{
		}

		public override IEnumerator Apply(bool skipApplyingToLocalState)
		{
			if (SlotSynchronizer == null)
			{
				UnityEngine.Debug.LogErrorFormat("SkinsCloudApplyer.Apply: SlotSynchronizer == null");
				yield break;
			}
			string currentResult = SlotSynchronizer.CurrentResult;
			if (currentResult == null)
			{
				UnityEngine.Debug.LogErrorFormat("SkinsCloudApplyer currentPullResult == null");
			}
			try
			{
				SkinsMemento cloudSkins = (currentResult.IsNullOrEmpty() ? default(SkinsMemento) : JsonUtility.FromJson<SkinsMemento>(currentResult));
				SkinsMemento localSkins = LoadLocalSkins();
				HashSet<string> hashSet = new HashSet<string>(cloudSkins.DeletedSkins);
				HashSet<string> hashSet2 = new HashSet<string>(localSkins.DeletedSkins);
				HashSet<string> hashSet3 = new HashSet<string>(hashSet);
				hashSet3.UnionWith(hashSet2);
				HashSet<string> hashSet4 = new HashSet<string>(cloudSkins.Skins.Select((SkinMemento s) => s.Id));
				HashSet<string> hashSet5 = new HashSet<string>(localSkins.Skins.Select((SkinMemento s) => s.Id));
				HashSet<string> hashSet6 = new HashSet<string>(hashSet4);
				hashSet6.UnionWith(hashSet5);
				CapeMemento capeMemento = CapeMemento.ChooseCape(localSkins.Cape, cloudSkins.Cape);
				bool flag = hashSet2.Count < hashSet3.Count || hashSet5.Count < hashSet6.Count || localSkins.Cape.Id < capeMemento.Id;
				if (flag && !skipApplyingToLocalState)
				{
					if (hashSet5.Count < hashSet6.Count && Storager.getInt(Defs.SkinsMakerInProfileBought) == 0)
					{
						Storager.setInt(Defs.SkinsMakerInProfileBought, 1);
					}
					OverwriteLocalSkins(localSkins, cloudSkins);
				}
				bool flag2 = hashSet.Count < hashSet3.Count || hashSet4.Count < hashSet6.Count || cloudSkins.Cape.Id < capeMemento.Id;
				if (flag2)
				{
					SlotSynchronizer.Push(JsonUtility.ToJson(LoadLocalSkins()));
				}
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.LogFormat("SkinsCloudApplyer: Succeeded to apply skins:\n'currentPullResult':{0},\n'cloudDeletedSkins':{1},\n'localDeletedSkins':{2},\n'cloudSkinIds':{3},\n'localSkinIds':{4},\n'localDirty':{5},\n'cloudDirty':{6}", currentResult, Json.Serialize(hashSet.ToList()), Json.Serialize(hashSet2.ToList()), Json.Serialize(hashSet4.ToList()), Json.Serialize(hashSet5.ToList()), flag, flag2);
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogErrorFormat("Exception in SkinsCloudApplyer.Apply: {0}", ex);
			}
		}

		private static SkinsMemento LoadLocalSkins()
		{
			string callee = string.Format(CultureInfo.InvariantCulture, "{0}.LoadLocalSkins()", "SkinsSynchronizer");
			using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
			{
				List<object> list = Json.Deserialize(PlayerPrefs.GetString("DeletedSkins", string.Empty)) as List<object>;
				List<string> deletedSkins = ((list != null) ? list.OfType<string>().ToList() : new List<string>());
				string @string = PlayerPrefs.GetString("User Skins", string.Empty);
				CapeMemento cape = Tools.DeserializeJson<CapeMemento>(PlayerPrefs.GetString("NewUserCape", string.Empty));
				Dictionary<string, object> dictionary = (Json.Deserialize(@string) as Dictionary<string, object>) ?? new Dictionary<string, object>();
				List<SkinMemento> list2 = new List<SkinMemento>(dictionary.Count);
				if (dictionary.Count == 0)
				{
					UnityEngine.Debug.LogFormat("Deserialized skins are empty: {0}", @string);
					return new SkinsMemento(list2, deletedSkins, cape);
				}
				Dictionary<string, object> dict = (Json.Deserialize(PlayerPrefs.GetString("User Name Skins", string.Empty)) as Dictionary<string, object>) ?? new Dictionary<string, object>();
				foreach (KeyValuePair<string, object> item2 in dictionary)
				{
					string key = item2.Key;
					string value;
					if (!dict.TryGetValue<string>(key, out value))
					{
						value = string.Empty;
					}
					string skin = (item2.Value as string) ?? string.Empty;
					SkinMemento item = new SkinMemento(key, value, skin);
					list2.Add(item);
				}
				return new SkinsMemento(list2, deletedSkins, cape);
			}
		}

		private static void OverwriteLocalSkins(SkinsMemento localSkins, SkinsMemento cloudSkins)
		{
			HashSet<string> hashSet = new HashSet<string>(localSkins.DeletedSkins.Concat(cloudSkins.DeletedSkins));
			Dictionary<string, string> dictionary = new Dictionary<string, string>(localSkins.Skins.Count);
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>(localSkins.Skins.Count);
			foreach (SkinMemento skin in cloudSkins.Skins)
			{
				if (!hashSet.Contains(skin.Id))
				{
					dictionary[skin.Id] = skin.Skin;
					dictionary2[skin.Id] = skin.Name;
				}
			}
			foreach (SkinMemento skin2 in localSkins.Skins)
			{
				if (!hashSet.Contains(skin2.Id))
				{
					dictionary[skin2.Id] = skin2.Skin;
					dictionary2[skin2.Id] = skin2.Name;
				}
			}
			string value = Json.Serialize(dictionary);
			PlayerPrefs.SetString("User Skins", value);
			string value2 = Json.Serialize(dictionary2);
			PlayerPrefs.SetString("User Name Skins", value2);
			CapeMemento capeMemento = CapeMemento.ChooseCape(localSkins.Cape, cloudSkins.Cape);
			string value3 = JsonUtility.ToJson(capeMemento);
			PlayerPrefs.SetString("NewUserCape", value3);
			RefreshGui(dictionary, dictionary2, capeMemento);
			PlayerPrefs.Save();
		}

		private static void RefreshGui(Dictionary<string, string> skins, Dictionary<string, string> skinNames, CapeMemento cape)
		{
			if (ShopNGUIController.sharedShop == null)
			{
				UnityEngine.Debug.LogErrorFormat("SkinsCloudApplyer: RefreshGui: ShopNGUIController.sharedShop == null");
				return;
			}
			if (!cape.Cape.IsNullOrEmpty())
			{
				Storager.setInt("cape_Custom", 1);
			}
			foreach (KeyValuePair<string, string> skin in skins)
			{
				if (!SkinsController.skinsForPers.ContainsKey(skin.Key))
				{
					Texture2D value = SkinsController.TextureFromString(skin.Value);
					SkinsController.skinsForPers.Add(skin.Key, value);
					SkinsController.customSkinIds.Add(skin.Key);
				}
			}
			foreach (KeyValuePair<string, string> skinName in skinNames)
			{
				SkinsController.skinsNamesForPers[skinName.Key] = skinName.Value;
			}
			SkinsController.capeUserTexture = SkinsController.TextureFromString(cape.Cape, 32);
			if (ShopNGUIController.GuiActive)
			{
				ShopNGUIController.sharedShop.ReloadGridOrCarousel(ShopNGUIController.sharedShop.CurrentItem);
				ShopNGUIController.sharedShop.ShowLockOrPropertiesAndButtons();
			}
		}
	}
}
