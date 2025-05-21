using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LobbyDayNightController : MonoBehaviour
{
	[Serializable]
	public class MaterialToChange 
	{
		public string description = "description";

		public Color[] cicleColors = new Color[6];

		public Material[] materials;

		public float[] cicleLerp = new float[6];

		public bool changecolor;

		public bool changeLM;

		[HideInInspector]
		public Color currentColor;

		[HideInInspector]
		public float currentLerp;

		public GameObject[] objectsToOnAtNight;

		[HideInInspector]
		public bool objectsIsActive;
	}

	[CompilerGenerated]
	internal sealed class _003CMatColorChange_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LobbyDayNightController _003C_003E4__this;

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
		public _003CMatColorChange_003Ed__8(int _003C_003E1__state)
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
				goto IL_0022;
			case 1:
			{
				_003C_003E1__state = -1;
				MaterialToChange[] matToChange = _003C_003E4__this.matToChange;
				foreach (MaterialToChange materialToChange in matToChange)
				{
					Material[] materials = materialToChange.materials;
					foreach (Material material in materials)
					{
						if (materialToChange.changecolor)
						{
							material.color = materialToChange.currentColor;
						}
						if (material.HasProperty("_Lerp"))
						{
							material.SetFloat("_Lerp", materialToChange.currentLerp);
						}
					}
				}
				_003C_003E2__current = new WaitForSeconds(0.5f);
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_0022;
				}
				IL_0022:
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
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

	public MaterialToChange[] matToChange;

	private float cicleTime;

	private float timeDelta;

	private float dayLength = 86400f;

	private void Start()
	{
		DateTime now = DateTime.Now;
		timeDelta = dayLength - (float)(now.Hour * 60 * 60) + (float)(now.Minute * 60) + (float)now.Second;
		cicleTime = dayLength / 6f;
		StartCoroutine(MatColorChange());
	}

	private void Update()
	{
		timeDelta -= Time.deltaTime;
		if (timeDelta < 0f)
		{
			timeDelta = dayLength;
		}
		MaterialToChange[] array = matToChange;
		foreach (MaterialToChange materialToChange in array)
		{
			int num = Mathf.FloorToInt(timeDelta / dayLength * 6f);
			if (num == 6)
			{
				num = 0;
			}
			int num2 = num + 1;
			if (num2 > 5)
			{
				num2 = 0;
			}
			float t = (timeDelta - cicleTime * (float)num) / cicleTime;
			if (materialToChange.changecolor)
			{
				materialToChange.currentColor = Color.Lerp(materialToChange.cicleColors[num], materialToChange.cicleColors[num2], t);
			}
			if (materialToChange.changeLM)
			{
				materialToChange.currentLerp = Mathf.Lerp(materialToChange.cicleLerp[num], materialToChange.cicleLerp[num2], t);
			}
			if (materialToChange.objectsToOnAtNight != null)
			{
				if ((num == 5 || num == 0) && !materialToChange.objectsIsActive)
				{
					ActiveGo(materialToChange.objectsToOnAtNight, true);
					materialToChange.objectsIsActive = true;
				}
				if (num > 0 && num < 5 && materialToChange.objectsIsActive)
				{
					ActiveGo(materialToChange.objectsToOnAtNight, false);
					materialToChange.objectsIsActive = false;
				}
			}
		}
	}

	private void ActiveGo(GameObject[] go, bool active)
	{
		for (int i = 0; i < go.Length; i++)
		{
			go[i].SetActive(active);
		}
	}

	private IEnumerator MatColorChange()
	{
		while (true)
		{
			yield return null;
			MaterialToChange[] array = matToChange;
			foreach (MaterialToChange materialToChange in array)
			{
				Material[] materials = materialToChange.materials;
				foreach (Material material in materials)
				{
					if (materialToChange.changecolor)
					{
						material.color = materialToChange.currentColor;
					}
					if (material.HasProperty("_Lerp"))
					{
						material.SetFloat("_Lerp", materialToChange.currentLerp);
					}
				}
			}
			yield return new WaitForSeconds(0.5f);
		}
	}
}
