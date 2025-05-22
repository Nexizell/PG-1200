using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public class EditorListBuilder : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CGetPromoActionsData_003Ed__25 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public EditorListBuilder _003C_003E4__this;

		private WWW _003CdownloadData_003E5__1;

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
		public _003CGetPromoActionsData_003Ed__25(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				WWWForm wWWForm = new WWWForm();
				string value2 = string.Format("{0}:{1}", new object[2]
				{
					ProtocolListGetter.CurrentPlatform,
					GlobalGameController.AppVersion
				});
				wWWForm.AddField("app_version", value2);
				string promoActionUrl = _003C_003E4__this.applyWindow.GetPromoActionUrl();
				_003CdownloadData_003E5__1 = Tools.CreateWwwIfNotConnected(promoActionUrl, wWWForm);
				if (_003CdownloadData_003E5__1 == null)
				{
					return false;
				}
				_003C_003E2__current = _003CdownloadData_003E5__1;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				if (!string.IsNullOrEmpty(_003CdownloadData_003E5__1.error))
				{
					UnityEngine.Debug.LogWarning("GetPromoActionsData error: " + _003CdownloadData_003E5__1.error);
					_003C_003E4__this.ClearPromoActionsData();
					return false;
				}
				Dictionary<string, object> dictionary = Json.Deserialize(URLs.Sanitize(_003CdownloadData_003E5__1)) as Dictionary<string, object>;
				if (dictionary == null)
				{
					UnityEngine.Debug.LogWarning("GetPromoActionsData promoActionsData = null");
					return false;
				}
				_003C_003E4__this.ClearPromoActionsData();
				if (dictionary.ContainsKey("news_up"))
				{
					object obj = dictionary["news_up"];
					if (obj != null)
					{
						List<object> list = obj as List<object>;
						if (list != null)
						{
							foreach (string item3 in list)
							{
								_003C_003E4__this._newsData.Add(item3);
							}
						}
					}
				}
				if (dictionary.ContainsKey("topSellers_up"))
				{
					object obj2 = dictionary["topSellers_up"];
					if (obj2 != null)
					{
						List<object> list2 = obj2 as List<object>;
						if (list2 != null)
						{
							foreach (string item4 in list2)
							{
								_003C_003E4__this._topSellersData.Add(item4);
							}
						}
					}
				}
				if (dictionary.ContainsKey("discounts_up"))
				{
					object obj3 = dictionary["discounts_up"];
					if (obj3 != null)
					{
						List<object> list3 = obj3 as List<object>;
						if (list3 != null)
						{
							for (int i = 0; i < list3.Count; i++)
							{
								List<object> obj4 = list3[i] as List<object>;
								string key = (string)obj4[0];
								int value = Convert.ToInt32((long)obj4[1]);
								_003C_003E4__this._discountsData.Add(key, value);
							}
						}
					}
				}
				_003C_003E4__this.FillShopItemList();
				_003C_003E4__this._isStart = false;
				return false;
			}
			}
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

	public GameObject itemPrefab;

	public UIScrollView scrollView;

	public UIGrid grid;

	public UIToggle defaultFilter;

	public UploadShopItemDataToServer applyWindow;

	public UIInput generateTextFile;

	public UIWidget promoActionPanel;

	public UIWidget x3Panel;

	private Dictionary<string, int> _discountsData = new Dictionary<string, int>();

	private List<string> _topSellersData = new List<string>();

	private List<string> _newsData = new List<string>();

	private EditorShopItemsType _currentFilter;

	private bool _isStart;

	private List<EditorShopItemData> _shopItemsData;

	private void Start()
	{
		string currentLanguage = LocalizationStore.CurrentLanguage;
		_currentFilter = EditorShopItemsType.All;
		_isStart = true;
	}

	private EditorShopItemData GetEditorItemDataByTag(string tag)
	{
		EditorShopItemData editorShopItemData = new EditorShopItemData();
		if (_discountsData.ContainsKey(tag))
		{
			editorShopItemData.discount = _discountsData[tag];
		}
		editorShopItemData.tag = tag;
		editorShopItemData.isTop = _topSellersData.Contains(tag);
		editorShopItemData.isNew = _newsData.Contains(tag);
		return editorShopItemData;
	}

	private List<EditorShopItemData> GetWeaponsData()
	{
		WeaponSounds[] array = Resources.LoadAll<WeaponSounds>("Weapons/");
		List<EditorShopItemData> list = new List<EditorShopItemData>();
		for (int i = 0; i < array.Length; i++)
		{
			EditorShopItemData editorItemDataByTag = GetEditorItemDataByTag(ItemDb.GetByPrefabName(array[i].name).Tag);
			editorItemDataByTag.localizeKey = array[i].localizeWeaponKey;
			editorItemDataByTag.type = EditorShopItemsType.Weapon;
			editorItemDataByTag.prefabName = array[i].name;
			list.Add(editorItemDataByTag);
		}
		return list;
	}

	private List<EditorShopItemData> GetWearData(EditorShopItemsType type)
	{
		string path = string.Empty;
		switch (type)
		{
		case EditorShopItemsType.Hats:
			path = "Hats_Info/";
			break;
		case EditorShopItemsType.Armor:
			path = "Armor_Info/";
			break;
		case EditorShopItemsType.Capes:
			path = "Capes_Info/";
			break;
		case EditorShopItemsType.Boots:
			path = "Shop_Boots_Info/";
			break;
		}
		ShopPositionParams[] array = Resources.LoadAll<ShopPositionParams>(path);
		List<EditorShopItemData> list = new List<EditorShopItemData>();
		for (int i = 0; i < array.Length; i++)
		{
			EditorShopItemData editorItemDataByTag = GetEditorItemDataByTag(array[i].name);
			editorItemDataByTag.localizeKey = array[i].localizeKey;
			editorItemDataByTag.type = type;
			editorItemDataByTag.prefabName = array[i].name;
			list.Add(editorItemDataByTag);
		}
		return list;
	}

	private List<EditorShopItemData> GetSkinsData()
	{
		List<EditorShopItemData> list = new List<EditorShopItemData>();
		foreach (KeyValuePair<string, string> item in SkinsController.shopKeyFromNameSkin)
		{
			EditorShopItemData editorItemDataByTag = GetEditorItemDataByTag(item.Key);
			editorItemDataByTag.localizeKey = SkinsController.skinsLocalizeKey[item.Key];
			editorItemDataByTag.type = EditorShopItemsType.Skins;
			list.Add(editorItemDataByTag);
		}
		return list;
	}

	public List<EditorShopItemData> GetItemsList(EditorShopItemsType filter)
	{
		switch (filter)
		{
		case EditorShopItemsType.Hats:
			return GetWearData(EditorShopItemsType.Hats);
		case EditorShopItemsType.Armor:
			return GetWearData(EditorShopItemsType.Armor);
		case EditorShopItemsType.Capes:
			return GetWearData(EditorShopItemsType.Capes);
		case EditorShopItemsType.Boots:
			return GetWearData(EditorShopItemsType.Boots);
		case EditorShopItemsType.Weapon:
			return GetWeaponsData();
		case EditorShopItemsType.Skins:
			return GetSkinsData();
		case EditorShopItemsType.All:
		{
			List<EditorShopItemData> wearData = GetWearData(EditorShopItemsType.Hats);
			wearData.AddRange(GetWearData(EditorShopItemsType.Armor));
			wearData.AddRange(GetWearData(EditorShopItemsType.Capes));
			wearData.AddRange(GetWearData(EditorShopItemsType.Boots));
			wearData.AddRange(GetWeaponsData());
			wearData.AddRange(GetSkinsData());
			return wearData;
		}
		default:
			return null;
		}
	}

	private void ClearShopItemList()
	{
		EditorShopItem[] componentsInChildren = grid.GetComponentsInChildren<EditorShopItem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			NGUITools.Destroy(componentsInChildren[i].gameObject);
		}
	}

	private bool IsItemEqualFilter(EditorShopItemData itemData)
	{
		if (_currentFilter == EditorShopItemsType.All)
		{
			return true;
		}
		if (_currentFilter == EditorShopItemsType.OnlyNew && itemData.isNew)
		{
			return true;
		}
		if (_currentFilter == EditorShopItemsType.OnlyTop && itemData.isTop)
		{
			return true;
		}
		if (_currentFilter == EditorShopItemsType.OnlyDiscount && itemData.discount > 0)
		{
			return true;
		}
		if (_currentFilter == itemData.type)
		{
			return true;
		}
		return false;
	}

	private int SortingWeaponByOrder(Transform left, Transform right)
	{
		EditorShopItem component = left.GetComponent<EditorShopItem>();
		EditorShopItem component2 = right.GetComponent<EditorShopItem>();
		string s = component.prefabName.Replace("Weapon", "");
		int result = 0;
		int.TryParse(s, out result);
		string s2 = component2.prefabName.Replace("Weapon", "");
		int result2 = 0;
		int.TryParse(s2, out result2);
		if (result > result2)
		{
			return -1;
		}
		if (result < result2)
		{
			return 1;
		}
		return 0;
	}

	private void FillShopItemList()
	{
		EditorShopItem editorShopItem = null;
		GameObject gameObject = null;
		if (_shopItemsData == null || _isStart)
		{
			_shopItemsData = GetItemsList(_currentFilter);
			applyWindow.itemsData = _shopItemsData;
		}
		ClearShopItemList();
		for (int i = 0; i < _shopItemsData.Count; i++)
		{
			if (IsItemEqualFilter(_shopItemsData[i]))
			{
				gameObject = NGUITools.AddChild(grid.gameObject, itemPrefab);
				gameObject.name = string.Format("{0:00}", new object[1] { i });
				editorShopItem = gameObject.GetComponent<EditorShopItem>();
				if (editorShopItem != null)
				{
					editorShopItem.SetData(_shopItemsData[i]);
				}
				gameObject.gameObject.SetActive(true);
			}
		}
		if (_currentFilter == EditorShopItemsType.Weapon)
		{
			grid.onCustomSort = SortingWeaponByOrder;
			grid.sorting = UIGrid.Sorting.Custom;
		}
		else
		{
			grid.sorting = UIGrid.Sorting.Alphabetic;
		}
		grid.Reposition();
		scrollView.ResetPosition();
	}

	private void ClearPromoActionsData()
	{
		_discountsData.Clear();
		_topSellersData.Clear();
		_newsData.Clear();
	}

	public IEnumerator GetPromoActionsData()
	{
		WWWForm wWWForm = new WWWForm();
		string value = string.Format("{0}:{1}", new object[2]
		{
			ProtocolListGetter.CurrentPlatform,
			GlobalGameController.AppVersion
		});
		wWWForm.AddField("app_version", value);
		string promoActionUrl = applyWindow.GetPromoActionUrl();
		WWW downloadData = Tools.CreateWwwIfNotConnected(promoActionUrl, wWWForm);
		if (downloadData == null)
		{
			yield break;
		}
		yield return downloadData;
		if (!string.IsNullOrEmpty(downloadData.error))
		{
			UnityEngine.Debug.LogWarning("GetPromoActionsData error: " + downloadData.error);
			ClearPromoActionsData();
			yield break;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(URLs.Sanitize(downloadData)) as Dictionary<string, object>;
		if (dictionary == null)
		{
			UnityEngine.Debug.LogWarning("GetPromoActionsData promoActionsData = null");
			yield break;
		}
		ClearPromoActionsData();
		if (dictionary.ContainsKey("news_up"))
		{
			object obj = dictionary["news_up"];
			if (obj != null)
			{
				List<object> list = obj as List<object>;
				if (list != null)
				{
					foreach (string item3 in list)
					{
						_newsData.Add(item3);
					}
				}
			}
		}
		if (dictionary.ContainsKey("topSellers_up"))
		{
			object obj2 = dictionary["topSellers_up"];
			if (obj2 != null)
			{
				List<object> list2 = obj2 as List<object>;
				if (list2 != null)
				{
					foreach (string item4 in list2)
					{
						_topSellersData.Add(item4);
					}
				}
			}
		}
		if (dictionary.ContainsKey("discounts_up"))
		{
			object obj3 = dictionary["discounts_up"];
			if (obj3 != null)
			{
				List<object> list3 = obj3 as List<object>;
				if (list3 != null)
				{
					for (int i = 0; i < list3.Count; i++)
					{
						List<object> obj4 = list3[i] as List<object>;
						string key = (string)obj4[0];
						int value2 = Convert.ToInt32((long)obj4[1]);
						_discountsData.Add(key, value2);
					}
				}
			}
		}
		FillShopItemList();
		_isStart = false;
	}

	public void ChangeCurrentFilter(UIToggle toggle)
	{
		if (!(toggle == null) && toggle.value)
		{
			ClearShopItemList();
			switch (toggle.name)
			{
			case "OnlyWeaponCheckbox":
				_currentFilter = EditorShopItemsType.Weapon;
				break;
			case "OnlySkinsCheckbox":
				_currentFilter = EditorShopItemsType.Skins;
				break;
			case "OnlyHatsCheckbox":
				_currentFilter = EditorShopItemsType.Hats;
				break;
			case "OnlyArmorCheckbox":
				_currentFilter = EditorShopItemsType.Armor;
				break;
			case "OnlyCapesCheckbox":
				_currentFilter = EditorShopItemsType.Capes;
				break;
			case "OnlyBootsCheckbox":
				_currentFilter = EditorShopItemsType.Boots;
				break;
			case "AllCheckbox":
				_currentFilter = EditorShopItemsType.All;
				break;
			case "OnlyNewCheckbox":
				_currentFilter = EditorShopItemsType.OnlyNew;
				break;
			case "OnlyTopsCheckbox":
				_currentFilter = EditorShopItemsType.OnlyTop;
				break;
			case "OnlyDiscountCheckbox":
				_currentFilter = EditorShopItemsType.OnlyDiscount;
				break;
			}
			if (_isStart)
			{
				StartCoroutine(GetPromoActionsData());
			}
			else
			{
				FillShopItemList();
			}
		}
	}

	public void SendDataToServerClick()
	{
		applyWindow.Show(UploadShopItemDataToServer.TypeWindow.UploadFileToServer);
	}

	public void CheckAllTopState(UIToggle topAllCheck)
	{
		EditorShopItem[] componentsInChildren = grid.GetComponentsInChildren<EditorShopItem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].topCheckbox.value = topAllCheck.value;
			componentsInChildren[i].SetTopState();
		}
	}

	public void CheckAllNewState(UIToggle newAllCheck)
	{
		EditorShopItem[] componentsInChildren = grid.GetComponentsInChildren<EditorShopItem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].newCheckbox.value = newAllCheck.value;
			componentsInChildren[i].SetNewState();
		}
	}

	public void SetAllDiscounts(UIInput inputAllDiscount)
	{
		EditorShopItem[] componentsInChildren = grid.GetComponentsInChildren<EditorShopItem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].discountInput.label.text = inputAllDiscount.label.text;
			componentsInChildren[i].SetDiscount();
		}
	}

	public static void CopyTextInClipboard(string text)
	{
		TextEditor textEditor = new TextEditor();
		//textEditor.content = new GUIContent(text);
		textEditor.SelectAll();
		textEditor.Copy();
	}

	public void GenerateUploadTextButtonClick()
	{
		generateTextFile.value = applyWindow.GenerateTextForUploadFile();
		CopyTextInClipboard(generateTextFile.value);
	}

	public void DownloadDataClick()
	{
		_isStart = true;
		applyWindow.Show(UploadShopItemDataToServer.TypeWindow.ChangePlatform);
	}

	public void ShowPromoActionsPanel()
	{
		promoActionPanel.alpha = 1f;
		x3Panel.alpha = 0f;
	}

	public void ShowX3ActionsPanel()
	{
		promoActionPanel.alpha = 0f;
		x3Panel.alpha = 1f;
	}
}
