using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;

public sealed class ClanIncomingInviteView : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CAcceptClanInviteCoroutine_003Ed__25 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ClanIncomingInviteView _003C_003E4__this;

		private WWW _003Crequest_003E5__1;

		private UIGrid _003Cg_003E5__2;

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
		public _003CAcceptClanInviteCoroutine_003Ed__25(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1 || num == 2)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
		}

		private bool MoveNext()
		{
			bool result;
			try
			{
				ClanIncomingInviteView[] componentsInChildren;
				switch (_003C_003E1__state)
				{
				default:
					result = false;
					goto end_IL_0000;
				case 0:
					_003C_003E1__state = -1;
					if (FriendsController.sharedController == null)
					{
						result = false;
					}
					else
					{
						string id = FriendsController.sharedController.id;
						if (!string.IsNullOrEmpty(id))
						{
							WWWForm wWWForm = new WWWForm();
							wWWForm.AddField("action", "accept_invite");
							wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
							wWWForm.AddField("id_player", id);
							wWWForm.AddField("id_clan", _003C_003E4__this.ClanId ?? string.Empty);
							wWWForm.AddField("uniq_id", FriendsController.sharedController.id);
							wWWForm.AddField("auth", FriendsController.Hash("accept_invite"));
							_003C_003E4__this.acceptButton.Do(delegate(UIButton b)
							{
								b.isEnabled = false;
							});
							_003C_003E4__this.rejectButton.Do(delegate(UIButton b)
							{
								b.isEnabled = false;
							});
							_003C_003E1__state = -3;
							_003Crequest_003E5__1 = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, wWWForm);
							if (_003Crequest_003E5__1 == null)
							{
								result = false;
								break;
							}
							goto IL_0190;
						}
						result = false;
					}
					goto end_IL_0000;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0190;
				case 2:
					{
						_003C_003E1__state = -3;
						_003Cg_003E5__2.Reposition();
						_003C_003E4__this.acceptButton = null;
						_003C_003E4__this.rejectButton = null;
						_003Cg_003E5__2.GetComponentInParent<ClanIncomingInvitesController>().Do(delegate(ClanIncomingInvitesController c)
						{
							c.Refresh();
						});
						goto IL_032f;
					}
					IL_0190:
					if (!_003Crequest_003E5__1.isDone)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 1;
						result = true;
					}
					else
					{
						if (!string.IsNullOrEmpty(_003Crequest_003E5__1.error))
						{
							UnityEngine.Debug.LogError(_003Crequest_003E5__1.error);
							result = false;
							break;
						}
						string value = URLs.Sanitize(_003Crequest_003E5__1);
						if ("fail".Equals(value, StringComparison.OrdinalIgnoreCase))
						{
							UnityEngine.Debug.LogError("accept_invite failed.");
							result = false;
							break;
						}
						FriendsController.sharedController.clanLogo = _003C_003E4__this.ClanLogo;
						FriendsController.sharedController.ClanID = _003C_003E4__this.ClanId ?? string.Empty;
						FriendsController.sharedController.clanName = _003C_003E4__this.ClanName;
						FriendsController.sharedController.clanLeaderID = _003C_003E4__this.ClanCreatorId ?? string.Empty;
						if (ClansGUIController.sharedController != null)
						{
							ClansGUIController.sharedController.nameClanLabel.text = FriendsController.sharedController.clanName;
						}
						_003Cg_003E5__2 = _003C_003E4__this.transform.parent.GetComponent<UIGrid>();
						if (!(_003Cg_003E5__2 != null))
						{
							goto IL_032f;
						}
						NGUITools.Destroy(_003C_003E4__this.transform);
						_003C_003E2__current = new WaitForEndOfFrame();
						_003C_003E1__state = 2;
						result = true;
					}
					goto end_IL_0000;
					IL_032f:
					componentsInChildren = _003Cg_003E5__2.GetComponentsInChildren<ClanIncomingInviteView>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].Refresh();
					}
					_003Crequest_003E5__1 = null;
					_003Cg_003E5__2 = null;
					_003C_003Em__Finally1();
					result = false;
					goto end_IL_0000;
				}
				_003C_003Em__Finally1();
				end_IL_0000:;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
			return result;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			_003C_003E4__this.acceptButton.Do(delegate(UIButton b)
			{
				b.isEnabled = string.IsNullOrEmpty(FriendsController.sharedController.ClanID);
			});
			_003C_003E4__this.rejectButton.Do(delegate(UIButton b)
			{
				b.isEnabled = true;
			});
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	internal sealed class _003CRejectClanInviteCoroutine_003Ed__26 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ClanIncomingInviteView _003C_003E4__this;

		private WWW _003Crequest_003E5__1;

		private UIGrid _003Cg_003E5__2;

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
		public _003CRejectClanInviteCoroutine_003Ed__26(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1 || num == 2)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
		}

		private bool MoveNext()
		{
			bool result;
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					result = false;
					goto end_IL_0000;
				case 0:
				{
					_003C_003E1__state = -1;
					string value = FriendsController.sharedController.Map((FriendsController sc) => sc.id) ?? string.Empty;
					if (!string.IsNullOrEmpty(value))
					{
						WWWForm wWWForm = new WWWForm();
						wWWForm.AddField("action", "reject_invite");
						wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
						wWWForm.AddField("id_player", value);
						wWWForm.AddField("id_clan", _003C_003E4__this.ClanId ?? string.Empty);
						wWWForm.AddField("id", value);
						wWWForm.AddField("uniq_id", FriendsController.sharedController.id);
						wWWForm.AddField("auth", FriendsController.Hash("reject_invite"));
						_003C_003E4__this.acceptButton.Do(delegate(UIButton b)
						{
							b.isEnabled = false;
						});
						_003C_003E4__this.rejectButton.Do(delegate(UIButton b)
						{
							b.isEnabled = false;
						});
						_003C_003E1__state = -3;
						_003Crequest_003E5__1 = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, wWWForm);
						if (_003Crequest_003E5__1 == null)
						{
							result = false;
							break;
						}
						goto IL_01b0;
					}
					result = false;
					goto end_IL_0000;
				}
				case 1:
					_003C_003E1__state = -3;
					goto IL_01b0;
				case 2:
					{
						_003C_003E1__state = -3;
						_003Cg_003E5__2.Reposition();
						_003C_003E4__this.acceptButton = null;
						_003C_003E4__this.rejectButton = null;
						_003Cg_003E5__2.GetComponentInParent<ClanIncomingInvitesController>().Do(delegate(ClanIncomingInvitesController c)
						{
							c.Refresh();
						});
						goto IL_02c3;
					}
					IL_02c3:
					_003Crequest_003E5__1 = null;
					_003Cg_003E5__2 = null;
					_003C_003Em__Finally1();
					result = false;
					goto end_IL_0000;
					IL_01b0:
					if (!_003Crequest_003E5__1.isDone)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 1;
						result = true;
					}
					else
					{
						if (!string.IsNullOrEmpty(_003Crequest_003E5__1.error))
						{
							UnityEngine.Debug.LogError(_003Crequest_003E5__1.error);
							result = false;
							break;
						}
						string value2 = URLs.Sanitize(_003Crequest_003E5__1);
						if ("fail".Equals(value2, StringComparison.OrdinalIgnoreCase))
						{
							UnityEngine.Debug.LogError("reject_invite failed.");
							result = false;
							break;
						}
						_003Cg_003E5__2 = _003C_003E4__this.transform.parent.GetComponent<UIGrid>();
						if (!(_003Cg_003E5__2 != null))
						{
							goto IL_02c3;
						}
						NGUITools.Destroy(_003C_003E4__this.transform);
						_003C_003E2__current = new WaitForEndOfFrame();
						_003C_003E1__state = 2;
						result = true;
					}
					goto end_IL_0000;
				}
				_003C_003Em__Finally1();
				end_IL_0000:;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
			return result;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			_003C_003E4__this.acceptButton.Do(delegate(UIButton b)
			{
				b.isEnabled = string.IsNullOrEmpty(FriendsController.sharedController.ClanID);
			});
			_003C_003E4__this.rejectButton.Do(delegate(UIButton b)
			{
				b.isEnabled = true;
			});
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	public UIButton acceptButton;

	public UIButton rejectButton;

	public UITexture clanLogo;

	public UILabel clanName;

	private string _clanName = string.Empty;

	private string _clanLogo = string.Empty;

	public string ClanId { get; set; }

	public string ClanCreatorId { get; set; }

	public string ClanName
	{
		get
		{
			return _clanName ?? string.Empty;
		}
		set
		{
			_clanName = value ?? string.Empty;
			clanName.Do(delegate(UILabel l)
			{
				l.text = _clanName;
			});
		}
	}

	public string ClanLogo
	{
		get
		{
			return _clanLogo ?? string.Empty;
		}
		set
		{
			_clanLogo = value ?? string.Empty;
			clanLogo.Do(delegate(UITexture t)
			{
				LeaderboardScript.SetClanLogo(t, _clanLogo);
			});
		}
	}

	public void HandleAcceptButton()
	{
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log(string.Format("Accept invite to clan {0} ({1})", new object[2] { ClanName, ClanId }));
		}
		FriendsController.sharedController.Do(delegate(FriendsController f)
		{
			f.StartCoroutine(AcceptClanInviteCoroutine());
		});
		ClanIncomingInvitesController.SetRequestDirty();
	}

	public void HandleRejectButton()
	{
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log(string.Format("Reject invite to clan {0} ({1})", new object[2] { ClanName, ClanId }));
		}
		FriendsController.sharedController.Do(delegate(FriendsController f)
		{
			f.StartCoroutine(RejectClanInviteCoroutine());
		});
		ClanIncomingInvitesController.SetRequestDirty();
	}

	private void Start()
	{
		Refresh();
	}

	private void OnEnable()
	{
		Refresh();
	}

	internal void Refresh()
	{
		if (acceptButton != null && rejectButton != null && FriendsController.sharedController != null)
		{
			bool flag = string.IsNullOrEmpty(FriendsController.sharedController.ClanID);
			Vector3[] array = ((!flag) ? new Vector3[2]
			{
				acceptButton.transform.localPosition,
				rejectButton.transform.localPosition
			} : new Vector3[2]
			{
				rejectButton.transform.localPosition,
				acceptButton.transform.localPosition
			});
			rejectButton.transform.localPosition = array[0];
			acceptButton.transform.localPosition = array[1];
			acceptButton.gameObject.SetActive(flag);
		}
	}

	private IEnumerator AcceptClanInviteCoroutine()
	{
		if (FriendsController.sharedController == null)
		{
			yield break;
		}
		string id = FriendsController.sharedController.id;
		if (string.IsNullOrEmpty(id))
		{
			yield break;
		}
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "accept_invite");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("id_player", id);
		wWWForm.AddField("id_clan", ClanId ?? string.Empty);
		wWWForm.AddField("uniq_id", FriendsController.sharedController.id);
		wWWForm.AddField("auth", FriendsController.Hash("accept_invite"));
		acceptButton.Do(delegate(UIButton b)
		{
			b.isEnabled = false;
		});
		rejectButton.Do(delegate(UIButton b)
		{
			b.isEnabled = false;
		});
		try
		{
			WWW request = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, wWWForm);
			if (request == null)
			{
				yield break;
			}
			while (!request.isDone)
			{
				yield return null;
			}
			if (!string.IsNullOrEmpty(request.error))
			{
				UnityEngine.Debug.LogError(request.error);
				yield break;
			}
			string value = URLs.Sanitize(request);
			if ("fail".Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				UnityEngine.Debug.LogError("accept_invite failed.");
				yield break;
			}
			FriendsController.sharedController.clanLogo = ClanLogo;
			FriendsController.sharedController.ClanID = ClanId ?? string.Empty;
			FriendsController.sharedController.clanName = ClanName;
			FriendsController.sharedController.clanLeaderID = ClanCreatorId ?? string.Empty;
			if (ClansGUIController.sharedController != null)
			{
				ClansGUIController.sharedController.nameClanLabel.text = FriendsController.sharedController.clanName;
			}
			UIGrid g = transform.parent.GetComponent<UIGrid>();
			if (g != null)
			{
				NGUITools.Destroy(transform);
				yield return new WaitForEndOfFrame();
				g.Reposition();
				acceptButton = null;
				rejectButton = null;
				g.GetComponentInParent<ClanIncomingInvitesController>().Do(delegate(ClanIncomingInvitesController c)
				{
					c.Refresh();
				});
			}
			ClanIncomingInviteView[] componentsInChildren = g.GetComponentsInChildren<ClanIncomingInviteView>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Refresh();
			}
		}
		finally
		{
			acceptButton.Do(delegate(UIButton b)
			{
				b.isEnabled = string.IsNullOrEmpty(FriendsController.sharedController.ClanID);
			});
			rejectButton.Do(delegate(UIButton b)
			{
				b.isEnabled = true;
			});
		}
	}

	private IEnumerator RejectClanInviteCoroutine()
	{
		string value = FriendsController.sharedController.Map((FriendsController sc) => sc.id) ?? string.Empty;
		if (string.IsNullOrEmpty(value))
		{
			yield break;
		}
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("action", "reject_invite");
		wWWForm.AddField("app_version", ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion);
		wWWForm.AddField("id_player", value);
		wWWForm.AddField("id_clan", ClanId ?? string.Empty);
		wWWForm.AddField("id", value);
		wWWForm.AddField("uniq_id", FriendsController.sharedController.id);
		wWWForm.AddField("auth", FriendsController.Hash("reject_invite"));
		acceptButton.Do(delegate(UIButton b)
		{
			b.isEnabled = false;
		});
		rejectButton.Do(delegate(UIButton b)
		{
			b.isEnabled = false;
		});
		try
		{
			WWW request = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, wWWForm);
			if (request == null)
			{
				yield break;
			}
			while (!request.isDone)
			{
				yield return null;
			}
			if (!string.IsNullOrEmpty(request.error))
			{
				UnityEngine.Debug.LogError(request.error);
				yield break;
			}
			string value2 = URLs.Sanitize(request);
			if ("fail".Equals(value2, StringComparison.OrdinalIgnoreCase))
			{
				UnityEngine.Debug.LogError("reject_invite failed.");
				yield break;
			}
			UIGrid g = transform.parent.GetComponent<UIGrid>();
			if (g != null)
			{
				NGUITools.Destroy(transform);
				yield return new WaitForEndOfFrame();
				g.Reposition();
				acceptButton = null;
				rejectButton = null;
				g.GetComponentInParent<ClanIncomingInvitesController>().Do(delegate(ClanIncomingInvitesController c)
				{
					c.Refresh();
				});
			}
		}
		finally
		{
			acceptButton.Do(delegate(UIButton b)
			{
				b.isEnabled = string.IsNullOrEmpty(FriendsController.sharedController.ClanID);
			});
			rejectButton.Do(delegate(UIButton b)
			{
				b.isEnabled = true;
			});
		}
	}
}
