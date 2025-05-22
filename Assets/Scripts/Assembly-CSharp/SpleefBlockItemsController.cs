using System.Collections.Generic;
using Photon;
using Rilisoft.WP8;
using UnityEngine;

[ExecuteInEditMode]
public class SpleefBlockItemsController : Photon.MonoBehaviour
{
	public static SpleefBlockItemsController instance;

	public SpleefBlockItem[] items;

	public Material[] stageMaterials;

	public AudioClip hitSound;

	public AudioClip deadSound;

	private void Start()
	{
		PhotonObjectCacher.AddObject(base.gameObject);
		instance = this;
	}

	private void OnDestroy()
	{
		instance = null;
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	private void Update()
	{
		if (Application.isEditor && !Application.isPlaying)
		{
			items = base.gameObject.transform.GetComponentsInChildren<SpleefBlockItem>();
			for (short num = 0; num < items.Length; num++)
			{
				items[num].index = num;
			}
		}
	}

	public void GetDamage(short index, float damage)
	{
		base.photonView.RPC("GetDamageRPC", PhotonTargets.Others, index, damage);
	}

	
	[PunRPC]
	public void GetDamageRPC(short index, float _minus)
	{
		items[index].GetDamageRPC(_minus);
	}

	
	[PunRPC]
	public void SynchLivesItems(short[] _index, byte[] _lives)
	{
		for (short num = 0; num < _index.Length; num++)
		{
			if (items[_index[num]].live > (float)(int)_lives[num])
			{
				if (_lives[num] <= 0)
				{
					items[_index[num]].gameObject.SetActive(false);
				}
				items[_index[num]].live = (int)_lives[num];
				items[_index[num]].isDamageble = true;
			}
		}
	}

	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		List<byte> list = new List<byte>();
		List<short> list2 = new List<short>();
		for (short num = 0; num < items.Length; num++)
		{
			if (items[num].isDamageble)
			{
				list2.Add(items[num].index);
				if (items[num].isKilled)
				{
					Debug.Log("Add 0 index =" + num);
					list.Add(0);
				}
				else
				{
					list.Add((byte)Mathf.CeilToInt(items[num].live));
					Debug.Log("Add " + (byte)Mathf.CeilToInt(items[num].live) + "index =" + num);
				}
			}
		}
		base.photonView.RPC("SynchLivesItems", player, list2.ToArray(), list.ToArray());
	}
}
