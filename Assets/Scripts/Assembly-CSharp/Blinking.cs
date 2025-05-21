using UnityEngine;

[RequireComponent(typeof(UISprite))]
public class Blinking : MonoBehaviour
{
	public float halfCycle = 1f;

	private UISprite _sprite;

	private float _time;

	private Color _baseColor;

	private void Awake()
	{
		_sprite = GetComponent<UISprite>();
		_baseColor = _sprite.color;
	}

	private void OnEnable()
	{
		_baseColor = _sprite.color;
	}

	private void Update()
	{
		_time += Time.unscaledDeltaTime;
		if (_sprite != null)
		{
			Color color = _sprite.color;
			float num = 2f * (_time - Mathf.Floor(_time / halfCycle) * halfCycle) / halfCycle;
			if (num > 1f)
			{
				num = 2f - num;
			}
			_sprite.color = new Color(color.r, color.g, color.b, num);
		}
	}

	private void OnDisable()
	{
		_baseColor = _sprite.color;
	}
}
