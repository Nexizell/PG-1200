using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rilisoft
{
	public class WindowBackgroundAnimation : MonoBehaviour
	{
		[CompilerGenerated]
		internal sealed class _003CLoopBackgroundAnimation_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public WindowBackgroundAnimation _003C_003E4__this;

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
			public _003CLoopBackgroundAnimation_003Ed__10(int _003C_003E1__state)
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
					GameObject gameObject = _003C_003E4__this.Arrows[0];
					if (_003C_003E4__this._bgArrowRows == null)
					{
						_003C_003E4__this._bgArrowRows = new GameObject[8];
						for (int i = 0; i < _003C_003E4__this._bgArrowRows.Length; i++)
						{
							GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
							gameObject2.transform.parent = gameObject.transform.parent;
							_003C_003E4__this._bgArrowRows[i] = gameObject2;
						}
					}
					for (int j = 0; j < _003C_003E4__this.Arrows.Length; j++)
					{
						_003C_003E4__this.Arrows[j].SetActive(false);
					}
					_003C_003E4__this._currentBgArrowPrefabIndex = -1;
				}
				if (_003C_003E4__this.interfaceHolder != null && _003C_003E4__this.interfaceHolder.gameObject.activeInHierarchy)
				{
					for (int k = 0; k < _003C_003E4__this.ShineNodes.Length; k++)
					{
						GameObject gameObject3 = _003C_003E4__this.ShineNodes[k];
						if (gameObject3 != null && gameObject3.activeInHierarchy)
						{
							gameObject3.transform.Rotate(Vector3.forward, Time.deltaTime * 10f, Space.Self);
							if (k != _003C_003E4__this._currentBgArrowPrefabIndex)
							{
								_003C_003E4__this._currentBgArrowPrefabIndex = k;
								_003C_003E4__this.ResetBackgroundArrows(_003C_003E4__this.Arrows[k].transform);
							}
						}
					}
					for (int l = 0; l < _003C_003E4__this._bgArrowRows.Length; l++)
					{
						if (!(_003C_003E4__this._bgArrowRows[l] == null))
						{
							Transform transform = _003C_003E4__this._bgArrowRows[l].transform;
							float num2 = transform.localPosition.y + Time.deltaTime * 60f;
							if (num2 > 474f)
							{
								num2 -= 880f;
							}
							transform.localPosition = new Vector3(transform.localPosition.x, num2, transform.localPosition.z);
						}
					}
				}
				_003C_003E2__current = null;
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

		public GameObject[] Arrows;

		public GameObject[] ShineNodes;

		public bool PlayOnEnable = true;

		private int _currentBgArrowPrefabIndex = -1;

		private GameObject[] _bgArrowRows;

		private UIRoot interfaceHolderValue;

		private UIRoot interfaceHolder
		{
			get
			{
				if (interfaceHolderValue == null)
				{
					interfaceHolderValue = base.gameObject.GetComponentInParents<UIRoot>();
				}
				return interfaceHolderValue;
			}
		}

		private void OnEnable()
		{
			if (PlayOnEnable)
			{
				Play();
			}
		}

		public void Play()
		{
			_currentBgArrowPrefabIndex = -1;
			StartCoroutine(LoopBackgroundAnimation());
		}

		private IEnumerator LoopBackgroundAnimation()
		{
			GameObject gameObject = Arrows[0];
			if (_bgArrowRows == null)
			{
				_bgArrowRows = new GameObject[8];
				for (int i = 0; i < _bgArrowRows.Length; i++)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject);
					gameObject2.transform.parent = gameObject.transform.parent;
					_bgArrowRows[i] = gameObject2;
				}
			}
			for (int j = 0; j < Arrows.Length; j++)
			{
				Arrows[j].SetActive(false);
			}
			_currentBgArrowPrefabIndex = -1;
			while (true)
			{
				if (interfaceHolder != null && interfaceHolder.gameObject.activeInHierarchy)
				{
					for (int k = 0; k < ShineNodes.Length; k++)
					{
						GameObject gameObject3 = ShineNodes[k];
						if (gameObject3 != null && gameObject3.activeInHierarchy)
						{
							gameObject3.transform.Rotate(Vector3.forward, Time.deltaTime * 10f, Space.Self);
							if (k != _currentBgArrowPrefabIndex)
							{
								_currentBgArrowPrefabIndex = k;
								ResetBackgroundArrows(Arrows[k].transform);
							}
						}
					}
					for (int l = 0; l < _bgArrowRows.Length; l++)
					{
						if (!(_bgArrowRows[l] == null))
						{
							Transform transform = _bgArrowRows[l].transform;
							float num = transform.localPosition.y + Time.deltaTime * 60f;
							if (num > 474f)
							{
								num -= 880f;
							}
							transform.localPosition = new Vector3(transform.localPosition.x, num, transform.localPosition.z);
						}
					}
				}
				yield return null;
			}
		}

		private void ResetBackgroundArrows(Transform target)
		{
			for (int i = 0; i < _bgArrowRows.Length; i++)
			{
				Transform obj = _bgArrowRows[i].transform;
				obj.parent = target.parent;
				obj.localScale = Vector3.one;
				obj.localPosition = new Vector3(target.localPosition.x + ((i % 2 == 1) ? 90f : 0f), target.localPosition.y - 110f * (float)i, target.localPosition.z);
				obj.localRotation = target.localRotation;
			}
		}
	}
}
