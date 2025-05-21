using System;

namespace LitJson
{
	public class JsonException : ApplicationException
	{
		public JsonException()
		{
		}

		internal JsonException(ParserToken token)
			: base(string.Format("Invalid token '{0}' in input string", new object[1] { token }))
		{
		}

		internal JsonException(ParserToken token, Exception inner_exception)
			: base(string.Format("Invalid token '{0}' in input string", new object[1] { token }), inner_exception)
		{
		}

		internal JsonException(int c)
			: base(string.Format("Invalid character '{0}' in input string", new object[1] { (char)c }))
		{
		}

		internal JsonException(int c, Exception inner_exception)
			: base(string.Format("Invalid character '{0}' in input string", new object[1] { (char)c }), inner_exception)
		{
		}

		public JsonException(string message)
			: base(message)
		{
		}

		public JsonException(string message, Exception inner_exception)
			: base(message, inner_exception)
		{
		}
	}
}
