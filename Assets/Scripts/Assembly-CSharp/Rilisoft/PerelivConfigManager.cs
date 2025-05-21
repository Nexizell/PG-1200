using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class PerelivConfigManager
	{
		[CompilerGenerated]
		internal sealed class _003CGetPerelivConfigLoop_003Ed__4 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public Task futureToWait;

			public PerelivConfigManager _003C_003E4__this;

			private float _003CadvertGetInfoStartTime_003E5__1;

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
			public _003CGetPerelivConfigLoop_003Ed__4(int _003C_003E1__state)
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
					if (futureToWait != null)
					{
						goto IL_0047;
					}
					goto IL_0054;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0047;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00a3;
				case 3:
					{
						_003C_003E1__state = -1;
						goto IL_00a3;
					}
					IL_00a3:
					if (Time.realtimeSinceStartup - _003CadvertGetInfoStartTime_003E5__1 < 960f)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0054;
					IL_0054:
					_003CadvertGetInfoStartTime_003E5__1 = Time.realtimeSinceStartup;
					_003C_003E2__current = CoroutineRunner.Instance.StartCoroutine(_003C_003E4__this.GetPerelivConfigOnce());
					_003C_003E1__state = 2;
					return true;
					IL_0047:
					if (!futureToWait.IsCompleted)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0054;
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

		[CompilerGenerated]
		internal sealed class _003CGetPerelivConfigOnce_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			private WWW _003Cresponse_003E5__1;

			public PerelivConfigManager _003C_003E4__this;

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
			public _003CGetPerelivConfigOnce_003Ed__8(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				string text;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					string newPerelivConfig = URLs.NewPerelivConfig;
					string value = PersistentCacheManager.Instance.GetValue(newPerelivConfig);
					if (!string.IsNullOrEmpty(value))
					{
						PersistentCacheManager.DebugReportCacheHit(newPerelivConfig);
						text = value;
						break;
					}
					PersistentCacheManager.DebugReportCacheMiss(newPerelivConfig);
					_003Cresponse_003E5__1 = Tools.CreateWww(newPerelivConfig);
					_003C_003E2__current = _003Cresponse_003E5__1;
					_003C_003E1__state = 1;
					return true;
				}
				case 1:
					_003C_003E1__state = -1;
					try
					{
						if (_003Cresponse_003E5__1 == null || !string.IsNullOrEmpty(_003Cresponse_003E5__1.error))
						{
							UnityEngine.Debug.LogWarningFormat("Pereliv response error: {0}", (_003Cresponse_003E5__1 != null) ? _003Cresponse_003E5__1.error : "null");
							return false;
						}
						text = URLs.Sanitize(_003Cresponse_003E5__1);
						if (string.IsNullOrEmpty(text))
						{
							UnityEngine.Debug.LogWarning("Pereliv response is empty");
							return false;
						}
						PersistentCacheManager.Instance.SetValue(_003Cresponse_003E5__1.url, text);
					}
					finally
					{
						_003Cresponse_003E5__1.Dispose();
					}
					_003Cresponse_003E5__1 = null;
					break;
				}
				Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
				_003C_003E4__this._lastLoadedConfig = dictionary ?? new Dictionary<string, object>(0);
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

		private const float AdvertInfoTimeoutInSeconds = 960f;

		private static readonly Lazy<PerelivConfigManager> s_instance = new Lazy<PerelivConfigManager>(() => new PerelivConfigManager());

		private static readonly Dictionary<string, object> s_emptyDictionary = new Dictionary<string, object>(0);

		private Dictionary<string, object> _lastLoadedConfig = new Dictionary<string, object>(0);

		public static PerelivConfigManager Instance
		{
			get
			{
				return s_instance.Value;
			}
		}

		public Dictionary<string, object> LastLoadedConfig
		{
			get
			{
				return _lastLoadedConfig;
			}
		}

		internal IEnumerator GetPerelivConfigLoop(Task futureToWait)
		{
			if (futureToWait != null)
			{
				while (!futureToWait.IsCompleted)
				{
					yield return null;
				}
			}
			while (true)
			{
				float advertGetInfoStartTime = Time.realtimeSinceStartup;
				yield return CoroutineRunner.Instance.StartCoroutine(GetPerelivConfigOnce());
				while (Time.realtimeSinceStartup - advertGetInfoStartTime < 960f)
				{
					yield return null;
				}
			}
		}

		internal PerelivSettings GetPerelivSettings(string category)
		{
			if (LastLoadedConfig.Count == 0)
			{
				return new PerelivSettings("Last loaded config is empty");
			}
			if (string.IsNullOrEmpty(category))
			{
				return new PerelivSettings("Category is empty");
			}
			object value;
			if (!LastLoadedConfig.TryGetValue("mainMenu", out value))
			{
				return new PerelivSettings("Root object not found");
			}
			Dictionary<string, object> dictionary = value as Dictionary<string, object>;
			if (value == null)
			{
				return new PerelivSettings("Root object is not dictionary");
			}
			object value2;
			dictionary.TryGetValue("any", out value2);
			Dictionary<string, object> root = (value2 as Dictionary<string, object>) ?? s_emptyDictionary;
			object value3;
			dictionary.TryGetValue(category, out value3);
			Dictionary<string, object> overrides = (value3 as Dictionary<string, object>) ?? s_emptyDictionary;
			try
			{
				object value4 = TryGetValue(root, overrides, "enabled");
				object obj = TryGetValue(root, overrides, "imageUrl");
				object obj2 = TryGetValue(root, overrides, "redirectUrl");
				object obj3 = TryGetValue(root, overrides, "text");
				object value5 = TryGetValue(root, overrides, "showAlways");
				object obj4 = TryGetValue(root, overrides, "minLevel");
				object obj5 = TryGetValue(root, overrides, "maxLevel");
				object value6 = TryGetValue(root, overrides, "buttonClose");
				int minLevel = ((obj4 != null) ? Convert.ToInt32(obj4) : (-1));
				int maxLevel = ((obj5 != null) ? Convert.ToInt32(obj5) : (-1));
				return new PerelivSettings(Convert.ToBoolean(value4), obj as string, obj2 as string, obj3 as string, Convert.ToBoolean(value5), minLevel, maxLevel, Convert.ToBoolean(value6));
			}
			catch (Exception ex)
			{
				return new PerelivSettings(ex.Message);
			}
		}

		private PerelivConfigManager()
		{
		}

		private static object TryGetValue(Dictionary<string, object> root, Dictionary<string, object> overrides, string key)
		{
			object value;
			if (overrides.TryGetValue(key, out value))
			{
				return value;
			}
			if (root.TryGetValue(key, out value))
			{
				return value;
			}
			return null;
		}

		private IEnumerator GetPerelivConfigOnce()
		{
			string newPerelivConfig = URLs.NewPerelivConfig;
			string value = PersistentCacheManager.Instance.GetValue(newPerelivConfig);
			string text;
			if (!string.IsNullOrEmpty(value))
			{
				PersistentCacheManager.DebugReportCacheHit(newPerelivConfig);
				text = value;
			}
			else
			{
				PersistentCacheManager.DebugReportCacheMiss(newPerelivConfig);
				WWW response = Tools.CreateWww(newPerelivConfig);
				yield return response;
				try
				{
					if (response == null || !string.IsNullOrEmpty(response.error))
					{
						UnityEngine.Debug.LogWarningFormat("Pereliv response error: {0}", (response != null) ? response.error : "null");
						yield break;
					}
					text = URLs.Sanitize(response);
					if (string.IsNullOrEmpty(text))
					{
						UnityEngine.Debug.LogWarning("Pereliv response is empty");
						yield break;
					}
					PersistentCacheManager.Instance.SetValue(response.url, text);
				}
				finally
				{
					response.Dispose();
				}
			}
			Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
			_lastLoadedConfig = dictionary ?? new Dictionary<string, object>(0);
		}
	}
}
