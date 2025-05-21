using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	public class NewsHandler : ScriptableObject
	{
		public List<string> ExistsIds = new List<string>();

		private static NewsHandler _instance;

		public static List<string> News
		{
			get
			{
				if (_instance == null)
				{
					_instance = Resources.Load<NewsHandler>("news_handler");
				}
				if (!(_instance != null))
				{
					return null;
				}
				return _instance.ExistsIds;
			}
		}
	}
}
