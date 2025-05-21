using System;

namespace DevToDev.Utils.Gzip.Zlib
{
	internal static class Path2
	{
		internal const int MAX_PATH = 260;

		internal const int MAX_DIRECTORY_PATH = 248;

		public static readonly char DirectorySeparatorChar;

		public static readonly char AltDirectorySeparatorChar;

		public static readonly char VolumeSeparatorChar;

		public static readonly char[] InvalidPathChars;

		internal static readonly char[] TrimEndChars;

		private static readonly char[] RealInvalidPathChars;

		private static readonly char[] InvalidFileNameChars;

		public static readonly char PathSeparator;

		internal static readonly int MaxPath;

		private static readonly int MaxDirectoryLength;

		internal static readonly int MaxLongPath;

		private static readonly string Prefix;

		private static readonly char[] s_Base32Char;

		static Path2()
		{
			DirectorySeparatorChar = '\\';
			AltDirectorySeparatorChar = '/';
			VolumeSeparatorChar = ':';
			char[] invalidPathChars = new char[36]
			{
				'"', '<', '>', '|', '\0', '\u0001', '\u0002', '\u0003', '\u0004', '\u0005',
				'\u0006', '\a', '\b', '\t', '\n', '\v', '\f', '\r', '\u000e', '\u000f',
				'\u0010', '\u0011', '\u0012', '\u0013', '\u0014', '\u0015', '\u0016', '\u0017', '\u0018', '\u0019',
				'\u001a', '\u001b', '\u001c', '\u001d', '\u001e', '\u001f'
			};
			InvalidPathChars = invalidPathChars;
			char[] trimEndChars = new char[8] { '\t', '\n', '\v', '\f', '\r', ' ', '\u0085', '\u00a0' };
			TrimEndChars = trimEndChars;
			char[] realInvalidPathChars = new char[36]
			{
				'"', '<', '>', '|', '\0', '\u0001', '\u0002', '\u0003', '\u0004', '\u0005',
				'\u0006', '\a', '\b', '\t', '\n', '\v', '\f', '\r', '\u000e', '\u000f',
				'\u0010', '\u0011', '\u0012', '\u0013', '\u0014', '\u0015', '\u0016', '\u0017', '\u0018', '\u0019',
				'\u001a', '\u001b', '\u001c', '\u001d', '\u001e', '\u001f'
			};
			RealInvalidPathChars = realInvalidPathChars;
			char[] invalidFileNameChars = new char[41]
			{
				'"', '<', '>', '|', '\0', '\u0001', '\u0002', '\u0003', '\u0004', '\u0005',
				'\u0006', '\a', '\b', '\t', '\n', '\v', '\f', '\r', '\u000e', '\u000f',
				'\u0010', '\u0011', '\u0012', '\u0013', '\u0014', '\u0015', '\u0016', '\u0017', '\u0018', '\u0019',
				'\u001a', '\u001b', '\u001c', '\u001d', '\u001e', '\u001f', ':', '*', '?', '\\',
				'/'
			};
			InvalidFileNameChars = invalidFileNameChars;
			PathSeparator = ';';
			MaxPath = 260;
			MaxDirectoryLength = 255;
			MaxLongPath = 32000;
			Prefix = "\\\\?\\";
			char[] array = new char[32]
			{
				'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
				'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
				'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3',
				'4', '5'
			};
			s_Base32Char = array;
		}

		internal static void CheckInvalidPathChars(string path, bool checkAdditional = false)
		{
			if (path != null)
			{
				if (!HasIllegalCharacters(path, checkAdditional))
				{
					return;
				}
				throw new ArgumentException("The path has invalid characters.", "path");
			}
			throw new ArgumentNullException("path");
		}

		internal static bool HasIllegalCharacters(string path, bool checkAdditional)
		{
			foreach (int num in path)
			{
				if (num == 34 || num == 60 || num == 62 || num == 124 || num < 32)
				{
					return true;
				}
				if (checkAdditional && (num == 63 || num == 42))
				{
					return true;
				}
			}
			return false;
		}

		public static string GetFileName(string path)
		{
			if (path != null)
			{
				CheckInvalidPathChars(path);
				int length = path.Length;
				int num = length;
				char c;
				do
				{
					int num2 = num - 1;
					num = num2;
					if (num2 < 0)
					{
						return path;
					}
					c = path[num];
				}
				while (c != DirectorySeparatorChar && c != AltDirectorySeparatorChar && c != VolumeSeparatorChar);
				return path.Substring(num + 1, length - num - 1);
			}
			return path;
		}
	}
}
