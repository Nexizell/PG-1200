using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	public class BindedBillboard : MonoBehaviour
	{
		private static List<BindedBillboard> _instances = new List<BindedBillboard>();

		[SerializeField]
		protected internal Transform _pointInWorld;

		[SerializeField]
		protected internal Collider _touchCollider;

		private Func<Transform> _pointGetter;

		public Collider Collider
		{
			get
			{
				if (_touchCollider == null)
				{
					_touchCollider = base.gameObject.GetComponent<Collider>();
					if (_touchCollider == null)
					{
						_touchCollider = GetComponentInChildren<Collider>();
					}
				}
				return _touchCollider;
			}
		}

		public static BindedBillboard GetByGameObjectName(string goName)
		{
			return _instances.FirstOrDefault((BindedBillboard inst) => inst.name == goName);
		}

		public void BindTo(Func<Transform> point)
		{
			_pointInWorld = null;
			_pointGetter = point;
		}

		private void Awake()
		{
			_instances.Add(this);
			if (_pointGetter == null && _pointInWorld != null)
			{
				_pointGetter = () => _pointInWorld;
			}
		}

		private void Update()
		{
			if (_pointInWorld != null)
			{
				_pointGetter = () => _pointInWorld;
			}
			if (_pointGetter != null && !(_pointGetter() == null) && !(NickLabelController.currentCamera == null))
			{
				base.transform.OverlayPosition(_pointGetter());
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, 0f);
				if (Collider != null)
				{
					Collider.enabled = _pointGetter().gameObject.activeSelf;
				}
			}
		}

		private void OnDestroy()
		{
			if (_instances.Contains(this))
			{
				_instances.Remove(this);
			}
		}
	}
}
