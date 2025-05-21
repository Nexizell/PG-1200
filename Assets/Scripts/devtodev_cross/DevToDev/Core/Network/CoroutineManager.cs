using System.Collections.Generic;

namespace DevToDev.Core.Network
{
	internal class CoroutineManager
	{
		private List<IEnumerator<Status>> Coroutines = new List<IEnumerator<Status>>();

		public void StartCoroutine(IEnumerator<Status> func)
		{
			Coroutines.Add(func);
		}

		public void Update()
		{
			foreach (IEnumerator<Status> coroutine in Coroutines)
			{
				coroutine.MoveNext();
			}
		}
	}
}
