using System;
using System.IO;

namespace DevToDev.Core.Utils
{
	public class JSONData : JSONNode
	{
		private string m_Data;

		public override string Value
		{
			get
			{
				return m_Data;
			}
			set
			{
				m_Data = value;
				Tag = JSONBinaryTag.Value;
			}
		}

		public JSONData(object aData)
		{
			if (aData == null)
			{
				m_Data = "null";
				Tag = JSONBinaryTag.Null;
				return;
			}
			if (aData is int)
			{
				AsInt = (int)aData;
				return;
			}
			if (aData is float)
			{
				AsFloat = (float)aData;
				return;
			}
			if (aData is double)
			{
				AsDouble = (double)aData;
				return;
			}
			if (aData is long)
			{
				AsLong = (long)aData;
				return;
			}
			if (aData is string)
			{
				m_Data = aData as string;
				Tag = JSONBinaryTag.Value;
				return;
			}
			if (aData is bool)
			{
				AsBool = (bool)aData;
				return;
			}
			throw new Exception("No such type in JSON parser: " + aData.GetType());
		}

		public JSONData(string aData)
		{
			if (aData == null)
			{
				m_Data = "null";
				Tag = JSONBinaryTag.Null;
			}
			m_Data = aData;
			Tag = JSONBinaryTag.Value;
		}

		public JSONData(long aData)
		{
			AsLong = aData;
		}

		public JSONData(float aData)
		{
			AsFloat = aData;
		}

		public JSONData(double aData)
		{
			AsDouble = aData;
		}

		public JSONData(bool aData)
		{
			AsBool = aData;
		}

		public JSONData(int aData)
		{
			AsInt = aData;
		}

		public override string ToString()
		{
			return "\"" + JSONNode.Escape(m_Data) + "\"";
		}

		public override string ToString(string aPrefix)
		{
			return "\"" + JSONNode.Escape(m_Data) + "\"";
		}

		public override string ToJSON(int prefix)
		{
			switch (Tag)
			{
			case JSONBinaryTag.IntValue:
			case JSONBinaryTag.DoubleValue:
			case JSONBinaryTag.BoolValue:
			case JSONBinaryTag.FloatValue:
				return m_Data;
			case JSONBinaryTag.Value:
				return string.Format("\"{0}\"", JSONNode.Escape(m_Data));
			case JSONBinaryTag.Null:
				return "null";
			default:
				throw new NotSupportedException("This shouldn't be here: " + Tag);
			}
		}

		public override void Serialize(BinaryWriter aWriter)
		{
			JSONData jSONData = new JSONData("");
			jSONData.AsInt = AsInt;
			if (jSONData.m_Data == m_Data)
			{
				aWriter.Write((byte)4);
				aWriter.Write(AsInt);
				return;
			}
			jSONData.AsFloat = AsFloat;
			if (jSONData.m_Data == m_Data)
			{
				aWriter.Write((byte)7);
				aWriter.Write(AsFloat);
				return;
			}
			jSONData.AsDouble = AsDouble;
			if (jSONData.m_Data == m_Data)
			{
				aWriter.Write((byte)5);
				aWriter.Write(AsDouble);
				return;
			}
			jSONData.AsBool = AsBool;
			if (jSONData.m_Data == m_Data)
			{
				aWriter.Write((byte)6);
				aWriter.Write(AsBool);
			}
			else
			{
				aWriter.Write((byte)3);
				aWriter.Write(m_Data);
			}
		}
	}
}
