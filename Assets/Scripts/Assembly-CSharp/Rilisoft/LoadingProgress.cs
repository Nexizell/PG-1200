using System;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class LoadingProgress
	{
		private readonly GUIStyle _labelStyle;

		private readonly Texture2D _backgroundTexture;

		private readonly Texture2D _progressbarTexture;

		private static LoadingProgress _instance;

		public static LoadingProgress Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new LoadingProgress();
				}
				return _instance;
			}
		}

		private LoadingProgress()
		{
			_backgroundTexture = Resources.Load<Texture2D>("line_shadow");
			_progressbarTexture = Resources.Load<Texture2D>("line");
			_labelStyle = new GUIStyle(GUI.skin.label)
			{
				alignment = TextAnchor.MiddleCenter,
				font = Resources.Load<Font>("04B_03"),
				fontSize = Convert.ToInt32(22f * Defs.Coef),
				normal = new GUIStyleState
				{
					textColor = Color.black
				}
			};
		}

		public static void Unload()
		{
			if (_instance != null)
			{
				Resources.UnloadAsset(_instance._backgroundTexture);
				Resources.UnloadAsset(_instance._progressbarTexture);
				_instance = null;
			}
		}

		public void Draw(float progress)
		{
			float num = Mathf.Clamp01(progress);
			if (_backgroundTexture != null)
			{
				float num2 = 1.8f * (float)_backgroundTexture.width * Defs.Coef;
				float num3 = 1.8f * (float)_backgroundTexture.height * Defs.Coef;
				Rect rect = new Rect(0.5f * ((float)Screen.width - num2), (float)Screen.height - (21f * Defs.Coef + num3), num2, num3);
				float width = (num2 - 7.2f * Defs.Coef) * num;
				float height = num3 - 7.2f * Defs.Coef;
				if (_progressbarTexture != null)
				{
					GUI.DrawTexture(new Rect(rect.xMin + 3.6f * Defs.Coef, rect.yMin + 3.6f * Defs.Coef, width, height), _progressbarTexture);
				}
				GUI.DrawTexture(rect, _backgroundTexture);
				Rect position = rect;
				position.yMin -= 1.8f * Defs.Coef;
				position.y += 1.8f * Defs.Coef;
				int num4 = Mathf.RoundToInt(num * 100f);
				string text = string.Format("{0}%", new object[1] { num4 });
				GUI.Label(position, text, _labelStyle);
			}
		}
	}
}
