using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public class ChooseBoxNGUIController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CStart_003Ed__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ChooseBoxNGUIController _003C_003E4__this;

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
		public _003CStart_003Ed__12(int _003C_003E1__state)
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
				_003C_003E4__this.ScrollTransform.GetComponent<UIPanel>().baseClipRegion = new Vector4(0f, 0f, 760 * Screen.width / Screen.height, 736f);
				_003C_003E4__this.countMap = _003C_003E4__this.grid.transform.childCount;
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				Defs.diffGame = PlayerPrefs.GetInt(Defs.DiffSett, 2);
				if (_003C_003E4__this.difficultyToggle != null)
				{
					_003C_003E4__this.difficultyToggle.buttons[Defs.diffGame].IsChecked = true;
					_003C_003E4__this.difficultyToggle.Clicked += delegate(object sender, MultipleToggleEventArgs e)
					{
						ButtonClickSound.Instance.PlayClick();
						PlayerPrefs.SetInt(Defs.DiffSett, e.Num);
						Defs.diffGame = e.Num;
						PlayerPrefs.Save();
					};
				}
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

	public List<UILabel> unlockPrice;

	public MultipleToggleButton difficultyToggle;

	public GameObject allInterfaceHolder;

	public UIButton backButton;

	public UIButton startButton;

	public UIButton unlockButton;

	public GameObject grid;

	public Transform ScrollTransform;

	public GameObject SelectMapPanel;

	public int selectIndexMap;

	public int countMap;

	public float widthCell = 824f;

	private IEnumerator Start()
	{
		ScrollTransform.GetComponent<UIPanel>().baseClipRegion = new Vector4(0f, 0f, 760 * Screen.width / Screen.height, 736f);
		countMap = grid.transform.childCount;
		yield return null;
		Defs.diffGame = PlayerPrefs.GetInt(Defs.DiffSett, 2);
		if (difficultyToggle != null)
		{
			difficultyToggle.buttons[Defs.diffGame].IsChecked = true;
			difficultyToggle.Clicked += delegate(object sender, MultipleToggleEventArgs e)
			{
				ButtonClickSound.Instance.PlayClick();
				PlayerPrefs.SetInt(Defs.DiffSett, e.Num);
				Defs.diffGame = e.Num;
				PlayerPrefs.Save();
			};
		}
	}

	private void Update()
	{
		if (SelectMapPanel.activeInHierarchy)
		{
			if (ScrollTransform.localPosition.x > 0f)
			{
				selectIndexMap = Mathf.RoundToInt((ScrollTransform.localPosition.x - (float)Mathf.FloorToInt(ScrollTransform.localPosition.x / widthCell / (float)countMap) * widthCell * (float)countMap) / widthCell);
				selectIndexMap = countMap - selectIndexMap;
			}
			else
			{
				selectIndexMap = -1 * Mathf.RoundToInt((ScrollTransform.localPosition.x - (float)Mathf.CeilToInt(ScrollTransform.localPosition.x / widthCell / (float)countMap) * widthCell * (float)countMap) / widthCell);
			}
			if (selectIndexMap == countMap)
			{
				selectIndexMap = 0;
			}
		}
	}
}
