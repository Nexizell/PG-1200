using UnityEngine;

public class RotationWeaponGameObject : MonoBehaviour
{
	public enum ConstraintAxis
	{
		X = 0,
		Y = 1,
		Z = 2
	}

	public ConstraintAxis axis;

	public float min;

	public float max;

	public Transform playerGun;

	public Transform mechGun;

	public Player_move_c player_move_c;

	private Transform thisTransform;

	private Vector3 rotateAround;

	private Quaternion minQuaternion;

	private Quaternion maxQuaternion;

	private float range;

	private void Start()
	{
		thisTransform = base.transform;
		switch (axis)
		{
		case ConstraintAxis.X:
			rotateAround = Vector3.right;
			break;
		case ConstraintAxis.Y:
			rotateAround = Vector3.up;
			break;
		case ConstraintAxis.Z:
			rotateAround = Vector3.forward;
			break;
		}
		Quaternion quaternion = Quaternion.AngleAxis((axis == ConstraintAxis.X) ? thisTransform.localRotation.eulerAngles.x : ((axis == ConstraintAxis.Y) ? thisTransform.localRotation.eulerAngles.y : thisTransform.localRotation.eulerAngles.z), rotateAround);
		minQuaternion = quaternion * Quaternion.AngleAxis(min, rotateAround);
		maxQuaternion = quaternion * Quaternion.AngleAxis(max, rotateAround);
		range = max - min;
	}

	private void SetActiveFalse()
	{
		base.enabled = false;
	}

	private void LateUpdate()
	{
		Quaternion localRotation = thisTransform.localRotation;
		Quaternion a = Quaternion.AngleAxis((axis == ConstraintAxis.X) ? localRotation.eulerAngles.x : ((axis == ConstraintAxis.Y) ? localRotation.eulerAngles.y : localRotation.eulerAngles.z), rotateAround);
		float num = Quaternion.Angle(a, minQuaternion);
		float num2 = Quaternion.Angle(a, maxQuaternion);
		if (num <= range && num2 <= range)
		{
			playerGun.rotation = thisTransform.rotation;
			playerGun.Rotate(player_move_c.deltaAngle, 0f, 0f);
			mechGun.rotation = thisTransform.rotation;
			mechGun.Rotate(player_move_c.deltaAngle, 0f, 0f);
		}
		else
		{
			Vector3 localEulerAngles = localRotation.eulerAngles;
			localEulerAngles = ((!(num > num2)) ? new Vector3((axis == ConstraintAxis.X) ? minQuaternion.eulerAngles.x : localEulerAngles.x, (axis == ConstraintAxis.Y) ? minQuaternion.eulerAngles.y : localEulerAngles.y, (axis == ConstraintAxis.Z) ? minQuaternion.eulerAngles.z : localEulerAngles.z) : new Vector3((axis == ConstraintAxis.X) ? maxQuaternion.eulerAngles.x : localEulerAngles.x, (axis == ConstraintAxis.Y) ? maxQuaternion.eulerAngles.y : localEulerAngles.y, (axis == ConstraintAxis.Z) ? maxQuaternion.eulerAngles.z : localEulerAngles.z));
			thisTransform.localEulerAngles = localEulerAngles;
			playerGun.rotation = thisTransform.rotation;
			playerGun.Rotate(player_move_c.deltaAngle, 0f, 0f);
			mechGun.rotation = thisTransform.rotation;
			mechGun.Rotate(player_move_c.deltaAngle, 0f, 0f);
		}
	}
}
