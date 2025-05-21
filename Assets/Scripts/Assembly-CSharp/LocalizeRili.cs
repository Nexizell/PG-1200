using System.Collections.Generic;
using I2.Loc;
using UnityEngine;

[ExecuteInEditMode]
public class LocalizeRili : MonoBehaviour
{
	public List<UILabel> labels;

	public string term;

	public bool execute;

	[Header("delete after execute?")]
	public bool selfDestroy;

	private void Start()
	{
		labels = new List<UILabel>();
		labels.AddRange(base.gameObject.GetComponentsInChildren<UILabel>());
	}

	private void Update()
	{
		if (!execute || labels == null)
		{
			return;
		}
		foreach (UILabel label in labels)
		{
			while (label.GetComponent<Localize>() != null)
			{
				Object.DestroyImmediate(label.GetComponent<Localize>());
			}
			label.gameObject.AddComponent<Localize>().SetTerm("Key_04B_03", "Key_04B_03");
			if (term != "")
			{
				label.gameObject.AddComponent<Localize>().SetTerm(term, term);
			}
		}
		execute = false;
		if (selfDestroy)
		{
			Object.DestroyImmediate(this);
		}
	}
}
