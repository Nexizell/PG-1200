using System;
using UnityEngine;

[Serializable]
public sealed class HiddenSettings : ScriptableObject
{
	public string PhotonAppIdEncoded;

	public string PhotonAppIdPad;

	public string PhotonAppIdSignatureEncoded;

	public string PhotonAppIdSignaturePad;

	public string devtodevSecretGoogle;

	public string devtodevSecretAmazon;

	public string devtodevSecretIos;

	public string devtodevSecretWsa;

	public string appsFlyerAppKey;

	public string flurryAmazonApiKey;

	public string flurryAmazonDevApiKey;

	public string flurryAndroidApiKey;

	public string flurryAndroidDevApiKey;

	public string flurryIosApiKey;

	public string flurryIosDevApiKey;

	[SerializeField]
	protected internal string persistentCacheManagerKey;

	[SerializeField]
	protected internal string cloudSynchronizerKey;

	[SerializeField]
	protected internal string playerPrefsKey;

	public string PersistentCacheManagerKey
	{
		get
		{
			return persistentCacheManagerKey;
		}
	}

	public string CloudSynchronizerKey
	{
		get
		{
			return cloudSynchronizerKey;
		}
	}

	public string PlayerPrefsKey
	{
		get
		{
			return playerPrefsKey;
		}
	}
}
