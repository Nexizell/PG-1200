using System.Collections.Generic;
using DevToDev.Logic;

namespace DevToDev
{
	public class People
	{
		public Gender Gender
		{
			set
			{
				SDKClient.Instance.Execute(delegate
				{
					SDKClient.Instance.UsersStorage.ActivePeople.Gender = value;
				});
			}
		}

		public int Age
		{
			set
			{
				SDKClient.Instance.Execute(delegate
				{
					SDKClient.Instance.UsersStorage.ActivePeople.Age = value;
				});
			}
		}

		public bool Cheater
		{
			set
			{
				SDKClient.Instance.Execute(delegate
				{
					SDKClient.Instance.UsersStorage.ActivePeople.Cheater = value;
				});
			}
		}

		public string Name
		{
			set
			{
				SDKClient.Instance.Execute(delegate
				{
					SDKClient.Instance.UsersStorage.ActivePeople.Name = value;
				});
			}
		}

		public string Email
		{
			set
			{
				SDKClient.Instance.Execute(delegate
				{
					SDKClient.Instance.UsersStorage.ActivePeople.Email = value;
				});
			}
		}

		public string Phone
		{
			set
			{
				SDKClient.Instance.Execute(delegate
				{
					SDKClient.Instance.UsersStorage.ActivePeople.Phone = value;
				});
			}
		}

		public string Photo
		{
			set
			{
				SDKClient.Instance.Execute(delegate
				{
					SDKClient.Instance.UsersStorage.ActivePeople.Photo = value;
				});
			}
		}

		internal People()
		{
		}

		public void SetUserData(string key, object value)
		{
			SDKClient.Instance.Execute(delegate
			{
				SDKClient.Instance.UsersStorage.ActivePeople.SetUserData(key, value);
			});
		}

		public void SetUserData(Dictionary<string, object> userData)
		{
			SDKClient.Instance.Execute(delegate
			{
				SDKClient.Instance.UsersStorage.ActivePeople.SetUserData(userData);
			});
		}

		public void ClearUserData()
		{
			SDKClient.Instance.Execute(delegate
			{
				SDKClient.Instance.UsersStorage.ActivePeople.ClearUserData();
			});
		}

		public void UnsetUserData(string key)
		{
			SDKClient.Instance.Execute(delegate
			{
				SDKClient.Instance.UsersStorage.ActivePeople.UnsetUserData(key);
			});
		}

		public void UnsetUserData(List<string> keysToRemove)
		{
			SDKClient.Instance.Execute(delegate
			{
				SDKClient.Instance.UsersStorage.ActivePeople.UnsetUserData(keysToRemove);
			});
		}

		public void AppendUserData(string key, object value)
		{
			SDKClient.Instance.Execute(delegate
			{
				SDKClient.Instance.UsersStorage.ActivePeople.AppendUserData(key, value);
			});
		}

		public void AppendUserData(Dictionary<string, object> userData)
		{
			SDKClient.Instance.Execute(delegate
			{
				SDKClient.Instance.UsersStorage.ActivePeople.AppendUserData(userData);
			});
		}

		public void UnionUserData(string key, object value)
		{
			SDKClient.Instance.Execute(delegate
			{
				SDKClient.Instance.UsersStorage.ActivePeople.UnionUserData(key, value);
			});
		}

		public void UnionUserData(Dictionary<string, object> userData)
		{
			SDKClient.Instance.Execute(delegate
			{
				SDKClient.Instance.UsersStorage.ActivePeople.UnionUserData(userData);
			});
		}

		public void Increment(string key, object value)
		{
			SDKClient.Instance.Execute(delegate
			{
				SDKClient.Instance.UsersStorage.ActivePeople.Increment(key, value);
			});
		}

		public void Increment(Dictionary<string, object> userData)
		{
			SDKClient.Instance.Execute(delegate
			{
				SDKClient.Instance.UsersStorage.ActivePeople.Increment(userData);
			});
		}
	}
}
