using UnityEngine;

public sealed class Shoot : MonoBehaviour
{
	public float Range = 1000f;

	public Transform _transform;

	public GameObject bullet;

	private GameObject _bulletSpawnPoint;

	public int lives = 100;

	private void Start()
	{
		_bulletSpawnPoint = GameObject.Find("BulletSpawnPoint");
	}

	public void shootS()
	{
		Debug.Log("Shot!!" + base.transform.position);
		RaycastHit hitInfo;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f)), out hitInfo, 100f, Player_move_c._ShootRaycastLayerMask))
		{
			Debug.Log("Hit!");
			hitInfo.collider.gameObject.transform.CompareTag("Enemy");
		}
	}
}
