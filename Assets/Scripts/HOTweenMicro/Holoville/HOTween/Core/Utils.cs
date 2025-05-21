using System;
using UnityEngine;

namespace Holoville.HOTween.Core
{
	internal static class Utils
	{
		internal static Quaternion MatrixToQuaternion(Matrix4x4 m)
		{
			Quaternion result = default(Quaternion);
			float num = 1f + m[0, 0] + m[1, 1] + m[2, 2];
			if (num < 0f)
			{
				num = 0f;
			}
			result.w = (float)Math.Sqrt(num) * 0.5f;
			num = 1f + m[0, 0] - m[1, 1] - m[2, 2];
			if (num < 0f)
			{
				num = 0f;
			}
			result.x = (float)Math.Sqrt(num) * 0.5f;
			num = 1f - m[0, 0] + m[1, 1] - m[2, 2];
			if (num < 0f)
			{
				num = 0f;
			}
			result.y = (float)Math.Sqrt(num) * 0.5f;
			num = 1f - m[0, 0] - m[1, 1] + m[2, 2];
			if (num < 0f)
			{
				num = 0f;
			}
			result.z = (float)Math.Sqrt(num) * 0.5f;
			result.x *= Mathf.Sign(result.x * (m[2, 1] - m[1, 2]));
			result.y *= Mathf.Sign(result.y * (m[0, 2] - m[2, 0]));
			result.z *= Mathf.Sign(result.z * (m[1, 0] - m[0, 1]));
			return result;
		}

		internal static string SimpleClassName(Type p_class)
		{
			string text = p_class.ToString();
			return text.Substring(text.LastIndexOf('.') + 1);
		}
	}
}
