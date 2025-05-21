using System;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct NetworkViewID
	{
		private const string NotSupportedMessage = "NetworkViewID is not supported for Windows Phone 8.";

		public bool isMine
		{
			get
			{
				throw new NotSupportedException("NetworkViewID is not supported for Windows Phone 8.");
			}
		}

		public static NetworkViewID unassigned
		{
			get
			{
				throw new NotSupportedException("NetworkViewID is not supported for Windows Phone 8.");
			}
		}

		public static bool operator !=(NetworkViewID lhs, NetworkViewID rhs)
		{
			throw new NotSupportedException("NetworkViewID is not supported for Windows Phone 8.");
		}

		public static bool operator ==(NetworkViewID lhs, NetworkViewID rhs)
		{
			throw new NotSupportedException("NetworkViewID is not supported for Windows Phone 8.");
		}

		public override bool Equals(object other)
		{
			throw new NotSupportedException("NetworkViewID is not supported for Windows Phone 8.");
		}

		public override int GetHashCode()
		{
			throw new NotSupportedException("NetworkViewID is not supported for Windows Phone 8.");
		}

		public override string ToString()
		{
			throw new NotSupportedException("NetworkViewID is not supported for Windows Phone 8.");
		}
	}
}
