using System;
using System.Text.RegularExpressions;

namespace DevToDev.Core.Utils.Validators
{
	internal class PhoneValidator : IValidator
	{
		public bool IsValid(string data)
		{
			try
			{
				return Regex.Match(data, "^(\\+[0-9]+?)$").Success;
			}
			catch (Exception)
			{
			}
			return false;
		}
	}
}
