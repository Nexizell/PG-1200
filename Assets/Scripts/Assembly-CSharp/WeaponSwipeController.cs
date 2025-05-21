using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class WeaponSwipeController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003C_DisableSwiping_003Ed__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public WeaponSwipeController _003C_003E4__this;

		public float tm;

		private int _003Cbef_003E5__1;

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
		public _003C_DisableSwiping_003Ed__8(int _003C_003E1__state)
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
				if (_003C_003E4__this._center == null)
				{
					return false;
				}
				if (!int.TryParse(_003C_003E4__this._center.centeredObject.name.Replace("preview_", string.Empty), out _003Cbef_003E5__1))
				{
					return false;
				}
				_003C_003E4__this._disabled = true;
				_003C_003E2__current = new WaitForSeconds(tm);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				_003C_003E4__this._disabled = false;
				if (_003C_003E4__this._center.centeredObject.name.Equals("preview_" + _003Cbef_003E5__1))
				{
					return false;
				}
				Transform transform = null;
				foreach (Transform item in _003C_003E4__this._center.transform)
				{
					if (item.gameObject.name.Equals("preview_" + _003Cbef_003E5__1))
					{
						transform = item;
						break;
					}
				}
				if (transform != null)
				{
					_003C_003E4__this._center.CenterOn(transform);
				}
				return false;
			}
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

	private UIWrapContent _wrapContent;

	private UIScrollView _scrollView;

	private Player_move_c move;

	private MyCenterOnChild _center;

	private bool _disabled;

	private void Start()
	{
		_wrapContent = GetComponentInChildren<UIWrapContent>();
		_center = GetComponentInChildren<MyCenterOnChild>();
		_scrollView = GetComponent<UIScrollView>();
		MyCenterOnChild center = _center;
		center.onFinished = (SpringPanel.OnFinished)Delegate.Combine(center.onFinished, new SpringPanel.OnFinished(HandleCenteringFinished));
		UpdateContent();
	}

	private void HandleWeaponEquipped()
	{
		UpdateContent();
	}

	private void OnEnable()
	{
		StartCoroutine(_DisableSwiping(0.5f));
	}

	private IEnumerator _DisableSwiping(float tm)
	{
		int bef;
		if (_center == null || !int.TryParse(_center.centeredObject.name.Replace("preview_", string.Empty), out bef))
		{
			yield break;
		}
		_disabled = true;
		yield return new WaitForSeconds(tm);
		_disabled = false;
		if (_center.centeredObject.name.Equals("preview_" + bef))
		{
			yield break;
		}
		Transform transform = null;
		foreach (Transform item in _center.transform)
		{
			if (item.gameObject.name.Equals("preview_" + bef))
			{
				transform = item;
				break;
			}
		}
		if (transform != null)
		{
			_center.CenterOn(transform);
		}
	}

	private void HandleCenteringFinished()
	{
		if (_disabled)
		{
			return;
		}
		int result;
		if (!int.TryParse(_center.centeredObject.name.Replace("preview_", string.Empty), out result))
		{
			if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log("HandleCenteringFinished: error parse");
			}
			return;
		}
		result--;
		if (!move)
		{
			if (!Defs.isMulti)
			{
				move = GameObject.FindGameObjectWithTag("Player").GetComponent<SkinName>().playerMoveC;
			}
			else
			{
				move = WeaponManager.sharedManager.myPlayerMoveC;
			}
		}
		if (result != WeaponManager.sharedManager.CurrentWeaponIndex)
		{
			TrainingState value;
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.stepTrainingList.TryGetValue("SwipeWeapon", out value) && TrainingController.stepTraining == value)
			{
				TrainingController.isNextStep = value;
			}
			WeaponManager.sharedManager.CurrentWeaponIndex = result % WeaponManager.sharedManager.playerWeapons.Count;
			WeaponManager.sharedManager.SaveWeaponAsLastUsed(WeaponManager.sharedManager.CurrentWeaponIndex);
			if (move != null)
			{
				move.ChangeWeapon(WeaponManager.sharedManager.CurrentWeaponIndex, false);
			}
		}
	}

	private void OnDestroy()
	{
		MyCenterOnChild center = _center;
		center.onFinished = (SpringPanel.OnFinished)Delegate.Remove(center.onFinished, new SpringPanel.OnFinished(HandleCenteringFinished));
	}

	public void UpdateContent()
	{
		List<string> list = new List<string>();
		foreach (Weapon playerWeapon in WeaponManager.sharedManager.playerWeapons)
		{
			list.Add(playerWeapon.weaponPrefab.name + "_InGamePreview");
		}
		UITexture[] componentsInChildren = GetComponentsInChildren<UITexture>();
		List<Texture> list2 = new List<Texture>();
		UITexture[] array = componentsInChildren;
		foreach (UITexture uITexture in array)
		{
			if ((bool)uITexture.mainTexture)
			{
				list2.Add(uITexture.mainTexture);
			}
		}
		List<string> list3 = new List<string>();
		foreach (string item in list)
		{
			bool flag = false;
			foreach (Texture item2 in list2)
			{
				if (item2.name.Equals(item))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list3.Add(item);
			}
		}
		foreach (string item3 in list3)
		{
			Texture texture = Resources.Load(WeaponManager.WeaponPreviewsPath + "/" + item3) as Texture;
			texture.name = item3;
			if (texture != null)
			{
				list2.Add(texture);
			}
		}
		Transform child = base.transform.GetChild(0);
		int childCount = child.childCount;
		if (childCount > list.Count)
		{
			for (int j = list.Count; j < childCount; j++)
			{
				Transform child2 = child.GetChild(j);
				child2.parent = null;
				UnityEngine.Object.Destroy(child2.gameObject);
			}
		}
		else if (childCount < list.Count)
		{
			for (int k = childCount; k < list.Count; k++)
			{
				if (k >= childCount)
				{
					GameObject obj = UnityEngine.Object.Instantiate(Resources.Load("WeaponPreviewPrefab") as GameObject);
					obj.transform.parent = child;
					obj.name = "preview_" + (k + 1);
					obj.transform.localScale = new Vector3(1f, 1f, 1f);
				}
			}
		}
		for (int l = 0; l < list.Count; l++)
		{
			Transform child3 = child.GetChild(l);
			if (!child3)
			{
				continue;
			}
			foreach (Texture item4 in list2)
			{
				if (item4.name.Equals(list[l]))
				{
					child3.GetComponent<UITexture>().mainTexture = item4;
					break;
				}
			}
		}
		_wrapContent.SortAlphabetically();
		Transform target = _center.transform.GetChild(0);
		foreach (Transform item5 in _wrapContent.transform)
		{
			if (item5.gameObject.name.Equals("preview_" + (WeaponManager.sharedManager.CurrentWeaponIndex + 1)))
			{
				target = item5;
				break;
			}
		}
		_center.CenterOn(target);
	}
}
