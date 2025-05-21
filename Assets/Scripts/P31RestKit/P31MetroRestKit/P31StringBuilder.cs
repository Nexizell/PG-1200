using System;
using System.Runtime.InteropServices;

namespace P31MetroRestKit
{
	public class P31StringBuilder
	{
		private string _holder;

		public P31StringBuilder()
		{
			_holder = string.Empty;
		}

		public void Append(string str)
		{
			_holder += str;
		}

		public void Append(object obj)
		{
			_holder += obj.ToString();
		}

		public override string ToString()
		{
			return _holder;
		}

		protected internal P31StringBuilder(UIntPtr P_0)
		{
		}

		protected internal unsafe P31StringBuilder(UIntPtr P_0, long* P_1)
		{
		}

		public unsafe static long _0024Invoke0(long instance, long* args)
		{
			return -1L;
		}
	}
}
