using System.Collections.Generic;
using UnityEngine;

public sealed class LoadingNGUIController : MonoBehaviour
{
	private string sceneToLoad = "";

	public UITexture loadingNGUITexture;

	public UILabel[] levelNameLabels;

	public UILabel recommendedForThisMap;

	public Transform gunsPoint;

	public string SceneToLoad
	{
		set
		{
			sceneToLoad = value;
		}
	}

	public void Init()
	{
		TextAsset textAsset = Resources.Load<TextAsset>("PromoForLoadings");
		if (textAsset == null)
		{
			return;
		}
		string text = textAsset.text;
		if (text == null)
		{
			return;
		}
		string[] array = text.Split('\r', '\n');
		Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string[] array3 = array2[i].Split('\t');
			if (array3.Length < 2 || array3[0] == null || sceneToLoad == null || !array3[0].Equals(sceneToLoad))
			{
				continue;
			}
			List<string> list = new List<string>();
			for (int j = 1; j < array3.Length; j++)
			{
				if (array3[j] != null && array3[j].Equals("armor"))
				{
					list.AddRange(Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0]);
				}
				else
				{
					if (array3[j] != null && array3[j].Equals("hat"))
					{
						continue;
					}
					for (int k = 0; k < WeaponManager.sharedManager.weaponsInGame.Length; k++)
					{
						if (WeaponManager.sharedManager.weaponsInGame[k].name.Equals(array3[j]))
						{
							array3[j] = ItemDb.GetByPrefabName(WeaponManager.sharedManager.weaponsInGame[k].name).Tag;
							array3[j] = PromoActionsGUIController.FilterForLoadings(array3[j], list) ?? string.Empty;
							break;
						}
					}
					if (!string.IsNullOrEmpty(array3[j]))
					{
						list.Add(array3[j]);
					}
				}
			}
			foreach (string item in PromoActionsGUIController.FilterPurchases(list, true))
			{
				list.Remove(item);
			}
			if (dictionary.ContainsKey(array3[0]))
			{
				dictionary[array3[0]] = list;
			}
			else
			{
				dictionary.Add(array3[0], list);
			}
		}
		UILabel[] label;
		if (sceneToLoad != null && dictionary.ContainsKey(sceneToLoad ?? ""))
		{
			List<string> list2 = dictionary[sceneToLoad ?? ""];
			if (list2 != null)
			{
				for (int l = 0; l < list2.Count; l++)
				{
					GameObject obj = Object.Instantiate(Resources.Load("PromoItemForLoading") as GameObject);
					obj.transform.parent = gunsPoint;
					obj.transform.localScale = new Vector3(1f, 1f, 1f);
					obj.transform.localPosition = new Vector3(-256f * (float)list2.Count / 2f + 128f + (float)l * 256f, 0f, 0f);
					PromoItemForLoading component = obj.GetComponent<PromoItemForLoading>();
					int itemCategory = ItemDb.GetItemCategory(list2[l]);
					Texture itemIcon = ItemDb.GetItemIcon(list2[l], (ShopNGUIController.CategoryNames)itemCategory, null, false);
					UITexture[] texture = component.texture;
					foreach (UITexture uITexture in texture)
					{
						if (itemIcon != null)
						{
							uITexture.mainTexture = itemIcon;
						}
					}
					label = component.label;
					for (int i = 0; i < label.Length; i++)
					{
						label[i].text = ItemDb.GetItemName(list2[l], (ShopNGUIController.CategoryNames)itemCategory).Trim().Replace(" -", "-");
					}
				}
			}
		}
		recommendedForThisMap.gameObject.SetActive(sceneToLoad != null && dictionary.ContainsKey(sceneToLoad) && dictionary[sceneToLoad] != null && dictionary[sceneToLoad].Count > 0);
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(sceneToLoad);
		label = levelNameLabels;
		foreach (UILabel uILabel in label)
		{
			if (infoScene != null && !string.IsNullOrEmpty(sceneToLoad))
			{
				uILabel.gameObject.SetActive(true);
				string translatePreviewName = infoScene.TranslatePreviewName;
				translatePreviewName = translatePreviewName.Replace("\n", " ");
				translatePreviewName = translatePreviewName.Replace("\r", " ");
				translatePreviewName = translatePreviewName.ToUpper();
				uILabel.text = translatePreviewName;
			}
			else
			{
				uILabel.gameObject.SetActive(false);
			}
		}
	}

	public void SetEnabledMapName(bool enabled)
	{
		for (int i = 0; i < levelNameLabels.Length; i++)
		{
			levelNameLabels[i].gameObject.SetActive(enabled);
		}
	}

	public void SetEnabledGunsScroll(bool enabled)
	{
		if (recommendedForThisMap != null)
		{
			recommendedForThisMap.gameObject.SetActive(enabled);
		}
		if (gunsPoint != null)
		{
			gunsPoint.gameObject.SetActive(enabled);
		}
	}

	private void OnDestroy()
	{
		loadingNGUITexture= null;
	}
}
