using System;
using Rilisoft;
using UnityEngine;

[Serializable]
public sealed class StorageExplanation : MonoBehaviour
{
	private static StorageExplanation s_firstInstance;

	[SerializeField]
	protected internal GameObject _window;

	[Obsolete]
	[SerializeField]
	protected internal GameObject _continueButton;

	private Action _continueCallback;

	private IDisposable _backSubscription;

	public static StorageExplanation Instance
	{
		get
		{
			return s_firstInstance;
		}
	}

	public void SetContinueCallback(Action continueCallback)
	{
		_continueCallback = continueCallback;
	}

	public void SetVisibiliity(bool windowVisible)
	{
		if (_window != null)
		{
			_window.SetActive(windowVisible);
		}
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
		if (windowVisible)
		{
			_backSubscription = BackSystem.Instance.Register(HandleQuit);
		}
	}

	public void HandleQuit()
	{
		SetVisibiliity(false);
		Application.Quit();
	}

	public void HandleContinue()
	{
		SetVisibiliity(false);
		if (_continueCallback != null)
		{
			_continueCallback();
		}
		_continueCallback = null;
	}

	private void Awake()
	{
		if (!(s_firstInstance != null))
		{
			s_firstInstance = this;
		}
	}
}
