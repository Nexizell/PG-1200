using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	public class WeaponSkinsTestComponent : MonoBehaviour
	{
		private GameObject _prevGo;

		[SerializeField]
		protected internal GameObject _go;

		[SerializeField]
		protected internal bool _doNotCreteBaseSkin;

		[ReadOnly]
		[SerializeField]
		protected internal List<WeaponSkin> _skins = new List<WeaponSkin>();

		[SerializeField]
		[ReadOnly]
		protected internal WeaponSkin _currentSkin;

		private WeaponSkin _baseSkin;

		private void OnGUI()
		{
		}
	}
}
