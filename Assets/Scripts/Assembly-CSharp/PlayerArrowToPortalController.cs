using System;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

public sealed class PlayerArrowToPortalController : MonoBehaviour
{
	public GameObject ArrowToPortalPoint;

	[Range(0f, 45f)]
	public float ArrowAngle = 30f;

	[Range(0f, 4f)]
	public float ArrowHeight = 1.5f;

	[SerializeField]
	protected internal Texture greenTexture;

	[SerializeField]
	protected internal Texture redTexture;

	private Transform _arrowToPortal;

	private readonly Rilisoft.Lazy<Transform> _arrowToPortalPoint;

	private readonly Rilisoft.Lazy<Camera> _camera;

	private readonly Rilisoft.Lazy<Camera> _grandpaCamera;

	private Transform _poi;

	private Vector3 _poiLocalOffset;

	private Renderer[] _renderers;

	private static readonly Queue<GameObject> _arrowPool = new Queue<GameObject>();

	private static readonly Rilisoft.Lazy<UnityEngine.Object> _arrowPrefab = new Rilisoft.Lazy<UnityEngine.Object>(() => Resources.Load("ArrowToPortal"));

	internal Camera ParentCamera
	{
		get
		{
			return _camera.Value;
		}
	}

	internal Camera GrandpaCamera
	{
		get
		{
			return _grandpaCamera.Value;
		}
	}

	public PlayerArrowToPortalController()
	{
		_arrowToPortalPoint = new Rilisoft.Lazy<Transform>(() => ArrowToPortalPoint.transform);
		_camera = new Rilisoft.Lazy<Camera>(() => ArrowToPortalPoint.GetComponentInParents<Camera>());
		_grandpaCamera = new Rilisoft.Lazy<Camera>(() => ParentCamera.transform.parent.gameObject.GetComponentInParents<Camera>());
	}

	private void Update()
	{
		if (_poi == null || _arrowToPortal == null)
		{
			return;
		}
		float z = (ArrowHeight + 0.3f) / Mathf.Tan((float)Math.PI / 180f * ParentCamera.fieldOfView * 0.5f);
		Vector3 localPosition = _arrowToPortalPoint.Value.localPosition;
		localPosition.y = ArrowHeight;
		localPosition.z = z;
		_arrowToPortalPoint.Value.localPosition = localPosition;
		Vector3 vector = _poi.TransformPoint(_poiLocalOffset);
		Vector3 position = _arrowToPortal.position;
		Vector3 forward = vector - position;
		Vector3 vector2 = ParentCamera.transform.position - GrandpaCamera.transform.position;
		forward += vector2;
		_arrowToPortal.rotation = Quaternion.LookRotation(forward);
		Vector3 position2 = ParentCamera.gameObject.transform.position;
		float num = Vector3.SqrMagnitude(position - position2);
		float num2 = Vector3.SqrMagnitude(vector - position);
		float num3 = Vector3.SqrMagnitude(vector - position2);
		float num4 = Mathf.Max(9f, 0.25f * num);
		bool num5 = num2 < num4 || num3 < num4;
		bool flag = !num5;
		if (num5)
		{
			Vector3 vector3 = ParentCamera.WorldToViewportPoint(vector);
			if (vector3.x < 0f || vector3.x > 1f)
			{
				flag = true;
			}
			if (vector3.y < 0f || vector3.y > 1f)
			{
				flag = true;
			}
		}
		Renderer[] renderers = _renderers;
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].enabled = flag;
		}
	}

	internal void SetPointOfInterest(Transform poi, Vector3 localOffset)
	{
		SetPointOfInterest(poi, localOffset, Color.green);
	}

	internal void SetPointOfInterest(Transform poi, Vector3 localOffset, Color color)
	{
		_poi = poi;
		_poiLocalOffset = localOffset;
		GameObject arrowFromPool = GetArrowFromPool();
		if (arrowFromPool == null)
		{
			return;
		}
		_arrowToPortal = arrowFromPool.transform;
		_renderers = arrowFromPool.GetComponentsInChildren<Renderer>();
		_arrowToPortal.parent = _arrowToPortalPoint.Value;
		_arrowToPortal.localPosition = Vector3.zero;
		if (_renderers.Length == 0)
		{
			return;
		}
		Renderer renderer = _renderers[0];
		if (!(renderer == null))
		{
			Texture texture = null;
			texture = ((!(color == Color.red)) ? greenTexture : redTexture);
			if (texture != null && (object)texture != renderer.material.mainTexture)
			{
				renderer.material.mainTexture = texture;
			}
		}
	}

	public void RemovePointOfInterest()
	{
		if (!(_arrowToPortal == null))
		{
			_poi = null;
			_poiLocalOffset = Vector3.zero;
			DisposeArrowToPool(_arrowToPortal.gameObject);
			_arrowToPortal = null;
			_renderers = new Renderer[0];
		}
	}

	public static bool PopulateArrowPoolIfEmpty()
	{
		if (_arrowPool.Count > 0)
		{
			return true;
		}
		if (_arrowPrefab.Value == null)
		{
			return false;
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(_arrowPrefab.Value);
		gameObject.SetActive(false);
		Transform obj = gameObject.transform;
		obj.parent = null;
		obj.localPosition = Vector3.zero;
		_arrowPool.Enqueue(gameObject);
		return true;
	}

	public static GameObject GetArrowFromPool()
	{
		GameObject gameObject = null;
		while (_arrowPool.Count > 0 && gameObject == null)
		{
			gameObject = _arrowPool.Dequeue();
			if (gameObject == null)
			{
				Debug.LogWarning("Arrow pointer from pool is null.");
			}
		}
		if (gameObject == null)
		{
			if (!PopulateArrowPoolIfEmpty())
			{
				return null;
			}
			gameObject = _arrowPool.Dequeue();
		}
		Transform obj = gameObject.transform;
		obj.parent = null;
		obj.localPosition = Vector3.zero;
		gameObject.SetActive(true);
		return gameObject;
	}

	public static void DisposeArrowToPool(GameObject arrow)
	{
		if (!(arrow == null))
		{
			arrow.SetActive(false);
			_arrowPool.Enqueue(arrow);
		}
	}
}
