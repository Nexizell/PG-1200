using UnityEngine;

public class ThirdPersonAnimationWrapper : MonoBehaviour
{
	public Animator animator;

	public Renderer[] playerRenderers;

	public visibleObjPhoton visObj;

	private bool _grounded = true;

	private bool _wasGrounded = true;

	private float tempTimer = 15f;

	private float jumpTimer = 1f;

	public float VelocityMagnitude
	{
		get
		{
			return animator.GetFloat("VelocityMagnitude");
		}
		set
		{
			animator.SetFloat("VelocityMagnitude", value);
		}
	}

	public bool isGrounded
	{
		get
		{
			return _grounded;
		}
		set
		{
			_grounded = value;
			animator.SetBool("isGrounded", _grounded);
		}
	}

	public void StartFall()
	{
		animator.SetTrigger("Fall");
	}

	public void Revive()
	{
		animator.ResetTrigger("Fall");
		animator.SetTrigger("Revive");
	}

	private void Start()
	{
		VelocityMagnitude = 0f;
	}

	private void Update()
	{
		if (_grounded && !_wasGrounded)
		{
			animator.SetTrigger("Land");
		}
		_wasGrounded = _grounded;
	}
}
