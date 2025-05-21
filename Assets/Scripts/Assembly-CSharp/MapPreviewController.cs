using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class MapPreviewController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CSetPopularity_003Ed__20 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private Lazy<HashSet<GameConnect.GameMode>> _003CloggedFailedModes_003E5__1;

		public MapPreviewController _003C_003E4__this;

		private Dictionary<string, string> _003C_mapsPoplarityInCurrentRegim_003E5__2;

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
		public _003CSetPopularity_003Ed__20(int _003C_003E1__state)
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
				_003C_003E1__state = -1;
				_003CloggedFailedModes_003E5__1 = new Lazy<HashSet<GameConnect.GameMode>>(() => new HashSet<GameConnect.GameMode>());
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
				_003C_003E1__state = -1;
				break;
			}
			if (FriendsController.mapPopularityDictionary.Count > 0)
			{
				GameConnect.GameMode gameMode = GameConnect.gameMode;
				Dictionary<string, Dictionary<string, string>> mapPopularityDictionary = FriendsController.mapPopularityDictionary;
				int num = (int)gameMode;
				if (!mapPopularityDictionary.TryGetValue(num.ToString(), out _003C_mapsPoplarityInCurrentRegim_003E5__2) && !_003CloggedFailedModes_003E5__1.Value.Contains(gameMode))
				{
					UnityEngine.Debug.LogWarningFormat("Cannot find given key in map popularity dictionary: {0} ({1})", (int)gameMode, gameMode);
					_003CloggedFailedModes_003E5__1.Value.Add(gameMode);
				}
				if (_003C_mapsPoplarityInCurrentRegim_003E5__2 == null)
				{
					_003C_003E4__this._rating = 0;
					_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.MyWaitForSeconds(2f));
					_003C_003E1__state = 1;
					return true;
				}
				int num2 = (_003C_mapsPoplarityInCurrentRegim_003E5__2.ContainsKey(_003C_003E4__this.mapID.ToString()) ? int.Parse(_003C_mapsPoplarityInCurrentRegim_003E5__2[_003C_003E4__this.mapID.ToString()]) : 0);
				if (num2 < 1)
				{
					_003C_003E4__this._rating = 1;
				}
				else if (num2 >= 1 && num2 < 8)
				{
					_003C_003E4__this._rating = 1;
				}
				else if (num2 >= 8 && num2 < 15)
				{
					_003C_003E4__this._rating = 2;
				}
				else if (num2 >= 15 && num2 < 35)
				{
					_003C_003E4__this._rating = 3;
				}
				else if (num2 >= 35 && num2 < 50)
				{
					_003C_003E4__this._rating = 4;
				}
				else if (num2 >= 50)
				{
					_003C_003E4__this._rating = 5;
				}
				return false;
			}
			_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.MyWaitForSeconds(2f));
			_003C_003E1__state = 2;
			return true;
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

	[CompilerGenerated]
	internal sealed class _003CMyWaitForSeconds_003Ed__21 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private float _003CstartTime_003E5__1;

		public float tm;

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
		public _003CMyWaitForSeconds_003Ed__21(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				if (!(Time.realtimeSinceStartup - _003CstartTime_003E5__1 < tm))
				{
					return false;
				}
			}
			else
			{
				_003C_003E1__state = -1;
				_003CstartTime_003E5__1 = Time.realtimeSinceStartup;
			}
			_003C_003E2__current = null;
			_003C_003E1__state = 1;
			return true;
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
		Lazy<HashSet<GameConnect.GameMode>> loggedFailedModes = new Lazy<HashSet<GameConnect.GameMode>>(() => new HashSet<GameConnect.GameMode>());
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
