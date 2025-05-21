using System.Collections.Generic;

namespace DevToDev.Core.Utils
{
	internal class DeploymentHelper
	{
		private static DeploymentHelper _instance;

		internal static DeploymentHelper Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new DeploymentHelper();
				}
				return _instance;
			}
		}

		internal List<string> GetApplicationsList()
		{
			return null;
		}
	}
}
