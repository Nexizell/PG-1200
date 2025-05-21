using System;
using UnityEngine;

namespace Holoville.HOTween.Core
{
	internal class Path
	{
		public float pathLength;

		public float[] waypointsLength;

		public float[] timesTable;

		private float[] lengthsTable;

		internal Vector3[] path;

		internal bool changed;

		private Vector3[] drawPs;

		private PathType pathType;

		public Path(PathType p_type, params Vector3[] p_path)
		{
			pathType = p_type;
			path = new Vector3[p_path.Length];
			Array.Copy(p_path, path, path.Length);
		}

		public Vector3 GetPoint(float t)
		{
			int out_waypointIndex;
			return GetPoint(t, out out_waypointIndex);
		}

		internal Vector3 GetPoint(float t, out int out_waypointIndex)
		{
			if (pathType == PathType.Linear)
			{
				if (t <= 0f)
				{
					out_waypointIndex = 1;
					return path[1];
				}
				int num = 0;
				int num2 = 0;
				int num3 = timesTable.Length;
				for (int i = 1; i < num3; i++)
				{
					if (timesTable[i] >= t)
					{
						num = i - 1;
						num2 = i;
						break;
					}
				}
				float num4 = timesTable[num];
				float num5 = timesTable[num2] - timesTable[num];
				num5 = t - num4;
				float maxLength = pathLength * num5;
				Vector3 vector = path[num];
				Vector3 vector2 = path[num2];
				out_waypointIndex = num2;
				return vector + Vector3.ClampMagnitude(vector2 - vector, maxLength);
			}
			int num6 = path.Length - 3;
			int num7 = (int)Math.Floor(t * (float)num6);
			int num8 = num6 - 1;
			if (num8 > num7)
			{
				num8 = num7;
			}
			float num9 = t * (float)num6 - (float)num8;
			Vector3 vector3 = path[num8];
			Vector3 vector4 = path[num8 + 1];
			Vector3 vector5 = path[num8 + 2];
			Vector3 vector6 = path[num8 + 3];
			out_waypointIndex = -1;
			return 0.5f * ((-vector3 + 3f * vector4 - 3f * vector5 + vector6) * (num9 * num9 * num9) + (2f * vector3 - 5f * vector4 + 4f * vector5 - vector6) * (num9 * num9) + (-vector3 + vector5) * num9 + 2f * vector4);
		}

		public Vector3 Velocity(float t)
		{
			int num = path.Length - 3;
			int num2 = (int)Math.Floor(t * (float)num);
			int num3 = num - 1;
			if (num3 > num2)
			{
				num3 = num2;
			}
			float num4 = t * (float)num - (float)num3;
			Vector3 vector = path[num3];
			Vector3 vector2 = path[num3 + 1];
			Vector3 vector3 = path[num3 + 2];
			Vector3 vector4 = path[num3 + 3];
			return 1.5f * (-vector + 3f * vector2 - 3f * vector3 + vector4) * (num4 * num4) + (2f * vector - 5f * vector2 + 4f * vector3 - vector4) * num4 + 0.5f * vector3 - 0.5f * vector;
		}

		public void GizmoDraw()
		{
			GizmoDraw(-1f, false);
		}

		public void GizmoDraw(float t, bool p_drawTrig)
		{
			Gizmos.color = new Color(0.6f, 0.6f, 0.6f, 0.6f);
			if (changed || (pathType == PathType.Curved && drawPs == null))
			{
				changed = false;
				if (pathType == PathType.Curved)
				{
					int num = path.Length * 10;
					drawPs = new Vector3[num + 1];
					for (int i = 0; i <= num; i++)
					{
						float t2 = (float)i / (float)num;
						Vector3 point = GetPoint(t2);
						drawPs[i] = point;
					}
				}
			}
			if (pathType == PathType.Linear)
			{
				Vector3 to = path[1];
				int num2 = path.Length;
				for (int j = 1; j < num2 - 1; j++)
				{
					Vector3 point = path[j];
					Gizmos.DrawLine(point, to);
					to = point;
				}
			}
			else
			{
				Vector3 to = drawPs[0];
				int num3 = drawPs.Length;
				for (int k = 1; k < num3; k++)
				{
					Vector3 point = drawPs[k];
					Gizmos.DrawLine(point, to);
					to = point;
				}
			}
			Gizmos.color = Color.white;
			int num4 = path.Length - 1;
			for (int l = 1; l < num4; l++)
			{
				Gizmos.DrawSphere(path[l], 0.1f);
			}
			if (!p_drawTrig || t == -1f)
			{
				return;
			}
			Vector3 point2 = GetPoint(t);
			Vector3 vector = point2;
			float num5 = t + 0.0001f;
			Vector3 vector2;
			Vector3 vector3;
			if (num5 > 1f)
			{
				vector2 = point2;
				vector = GetPoint(t - 0.0001f);
				vector3 = GetPoint(t - 0.0002f);
			}
			else
			{
				float num6 = t - 0.0001f;
				if (num6 < 0f)
				{
					vector3 = point2;
					vector = GetPoint(t + 0.0001f);
					vector2 = GetPoint(t + 0.0002f);
				}
				else
				{
					vector3 = GetPoint(num6);
					vector2 = GetPoint(num5);
				}
			}
			Vector3 vector4 = vector2 - vector;
			vector4.Normalize();
			Vector3 rhs = vector - vector3;
			rhs.Normalize();
			Vector3 vector5 = Vector3.Cross(vector4, rhs);
			vector5.Normalize();
			Vector3 vector6 = Vector3.Cross(vector4, vector5);
			vector6.Normalize();
			Gizmos.color = Color.black;
			Gizmos.DrawLine(point2, point2 + vector4);
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(point2, point2 + vector5);
			Gizmos.color = Color.red;
			Gizmos.DrawLine(point2, point2 + vector6);
		}

		internal Vector3 GetConstPoint(float t)
		{
			if (pathType == PathType.Linear)
			{
				return GetPoint(t);
			}
			float constPathPercFromTimePerc = GetConstPathPercFromTimePerc(t);
			return GetPoint(constPathPercFromTimePerc);
		}

		internal Vector3 GetConstPoint(float t, out float out_pathPerc, out int out_waypointIndex)
		{
			if (pathType == PathType.Linear)
			{
				out_pathPerc = t;
				return GetPoint(t, out out_waypointIndex);
			}
			float t2 = (out_pathPerc = GetConstPathPercFromTimePerc(t));
			out_waypointIndex = -1;
			return GetPoint(t2);
		}

		internal void StoreTimeToLenTables(int p_subdivisions)
		{
			if (pathType == PathType.Linear)
			{
				pathLength = 0f;
				int num = path.Length;
				waypointsLength = new float[num];
				Vector3 b = path[1];
				for (int i = 1; i < num; i++)
				{
					Vector3 vector = path[i];
					float num2 = Vector3.Distance(vector, b);
					if (i < num - 1)
					{
						pathLength += num2;
					}
					b = vector;
					waypointsLength[i] = num2;
				}
				timesTable = new float[num];
				float num3 = 0f;
				for (int j = 2; j < num; j++)
				{
					num3 += waypointsLength[j];
					timesTable[j] = num3 / pathLength;
				}
			}
			else
			{
				pathLength = 0f;
				float num4 = 1f / (float)p_subdivisions;
				timesTable = new float[p_subdivisions];
				lengthsTable = new float[p_subdivisions];
				Vector3 b = GetPoint(0f);
				for (int k = 1; k < p_subdivisions + 1; k++)
				{
					float num5 = num4 * (float)k;
					Vector3 vector = GetPoint(num5);
					pathLength += Vector3.Distance(vector, b);
					b = vector;
					timesTable[k - 1] = num5;
					lengthsTable[k - 1] = pathLength;
				}
			}
		}

		internal void StoreWaypointsLengths(int p_subdivisions)
		{
			int num = this.path.Length - 2;
			waypointsLength = new float[num];
			waypointsLength[0] = 0f;
			Path path = null;
			for (int i = 2; i < num + 1; i++)
			{
				Vector3[] p_path = new Vector3[4]
				{
					this.path[i - 2],
					this.path[i - 1],
					this.path[i],
					this.path[i + 1]
				};
				if (i == 2)
				{
					path = new Path(pathType, p_path);
				}
				else
				{
					path.path = p_path;
				}
				float num2 = 0f;
				float num3 = 1f / (float)p_subdivisions;
				Vector3 b = path.GetPoint(0f);
				for (int j = 1; j < p_subdivisions + 1; j++)
				{
					float t = num3 * (float)j;
					Vector3 point = path.GetPoint(t);
					num2 += Vector3.Distance(point, b);
					b = point;
				}
				waypointsLength[i - 1] = num2;
			}
		}

		private float GetConstPathPercFromTimePerc(float t)
		{
			if (t > 0f && t < 1f)
			{
				float num = pathLength * t;
				float num2 = 0f;
				float num3 = 0f;
				float num4 = 0f;
				float num5 = 0f;
				int num6 = lengthsTable.Length;
				for (int i = 0; i < num6; i++)
				{
					if (lengthsTable[i] > num)
					{
						num4 = timesTable[i];
						num5 = lengthsTable[i];
						if (i > 0)
						{
							num3 = lengthsTable[i - 1];
						}
						break;
					}
					num2 = timesTable[i];
				}
				t = num2 + (num - num3) / (num5 - num3) * (num4 - num2);
			}
			if (t > 1f)
			{
				t = 1f;
			}
			else if (t < 0f)
			{
				t = 0f;
			}
			return t;
		}
	}
}
