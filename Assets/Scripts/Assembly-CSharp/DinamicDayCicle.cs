using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DinamicDayCicle : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CMatColorChange_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public DinamicDayCicle _003C_003E4__this;

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
		public _003CMatColorChange_003Ed__10(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
			}
			else
			{
				_003C_003E1__state = -1;
			}
			if (_003C_003E4__this.matchTime != _003C_003E4__this.timeDelta)
			{
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
			}
			_003C_003E2__current = new WaitForSeconds(0.5f);
			_003C_003E1__state = 1;
			return true;
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

	public float lerpFactor;

	private int nextCicle;

	private float cicleTime;

	public int currentCicle;

	private float matchTime;

	private float timeDelta;

	private void Start()
	{
		ResetColors();
		StartCoroutine(MatColorChange());
	}

	private void Update()
	{
		if (TimeGameController.sharedController != null && PhotonNetwork.room != null && !string.IsNullOrEmpty(GameConnect.maxKillProperty))
		{
			if (!PhotonNetwork.room.customProperties.ContainsKey(GameConnect.maxKillProperty))
			{
				return;
			}
			int result = -1;
			int.TryParse(PhotonNetwork.room.customProperties[GameConnect.maxKillProperty].ToString(), out result);
			if (result < 0)
			{
				ResetColors();
				return;
			}
			matchTime = (float)result * 60f;
			if (!((float)TimeGameController.sharedController.timerToEndMatch < matchTime))
			{
				return;
			}
			timeDelta = matchTime - (float)TimeGameController.sharedController.timerToEndMatch;
			if (matchTime == timeDelta)
			{
				return;
			}
			MaterialToChange[] array = matToChange;
			foreach (MaterialToChange materialToChange in array)
			{
				cicleTime = matchTime / (float)materialToChange.cicleColors.Length;
				currentCicle = Mathf.FloorToInt(timeDelta / matchTime * (float)materialToChange.cicleColors.Length);
				nextCicle = Mathf.Min(currentCicle + 1, materialToChange.cicleColors.Length - 1);
				lerpFactor = (timeDelta - cicleTime * (float)currentCicle) / cicleTime;
				if (materialToChange.changecolor && currentCicle < materialToChange.cicleColors.Length)
				{
					materialToChange.currentColor = Color.Lerp(materialToChange.cicleColors[currentCicle], materialToChange.cicleColors[nextCicle], lerpFactor);
				}
				if (materialToChange.cicleLerp != null && materialToChange.cicleLerp.Length == materialToChange.cicleColors.Length && currentCicle < materialToChange.cicleColors.Length)
				{
					materialToChange.currentLerp = Mathf.Lerp(materialToChange.cicleLerp[currentCicle], materialToChange.cicleLerp[nextCicle], lerpFactor);
				}
			}
		}
		else
		{
			ResetColors();
		}
	}

	private void ResetColors()
	{
		MaterialToChange[] array = matToChange;
		foreach (MaterialToChange materialToChange in array)
		{
			if (materialToChange.changecolor)
			{
				materialToChange.currentColor = materialToChange.cicleColors[0];
			}
			if (materialToChange.cicleLerp != null && materialToChange.cicleLerp.Length == materialToChange.cicleColors.Length)
			{
				materialToChange.currentLerp = materialToChange.cicleLerp[0];
			}
		}
	}

	private IEnumerator MatColorChange()
	{
		while (true)
		{
			if (matchTime != timeDelta)
			{
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
			}
			yield return new WaitForSeconds(0.5f);
		}
	}
}
