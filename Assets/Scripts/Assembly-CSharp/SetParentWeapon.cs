using Rilisoft;
using UnityEngine;

public sealed class SetParentWeapon : MonoBehaviour
{
	private bool isMine;

	private bool isInet;

	private bool isMulti;

	private PhotonView photonView;

	private void Start()
	{
		if (SceneLoader.ActiveSceneName.Equals(Defs.MainMenuScene))
		{
			return;
		}
		isMulti = Defs.isMulti;
		if (isMulti)
		{
			isInet = Defs.isInet;
			photonView = PhotonView.Get(this);
			if (isInet)
			{
				isMine = photonView.isMine;
			}
			SetParent();
		}
	}

	private void SetParent()
	{
		int num = -1;
		if (isInet && (bool)photonView && photonView.owner != null)
		{
			num = photonView.owner.ID;
		}
		foreach (Player_move_c player in Initializer.players)
		{
			if (!isInet || !(player.mySkinName.photonView != null) || player.mySkinName.photonView.owner == null || player.mySkinName.photonView.owner.ID != num)
			{
				continue;
			}
			GameObject playerGameObject = player.mySkinName.playerGameObject;
			GameObject gameObject = null;
			base.transform.position = Vector3.zero;
			if (!base.transform.GetComponent<WeaponSounds>().isMelee)
			{
				foreach (Transform item in base.transform)
				{
					if (item.gameObject.name.Equals("BulletSpawnPoint") && item.childCount >= 0)
					{
						gameObject = item.GetChild(0).gameObject;
						if (!isMine)
						{
							WeaponManager.SetGunFlashActive(gameObject, false);
						}
						break;
					}
				}
			}
			foreach (Transform item2 in playerGameObject.transform)
			{
				item2.parent = null;
				item2.position += -Vector3.up * 1000f;
			}
			base.transform.parent = playerGameObject.transform;
			if (base.transform.Find("BulletSpawnPoint") != null)
			{
				player._bulletSpawnPoint = base.transform.Find("BulletSpawnPoint").gameObject;
			}
			base.transform.localPosition = new Vector3(0f, -1.7f, 0f);
			base.transform.rotation = playerGameObject.transform.rotation;
			if (playerGameObject.transform.parent.gameObject != null)
			{
				player.SetTextureForBodyPlayer(player._skin);
			}
			return;
		}
		Invoke("SetParent", 0.1f);
	}
}
