using System;
using System.Collections;
using UnityEngine;

namespace Rilisoft
{
	internal sealed class ArrayListChecker : IDisposable
	{
		private ArrayList _arrayList;

		private bool _disposed;

		private string _label;

		private const int CapacityThreshold = 1000;

		private const int CountThreshold = 50;

		public ArrayListChecker(ArrayList arrayList, string label)
		{
			_arrayList = arrayList;
			_label = label ?? string.Empty;
			CheckOverflowIfDebug();
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				CheckOverflowIfDebug();
				_disposed = true;
			}
		}

		private void CheckOverflowIfDebug()
		{
			if (Debug.isDebugBuild)
			{
				if (_arrayList == null)
				{
					Debug.LogWarning(_label + ": ArrayList is null.");
				}
				else if (_arrayList.Count > 50 || _arrayList.Capacity > 1000)
				{
					HandleOverflow();
				}
			}
		}

		private void HandleOverflow()
		{
			Debug.LogWarning(string.Format("{0}: Count: {1}, Capacity: {2}", new object[3] { _label, _arrayList.Count, _arrayList.Capacity }));
		}
	}
}
