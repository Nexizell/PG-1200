using System;

namespace DevToDev.Core.Utils.Validators
{
	internal class UrlValidator : IValidator
	{
		public bool IsValid(string data)
		{
			return Uri.IsWellFormedUriString(data, UriKind.Absolute);
		}
	}
}
