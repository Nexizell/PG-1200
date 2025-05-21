using System;
using System.Reflection;
using Holoville.HOTween.Core;
using Holoville.HOTween.Plugins.Core;
using UnityEngine;

namespace Holoville.HOTween.Plugins
{
	public class PlugVector3Path : ABSTweenPlugin
	{
		private enum OrientType
		{
			None = 0,
			ToPath = 1,
			LookAtTransform = 2,
			LookAtPosition = 3
		}

		private const int SUBDIVISIONS_MULTIPLIER = 16;

		private const float EPSILON = 0.001f;

		private const float MIN_LOOKAHEAD = 0.0001f;

		private const float MAX_LOOKAHED = 0.9999f;

		internal static Type[] validPropTypes = new Type[1] { typeof(Vector3) };

		internal static Type[] validValueTypes = new Type[1] { typeof(Vector3[]) };

		internal Path path;

		internal float pathPerc;

		internal bool hasAdditionalStartingP;

		private Vector3 typedStartVal;

		private Vector3[] points;

		private Vector3 diffChangeVal;

		internal bool isClosedPath;

		private OrientType orientType;

		private float lookAheadVal = 0.0001f;

		private Axis lockPositionAxis;

		private Axis lockRotationAxis;

		private bool isPartialPath;

		private bool usesLocalPosition;

		private float startPerc;

		private float changePerc = 1f;

		private Vector3 lookPos;

		private Transform lookTrans;

		private Transform orientTrans;

		internal PathType pathType { get; private set; }

		protected override object startVal
		{
			get
			{
				return _startVal;
			}
			set
			{
				if (tweenObj.isFrom)
				{
					_endVal = value;
					Vector3[] array = (Vector3[])value;
					points = new Vector3[array.Length];
					Array.Copy(array, points, array.Length);
					Array.Reverse(points);
				}
				else
				{
					_startVal = (typedStartVal = (Vector3)value);
				}
			}
		}

		protected override object endVal
		{
			get
			{
				return _endVal;
			}
			set
			{
				if (tweenObj.isFrom)
				{
					_startVal = (typedStartVal = (Vector3)value);
					return;
				}
				_endVal = value;
				Vector3[] array = (Vector3[])value;
				points = new Vector3[array.Length];
				Array.Copy(array, points, array.Length);
			}
		}

		public PlugVector3Path(Vector3[] p_path, PathType p_type = PathType.Curved)
			: base(p_path, false)
		{
			pathType = p_type;
		}

		public PlugVector3Path(Vector3[] p_path, EaseType p_easeType, PathType p_type = PathType.Curved)
			: base(p_path, p_easeType, false)
		{
			pathType = p_type;
		}

		public PlugVector3Path(Vector3[] p_path, bool p_isRelative, PathType p_type = PathType.Curved)
			: base(p_path, p_isRelative)
		{
			pathType = p_type;
		}

		public PlugVector3Path(Vector3[] p_path, EaseType p_easeType, bool p_isRelative, PathType p_type = PathType.Curved)
			: base(p_path, p_easeType, p_isRelative)
		{
			pathType = p_type;
		}

		public PlugVector3Path(Vector3[] p_path, AnimationCurve p_easeAnimCurve, bool p_isRelative, PathType p_type = PathType.Curved)
			: base(p_path, p_easeAnimCurve, p_isRelative)
		{
			pathType = p_type;
		}

		internal override void Init(Tweener p_tweenObj, string p_propertyName, EaseType p_easeType, Type p_targetType, PropertyInfo p_propertyInfo, FieldInfo p_fieldInfo)
		{
			if (isRelative && p_tweenObj.isFrom)
			{
				isRelative = false;
				TweenWarning.Log(string.Concat("\"", p_tweenObj.target, ".", p_propertyName, "\": PlugVector3Path \"isRelative\" parameter is incompatible with HOTween.From. The tween will be treated as absolute."));
			}
			usesLocalPosition = p_propertyName == "localPosition";
			base.Init(p_tweenObj, p_propertyName, p_easeType, p_targetType, p_propertyInfo, p_fieldInfo);
		}

		public PlugVector3Path ClosePath()
		{
			return ClosePath(true);
		}

		public PlugVector3Path ClosePath(bool p_close)
		{
			isClosedPath = p_close;
			return this;
		}

		public PlugVector3Path OrientToPath()
		{
			return OrientToPath(true);
		}

		public PlugVector3Path OrientToPath(bool p_orient)
		{
			return OrientToPath(p_orient, 0.0001f, Axis.None);
		}

		public PlugVector3Path OrientToPath(float p_lookAhead)
		{
			return OrientToPath(true, p_lookAhead, Axis.None);
		}

		public PlugVector3Path OrientToPath(Axis p_lockRotationAxis)
		{
			return OrientToPath(true, 0.0001f, p_lockRotationAxis);
		}

		public PlugVector3Path OrientToPath(float p_lookAhead, Axis p_lockRotationAxis)
		{
			return OrientToPath(true, p_lookAhead, p_lockRotationAxis);
		}

		public PlugVector3Path OrientToPath(bool p_orient, float p_lookAhead, Axis p_lockRotationAxis)
		{
			if (p_orient)
			{
				orientType = OrientType.ToPath;
			}
			lookAheadVal = p_lookAhead;
			if (lookAheadVal < 0.0001f)
			{
				lookAheadVal = 0.0001f;
			}
			else if (lookAheadVal > 0.9999f)
			{
				lookAheadVal = 0.9999f;
			}
			lockRotationAxis = p_lockRotationAxis;
			return this;
		}

		public PlugVector3Path LookAt(Transform p_transform)
		{
			if (p_transform != null)
			{
				orientType = OrientType.LookAtTransform;
				lookTrans = p_transform;
			}
			return this;
		}

		public PlugVector3Path LookAt(Vector3 p_position)
		{
			orientType = OrientType.LookAtPosition;
			lookPos = p_position;
			lookTrans = null;
			return this;
		}

		public PlugVector3Path LockPosition(Axis p_lockAxis)
		{
			lockPositionAxis = p_lockAxis;
			return this;
		}

		protected override float GetSpeedBasedDuration(float p_speed)
		{
			return path.pathLength / p_speed;
		}

		protected override void SetChangeVal()
		{
			if (orientType != 0 && orientTrans == null)
			{
				orientTrans = tweenObj.target as Transform;
			}
			int num = 1;
			int num2 = (isClosedPath ? 1 : 0);
			int num3 = points.Length;
			Vector3[] array;
			if (isRelative)
			{
				hasAdditionalStartingP = false;
				Vector3 vector = points[0] - typedStartVal;
				array = new Vector3[num3 + 2 + num2];
				for (int i = 0; i < num3; i++)
				{
					array[i + num] = points[i] - vector;
				}
			}
			else
			{
				Vector3 vector2 = (Vector3)GetValue();
				Vector3 vector3 = vector2 - points[0];
				if (vector3.x < 0f)
				{
					vector3.x = 0f - vector3.x;
				}
				if (vector3.y < 0f)
				{
					vector3.y = 0f - vector3.y;
				}
				if (vector3.z < 0f)
				{
					vector3.z = 0f - vector3.z;
				}
				if (vector3.x < 0.001f && vector3.y < 0.001f && vector3.z < 0.001f)
				{
					hasAdditionalStartingP = false;
					array = new Vector3[num3 + 2 + num2];
				}
				else
				{
					hasAdditionalStartingP = true;
					array = new Vector3[num3 + 3 + num2];
					if (tweenObj.isFrom)
					{
						array[array.Length - 2] = vector2;
					}
					else
					{
						array[1] = vector2;
						num = 2;
					}
				}
				for (int j = 0; j < num3; j++)
				{
					array[j + num] = points[j];
				}
			}
			num3 = array.Length;
			if (isClosedPath)
			{
				array[num3 - 2] = array[1];
			}
			if (isClosedPath)
			{
				array[0] = array[num3 - 3];
				array[num3 - 1] = array[2];
			}
			else
			{
				array[0] = array[1];
				Vector3 vector4 = array[num3 - 2];
				Vector3 vector5 = vector4 - array[num3 - 3];
				array[num3 - 1] = vector4 + vector5;
			}
			if (lockPositionAxis != 0)
			{
				bool flag = (lockPositionAxis & Axis.X) == Axis.X;
				bool flag2 = (lockPositionAxis & Axis.Y) == Axis.Y;
				bool flag3 = (lockPositionAxis & Axis.Z) == Axis.Z;
				Vector3 vector6 = typedStartVal;
				for (int k = 0; k < num3; k++)
				{
					Vector3 vector7 = array[k];
					array[k] = new Vector3(flag ? vector6.x : vector7.x, flag2 ? vector6.y : vector7.y, flag3 ? vector6.z : vector7.z);
				}
			}
			path = new Path(pathType, array);
			path.StoreTimeToLenTables(path.path.Length * 16);
			if (!isClosedPath)
			{
				diffChangeVal = array[num3 - 2] - array[1];
			}
		}

		protected override void SetIncremental(int p_diffIncr)
		{
			if (!isClosedPath)
			{
				Vector3[] array = path.path;
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					array[i] += diffChangeVal * p_diffIncr;
				}
				path.changed = true;
			}
		}

		protected override void DoUpdate(float p_totElapsed)
		{
			pathPerc = ease(p_totElapsed, startPerc, changePerc, _duration, tweenObj.easeOvershootOrAmplitude, tweenObj.easePeriod);
			int out_waypointIndex;
			Vector3 constPointOnPath = GetConstPointOnPath(pathPerc, true, path, out out_waypointIndex);
			SetValue(constPointOnPath);
			if (orientType == OrientType.None || !(orientTrans != null) || orientTrans.Equals(null))
			{
				return;
			}
			Transform transform = (usesLocalPosition ? orientTrans.parent : null);
			switch (orientType)
			{
			case OrientType.LookAtPosition:
				orientTrans.LookAt(lookPos, Vector3.up);
				break;
			case OrientType.LookAtTransform:
				if (orientTrans != null && !orientTrans.Equals(null))
				{
					orientTrans.LookAt(lookTrans.position, Vector3.up);
				}
				break;
			case OrientType.ToPath:
			{
				Vector3 vector;
				if (pathType == PathType.Linear && lookAheadVal <= 0.0001f)
				{
					vector = constPointOnPath + path.path[out_waypointIndex] - path.path[out_waypointIndex - 1];
				}
				else
				{
					float num = pathPerc + lookAheadVal;
					if (num > 1f)
					{
						num = (isClosedPath ? (num - 1f) : 1.000001f);
					}
					vector = path.GetPoint(num);
				}
				Vector3 worldUp = orientTrans.up;
				if (usesLocalPosition && transform != null)
				{
					vector = transform.TransformPoint(vector);
				}
				if (lockRotationAxis != 0 && orientTrans != null)
				{
					if ((lockRotationAxis & Axis.X) == Axis.X)
					{
						Vector3 position = orientTrans.InverseTransformPoint(vector);
						position.y = 0f;
						vector = orientTrans.TransformPoint(position);
						worldUp = ((usesLocalPosition && transform != null) ? transform.up : Vector3.up);
					}
					if ((lockRotationAxis & Axis.Y) == Axis.Y)
					{
						Vector3 position2 = orientTrans.InverseTransformPoint(vector);
						if (position2.z < 0f)
						{
							position2.z = 0f - position2.z;
						}
						position2.x = 0f;
						vector = orientTrans.TransformPoint(position2);
					}
					if ((lockRotationAxis & Axis.Z) == Axis.Z)
					{
						worldUp = ((usesLocalPosition && transform != null) ? transform.up : Vector3.up);
					}
				}
				orientTrans.LookAt(vector, worldUp);
				break;
			}
			}
		}

		internal override void Rewind()
		{
			if (isPartialPath)
			{
				DoUpdate(0f);
			}
			else
			{
				base.Rewind();
			}
		}

		internal override void Complete()
		{
			if (isPartialPath)
			{
				DoUpdate(_duration);
			}
			else
			{
				base.Complete();
			}
		}

		internal Vector3 GetConstPointOnPath(float t)
		{
			int out_waypointIndex;
			return GetConstPointOnPath(t, false, null, out out_waypointIndex);
		}

		internal Vector3 GetConstPointOnPath(float t, bool p_updatePathPerc, Path p_path, out int out_waypointIndex)
		{
			if (p_updatePathPerc)
			{
				return p_path.GetConstPoint(t, out pathPerc, out out_waypointIndex);
			}
			out_waypointIndex = -1;
			return path.GetConstPoint(t);
		}

		internal float GetWaypointsLengthPercentage(int p_pathWaypointId0, int p_pathWaypointId1)
		{
			if (pathType == PathType.Linear)
			{
				if (path.waypointsLength == null)
				{
					path.StoreWaypointsLengths(16);
				}
				return path.timesTable[p_pathWaypointId1] - path.timesTable[p_pathWaypointId0];
			}
			if (path.waypointsLength == null)
			{
				path.StoreWaypointsLengths(16);
			}
			float num = 0f;
			for (int i = p_pathWaypointId0; i < p_pathWaypointId1; i++)
			{
				num += path.waypointsLength[i];
			}
			float num2 = num / path.pathLength;
			if (num2 > 1f)
			{
				num2 = 1f;
			}
			return num2;
		}

		private bool IsWaypoint(Vector3 position, out int waypointIndex)
		{
			int num = path.path.Length;
			for (int i = 0; i < num; i++)
			{
				float num2 = path.path[i].x - position.x;
				float num3 = path.path[i].y - position.y;
				float num4 = path.path[i].z - position.z;
				if (num2 < 0f)
				{
					num2 = 0f;
				}
				if (num3 < 0f)
				{
					num3 = 0f;
				}
				if (num4 < 0f)
				{
					num4 = 0f;
				}
				if (num2 < 0.001f && num3 < 0.001f && num4 < 0.001f)
				{
					waypointIndex = i;
					return true;
				}
			}
			waypointIndex = -1;
			return false;
		}

		internal void SwitchToPartialPath(float p_duration, EaseType p_easeType, float p_partialStartPerc, float p_partialChangePerc)
		{
			isPartialPath = true;
			_duration = p_duration;
			SetEase(p_easeType);
			startPerc = p_partialStartPerc;
			changePerc = p_partialChangePerc;
		}

		internal void ResetToFullPath(float p_duration, EaseType p_easeType)
		{
			isPartialPath = false;
			_duration = p_duration;
			SetEase(p_easeType);
			startPerc = 0f;
			changePerc = 1f;
		}
	}
}
