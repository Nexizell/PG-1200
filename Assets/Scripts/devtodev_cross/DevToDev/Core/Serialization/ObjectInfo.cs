using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DevToDev.Core.Utils;

namespace DevToDev.Core.Serialization
{
	public class ObjectInfo
	{
		private static readonly string TYPE_KEY = "#type";

		private static readonly string COLLECTION_TYPE_KEY = "#collectionType";

		private static readonly string PRIMITIVE_KEY = "#primitive";

		private static readonly string COLLECTION_KEY = "#collection";

		private static readonly string ARRAY_KEY = "#array";

		private static readonly string DICTIONARY_KEY = "#dictionary";

		private static readonly string VALUE_KEY = "#value";

		private static readonly string SERIAL_VERSION_ID_KEY = "#serialVersionId";

		private int serialVersionId;

		private string type;

		private Dictionary<string, object> objectData;

		public int SerialVersionId
		{
			get
			{
				return serialVersionId;
			}
		}

		public string TypeInfo
		{
			get
			{
				return type;
			}
		}

		public ObjectInfo()
		{
			objectData = new Dictionary<string, object>();
		}

		public ObjectInfo(ISaveable saveableObject)
		{
			objectData = new Dictionary<string, object>();
			type = saveableObject.GetType().FullName;
			FieldInfo fieldInfo = ObjectInfoPlatform.GetFieldInfo(saveableObject, "SerialVersionId");
			if (fieldInfo != null)
			{
				serialVersionId = (int)fieldInfo.GetValue(null);
			}
			saveableObject.GetObjectData(this);
		}

		public object GetValue(string name, Type type)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name is null");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type is null");
			}
			if (!objectData.ContainsKey(name))
			{
				return null;
			}
			object obj = objectData[name];
			if (obj != null)
			{
				return Decode(obj);
			}
			return obj;
		}

		private object Encode(object value)
		{
			if (value is IDictionary)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Add(COLLECTION_KEY, DICTIONARY_KEY);
				dictionary.Add(COLLECTION_TYPE_KEY, value.GetType().FullName);
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				foreach (DictionaryEntry item in value as IDictionary)
				{
					dictionary2.Add(item.Key.ToString(), EncodeValue(item.Value));
				}
				dictionary.Add(VALUE_KEY, dictionary2);
				return dictionary;
			}
			if (value is IList)
			{
				Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
				dictionary3.Add(COLLECTION_KEY, ARRAY_KEY);
				dictionary3.Add(COLLECTION_TYPE_KEY, value.GetType().FullName);
				List<object> list = new List<object>();
				foreach (object item2 in value as IList)
				{
					list.Add(EncodeValue(item2));
				}
				dictionary3.Add(VALUE_KEY, list);
				return dictionary3;
			}
			if (value is ISaveable)
			{
				return new ObjectInfo(value as ISaveable);
			}
			throw new ArgumentException("value does not inherit ISaveable. the type is: " + value.GetType());
		}

		public ObjectInfo(JSONNode json)
		{
			objectData = new Dictionary<string, object>();
			serialVersionId = json[SERIAL_VERSION_ID_KEY].AsInt;
			type = json[TYPE_KEY];
			foreach (KeyValuePair<string, JSONNode> item in json.AsObject)
			{
				object obj = null;
				obj = ((!(item.Value is JSONClass)) ? ((!(item.Value is JSONArray)) ? ((IEnumerable)item.Value.Value) : ((IEnumerable)DecodeList(item.Value))) : ((!(item.Value[TYPE_KEY] != null)) ? ((object)DecodeDictionary(item.Value)) : ((object)new ObjectInfo(item.Value))));
				objectData.Add(item.Key, obj);
			}
		}

		private List<object> DecodeList(JSONNode data)
		{
			List<object> list = new List<object>();
			foreach (JSONNode item in data.AsArray)
			{
				object obj = null;
				obj = ((!(item is JSONClass)) ? ((!(item is JSONArray)) ? ((IEnumerable)item.Value) : ((IEnumerable)DecodeList(item))) : ((!(item[TYPE_KEY] != null)) ? ((object)DecodeDictionary(item)) : ((object)new ObjectInfo(item))));
				list.Add(obj);
			}
			return list;
		}

		private Dictionary<string, object> DecodeDictionary(JSONNode data)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			foreach (KeyValuePair<string, JSONNode> item in data.AsObject)
			{
				object obj = null;
				dictionary.Add(value: (!(item.Value is JSONClass)) ? ((!(item.Value is JSONArray)) ? ((IEnumerable)item.Value.Value) : ((IEnumerable)DecodeList(item.Value))) : ((!(item.Value[TYPE_KEY] != null)) ? ((object)DecodeDictionary(item.Value)) : ((object)new ObjectInfo(item.Value))), key: item.Key);
			}
			return dictionary;
		}

		private object DecodePrimitive(Dictionary<string, object> data)
		{
			switch (data[PRIMITIVE_KEY] as string)
			{
			case "short":
			{
				short result2;
				if (short.TryParse(data[VALUE_KEY] as string, out result2))
				{
					return result2;
				}
				return (short)0;
			}
			case "ushort":
			{
				ushort result11;
				if (ushort.TryParse(data[VALUE_KEY] as string, out result11))
				{
					return result11;
				}
				return (ushort)0;
			}
			case "int":
			{
				int result4;
				if (int.TryParse(data[VALUE_KEY] as string, out result4))
				{
					return result4;
				}
				return 0;
			}
			case "byte":
			{
				byte result8;
				if (byte.TryParse(data[VALUE_KEY] as string, out result8))
				{
					return result8;
				}
				return (byte)0;
			}
			case "bool":
			{
				bool result13;
				if (bool.TryParse(data[VALUE_KEY] as string, out result13))
				{
					return result13;
				}
				return false;
			}
			case "char":
			{
				char result7;
				if (char.TryParse(data[VALUE_KEY] as string, out result7))
				{
					return result7;
				}
				return '\0';
			}
			case "sbyte":
			{
				sbyte result14;
				if (sbyte.TryParse(data[VALUE_KEY] as string, out result14))
				{
					return result14;
				}
				return (sbyte)0;
			}
			case "double":
			{
				double result10;
				if (double.TryParse(data[VALUE_KEY] as string, out result10))
				{
					return result10;
				}
				return 0.0;
			}
			case "decimal":
			{
				decimal result5;
				if (decimal.TryParse(data[VALUE_KEY] as string, out result5))
				{
					return result5;
				}
				return 0m;
			}
			case "#DateTime":
			{
				DateTime result;
				if (DateTime.TryParse(data[VALUE_KEY] as string, out result))
				{
					return result;
				}
				return default(DateTime);
			}
			case "uint":
			{
				uint result12;
				if (uint.TryParse(data[VALUE_KEY] as string, out result12))
				{
					return result12;
				}
				return 0u;
			}
			case "long":
			{
				long result9;
				if (long.TryParse(data[VALUE_KEY] as string, out result9))
				{
					return result9;
				}
				return 0L;
			}
			case "ulong":
			{
				ulong result6;
				if (ulong.TryParse(data[VALUE_KEY] as string, out result6))
				{
					return result6;
				}
				return 0uL;
			}
			case "float":
			{
				float result3;
				if (float.TryParse(data[VALUE_KEY] as string, out result3))
				{
					return result3;
				}
				return 0f;
			}
			case "string":
				if (!data.ContainsKey(VALUE_KEY))
				{
					return "";
				}
				return data[VALUE_KEY];
			default:
				return null;
			}
		}

		private object Decode(object data)
		{
			if (ObjectInfoPlatform.IsPrimitive(data))
			{
				return data;
			}
			if (data is IDictionary && data is Dictionary<string, object>)
			{
				Dictionary<string, object> dictionary = data as Dictionary<string, object>;
				if (dictionary.ContainsKey(PRIMITIVE_KEY))
				{
					return DecodePrimitive(dictionary);
				}
				if (dictionary.ContainsKey(COLLECTION_KEY))
				{
					string text = dictionary[COLLECTION_KEY].ToString();
					string typeName = dictionary[COLLECTION_TYPE_KEY].ToString();
					if (text.Equals(ARRAY_KEY))
					{
						if (Type.GetType(typeName) == null)
						{
							return null;
						}
						IList list = Activator.CreateInstance(Type.GetType(typeName)) as IList;
						IList list2 = dictionary[VALUE_KEY] as IList;
						{
							foreach (object item in list2)
							{
								list.Add(Decode(item));
							}
							return list;
						}
					}
					if (text.Equals(DICTIONARY_KEY))
					{
						if (Type.GetType(typeName) == null)
						{
							return null;
						}
						IDictionary dictionary2 = Activator.CreateInstance(Type.GetType(typeName)) as IDictionary;
						IDictionary dictionary3 = dictionary[VALUE_KEY] as IDictionary;
						{
							foreach (DictionaryEntry item2 in dictionary3)
							{
								dictionary2.Add(item2.Key, Decode(item2.Value));
							}
							return dictionary2;
						}
					}
				}
			}
			if (data is ObjectInfo)
			{
				ObjectInfo objectInfo = data as ObjectInfo;
				if (Type.GetType(objectInfo.TypeInfo) == null)
				{
					return null;
				}
				return Activator.CreateInstance(Type.GetType(objectInfo.TypeInfo), objectInfo) as ISaveable;
			}
			throw new ArgumentException("Something went wrong :" + data.GetType().ToString());
		}

		public object SelfValue(Type type)
		{
			return Decode(this);
		}

		private Dictionary<string, object> EncodeSimpleValue(short value)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(PRIMITIVE_KEY, "short");
			dictionary.Add(VALUE_KEY, value.ToString());
			return dictionary;
		}

		private Dictionary<string, object> EncodeSimpleValue(ushort value)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(PRIMITIVE_KEY, "ushort");
			dictionary.Add(VALUE_KEY, value.ToString());
			return dictionary;
		}

		private Dictionary<string, object> EncodeSimpleValue(int value)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(PRIMITIVE_KEY, "int");
			dictionary.Add(VALUE_KEY, value.ToString());
			return dictionary;
		}

		private Dictionary<string, object> EncodeSimpleValue(byte value)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(PRIMITIVE_KEY, "byte");
			dictionary.Add(VALUE_KEY, value.ToString());
			return dictionary;
		}

		private Dictionary<string, object> EncodeSimpleValue(bool value)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(PRIMITIVE_KEY, "bool");
			dictionary.Add(VALUE_KEY, value.ToString());
			return dictionary;
		}

		private Dictionary<string, object> EncodeSimpleValue(char value)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(PRIMITIVE_KEY, "char");
			dictionary.Add(VALUE_KEY, value.ToString());
			return dictionary;
		}

		private Dictionary<string, object> EncodeSimpleValue(sbyte value)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(PRIMITIVE_KEY, "sbyte");
			dictionary.Add(VALUE_KEY, value.ToString());
			return dictionary;
		}

		private Dictionary<string, object> EncodeSimpleValue(double value)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(PRIMITIVE_KEY, "double");
			dictionary.Add(VALUE_KEY, value.ToString());
			return dictionary;
		}

		private Dictionary<string, object> EncodeSimpleValue(decimal value)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(PRIMITIVE_KEY, "decimal");
			dictionary.Add(VALUE_KEY, value.ToString());
			return dictionary;
		}

		private Dictionary<string, object> EncodeSimpleValue(DateTime value)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(PRIMITIVE_KEY, "#DateTime");
			dictionary.Add(VALUE_KEY, value.ToString());
			return dictionary;
		}

		private Dictionary<string, object> EncodeSimpleValue(float value)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(PRIMITIVE_KEY, "float");
			dictionary.Add(VALUE_KEY, value.ToString());
			return dictionary;
		}

		private Dictionary<string, object> EncodeSimpleValue(uint value)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(PRIMITIVE_KEY, "uint");
			dictionary.Add(VALUE_KEY, value.ToString());
			return dictionary;
		}

		private Dictionary<string, object> EncodeSimpleValue(long value)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(PRIMITIVE_KEY, "long");
			dictionary.Add(VALUE_KEY, value.ToString());
			return dictionary;
		}

		private Dictionary<string, object> EncodeSimpleValue(ulong value)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(PRIMITIVE_KEY, "ulong");
			dictionary.Add(VALUE_KEY, value.ToString());
			return dictionary;
		}

		private Dictionary<string, object> EncodeSimpleValue(string value)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(PRIMITIVE_KEY, "string");
			dictionary.Add(VALUE_KEY, value);
			return dictionary;
		}

		private Dictionary<string, object> EncodeNullValue()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add(PRIMITIVE_KEY, "null");
			return dictionary;
		}

		private object EncodeValue(object value)
		{
			if (value is bool)
			{
				return EncodeSimpleValue((bool)value);
			}
			if (value is short)
			{
				return EncodeSimpleValue((short)value);
			}
			if (value is ushort)
			{
				return EncodeSimpleValue((ushort)value);
			}
			if (value is int)
			{
				return EncodeSimpleValue((int)value);
			}
			if (value is byte)
			{
				return EncodeSimpleValue((byte)value);
			}
			if (value is char)
			{
				return EncodeSimpleValue((char)value);
			}
			if (value is sbyte)
			{
				return EncodeSimpleValue((sbyte)value);
			}
			if (value is double)
			{
				return EncodeSimpleValue((double)value);
			}
			if (value is decimal)
			{
				return EncodeSimpleValue((decimal)value);
			}
			if (value is DateTime)
			{
				return EncodeSimpleValue((DateTime)value);
			}
			if (value is uint)
			{
				return EncodeSimpleValue((uint)value);
			}
			if (value is long)
			{
				return EncodeSimpleValue((long)value);
			}
			if (value is ulong)
			{
				return EncodeSimpleValue((ulong)value);
			}
			if (value is string)
			{
				return EncodeSimpleValue((string)value);
			}
			if (value is float)
			{
				return EncodeSimpleValue((float)value);
			}
			if (value == null)
			{
				return EncodeNullValue();
			}
			return Encode(value);
		}

		public void AddValue(string name, object value)
		{
			if (objectData.ContainsKey(name))
			{
				throw new ArgumentException("value is already serialized.");
			}
			object value2 = EncodeValue(value);
			objectData.Add(name, value2);
		}

		private JSONNode GetDictionary(IDictionary dictionary)
		{
			JSONClass jSONClass = new JSONClass();
			foreach (DictionaryEntry item in dictionary)
			{
				if (item.Value is ObjectInfo)
				{
					jSONClass.Add(item.Key as string, (item.Value as ObjectInfo).ToJson());
				}
				else if (item.Value is IDictionary)
				{
					jSONClass.Add(item.Key as string, GetDictionary(item.Value as IDictionary));
				}
				else if (item.Value is IList)
				{
					jSONClass.Add(item.Key as string, GetArray(item.Value as IList));
				}
				else
				{
					jSONClass.Add(item.Key as string, new JSONData(item.Value));
				}
			}
			return jSONClass;
		}

		private JSONNode GetArray(IList array)
		{
			JSONArray jSONArray = new JSONArray();
			foreach (object item in array)
			{
				if (item is IDictionary)
				{
					jSONArray.Add(GetDictionary(item as IDictionary));
				}
				else if (item is IList)
				{
					jSONArray.Add(GetArray(item as IList));
				}
				else if (item is ObjectInfo)
				{
					jSONArray.Add((item as ObjectInfo).ToJson());
				}
				else
				{
					jSONArray.Add(new JSONData(item));
				}
			}
			return jSONArray;
		}

		public JSONNode ToJson()
		{
			JSONClass jSONClass = new JSONClass();
			jSONClass.Add(SERIAL_VERSION_ID_KEY, new JSONData(SerialVersionId));
			jSONClass.Add(TYPE_KEY, new JSONData(TypeInfo));
			foreach (KeyValuePair<string, object> objectDatum in objectData)
			{
				if (objectDatum.Value is ObjectInfo)
				{
					jSONClass.Add(objectDatum.Key, (objectDatum.Value as ObjectInfo).ToJson());
				}
				else if (objectDatum.Value is IDictionary)
				{
					jSONClass.Add(objectDatum.Key, GetDictionary(objectDatum.Value as IDictionary));
				}
				else if (objectDatum.Value is IList)
				{
					jSONClass.Add(objectDatum.Key, GetArray(objectDatum.Value as IList));
				}
				else
				{
					jSONClass.Add(objectDatum.Key, new JSONData(objectDatum.Value));
				}
			}
			return jSONClass;
		}
	}
}
