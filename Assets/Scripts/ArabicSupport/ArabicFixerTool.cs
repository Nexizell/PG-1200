using System;
using System.Collections.Generic;

internal class ArabicFixerTool
{
	internal static bool showTashkeel = true;

	internal static bool useHinduNumbers = false;

	internal static string RemoveTashkeel(string str, out List<TashkeelLocation> tashkeelLocation)
	{
		tashkeelLocation = new List<TashkeelLocation>();
		char[] array = str.ToCharArray();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == '\u064b')
			{
				tashkeelLocation.Add(new TashkeelLocation('\u064b', i));
			}
			else if (array[i] == '\u064c')
			{
				tashkeelLocation.Add(new TashkeelLocation('\u064c', i));
			}
			else if (array[i] == '\u064d')
			{
				tashkeelLocation.Add(new TashkeelLocation('\u064d', i));
			}
			else if (array[i] == '\u064e')
			{
				tashkeelLocation.Add(new TashkeelLocation('\u064e', i));
			}
			else if (array[i] == '\u064f')
			{
				tashkeelLocation.Add(new TashkeelLocation('\u064f', i));
			}
			else if (array[i] == '\u0650')
			{
				tashkeelLocation.Add(new TashkeelLocation('\u0650', i));
			}
			else if (array[i] == '\u0651')
			{
				tashkeelLocation.Add(new TashkeelLocation('\u0651', i));
			}
			else if (array[i] == '\u0652')
			{
				tashkeelLocation.Add(new TashkeelLocation('\u0652', i));
			}
			else if (array[i] == '\u0653')
			{
				tashkeelLocation.Add(new TashkeelLocation('\u0653', i));
			}
		}
		string[] array2 = str.Split('\u064b', '\u064c', '\u064d', '\u064e', '\u064f', '\u0650', '\u0651', '\u0652', '\u0653');
		str = "";
		string[] array3 = array2;
		foreach (string text in array3)
		{
			str += text;
		}
		return str;
	}

	internal static char[] ReturnTashkeel(char[] letters, List<TashkeelLocation> tashkeelLocation)
	{
		char[] array = new char[letters.Length + tashkeelLocation.Count];
		int num = 0;
		for (int i = 0; i < letters.Length; i++)
		{
			array[num] = letters[i];
			num++;
			foreach (TashkeelLocation item in tashkeelLocation)
			{
				if (item.position == num)
				{
					array[num] = item.tashkeel;
					num++;
				}
			}
		}
		return array;
	}

	internal static string FixLine(string str)
	{
		string text = "";
		List<TashkeelLocation> tashkeelLocation;
		string text2 = RemoveTashkeel(str, out tashkeelLocation);
		char[] array = text2.ToCharArray();
		char[] array2 = text2.ToCharArray();
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (char)ArabicTable.ArabicMapper.Convert(array[i]);
		}
		for (int i = 0; i < array.Length; i++)
		{
			bool flag = false;
			if (i == 2)
			{
				num = 0;
			}
			if (array[i] == '\udf21')
			{
				num = num;
			}
			if (array[i] == 'ﻝ' && i < array.Length - 1)
			{
				if (array[i + 1] == 'ﺇ')
				{
					array[i] = 'ﻷ';
					array2[i + 1] = '\uffff';
					flag = true;
				}
				else if (array[i + 1] == 'ﺍ')
				{
					array[i] = 'ﻹ';
					array2[i + 1] = '\uffff';
					flag = true;
				}
				else if (array[i + 1] == 'ﺃ')
				{
					array[i] = 'ﻵ';
					array2[i + 1] = '\uffff';
					flag = true;
				}
				else if (array[i + 1] == 'ﺁ')
				{
					array[i] = 'ﻳ';
					array2[i + 1] = '\uffff';
					flag = true;
				}
			}
			if (!IsIgnoredCharacter(array[i]) && array[i] != 'A' && array[i] != 'A')
			{
				if (IsMiddleLetter(array, i))
				{
					array2[i] = (char)(array[i] + 3);
				}
				else if (IsFinishingLetter(array, i))
				{
					array2[i] = (char)(array[i] + 1);
				}
				else if (IsLeadingLetter(array, i))
				{
					array2[i] = (char)(array[i] + 2);
				}
			}
			text = text + Convert.ToString(array[i], 16) + " ";
			if (flag)
			{
				i++;
			}
			if (useHinduNumbers)
			{
				if (array[i] == '0')
				{
					array2[i] = '٠';
				}
				else if (array[i] == '1')
				{
					array2[i] = '١';
				}
				else if (array[i] == '2')
				{
					array2[i] = '٢';
				}
				else if (array[i] == '3')
				{
					array2[i] = '٣';
				}
				else if (array[i] == '4')
				{
					array2[i] = '٤';
				}
				else if (array[i] == '5')
				{
					array2[i] = '٥';
				}
				else if (array[i] == '6')
				{
					array2[i] = '٦';
				}
				else if (array[i] == '7')
				{
					array2[i] = '٧';
				}
				else if (array[i] == '8')
				{
					array2[i] = '٨';
				}
				else if (array[i] == '9')
				{
					array2[i] = '٩';
				}
			}
		}
		if (showTashkeel)
		{
			array2 = ReturnTashkeel(array2, tashkeelLocation);
		}
		List<char> list = new List<char>();
		List<char> list2 = new List<char>();
		for (int i = array2.Length - 1; i >= 0; i--)
		{
			if (char.IsPunctuation(array2[i]) && i > 0 && i < array2.Length - 1 && (char.IsPunctuation(array2[i - 1]) || char.IsPunctuation(array2[i + 1])))
			{
				if (array2[i] == '(')
				{
					list.Add(')');
				}
				else if (array2[i] == ')')
				{
					list.Add('(');
				}
				else if (array2[i] == '<')
				{
					list.Add('>');
				}
				else if (array2[i] == '>')
				{
					list.Add('<');
				}
				else if (array2[i] != '\uffff')
				{
					list.Add(array2[i]);
				}
			}
			else if (array2[i] == ' ' && i > 0 && i < array2.Length - 1 && (char.IsLower(array2[i - 1]) || char.IsUpper(array2[i - 1]) || char.IsNumber(array2[i - 1])) && (char.IsLower(array2[i + 1]) || char.IsUpper(array2[i + 1]) || char.IsNumber(array2[i + 1])))
			{
				list2.Add(array2[i]);
			}
			else if (char.IsNumber(array2[i]) || char.IsLower(array2[i]) || char.IsUpper(array2[i]) || char.IsSymbol(array2[i]) || char.IsPunctuation(array2[i]))
			{
				if (array2[i] == '(')
				{
					list2.Add(')');
				}
				else if (array2[i] == ')')
				{
					list2.Add('(');
				}
				else if (array2[i] == '<')
				{
					list2.Add('>');
				}
				else if (array2[i] == '>')
				{
					list2.Add('<');
				}
				else
				{
					list2.Add(array2[i]);
				}
			}
			else if ((array2[i] >= '\ud800' && array2[i] <= '\udbff') || (array2[i] >= '\udc00' && array2[i] <= '\udfff'))
			{
				list2.Add(array2[i]);
			}
			else
			{
				if (list2.Count > 0)
				{
					for (int j = 0; j < list2.Count; j++)
					{
						list.Add(list2[list2.Count - 1 - j]);
					}
					list2.Clear();
				}
				if (array2[i] != '\uffff')
				{
					list.Add(array2[i]);
				}
			}
		}
		if (list2.Count > 0)
		{
			for (int j = 0; j < list2.Count; j++)
			{
				list.Add(list2[list2.Count - 1 - j]);
			}
			list2.Clear();
		}
		array2 = new char[list.Count];
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = list[i];
		}
		str = new string(array2);
		return str;
	}

	internal static bool IsIgnoredCharacter(char ch)
	{
		bool flag = char.IsPunctuation(ch);
		bool flag2 = char.IsNumber(ch);
		bool flag3 = char.IsLower(ch);
		bool flag4 = char.IsUpper(ch);
		bool flag5 = char.IsSymbol(ch);
		bool flag6 = ch == 'ﭖ' || ch == 'ﭺ' || ch == 'ﮊ' || ch == 'ﮒ';
		bool flag7 = (ch <= '\ufeff' && ch >= 'ﹰ') || flag6;
		return flag || flag2 || flag3 || flag4 || flag5 || !flag7 || ch == 'a' || ch == '>' || ch == '<' || ch == '؛';
	}

	internal static bool IsLeadingLetter(char[] letters, int index)
	{
		if ((index == 0 || letters[index - 1] == ' ' || letters[index - 1] == '*' || letters[index - 1] == 'A' || char.IsPunctuation(letters[index - 1]) || letters[index - 1] == '>' || letters[index - 1] == '<' || letters[index - 1] == 'ﺍ' || letters[index - 1] == 'ﺩ' || letters[index - 1] == 'ﺫ' || letters[index - 1] == 'ﺭ' || letters[index - 1] == 'ﺯ' || letters[index - 1] == 'ﮊ' || letters[index - 1] == 'ﻯ' || letters[index - 1] == 'ﻭ' || letters[index - 1] == 'ﺁ' || letters[index - 1] == 'ﺃ' || letters[index - 1] == 'ﺇ' || letters[index - 1] == 'ﺅ') && letters[index] != ' ' && letters[index] != 'ﺩ' && letters[index] != 'ﺫ' && letters[index] != 'ﺭ' && letters[index] != 'ﺯ' && letters[index] != 'ﮊ' && letters[index] != 'ﺍ' && letters[index] != 'ﺃ' && letters[index] != 'ﺇ' && letters[index] != 'ﻭ' && letters[index] != 'ﺀ' && index < letters.Length - 1 && letters[index + 1] != ' ' && !char.IsPunctuation(letters[index + 1]) && letters[index + 1] != 'ﺀ')
		{
			return true;
		}
		return false;
	}

	internal static bool IsFinishingLetter(char[] letters, int index)
	{
		if (index != 0 && letters[index - 1] != ' ' && letters[index - 1] != '*' && letters[index - 1] != 'A' && letters[index - 1] != 'ﺩ' && letters[index - 1] != 'ﺫ' && letters[index - 1] != 'ﺭ' && letters[index - 1] != 'ﺯ' && letters[index - 1] != 'ﮊ' && letters[index - 1] != 'ﻯ' && letters[index - 1] != 'ﻭ' && letters[index - 1] != 'ﺍ' && letters[index - 1] != 'ﺁ' && letters[index - 1] != 'ﺃ' && letters[index - 1] != 'ﺇ' && letters[index - 1] != 'ﺅ' && letters[index - 1] != 'ﺀ' && !char.IsPunctuation(letters[index - 1]) && letters[index - 1] != '>' && letters[index - 1] != '<' && letters[index] != ' ' && index < letters.Length && letters[index] != 'ﺀ')
		{
			return true;
		}
		return false;
	}

	internal static bool IsMiddleLetter(char[] letters, int index)
	{
		if (index != 0 && letters[index] != ' ' && letters[index] != 'ﺍ' && letters[index] != 'ﺩ' && letters[index] != 'ﺫ' && letters[index] != 'ﺭ' && letters[index] != 'ﺯ' && letters[index] != 'ﮊ' && letters[index] != 'ﻯ' && letters[index] != 'ﻭ' && letters[index] != 'ﺁ' && letters[index] != 'ﺃ' && letters[index] != 'ﺇ' && letters[index] != 'ﺅ' && letters[index] != 'ﺀ' && letters[index - 1] != 'ﺍ' && letters[index - 1] != 'ﺩ' && letters[index - 1] != 'ﺫ' && letters[index - 1] != 'ﺭ' && letters[index - 1] != 'ﺯ' && letters[index - 1] != 'ﮊ' && letters[index - 1] != 'ﻯ' && letters[index - 1] != 'ﻭ' && letters[index - 1] != 'ﺁ' && letters[index - 1] != 'ﺃ' && letters[index - 1] != 'ﺇ' && letters[index - 1] != 'ﺅ' && letters[index - 1] != 'ﺀ' && letters[index - 1] != '>' && letters[index - 1] != '<' && letters[index - 1] != ' ' && letters[index - 1] != '*' && !char.IsPunctuation(letters[index - 1]) && index < letters.Length - 1 && letters[index + 1] != ' ' && letters[index + 1] != '\r' && letters[index + 1] != 'A' && letters[index + 1] != '>' && letters[index + 1] != '>' && letters[index + 1] != 'ﺀ')
		{
			try
			{
				if (char.IsPunctuation(letters[index + 1]))
				{
					return false;
				}
				return true;
			}
			catch
			{
				return false;
			}
		}
		return false;
	}
}
