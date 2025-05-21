using UnityEngine;

namespace DevToDev.Core.Utils
{
	internal class LocationUtils
	{
		private float[] location;

		private static LocationUtils _instance;

		internal static LocationUtils Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new LocationUtils();
				}
				return _instance;
			}
		}

		public float[] Location
		{
			get
			{
				return location;
			}
		}

		public LocationUtils()
		{
			if (Input.location.isEnabledByUser)
			{
				Input.location.Start();
			}
		}

		public void UpdateGPSLocation()
		{
			if (Input.location.isEnabledByUser && Input.location.status == LocationServiceStatus.Running)
			{
				location = new float[2]
				{
					Input.location.lastData.latitude,
					Input.location.lastData.longitude
				};
			}
		}
	}
}
