using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Rilisoft
{
	internal class BackgroundsManager
	{
		private const int MIN_IMAGE_FILE_SIZE = 100000;

		private const string SCREENSHOT_TEXTURE_FILE_NAME = "lobby_textures/custom_lobby_foto.png";

		private const string SCREENSHOT_TEXTURE_FILE_NAME_BLURED = "lobby_textures/custom_lobby_foto_blured.png";

		private static Lazy<BackgroundsManager> _instance = new Lazy<BackgroundsManager>(() => new BackgroundsManager());

		private static Texture2D _loadingBackgroudTexture;

		private static Texture2D _loadingBackgroudTextureBlured;

		private int _lastScreenshotFrame;

		public static BackgroundsManager Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		public static Texture2D LoadingBackgroudTexture
		{
			get
			{
				if (_loadingBackgroudTexture == null)
				{
					_loadingBackgroudTexture = LoadTexture("lobby_textures/custom_lobby_foto.png");
				}
				return _loadingBackgroudTexture;
			}
			private set
			{
				if (_loadingBackgroudTexture != null)
				{
					Resources.UnloadAsset(_loadingBackgroudTexture);
				}
				_loadingBackgroudTexture = value;
			}
		}

		public static Texture2D LoadingBackgroudTextureBlured
		{
			get
			{
				if (_loadingBackgroudTextureBlured == null)
				{
					_loadingBackgroudTextureBlured = LoadTexture("lobby_textures/custom_lobby_foto_blured.png");
					if (_loadingBackgroudTextureBlured == null)
					{
						_loadingBackgroudTextureBlured = LoadingBackgroudTexture;
					}
				}
				return _loadingBackgroudTextureBlured;
			}
			private set
			{
				if (_loadingBackgroudTextureBlured != null)
				{
					Resources.UnloadAsset(_loadingBackgroudTextureBlured);
				}
				_loadingBackgroudTextureBlured = value;
			}
		}

		public void MakeLobbyScreenshot()
		{
			if (_lastScreenshotFrame == Time.frameCount)
			{
				return;
			}
			_lastScreenshotFrame = Time.frameCount;
			HideObjects();
			try
			{
				Camera mainCamera = MainMenuHeroCamera.Instance.MainCamera;
				int num = 0;
				int num2 = 0;
				if (Device.isRetinaAndStrong)
				{
					num = 1920;
					num2 = 1080;
				}
				else
				{
					num = 1280;
					num2 = 720;
				}
				Texture2D texture2D = ScreenshotManager.TakeScreenshot(mainCamera, num, num2);
				SaveTexture(texture2D, "lobby_textures/custom_lobby_foto.png");
				UnityEngine.Object.Destroy(texture2D);
				_loadingBackgroudTexture = null;
				int num3 = 640;
				int num4 = 360;
				RenderTexture renderTexture2 = (mainCamera.targetTexture = new RenderTexture(num3, num4, 16));
				mainCamera.Render();
				RenderTexture.active = renderTexture2;
				Material mat = new Material(Shader.Find("ImageEffect/Blur"));
				Texture2D texture2D2 = new Texture2D(num3, num4, TextureFormat.RGB24, false);
				texture2D2.ReadPixels(new Rect(0f, 0f, num3, num4), 0, 0);
				texture2D2.Apply();
				renderTexture2.DiscardContents();
				Graphics.Blit(texture2D2, renderTexture2, mat);
				texture2D2.ReadPixels(new Rect(0f, 0f, num3, num4), 0, 0);
				texture2D2.Apply();
				renderTexture2.DiscardContents();
				Graphics.Blit(texture2D2, renderTexture2, mat);
				texture2D2.ReadPixels(new Rect(0f, 0f, num3, num4), 0, 0);
				texture2D2.Apply();
				mainCamera.targetTexture = null;
				RenderTexture.active = null;
				SaveTexture(texture2D2, "lobby_textures/custom_lobby_foto_blured.png");
				_loadingBackgroudTextureBlured = null;
				UnityEngine.Object.Destroy(renderTexture2);
				UnityEngine.Object.Destroy(texture2D2);
			}
			catch (Exception ex)
			{
				ShowObjects();
				throw ex;
			}
			finally
			{
				ShowObjects();
			}
		}

		private void HideObjects()
		{
			RiliExtensions.ForEach(GetObjectsForHide(), delegate(GameObject o)
			{
				SetObjectVisible(o, false);
			});
		}

		private void ShowObjects()
		{
			RiliExtensions.ForEach(GetObjectsForHide(), delegate(GameObject o)
			{
				SetObjectVisible(o, true);
			});
		}

		public static List<GameObject> GetObjectsForHide(bool hidePers = true)
		{
			List<GameObject> list = new List<GameObject>();
			if (AnimationGift.instance != null)
			{
				list.Add(AnimationGift.instance.gameObject);
			}
			if (FreeAwardShowHandler.Instance != null)
			{
				list.Add(FreeAwardShowHandler.Instance.gameObject);
			}
			if (Nest.Instance != null)
			{
				list.Add(Nest.Instance.gameObject);
			}
			if (LeprechauntLobbyView.Instance != null)
			{
				list.Add(LeprechauntLobbyView.Instance.gameObject);
			}
			if (hidePers)
			{
				if (PersConfigurator.currentConfigurator != null)
				{
					GameObject gameObject = PersConfigurator.currentConfigurator.gameObject.transform.root.gameObject;
					list.Add(gameObject);
				}
				if (PersConfigurator.currentConfigurator != null && PersConfigurator.currentConfigurator.characterInterface != null)
				{
					GameObject myPet = PersConfigurator.currentConfigurator.characterInterface.myPet;
					list.Add(myPet);
				}
			}
			return list;
		}

		public static void SetObjectVisible(GameObject go, bool visible)
		{
			if (!(go == null) && !go.Equals(null))
			{
				int num = (visible ? 5000 : (-5000));
				Vector3 localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y + (float)num, go.transform.localPosition.z);
				go.transform.localPosition = localPosition;
			}
		}

		private static void SaveTexture(Texture2D texture, string fileName)
		{
			if (texture == null)
			{
				throw new ArgumentNullException("texture is null");
			}
			string text = Path.Combine(PersistentCache.Instance.PersistentDataPath, fileName);
			string directoryName = Path.GetDirectoryName(text);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			byte[] array = texture.EncodeToPNG();
			if (array.Length < 100000)
			{
				if (File.Exists(text) && File.ReadAllBytes(text).Length < 100000)
				{
					File.Delete(text);
				}
			}
			else
			{
				File.WriteAllBytes(text, array);
				Debug.Log(string.Format(">>> save texture to {0}, file size: {1}b", new object[2] { text, array.Length }));
			}
		}

		private static Texture2D LoadTexture(string fileName)
		{
			string path = Path.Combine(PersistentCache.Instance.PersistentDataPath, fileName);
			if (!File.Exists(path))
			{
				return null;
			}
			byte[] array = File.ReadAllBytes(path);
			if (array.Length < 100000)
			{
				return null;
			}
			Texture2D texture2D = new Texture2D(2, 2);
			texture2D.LoadImage(array);
			texture2D.filterMode = FilterMode.Trilinear;
			return texture2D;
		}
	}
}
