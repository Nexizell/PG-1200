using System;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

namespace DevToDev.Core.Utils
{
	internal static class Log
	{
		private static readonly string tag;

		private static string DIRECTORY_NAME;

		private static readonly string FILE_NAME;

		private static readonly bool DEBUG_LOG_ENABLED;

		private static StringBuilder logBuffer;

		private static Timer timer;

		public static bool LogEnabled { get; set; }

		static Log()
		{
			tag = "DevToDev";
			FILE_NAME = "log.txt";
			DEBUG_LOG_ENABLED = false;
			logBuffer = new StringBuilder();
			if (DIRECTORY_NAME == null)
			{
				DIRECTORY_NAME = LogPlatform.GetDirectoryPath();
			}
			if (DEBUG_LOG_ENABLED && !UnityPlayerPlatform.isUnityWebPlatform())
			{
				timer = new Timer(AppendLog, null, 1000, 1000);
			}
		}

		public static void Resume()
		{
			if (!UnityPlayerPlatform.isUnityWebPlatform() && DEBUG_LOG_ENABLED && timer == null)
			{
				timer = new Timer(AppendLog, null, 1000, 1000);
			}
		}

		public static void Suspend()
		{
			if (timer != null)
			{
				timer.Dispose();
				timer = null;
			}
		}

		private static void AppendLog(object state)
		{
			if (logBuffer.Length <= 0)
			{
				return;
			}
			Stream stream = null;
			try
			{
				if (!Directory.Exists(DIRECTORY_NAME))
				{
					Directory.CreateDirectory(DIRECTORY_NAME);
				}
				string path = DIRECTORY_NAME + "\\" + FILE_NAME;
				using (stream = File.Open(path, FileMode.Append))
				{
					lock (logBuffer)
					{
						byte[] bytes = Encoding.UTF8.GetBytes(logBuffer.ToString());
						stream.Write(bytes, 0, bytes.Length);
						logBuffer.Length = 0;
						logBuffer.Capacity = 0;
					}
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				if (stream != null)
				{
					LogPlatform.CloseStream(stream);
				}
			}
		}

		public static void D(string logMessage)
		{
			D(logMessage, null);
		}

		public static void D(string format, params object[] args)
		{
			if (LogEnabled && DEBUG_LOG_ENABLED)
			{
				string text = "[" + tag + "] " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " : " + ((args != null) ? string.Format(format, args) : format);
				Debug.Log(text);
				lock (logBuffer)
				{
					logBuffer.Append(text).Append("\r\n");
				}
			}
		}

		public static void E(string logMessage)
		{
			E(logMessage, null);
		}

		public static void E(string format, params object[] args)
		{
			if (LogEnabled && DEBUG_LOG_ENABLED)
			{
				string text = "[" + tag + "] " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " : " + ((args != null) ? string.Format(format, args) : format);
				Debug.LogError(text);
				lock (logBuffer)
				{
					logBuffer.Append(text).Append("\r\n");
				}
			}
		}

		public static void R(string logMessage)
		{
			R(logMessage, null);
		}

		public static void R(string format, params object[] args)
		{
			if (!LogEnabled)
			{
				return;
			}
			string text = "[" + tag + "] " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " : " + ((args != null) ? string.Format(format, args) : format);
			Debug.Log(text);
			if (DEBUG_LOG_ENABLED)
			{
				lock (logBuffer)
				{
					logBuffer.Append(text).Append("\r\n");
				}
			}
		}
	}
}
