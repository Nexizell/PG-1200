using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class MapPreviewController : MonoBehaviour
{
	private readonly string[] _ratingLabelsKeys = new string[6] { "Key_0545", "Key_0546", "Key_0547", "Key_0548", "Key_0549", "Key_2183" };

	public SceneInfo curSceneInfo;

	public UILabel NameMapLbl;

	public GameObject[] SizeMapNameLbl;

	public UILabel popularityLabel;

	public UISprite popularitySprite;

	public GameObject premium;

	public GameObject milee;

	public GameObject dater;

	public int mapID;

	public string sceneMapName;

	public UITexture mapPreviewTexture;

	public GameObject bottomPanel;

	private MyCenterOnChild centerChild;

	[ReadOnly]
	[SerializeField]
	protected internal int _ratingVal;

	private int _rating
	{
		get
		{
			return _ratingVal;
		}
		set
		{
			_ratingVal = value;
			if (_ratingVal >= 0)
			{
				popularitySprite.spriteName = string.Format("Nb_Players_{0}", new object[1] { _ratingVal });
			}
		}
	}

	private void Start()
	{
	}

	public void UpdatePopularity()
	{
		StopCoroutine(SetPopularity());
		StartCoroutine(SetPopularity());
	}

	private IEnumerator SetPopularity()
	{
		Rilisoft.Lazy<HashSet<GameConnect.GameMode>> loggedFailedModes = new Rilisoft.Lazy<HashSet<GameConnect.GameMode>>(() => new HashSet<GameConnect.GameMode>());
		Dictionary<string, string> _mapsPoplarityInCurrentRegim;
		while (true)
		{
			if (FriendsController.mapPopularityDictionary.Count > 0)
			{
				GameConnect.GameMode gameMode = GameConnect.gameMode;
				Dictionary<string, Dictionary<string, string>> mapPopularityDictionary = FriendsController.mapPopularityDictionary;
				int num = (int)gameMode;
				if (!mapPopularityDictionary.TryGetValue(num.ToString(), out _mapsPoplarityInCurrentRegim) && !loggedFailedModes.Value.Contains(gameMode))
				{
					UnityEngine.Debug.LogWarningFormat("Cannot find given key in map popularity dictionary: {0} ({1})", (int)gameMode, gameMode);
					loggedFailedModes.Value.Add(gameMode);
				}
				if (_mapsPoplarityInCurrentRegim != null)
				{
					break;
				}
				_rating = 0;
				yield return StartCoroutine(MyWaitForSeconds(2f));
			}
			else
			{
				yield return StartCoroutine(MyWaitForSeconds(2f));
			}
		}
		int num2 = (_mapsPoplarityInCurrentRegim.ContainsKey(mapID.ToString()) ? int.Parse(_mapsPoplarityInCurrentRegim[mapID.ToString()]) : 0);
		if (num2 < 1)
		{
			_rating = 1;
		}
		else if (num2 >= 1 && num2 < 8)
		{
			_rating = 1;
		}
		else if (num2 >= 8 && num2 < 15)
		{
			_rating = 2;
		}
		else if (num2 >= 15 && num2 < 35)
		{
			_rating = 3;
		}
		else if (num2 >= 35 && num2 < 50)
		{
			_rating = 4;
		}
		else if (num2 >= 50)
		{
			_rating = 5;
		}
	}

	public IEnumerator MyWaitForSeconds(float tm)
	{
		float startTime = Time.realtimeSinceStartup;
		do
		{
			yield return null;
		}
		while (Time.realtimeSinceStartup - startTime < tm);
	}

	private void OnEnable()
	{
		if (curSceneInfo != null && !string.IsNullOrEmpty(curSceneInfo.TranslatePreviewName))
		{
			NameMapLbl.GetComponent<SetHeadLabelText>().SetText(curSceneInfo.TranslatePreviewName.ToUpper());
		}
		else if (mapID == -1)
		{
			NameMapLbl.GetComponent<SetHeadLabelText>().SetText(LocalizationStore.Get("Key_2463"));
		}
	}

	private void OnClick()
	{
		if (ConnectScene.sharedController.selectMap.Equals(this))
		{
			if (!CustomPanelConnectScene.Instance.createPanel.activeSelf)
			{
				ConnectScene.sharedController.HandleGoBtnClicked();
			}
		}
		else
		{
			ConnectScene.sharedController.selectMap = this;
			ConnectScene.sharedController.StopFingerAnimation();
		}
	}
}
