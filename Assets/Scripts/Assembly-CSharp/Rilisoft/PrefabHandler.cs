using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public class PrefabHandler 
	{
		[SerializeField]
		public string FullPath;

		public GameObject _prefab;

		public string ResourcePath
		{
			get
			{
				return ToResourcePath(FullPath);
			}
		}

		public GameObject Prefab
		{
			get
			{
				if (_prefab == null)
				{
					_prefab = Resources.Load<GameObject>(ResourcePath);
				}
				return _prefab;
			}
		}

		public bool PrefabIsLoaded
		{
			get
			{
				return _prefab != null;
			}
		}

		public static string ToResourcePath(string fullPath)
		{
			if (fullPath.IsNullOrEmpty())
			{
				return string.Empty;
			}
			List<string> list = fullPath.Split(fullPath.Contains("/") ? '/' : '\\').ToList();
			if (list.Count > 0)
			{
				int num = -1;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].Equals("Resources"))
					{
						num = i;
						break;
					}
				}
				if (num >= 0)
				{
					list.RemoveRange(0, num + 1);
				}
			}
			fullPath = string.Join("\\", list.ToArray());
			fullPath = Path.Combine(Path.GetDirectoryName(fullPath), Path.GetFileNameWithoutExtension(fullPath));
			return fullPath;
		}
	}
}
