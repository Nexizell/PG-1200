using UnityEngine;

namespace Rilisoft
{
	[ExecuteInEditMode]
	public class LobbyItemSkyboxComponent : MonoBehaviour
	{
		[SerializeField]
		protected internal Color _ambientColor;

		[SerializeField]
		protected internal FogSettings _fog;

		private Color _baseAmbientColor;

		private FogSettings _baseFog;

		private void OnEnable()
		{
			_baseAmbientColor = RenderSettings.ambientSkyColor;
			_baseFog = new FogSettings
			{
				Active = RenderSettings.fog,
				Mode = RenderSettings.fogMode,
				Color = RenderSettings.fogColor,
				Start = RenderSettings.fogStartDistance,
				End = RenderSettings.fogEndDistance
			};
			RenderSettings.ambientSkyColor = _ambientColor;
			RenderSettings.fog = _fog.Active;
			RenderSettings.fogMode = _fog.Mode;
			RenderSettings.fogColor = _fog.Color;
			RenderSettings.fogStartDistance = _fog.Start;
			RenderSettings.fogEndDistance = _fog.End;
		}

		private void OnDisable()
		{
			RenderSettings.ambientSkyColor = _baseAmbientColor;
			RenderSettings.fog = _baseFog.Active;
			RenderSettings.fogMode = _baseFog.Mode;
			RenderSettings.fogColor = _baseFog.Color;
			RenderSettings.fogStartDistance = _baseFog.Start;
			RenderSettings.fogEndDistance = _baseFog.End;
		}
	}
}
