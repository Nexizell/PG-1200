using System;
using System.IO;
using UnityEngine;

public sealed class SkinsManager
{
	public static string _PathBase
	{
		get
		{
			return Application.persistentDataPath;
		}
	}

	private static void _WriteImageAtPathToGal(string pathToImage)
	{
	}

	public static void SaveTextureToGallery(Texture2D t, string nm)
	{
		string pathToImage = Path.Combine(_PathBase, nm);
		_WriteImageAtPathToGal(pathToImage);
	}

	public static bool SaveTextureWithName(Texture2D t, string nm, bool writeToGallery = true)
	{
		string text = Path.Combine(_PathBase, nm);
		try
		{
			byte[] array = t.EncodeToPNG();
			if (File.Exists(text))
			{
				File.Delete(text);
			}
			using (FileStream fileStream = new FileStream(text, FileMode.Create, FileAccess.Write))
			{
				fileStream.Write(array, 0, array.Length);
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
		if (writeToGallery)
		{
			_WriteImageAtPathToGal(text);
		}
		return true;
	}

	public static byte[] ReadAllBytes(string path)
	{
		using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
		{
			int num = 0;
			long length = fileStream.Length;
			if (length > int.MaxValue)
			{
				throw new IOException("File is too long.");
			}
			int num2 = (int)length;
			byte[] array = new byte[num2];
			while (num2 > 0)
			{
				int num3 = fileStream.Read(array, num, num2);
				if (num3 == 0)
				{
					throw new EndOfStreamException("Read beyond end of file.");
				}
				num += num3;
				num2 -= num3;
			}
			return array;
		}
	}

	public static Texture2D TextureForName(string nm, int w = 64, int h = 32, bool disableMimMap = false)
	{
		Texture2D texture2D = (disableMimMap ? new Texture2D(w, h, TextureFormat.ARGB32, false) : new Texture2D(w, h));
		string text = Path.Combine(_PathBase, nm);
		try
		{
			byte[] data = ReadAllBytes(text);
			texture2D.LoadImage(data);
		}
		catch (Exception ex)
		{
			Debug.LogError(string.Format("Failed to read bytes from {0}\n{1}", new object[2] { text, ex }));
		}
		return texture2D;
	}

	public static bool DeleteTexture(string nm)
	{
		try
		{
			File.Delete(Path.Combine(_PathBase, nm));
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
		return true;
	}
}
