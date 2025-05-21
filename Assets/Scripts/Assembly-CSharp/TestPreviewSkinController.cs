using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class TestPreviewSkinController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CStart_003Ed__2 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public TestPreviewSkinController _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CStart_003Ed__2(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this.Skins();
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	public UIGrid grid;

	public GameObject previewPrefab;

	private IEnumerator Start()
	{
		yield return null;
		Skins();
	}

	private void Skins()
	{
		for (int i = 0; i < SkinsController.baseSkinsForPersInString.Length; i++)
		{
			Texture2D texture = SkinsController.TextureFromString(SkinsController.baseSkinsForPersInString[i]);
			Texture2D texture2D = new Texture2D(16, 32, TextureFormat.ARGB32, false);
			for (int j = 0; j < 16; j++)
			{
				for (int k = 0; k < 32; k++)
				{
					texture2D.SetPixel(j, k, Color.clear);
				}
			}
			texture2D.SetPixels(4, 24, 8, 8, GetPixelsByRect(texture, new Rect(8f, 16f, 8f, 8f)));
			texture2D.SetPixels(4, 12, 8, 12, GetPixelsByRect(texture, new Rect(20f, 0f, 8f, 12f)));
			texture2D.SetPixels(0, 12, 4, 12, GetPixelsByRect(texture, new Rect(44f, 0f, 4f, 12f)));
			texture2D.SetPixels(12, 12, 4, 12, GetPixelsByRect(texture, new Rect(44f, 0f, 4f, 12f)));
			texture2D.SetPixels(4, 0, 4, 12, GetPixelsByRect(texture, new Rect(4f, 0f, 4f, 12f)));
			texture2D.SetPixels(8, 0, 4, 12, GetPixelsByRect(texture, new Rect(4f, 0f, 4f, 12f)));
			texture2D.anisoLevel = 1;
			texture2D.mipMapBias = -0.5f;
			texture2D.Apply();
			texture2D.filterMode = FilterMode.Point;
			texture2D.Apply();
			GameObject obj = UnityEngine.Object.Instantiate(previewPrefab);
			obj.transform.parent = grid.transform;
			obj.transform.localPosition = new Vector3(160 * i, 0f, 0f);
			obj.transform.localScale = new Vector3(1f, 1f, 1f);
			obj.GetComponent<SetTestSkinPreview>().texture.mainTexture = texture2D;
			obj.GetComponent<SetTestSkinPreview>().nameLabel.text = i.ToString();
			obj.GetComponent<SetTestSkinPreview>().keyLabel.text = (SkinsController.shopKeyFromNameSkin.ContainsKey(i.ToString()) ? SkinsController.shopKeyFromNameSkin[i.ToString()] : "");
			obj.name = i.ToString();
		}
		string text = "";
		string[] array = new string[2] { "player3", "player4" };
		for (int l = 1; l <= 4; l++)
		{
			text = text + "\"" + SkinsController.StringFromTexture(Resources.Load("MultSkins_6_3/10.6.0_Skin" + l) as Texture2D) + "\",\n";
		}
		UnityEngine.Debug.Log(text);
	}

	private Color[] GetPixelsByRect(Texture2D texture, Rect rect)
	{
		return texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
	}
}
