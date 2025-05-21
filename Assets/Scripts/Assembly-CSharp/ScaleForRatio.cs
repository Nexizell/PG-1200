using UnityEngine;

public class ScaleForRatio : MonoBehaviour
{
	public Transform[] objectsForScale;

	public float multipler = 0.9f;

	private Vector3[] startScales;

	private void Awake()
	{
		startScales = new Vector3[objectsForScale.Length];
		for (int i = 0; i < objectsForScale.Length; i++)
		{
			if (objectsForScale[i] != null)
			{
				startScales[i] = objectsForScale[i].localScale;
			}
		}
	}

	[ContextMenu("Execute")]
	private void OnEnable()
	{
		for (int i = 0; i < objectsForScale.Length; i++)
		{
			if (objectsForScale[i] != null)
			{
				float num = 1f + (1f - 1.3333334f / ((float)Screen.width / (float)Screen.height));
				if (num > 1.05f)
				{
					objectsForScale[i].localScale = startScales[i] * num * multipler;
				}
				else
				{
					objectsForScale[i].localScale = startScales[i] * num;
				}
			}
		}
	}
}
