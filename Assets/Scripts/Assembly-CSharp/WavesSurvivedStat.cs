using System.Globalization;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class WavesSurvivedStat : MonoBehaviour
{
	internal static int SurvivedWaveCount { get; set; }

	private void Start()
	{
		UILabel component = GetComponent<UILabel>();
		if (component != null)
		{
			component.text = SurvivedWaveCount.ToString(CultureInfo.InvariantCulture);
		}
	}
}
