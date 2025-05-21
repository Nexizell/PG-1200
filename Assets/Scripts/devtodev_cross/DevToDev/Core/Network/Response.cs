namespace DevToDev.Core.Network
{
	internal class Response
	{
		private readonly string _resposeString;

		private readonly int _statusCode;

		private readonly ResponseState _responseState;

		private readonly string _redirectUrl;

		public string RedirectUrl
		{
			get
			{
				return _redirectUrl;
			}
		}

		public string ResposeString
		{
			get
			{
				return _resposeString;
			}
		}

		public int StatusCode
		{
			get
			{
				return _statusCode;
			}
		}

		public ResponseState ResponseState
		{
			get
			{
				return _responseState;
			}
		}

		public Response(string resposeString, int statusCode, ResponseState responseState, string redirectUrl = null)
		{
			_resposeString = resposeString;
			_statusCode = statusCode;
			_responseState = responseState;
			_redirectUrl = redirectUrl;
		}

		public static Response BadResponse()
		{
			return new Response(string.Empty, -1, ResponseState.Failure);
		}
	}
}
