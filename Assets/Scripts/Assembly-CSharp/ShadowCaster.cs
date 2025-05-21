using UnityEngine;

public class ShadowCaster : MonoBehaviour
{
	[SerializeField]
	protected internal LayerMask _ground;

	[SerializeField]
	protected internal Material _shadow;

	[SerializeField]
	protected internal Transform _shadowObj;

	[SerializeField]
	protected internal float maxDistance = 2f;

	private Transform follow;

	private Vector3 delta;

	private void Start()
	{
		follow = base.transform.parent;
		delta = base.transform.localPosition;
		_shadow = Object.Instantiate(_shadow);
		_shadowObj.GetComponent<Renderer>().sharedMaterial = _shadow;
		base.transform.parent = null;
	}

	private void LateUpdate()
	{
		if (follow == null || follow.Equals(null))
		{
			Object.Destroy(base.gameObject);
			return;
		}
		base.transform.position = follow.position;
		RaycastHit hitInfo;
		if (Physics.Raycast(base.transform.position, Vector3.down, out hitInfo, maxDistance, _ground))
		{
			float num = Vector3.Distance(base.transform.position, hitInfo.point);
			_shadow.SetColor("_Color", new Color(1f, 1f, 1f, 1f - num / maxDistance));
			_shadowObj.position = hitInfo.point + Vector3.up * 0.05f;
			_shadowObj.forward = -hitInfo.normal;
		}
		else
		{
			_shadow.SetColor("_Color", new Color(1f, 1f, 1f, 0f));
		}
	}
}
