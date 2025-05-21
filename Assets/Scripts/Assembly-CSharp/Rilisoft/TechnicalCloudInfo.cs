using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public class TechnicalCloudInfo 
	{
		public const string TECHNICAL_CLOUD_INFO_STORAGER_KEY = "TechnicalCloudInfo.TECHNICAL_CLOUD_INFO_STORAGER_KEY";

		[SerializeField]
		protected internal List<string> m_playerIds = new List<string>();

		[SerializeField]
		protected internal int _totalInapps;

		[SerializeField]
		protected internal int _sessionDayCount;

		[SerializeField]
		protected internal float _inGameSeconds;

		public List<string> PlayerIds
		{
			get
			{
				return m_playerIds;
			}
			set
			{
				m_playerIds = value;
			}
		}

		public int TotalInapps
		{
			get
			{
				return _totalInapps;
			}
			set
			{
				_totalInapps = value;
			}
		}

		public int SessionDayCount
		{
			get
			{
				return _sessionDayCount;
			}
			set
			{
				_sessionDayCount = value;
			}
		}

		public float InGameSeconds
		{
			get
			{
				return _inGameSeconds;
			}
			set
			{
				_inGameSeconds = value;
			}
		}
	}
}
