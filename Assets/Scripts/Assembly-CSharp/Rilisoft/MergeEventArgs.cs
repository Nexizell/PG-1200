using System;

namespace Rilisoft
{
	internal sealed class MergeEventArgs : EventArgs
	{
		private readonly string _merge;

		internal string Merge
		{
			get
			{
				return _merge;
			}
		}

		internal MergeEventArgs(string merge)
		{
			_merge = merge;
		}
	}
}
