using System;
using UnityEngine;

namespace Rilisoft
{
	public class BindedUI : MonoBehaviour
	{
		private Func<Vector3> _targetPositionGetter;

		private Func<int> _targetLayerGetter;

		public void BindTo(Func<Vector3> targetPositionGetter, Func<int> targetLayerGetter)
		{
			_targetPositionGetter = targetPositionGetter;
			_targetLayerGetter = targetLayerGetter;
		}

		public void BindTo(Transform target)
		{
			if (!(target == null))
			{
				_targetPositionGetter = () => target.position;
				_targetLayerGetter = () => target.gameObject.layer;
			}
		}

		public void BindTo(GameObject target)
		{
			if (!(target == null))
			{
				_targetPositionGetter = () => target.transform.position;
				_targetLayerGetter = () => target.layer;
			}
		}

		public void Unbind()
		{
			_targetPositionGetter = null;
			_targetLayerGetter = null;
		}

		private void Update()
		{
			if (_targetPositionGetter != null && _targetLayerGetter != null)
			{
				OverlayPosition(_targetPositionGetter(), _targetLayerGetter());
			}
		}

		private void OverlayPosition(Vector3 targetPositon, int targetLayer)
		{
			Camera camera = NGUITools.FindCameraForLayer(base.transform.gameObject.layer);
			Camera camera2 = NGUITools.FindCameraForLayer(targetLayer);
			if (camera != null && camera2 != null)
			{
				base.transform.OverlayPosition(targetPositon, camera2, camera);
			}
		}
	}
}
