using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class ChatViewrController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CResetpositionCoroutine_003Ed__38 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ChatViewrController _003C_003E4__this;

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
		public _003CResetpositionCoroutine_003Ed__38(int _003C_003E1__state)
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
				_003C_003E4__this.scrollLabels.ResetPosition();
				_003C_003E4__this.smilePanelTransform.gameObject.SetActive(false);
				_003C_003E4__this.smilePanelTransform.gameObject.SetActive(true);
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

	public static ChatViewrController sharedController;

	public MyUIInput sendMessageInput;

	public MyUIInput sendMessageInputDater;

	public GameObject fastCommands;

	public Transform chatLabelsHolder;

	private List<ChatLabel> labelChat = new List<ChatLabel>();

	public GameObject buySmileBannerPrefab;

	public Transform smilePanelTransform;

	public GameObject smilesPanel;

	public GameObject showSmileButton;

	public GameObject hideSmileButton;

	public GameObject buySmileButton;

	public GameObject chatLabelPrefab;

	public AudioClip sendChatClip;

	public bool isClanMode;

	public UIButton sendMessageButton;

	public Transform bottomAnchor;

	public UITable labelTable;

	public UIScrollView scrollLabels;

	[NonSerialized]
	public bool isBuySmile;

	private float keyboardSize;

	public bool isShowSmilePanel;

	private bool needReset;

	private void Awake()
	{
		isBuySmile = StickersController.IsBuyAnyPack();
		buySmileBannerPrefab.SetActive(false);
		hideSmileButton.SetActive(false);
		sendMessageInput.gameObject.SetActive(false);
		sendMessageInput = sendMessageInputDater;
		fastCommands.SetActive(false);
		if (isBuySmile)
		{
			showSmileButton.SetActive(true);
			buySmileButton.SetActive(false);
		}
		else
		{
			showSmileButton.SetActive(false);
			buySmileButton.SetActive(true);
		}
		if (sendMessageInput != null)
		{
			MyUIInput myUIInput = sendMessageInput;
			myUIInput.onKeyboardInter = (Action)Delegate.Combine(myUIInput.onKeyboardInter, new Action(SendMessageFromInput));
			MyUIInput myUIInput2 = sendMessageInput;
			myUIInput2.onKeyboardCancel = (Action)Delegate.Combine(myUIInput2.onKeyboardCancel, new Action(CancelSendPrivateMessage));
			MyUIInput myUIInput3 = sendMessageInput;
			myUIInput3.onKeyboardVisible = (Action)Delegate.Combine(myUIInput3.onKeyboardVisible, new Action(OnKeyboardVisible));
			MyUIInput myUIInput4 = sendMessageInput;
			myUIInput4.onKeyboardHide = (Action)Delegate.Combine(myUIInput4.onKeyboardHide, new Action(OnKeyboardHide));
		}
	}

	private void Start()
	{
		sharedController = this;
		if (sendMessageInput != null)
		{
			sendMessageInput.isSelected = true;
		}
	}

	private void OnEnable()
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.messageDelegate += UpdateMessages;
		}
		UpdateMessages();
	}

	private void OnDisable()
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.messageDelegate -= UpdateMessages;
		}
	}

	private void UpdateMessages()
	{
		if (WeaponManager.sharedManager.myPlayer == null)
		{
			return;
		}
		Player_move_c myPlayerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
		while (labelChat.Count < myPlayerMoveC.messages.Count)
		{
			GameObject gameObject = NGUITools.AddChild(labelTable.gameObject, chatLabelPrefab);
			labelChat.Add(gameObject.GetComponent<ChatLabel>());
		}
		for (int i = 0; i < labelChat.Count; i++)
		{
			string text = "[00FF26]";
			if (Defs.isInet && myPlayerMoveC.messages[i].ID == WeaponManager.sharedManager.myPlayer.GetComponent<PhotonView>().viewID)
			{
				text = "[00FF26]";
			}
			else
			{
				if (myPlayerMoveC.messages[i].command == 0)
				{
					text = "[FFFF26]";
				}
				if (myPlayerMoveC.messages[i].command == 1)
				{
					text = "[0000FF]";
				}
				if (myPlayerMoveC.messages[i].command == 2)
				{
					text = "[FF0000]";
				}
			}
			ChatLabel chatLabel = labelChat[labelChat.Count - i - 1];
			chatLabel.nickLabel.text = text + myPlayerMoveC.messages[i].text;
			if (string.IsNullOrEmpty(myPlayerMoveC.messages[i].iconName))
			{
				if (chatLabel.stickerObject.activeInHierarchy)
				{
					chatLabel.stickerObject.SetActive(false);
				}
			}
			else
			{
				if (!chatLabel.stickerObject.activeInHierarchy)
				{
					chatLabel.stickerObject.SetActive(true);
				}
				chatLabel.iconSprite.spriteName = myPlayerMoveC.messages[i].iconName;
			}
			Transform transform = chatLabel.iconSprite.transform;
			transform.localPosition = new Vector3(chatLabel.nickLabel.width + 20, transform.localPosition.y, transform.localPosition.z);
			chatLabel.clanTexture.mainTexture = myPlayerMoveC.messages[i].clanLogo;
			labelChat[i].gameObject.SetActive(true);
		}
		labelTable.Reposition();
	}

	private void Update()
	{
		if (isShowSmilePanel && smilePanelTransform.localPosition.y < -150f)
		{
			smilesPanel.SetActive(false);
			smilesPanel.SetActive(true);
			smilePanelTransform.localPosition = new Vector3(smilePanelTransform.localPosition.x, smilePanelTransform.localPosition.y + Time.deltaTime * 500f, smilePanelTransform.localPosition.z);
			scrollLabels.MoveRelative(Vector3.up * Time.deltaTime * 500f);
			if (smilePanelTransform.localPosition.y > -150f)
			{
				smilePanelTransform.localPosition = new Vector3(smilePanelTransform.localPosition.x, -150f, smilePanelTransform.localPosition.z);
				scrollLabels.ResetPosition();
				smilePanelTransform.gameObject.SetActive(false);
				smilePanelTransform.gameObject.SetActive(true);
			}
		}
		if (!isShowSmilePanel && smilePanelTransform.localPosition.y > -314f)
		{
			smilePanelTransform.localPosition = new Vector3(smilePanelTransform.localPosition.x, smilePanelTransform.localPosition.y - Time.deltaTime * 500f, smilePanelTransform.localPosition.z);
			scrollLabels.MoveRelative(Vector3.down * Time.deltaTime * 500f);
			if (smilePanelTransform.localPosition.y < -314f)
			{
				smilePanelTransform.localPosition = new Vector3(smilePanelTransform.localPosition.x, -314f, smilePanelTransform.localPosition.z);
				scrollLabels.ResetPosition();
				smilePanelTransform.gameObject.SetActive(false);
				smilePanelTransform.gameObject.SetActive(true);
			}
		}
		if (sendMessageButton.isEnabled == string.IsNullOrEmpty(sendMessageInput.value))
		{
			sendMessageButton.isEnabled = !string.IsNullOrEmpty(sendMessageInput.value);
		}
	}

	private void LateUpdate()
	{
		if (needReset)
		{
			needReset = false;
			scrollLabels.ResetPosition();
		}
	}

	public void ShowSmilePannelOnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		isShowSmilePanel = true;
		showSmileButton.SetActive(false);
		hideSmileButton.SetActive(true);
		scrollLabels.ResetPosition();
	}

	public void HideSmilePannelOnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		HideSmilePannel();
	}

	public void OnClickSendMessageFromButton()
	{
		if (!string.IsNullOrEmpty(sendMessageInput.value))
		{
			PostChat(sendMessageInput.value);
			sendMessageInput.value = string.Empty;
		}
		if (isShowSmilePanel)
		{
			HideSmilePannel();
		}
		needReset = true;
	}

	public void SendMessageFromInput()
	{
		if (!string.IsNullOrEmpty(sendMessageInput.value))
		{
			PostChat(sendMessageInput.value);
			sendMessageInput.value = string.Empty;
		}
		if (isShowSmilePanel)
		{
			HideSmilePannel();
		}
		CloseChat();
	}

	private void HideSmilePannel()
	{
		isShowSmilePanel = false;
		showSmileButton.SetActive(true);
		hideSmileButton.SetActive(false);
	}

	public void CancelSendPrivateMessage()
	{
		sendMessageInput.value = string.Empty;
	}

	public void OnKeyboardVisible()
	{
		keyboardSize = sendMessageInput.heightKeyboard;
		if (Application.isEditor)
		{
			keyboardSize = 200f;
		}
		bottomAnchor.localPosition = new Vector3(bottomAnchor.localPosition.x, bottomAnchor.localPosition.y + keyboardSize / Defs.Coef, bottomAnchor.localPosition.z);
		StartCoroutine(ResetpositionCoroutine());
	}

	public void OnKeyboardHide()
	{
		bottomAnchor.localPosition = new Vector3(bottomAnchor.localPosition.x, bottomAnchor.localPosition.y - keyboardSize / Defs.Coef, bottomAnchor.localPosition.z);
		StartCoroutine(ResetpositionCoroutine());
		smilePanelTransform.gameObject.SetActive(false);
		smilePanelTransform.gameObject.SetActive(true);
	}

	private IEnumerator ResetpositionCoroutine()
	{
		yield return null;
		scrollLabels.ResetPosition();
		smilePanelTransform.gameObject.SetActive(false);
		smilePanelTransform.gameObject.SetActive(true);
	}

	public void PostChat(string _text)
	{
		if (Defs.isSoundFX)
		{
			NGUITools.PlaySound(sendChatClip);
		}
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.SendChat(_text, isClanMode, string.Empty);
		}
	}

	public void HandleCloseChat()
	{
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log("[Close Chat] pressed.");
		}
		CloseChat();
	}

	public void CloseChat(bool isHardClose = false)
	{
		if (!isHardClose && buySmileBannerPrefab.activeSelf)
		{
			UnityEngine.Debug.LogFormat("Ignoring CloseChat({0}), buySmiley: {1}", isHardClose, buySmileBannerPrefab.activeSelf);
			return;
		}
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.showChat = false;
			WeaponManager.sharedManager.myPlayerMoveC.AddButtonHandlers();
			WeaponManager.sharedManager.myPlayerMoveC.inGameGUI.gameObject.SetActive(true);
			if (WeaponManager.sharedManager.myPlayerMoveC.isMechActive)
			{
				if (GameConnect.isDaterRegim)
				{
					WeaponManager.sharedManager.myPlayerMoveC.mechBearPoint.SetActive(true);
				}
				else if (WeaponManager.sharedManager.myPlayerMoveC.currentMech != null)
				{
					WeaponManager.sharedManager.myPlayerMoveC.currentMech.point.SetActive(true);
				}
			}
			else if (!GameConnect.isSpeedrun && !GameConnect.isDeathEscape)
			{
				WeaponManager.sharedManager.currentWeaponSounds.gameObject.SetActive(true);
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
		sharedController = null;
	}

	private void OnDestroy()
	{
		MyUIInput myUIInput = sendMessageInput;
		myUIInput.onKeyboardInter = (Action)Delegate.Remove(myUIInput.onKeyboardInter, new Action(SendMessageFromInput));
		MyUIInput myUIInput2 = sendMessageInput;
		myUIInput2.onKeyboardCancel = (Action)Delegate.Remove(myUIInput2.onKeyboardCancel, new Action(CancelSendPrivateMessage));
		MyUIInput myUIInput3 = sendMessageInput;
		myUIInput3.onKeyboardVisible = (Action)Delegate.Remove(myUIInput3.onKeyboardVisible, new Action(OnKeyboardVisible));
		MyUIInput myUIInput4 = sendMessageInput;
		myUIInput4.onKeyboardHide = (Action)Delegate.Remove(myUIInput4.onKeyboardHide, new Action(OnKeyboardHide));
		sharedController = null;
	}

	public void BuySmileOnClick()
	{
		buySmileBannerPrefab.SetActive(true);
		sendMessageInput.isSelected = false;
		sendMessageInput.DeselectInput();
	}
}
