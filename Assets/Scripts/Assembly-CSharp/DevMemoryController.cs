using UnityEngine;

public class DevMemoryController : MonoBehaviour
{
	public static string keyActiveMemoryInfo = "keyActiveMemoryInfo";

	public static DevMemoryController instance = null;

	private void Awake()
	{
		Object.Destroy(this);
	}
}
