using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public abstract class ABTestBase
{
	[CompilerGenerated]
	internal sealed class _003CGetABTestConfig_003Ed__25 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ABTestBase _003C_003E4__this;

		private WWW _003Cdownload_003E5__1;

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
		public _003CGetABTestConfig_003Ed__25(int _003C_003E1__state)
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
				goto IL_0189;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0189;
			case 2:
			{
				_003C_003E1__state = -1;
				if (!string.IsNullOrEmpty(_003Cdownload_003E5__1.error))
				{
					if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
					{
						UnityEngine.Debug.LogWarning(string.Format("GetABTest {0} error: {1}", new object[2] { _003C_003E4__this.currentFolder, _003Cdownload_003E5__1.error }));
					}
					_003C_003E2__current = CoroutineRunner.Instance.StartCoroutine(CoroutineRunner.WaitForSeconds(30f + (float)UnityEngine.Random.Range(0, 10)));
					_003C_003E1__state = 3;
					return true;
				}
				string text = URLs.Sanitize(_003Cdownload_003E5__1);
				if (!string.IsNullOrEmpty(text))
				{
					Storager.setString(_003C_003E4__this.abTestConfigKey, text);
					_003C_003E4__this.ParseABTestConfig();
				}
				_003C_003E4__this.isRunGetABTestConfig = false;
				break;
			}
			case 3:
				{
					_003C_003E1__state = -1;
					goto IL_0189;
				}
				IL_0189:
				if (!_003C_003E4__this.isRunGetABTestConfig)
				{
					_003C_003E4__this.isRunGetABTestConfig = true;
					new WWWForm();
					_003Cdownload_003E5__1 = Tools.CreateWwwIfNotConnected(string.Format(URLs.ABTestPathFormat, new object[1] { _003C_003E4__this.currentFolder }));
					if (_003Cdownload_003E5__1 == null)
					{
						_003C_003E2__current = CoroutineRunner.Instance.StartCoroutine(CoroutineRunner.WaitForSeconds(30f + (float)UnityEngine.Random.Range(0, 10)));
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003E2__current = _003Cdownload_003E5__1;
					_003C_003E1__state = 2;
					return true;
				}
				break;
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

	private const string baseFolder = "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests";

	private bool _isConfigNameInit;

	private string _configName = "none";

	private ABTestController.ABTestCohortsType _cohort;

	private bool _isInitCohort;

	private bool isRunGetABTestConfig;

	public abstract string currentFolder { get; }

	private string platformFolder
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "test";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
				{
					return "android";
				}
				return "amazon";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "wp";
			}
			return "ios";
		}
	}

	private string url
	{
		get
		{
			return string.Format("{0}/{1}/abtestconfig_{2}.json", new object[3] { "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests", currentFolder, platformFolder });
		}
	}

	private string configNameKey
	{
		get
		{
			return string.Format("CN_{0}", new object[1] { currentFolder });
		}
	}

	public string configName
	{
		get
		{
			if (!_isConfigNameInit)
			{
				_configName = PlayerPrefs.GetString(configNameKey, "none");
				_isConfigNameInit = true;
			}
			return _configName;
		}
		set
		{
			_isConfigNameInit = true;
			_configName = value;
			PlayerPrefs.SetString(configNameKey, _configName);
		}
	}

	private string cohortKey
	{
		get
		{
			return string.Format("cohort_{0}", new object[1] { currentFolder });
		}
	}

	public ABTestController.ABTestCohortsType cohort
	{
		get
		{
			if (!_isInitCohort)
			{
				_cohort = (ABTestController.ABTestCohortsType)PlayerPrefs.GetInt(cohortKey, 0);
				_isInitCohort = true;
			}
			return _cohort;
		}
		set
		{
			_cohort = value;
			_isInitCohort = true;
			PlayerPrefs.SetInt(cohortKey, (int)_cohort);
		}
	}

	public string cohortName
	{
		get
		{
			return configName + cohort;
		}
	}

	private string abTestConfigKey
	{
		get
		{
			return string.Format("abTest{0}ConfigKey", new object[1] { currentFolder });
		}
	}

	public void UpdateABTestConfig()
	{
		CoroutineRunner.Instance.StartCoroutine(GetABTestConfig());
	}

	private IEnumerator GetABTestConfig()
	{
		while (!isRunGetABTestConfig)
		{
			isRunGetABTestConfig = true;
			new WWWForm();
			WWW download = Tools.CreateWwwIfNotConnected(string.Format(URLs.ABTestPathFormat, new object[1] { currentFolder }));
			if (download == null)
			{
				yield return CoroutineRunner.Instance.StartCoroutine(CoroutineRunner.WaitForSeconds(30f + (float)UnityEngine.Random.Range(0, 10)));
				continue;
			}
			yield return download;
			if (!string.IsNullOrEmpty(download.error))
			{
				if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
				{
					UnityEngine.Debug.LogWarning(string.Format("GetABTest {0} error: {1}", new object[2] { currentFolder, download.error }));
				}
				yield return CoroutineRunner.Instance.StartCoroutine(CoroutineRunner.WaitForSeconds(30f + (float)UnityEngine.Random.Range(0, 10)));
				continue;
			}
			string text = URLs.Sanitize(download);
			if (!string.IsNullOrEmpty(text))
			{
				Storager.setString(abTestConfigKey, text);
				ParseABTestConfig();
			}
			isRunGetABTestConfig = false;
			break;
		}
	}

	public void InitTest()
	{
		if (Storager.hasKey(abTestConfigKey))
		{
			ParseABTestConfig();
		}
		else
		{
			Storager.setString(abTestConfigKey, string.Empty);
		}
	}

	private void ParseABTestConfig(bool isFromReset = false)
	{
		if (string.IsNullOrEmpty(Storager.getString(abTestConfigKey)))
		{
			return;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(Storager.getString(abTestConfigKey)) as Dictionary<string, object>;
		if (dictionary == null || !dictionary.ContainsKey("enableABTest"))
		{
			return;
		}
		int num = Convert.ToInt32(dictionary["enableABTest"]);
		object settingsB = null;
		if (dictionary.ContainsKey("SettingsB"))
		{
			settingsB = dictionary["SettingsB"];
		}
		if (num == 1 && cohort != ABTestController.ABTestCohortsType.SKIP)
		{
			if (cohort == ABTestController.ABTestCohortsType.NONE)
			{
				configName = Convert.ToString(dictionary["configName"]);
				int num2 = UnityEngine.Random.Range(1, 3);
				cohort = (ABTestController.ABTestCohortsType)num2;
				AnalyticsStuff.LogABTest(currentFolder, cohortName);
				if (FriendsController.sharedController != null)
				{
					FriendsController.sharedController.SendOurData();
				}
			}
			ApplyState(cohort, settingsB);
		}
		else
		{
			if (!isFromReset)
			{
				ResetABTest();
			}
			bool flag = false;
			if (dictionary.ContainsKey("currentStateIsB"))
			{
				flag = Convert.ToBoolean(dictionary["currentStateIsB"]);
			}
			ApplyState((!flag) ? ABTestController.ABTestCohortsType.A : ABTestController.ABTestCohortsType.B, settingsB);
		}
	}

	protected virtual void ApplyState(ABTestController.ABTestCohortsType _state, object settingsB)
	{
	}

	public void ResetABTest()
	{
		if (cohort == ABTestController.ABTestCohortsType.SKIP)
		{
			return;
		}
		if (cohort == ABTestController.ABTestCohortsType.A || cohort == ABTestController.ABTestCohortsType.B)
		{
			AnalyticsStuff.LogABTest(currentFolder, cohortName, false);
			if (FriendsController.sharedController != null)
			{
				FriendsController.sharedController.SendOurData();
			}
		}
		cohort = ABTestController.ABTestCohortsType.SKIP;
		ParseABTestConfig(true);
	}
}
