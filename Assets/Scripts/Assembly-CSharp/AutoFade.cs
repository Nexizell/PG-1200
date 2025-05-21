using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public class AutoFade : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CFade_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private float _003Ct_003E5__1;

		public float aFadeOutTime;

		public AutoFade _003C_003E4__this;

		public Color aColor;

		public bool collectGrabage;

		public float aFadeInTime;

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
		public _003CFade_003Ed__12(int _003C_003E1__state)
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
				_003Ct_003E5__1 = 0f;
				goto IL_0083;
			case 1:
				_003C_003E1__state = -1;
				_003Ct_003E5__1 = Mathf.Clamp01(_003Ct_003E5__1 + Time.deltaTime / aFadeOutTime);
				_003C_003E4__this.DrawQuad(aColor, _003Ct_003E5__1);
				goto IL_0083;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0130;
			case 3:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0083:
				if (_003Ct_003E5__1 < 1f)
				{
					_003C_003E2__current = new WaitForEndOfFrame();
					_003C_003E1__state = 1;
					return true;
				}
				if (collectGrabage)
				{
					GC.Collect();
				}
				if (_003C_003E4__this.isLoadScene)
				{
					if (_003C_003E4__this.m_LevelName != "")
					{
						Singleton<SceneLoader>.Instance.LoadScene(_003C_003E4__this.m_LevelName);
					}
					break;
				}
				goto IL_0130;
				IL_0130:
				if (_003C_003E4__this.killedTime > 0f)
				{
					_003C_003E4__this.killedTime -= Time.deltaTime;
					_003C_003E4__this.DrawQuad(aColor, _003Ct_003E5__1);
					_003C_003E2__current = new WaitForEndOfFrame();
					_003C_003E1__state = 2;
					return true;
				}
				break;
			}
			if (_003Ct_003E5__1 > 0f && !(Mathf.Abs(aFadeInTime) < 1E-06f))
			{
				_003Ct_003E5__1 = Mathf.Clamp01(_003Ct_003E5__1 - Time.deltaTime / aFadeInTime);
				_003C_003E4__this.DrawQuad(aColor, _003Ct_003E5__1);
				_003C_003E2__current = new WaitForEndOfFrame();
				_003C_003E1__state = 3;
				return true;
			}
			_003C_003E4__this.m_Fading = false;
			return false;
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

	private static AutoFade m_Instance;

	private Material m_Material;

	private string m_LevelName = "";

	private bool m_Fading;

	private bool isLoadScene = true;

	private float killedTime;

	public Coroutine fadeCoroutine;

	private static AutoFade Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = new GameObject("AutoFade").AddComponent<AutoFade>();
			}
			return m_Instance;
		}
	}

	public static bool Fading
	{
		get
		{
			return Instance.m_Fading;
		}
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
		m_Instance = this;
		Shader shader = Shader.Find("Mobile/Particles/Alpha Blended");
		m_Material = new Material(shader);
	}

	private void DrawQuad(Color aColor, float aAlpha)
	{
		if (!ShopNGUIController.GuiActive && !BankController.Instance.uiRoot.gameObject.activeInHierarchy)
		{
			aColor.a = aAlpha;
			if (m_Material.SetPass(0))
			{
				GL.PushMatrix();
				GL.LoadOrtho();
				GL.Begin(7);
				GL.Color(aColor);
				GL.Vertex3(0f, 0f, -1f);
				GL.Vertex3(0f, 1f, -1f);
				GL.Vertex3(1f, 1f, -1f);
				GL.Vertex3(1f, 0f, -1f);
				GL.End();
				GL.PopMatrix();
			}
			else
			{
				UnityEngine.Debug.LogWarning("Couldnot set pass for material.");
			}
		}
	}

	private IEnumerator Fade(float aFadeOutTime, float aFadeInTime, Color aColor, bool collectGrabage)
	{
		float t = 0f;
		while (t < 1f)
		{
			yield return new WaitForEndOfFrame();
			t = Mathf.Clamp01(t + Time.deltaTime / aFadeOutTime);
			DrawQuad(aColor, t);
		}
		if (collectGrabage)
		{
			GC.Collect();
		}
		if (isLoadScene)
		{
			if (m_LevelName != "")
			{
				Singleton<SceneLoader>.Instance.LoadScene(m_LevelName);
			}
		}
		else
		{
			while (killedTime > 0f)
			{
				killedTime -= Time.deltaTime;
				DrawQuad(aColor, t);
				yield return new WaitForEndOfFrame();
			}
		}
		while (t > 0f && !(Mathf.Abs(aFadeInTime) < 1E-06f))
		{
			t = Mathf.Clamp01(t - Time.deltaTime / aFadeInTime);
			DrawQuad(aColor, t);
			yield return new WaitForEndOfFrame();
		}
		m_Fading = false;
	}

	private void StartFade(float aFadeOutTime, float aFadeInTime, Color aColor, bool collectGarbage = false)
	{
		m_Fading = true;
		fadeCoroutine = StartCoroutine(Fade(aFadeOutTime, aFadeInTime, aColor, collectGarbage));
	}

	public static void LoadLevel(string aLevelName, float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		if (!Fading)
		{
			Instance.isLoadScene = true;
			Instance.m_LevelName = aLevelName;
			Instance.StartFade(aFadeOutTime, aFadeInTime, aColor);
		}
	}

	public static void fadeKilled(float aFadeOutTime, float aFadeKilledTime, float aFadeInTime, Color aColor)
	{
		if (Fading)
		{
			Instance.StopCoroutine(Instance.fadeCoroutine);
		}
		Instance.isLoadScene = false;
		Instance.killedTime = aFadeKilledTime;
		Instance.StartFade(aFadeOutTime, aFadeInTime, aColor, true);
	}
}
