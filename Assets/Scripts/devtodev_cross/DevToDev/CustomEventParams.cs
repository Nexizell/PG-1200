using System;
using System.Collections.Generic;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;

namespace DevToDev
{
	public sealed class CustomEventParams : ISaveable
	{
		internal Dictionary<string, double> DoubleParams { get; set; }

		internal Dictionary<string, int> IntParams { get; set; }

		internal Dictionary<string, long> LongParams { get; set; }

		internal Dictionary<string, string> StringParams { get; set; }

		public int Count { get; internal set; }

		internal bool HasNumeric
		{
			get
			{
				return IntParams.Count + LongParams.Count + DoubleParams.Count > 0;
			}
		}

		internal bool HasStrings
		{
			get
			{
				return StringParams.Count > 0;
			}
		}

		public CustomEventParams()
		{
			IntParams = new Dictionary<string, int>();
			LongParams = new Dictionary<string, long>();
			DoubleParams = new Dictionary<string, double>();
			StringParams = new Dictionary<string, string>();
		}

		public CustomEventParams(ObjectInfo info)
		{
			try
			{
				DoubleParams = info.GetValue("DoubleParams", typeof(Dictionary<string, double>)) as Dictionary<string, double>;
				IntParams = info.GetValue("IntParams", typeof(Dictionary<string, int>)) as Dictionary<string, int>;
				LongParams = info.GetValue("LongParams", typeof(Dictionary<string, long>)) as Dictionary<string, long>;
				StringParams = info.GetValue("StringParams", typeof(Dictionary<string, string>)) as Dictionary<string, string>;
				Count = (int)info.GetValue("Count", typeof(int));
			}
			catch (Exception ex)
			{
				Log.D("Error in desealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public override void GetObjectData(ObjectInfo info)
		{
			try
			{
				info.AddValue("DoubleParams", DoubleParams);
				info.AddValue("IntParams", IntParams);
				info.AddValue("LongParams", LongParams);
				info.AddValue("StringParams", StringParams);
				info.AddValue("Count", Count);
			}
			catch (Exception ex)
			{
				Log.D("Error in sealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public void AddParam(string key, int value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", "Key can't be null");
			}
			if (Count < 20)
			{
				if (HasNumericParam(key))
				{
					RemoveNumeric(key);
					Count--;
				}
				IntParams.Add(key, value);
				Count++;
			}
		}

		public void AddParam(string key, long value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", "Key can't be null");
			}
			if (Count < 20)
			{
				if (HasNumericParam(key))
				{
					RemoveNumeric(key);
					Count--;
				}
				LongParams.Add(key, value);
				Count++;
			}
		}

		public void AddParam(string key, double value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", "Key can't be null");
			}
			if (Count < 20)
			{
				if (HasNumericParam(key))
				{
					RemoveNumeric(key);
					Count--;
				}
				DoubleParams.Add(key, value);
				Count++;
			}
		}

		public void AddParam(string key, string value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key", "Key can't be null");
			}
			value = value ?? "null";
			if (Count < 20)
			{
				if (StringParams.ContainsKey(key))
				{
					StringParams.Remove(key);
					Count--;
				}
				StringParams.Add(key, value);
				Count++;
			}
		}

		public void CopyFromAnother(CustomEventParams cep)
		{
			foreach (KeyValuePair<string, double> doubleParam in cep.DoubleParams)
			{
				AddParam(doubleParam.Key, doubleParam.Value);
			}
			foreach (KeyValuePair<string, string> stringParam in cep.StringParams)
			{
				AddParam(stringParam.Key, stringParam.Value);
			}
			foreach (KeyValuePair<string, int> intParam in cep.IntParams)
			{
				AddParam(intParam.Key, intParam.Value);
			}
			foreach (KeyValuePair<string, long> longParam in cep.LongParams)
			{
				AddParam(longParam.Key, longParam.Value);
			}
		}

		private bool HasNumericParam(string key)
		{
			if (!IntParams.ContainsKey(key) && !LongParams.ContainsKey(key))
			{
				return DoubleParams.ContainsKey(key);
			}
			return true;
		}

		private void RemoveNumeric(string key)
		{
			if (IntParams.ContainsKey(key))
			{
				IntParams.Remove(key);
			}
			else if (LongParams.ContainsKey(key))
			{
				LongParams.Remove(key);
			}
			else
			{
				DoubleParams.Remove(key);
			}
		}
	}
}
