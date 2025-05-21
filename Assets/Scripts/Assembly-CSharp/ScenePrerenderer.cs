using System.Collections.Generic;
using UnityEngine;

public class ScenePrerenderer : MonoBehaviour
{
	public Camera activeCamera;

	private RenderTexture _rt;

	public bool FinishPrerendering;

	public GameObject[] objsToRender;

	private GameObject _enemiesToRender;

	private void Awake()
	{
		_rt = new RenderTexture(32, 32, 24);
		_rt.Create();
		activeCamera.targetTexture = _rt;
		activeCamera.useOcclusionCulling = false;
	}

	private void Start()
	{
		Render_();
	}

	private void Render_()
	{
		List<GameObject> zombiePrefabs = GameObject.FindGameObjectWithTag("GameController").GetComponent<ZombieCreator>().zombiePrefabs;
		GameObject[] array = new GameObject[zombiePrefabs.Count];
		int num = 0;
		foreach (GameObject item in zombiePrefabs)
		{
			GameObject gameObject = Object.Instantiate(item.transform.GetChild(0).gameObject, new Vector3(base.transform.position.x, base.transform.position.y - 20f, base.transform.position.z), item.transform.GetChild(0).gameObject.transform.rotation);
			string text = "(Clone)";
			int num2 = gameObject.name.IndexOf(text);
			gameObject.name = ((num2 < 0) ? gameObject.name : gameObject.name.Remove(num2, text.Length));
			gameObject.transform.parent = base.transform.parent;
			SkinsController.GetSkinForObj(gameObject);
			array[num] = gameObject;
			num++;
		}
		activeCamera.Render();
		RenderTexture.active = _rt;
		activeCamera.targetTexture = null;
		RenderTexture.active = null;
		GameObject[] array2 = objsToRender;
		for (int i = 0; i < array2.Length; i++)
		{
			Object.Destroy(array2[i]);
		}
		Object.Destroy(base.transform.parent.parent.gameObject);
		Object.Destroy(activeCamera);
	}
}
