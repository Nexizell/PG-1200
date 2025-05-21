using System;
using System.IO;
using System.Text;
using DevToDev.Core.Utils;
using DevToDev.Logic;
using UnityEngine;

namespace DevToDev.Core.Serialization
{
	public abstract class IStorageable<T> : ISaveable where T : ISaveable
	{
		public abstract string StorageName();

		public abstract ISaveable GetBlankObject();

		public abstract ISaveable GetObject(byte[] data);

		public abstract byte[] SaveObject(ISaveable obj);

		public void Save(ISaveable storage)
		{
			string text = FullPath();
			string text2 = FileName();
			Stream stream = null;
			if (!UnityPlayerPlatform.isUnityWebPlatform())
			{
				try
				{
					if (!Directory.Exists(text))
					{
						try
						{
							Directory.CreateDirectory(text);
						}
						catch
						{
						}
					}
					string path = text + text2;
					if (File.Exists(path))
					{
						try
						{
							File.Delete(path);
						}
						catch
						{
						}
					}
					stream = File.Open(path, FileMode.CreateNew);
					byte[] array = SaveObject(storage);
					stream.Write(array, 0, array.Length);
					DataSaverPlatform.CloseStream(stream);
					return;
				}
				catch (Exception ex)
				{
					Log.E("Error write to file: " + ex.Message + "\n" + ex.StackTrace);
					return;
				}
				finally
				{
					if (stream != null)
					{
						DataSaverPlatform.CloseStream(stream);
					}
				}
			}
			try
			{
				byte[] array2 = SaveObject(storage);
				PlayerPrefs.SetString(text + text2, Encoding.UTF8.GetString(array2, 0, array2.Length));
				PlayerPrefs.Save();
			}
			catch (Exception ex2)
			{
				Log.E(ex2.Message + ":" + ex2.StackTrace);
			}
		}

		public ISaveable Load()
		{
			ISaveable saveable = null;
			string text = FullPath();
			string text2 = FileName();
			Stream stream = null;
			try
			{
				saveable = GetBlankObject();
				saveable.GetType();
				if (!UnityPlayerPlatform.isUnityWebPlatform())
				{
					stream = File.Open(text + text2, FileMode.Open);
					byte[] array = new byte[stream.Length];
					stream.Read(array, 0, array.Length);
					saveable = GetObject(array);
					if (stream != null)
					{
						DataSaverPlatform.CloseStream(stream);
					}
				}
				else
				{
					try
					{
						string @string = PlayerPrefs.GetString(text + text2);
						if (!string.IsNullOrEmpty(@string))
						{
							saveable = GetObject(Encoding.UTF8.GetBytes(@string));
						}
					}
					catch (Exception ex)
					{
						Log.E(ex.Message + ":" + ex.StackTrace);
					}
				}
			}
			catch (Exception ex2)
			{
				Log.D("File not found: " + text + text2 + "\r\n" + ex2.Message + ex2.StackTrace);
				if (stream != null)
				{
					DataSaverPlatform.CloseStream(stream);
				}
				if (saveable != null)
				{
					Save(saveable);
				}
			}
			return saveable;
		}

		private string FullPath()
		{
			if (!UnityPlayerPlatform.isUnityWebPlatform())
			{
				return DataSaverPlatform.GetFullPath(SDKClient.Instance.AppKey);
			}
			return "devtodev_" + SDKClient.Instance.AppKey + "_";
		}

		private string FileName()
		{
			return StorageName();
		}
	}
}
