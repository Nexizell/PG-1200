using System;
using System.IO;
using UnityEngine;

namespace Rilisoft
{
	public sealed class CrashReporter : MonoBehaviour
	{
		private string _reportText = string.Empty;

		private string _reportTime = string.Empty;

		private bool _showReport;

		internal void OnGUI()
		{
			float num = ((Screen.dpi == 0f) ? 160f : Screen.dpi);
			if (GUILayout.Button("Simulate exception", GUILayout.Width(1f * num)))
			{
				throw new InvalidOperationException(DateTime.Now.ToString("s"));
			}
			GUILayout.Label("Report path: " + Application.persistentDataPath);
			if (!string.IsNullOrEmpty(_reportText))
			{
				_showReport = GUILayout.Toggle(_showReport, "Show: " + _reportTime);
				if (_showReport)
				{
					GUILayout.Label(_reportText);
				}
			}
		}

		internal void Start()
		{
			if (Debug.isDebugBuild)
			{
				string[] array = new string[0];
				if (array.Length != 0)
				{
					string path = array[array.Length - 1];
					_reportTime = Path.GetFileNameWithoutExtension(path);
					_reportText = string.Empty;
				}
			}
			else
			{
				base.enabled = false;
			}
		}
	}
}
