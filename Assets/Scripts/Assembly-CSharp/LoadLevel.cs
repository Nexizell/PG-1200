using Rilisoft;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
	public Texture fon;

	private void Start()
	{
		Singleton<SceneLoader>.Instance.LoadScene("Level3");
	}

	private void OnGUI()
	{
		GUI.DrawTexture(new Rect(((float)Screen.width - 1366f * Defs.Coef) / 2f, 0f, 1366f * Defs.Coef, Screen.height), fon, ScaleMode.StretchToFill);
	}
}
