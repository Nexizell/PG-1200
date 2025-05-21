using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CleanUpAndDoAction : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CStart_003Ed__3 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private Action _003Chandler_003E5__1;

		private int _003Ci_003E5__2;

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
		public _003CStart_003Ed__3(int _003C_003E1__state)
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
				_003Chandler_003E5__1 = action;
				if (ShopNGUIController.GuiActive)
				{
					ShopNGUIController.GuiActive = false;
				}
				GameConnect.Disconnect();
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				WeaponManager.sharedManager.UnloadAll();
				_003C_003E2__current = null;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = null;
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				if (_003Chandler_003E5__1 != null)
				{
					_003Chandler_003E5__1();
				}
				action = null;
				goto IL_00eb;
			case 5:
				_003C_003E1__state = -1;
				goto IL_00eb;
			case 6:
				_003C_003E1__state = -1;
				goto IL_0122;
			case 7:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_00eb:
				if (FacebookController.LoggingIn)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 5;
					return true;
				}
				_003Ci_003E5__2 = 0;
				goto IL_0122;
				IL_0122:
				if (_003Ci_003E5__2 < 60)
				{
					_003Ci_003E5__2++;
					_003C_003E2__current = null;
					_003C_003E1__state = 6;
					return true;
				}
				break;
			}
			if (FacebookController.LoggingIn)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 7;
				return true;
			}
			Application.LoadLevel(Defs.MainMenuScene);
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

	public Texture riliFon;

	public Texture blackPixel;

	public static Action action;

	private IEnumerator Start()
	{
		Action handler = action;
		if (ShopNGUIController.GuiActive)
		{
			ShopNGUIController.GuiActive = false;
		}
		GameConnect.Disconnect();
		yield return null;
		yield return null;
		WeaponManager.sharedManager.UnloadAll();
		yield return null;
		yield return null;
		if (handler != null)
		{
			handler();
		}
		action = null;
		while (FacebookController.LoggingIn)
		{
			yield return null;
		}
		int i = 0;
		while (i < 60)
		{
			i++;
			yield return null;
		}
		while (FacebookController.LoggingIn)
		{
			yield return null;
		}
		Application.LoadLevel(Defs.MainMenuScene);
	}

	private void OnGUI()
	{
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), blackPixel, ScaleMode.StretchToFill);
		GUI.DrawTexture(AppsMenu.RiliFonRect(), riliFon, ScaleMode.StretchToFill);
	}
}
