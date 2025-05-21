using System;
using UnityEngine;

public class TableGearController : MonoBehaviour
{
	internal enum TypeGear
	{
		Turret = 0,
		Mech = 1,
		Jetpack = 2,
		InvisibilityPotion = 3
	}

	public static TableGearController sharedController;

	public TimePotionUpdate[] potionLables;

	public UITable table;

	public UILabel activatePotionLabel;

	private string[] keysForLabel = new string[4] { "Key_1813", "Key_1810", "Key_1812", "Key_1811" };

	private string[] keysForLabelDater = new string[4] { "Key_1853", "Key_1851", "Key_1854", "Key_1852" };

	private float timerShowLabel = -1f;

	private void Start()
	{
		sharedController = this;
	}

	private void OnDestroy()
	{
		sharedController = null;
	}

	private void Update()
	{
		if (timerShowLabel > 0f)
		{
			timerShowLabel -= Time.deltaTime;
			if (timerShowLabel < 0f)
			{
				activatePotionLabel.gameObject.SetActive(false);
			}
		}
		for (int i = 0; i < potionLables.Length; i++)
		{
			if (!PotionsController.sharedController.PotionIsActive(potionLables[i].myPotionName))
			{
				if (potionLables[i].gameObject.activeSelf)
				{
					potionLables[i].gameObject.SetActive(false);
					potionLables[i].myLabel.text = string.Empty;
					table.Reposition();
				}
				continue;
			}
			if (!potionLables[i].gameObject.activeSelf)
			{
				potionLables[i].transform.GetChild(0).GetComponent<TweenScale>().enabled = true;
				potionLables[i].gameObject.SetActive(true);
				ReNameLabelObjects();
				table.Reposition();
				string myPotionName = potionLables[i].myPotionName;
				int num = (int)(TypeGear)Enum.Parse(typeof(TypeGear), myPotionName);
				activatePotionLabel.text = LocalizationStore.Get(GameConnect.isDaterRegim ? keysForLabelDater[num] : keysForLabel[num]);
				activatePotionLabel.gameObject.SetActive(true);
				timerShowLabel = 2f;
			}
			potionLables[i].timerUpdate -= Time.deltaTime;
			if (potionLables[i].timerUpdate < 0f)
			{
				potionLables[i].timerUpdate = 0.25f;
				potionLables[i].UpdateTime();
			}
		}
	}

	private void ReNameLabelObjects()
	{
		for (int i = 0; i < PotionsController.sharedController.activePotionsList.Count; i++)
		{
			string value = PotionsController.sharedController.activePotionsList[i];
			int num = (int)(TypeGear)Enum.Parse(typeof(TypeGear), value);
			potionLables[num].name = i.ToString();
		}
	}

	public void ReactivatePotion(string _potion)
	{
		int num = (int)(TypeGear)Enum.Parse(typeof(TypeGear), _potion);
		potionLables[num].transform.GetChild(0).GetComponent<TweenScale>().enabled = true;
		ReNameLabelObjects();
		table.Reposition();
		activatePotionLabel.text = LocalizationStore.Get(GameConnect.isDaterRegim ? keysForLabelDater[num] : keysForLabel[num]);
		activatePotionLabel.gameObject.SetActive(true);
		timerShowLabel = 2f;
	}
}
