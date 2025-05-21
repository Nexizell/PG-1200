using System;
using System.Collections.Generic;
using System.Text;
using DevToDev.Core.Network;

namespace DevToDev.Core.Utils.Builders
{
	internal class RequestBuilder
	{
		public static readonly string DEFAULT_SIGNATURE_LETTER = "s";

		private string url;

		private string secret;

		private SortedDictionary<string, string> parameters;

		private byte[] postData;

		private bool isNeedSigned;

		public RequestBuilder()
		{
			parameters = new SortedDictionary<string, string>();
		}

		public RequestBuilder Url(string url)
		{
			this.url = url;
			return this;
		}

		public RequestBuilder AddParameter(string key, object value)
		{
			parameters.Add(key, value.ToString());
			return this;
		}

		public RequestBuilder NeedSigned(bool isNeedSigned)
		{
			this.isNeedSigned = isNeedSigned;
			return this;
		}

		public RequestBuilder Secret(string secret)
		{
			this.secret = secret;
			return this;
		}

		public RequestBuilder PostData(byte[] postData)
		{
			this.postData = postData;
			return this;
		}

		public Request Build()
		{
			return new Request(CalculateUrl(), (postData != null) ? new GZipHelper().Pack(postData) : null);
		}

		private string CalculateUrl()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, string> parameter in parameters)
			{
				stringBuilder.AppendFormat("{0}={1}&", parameter.Key, Uri.EscapeDataString(parameter.Value));
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			if (isNeedSigned)
			{
				stringBuilder.AppendFormat("&{0}={1}", DEFAULT_SIGNATURE_LETTER, Sign());
			}
			return new StringBuilder(url).Append(stringBuilder).ToString();
		}

		private string Sign()
		{
			if (secret == null && secret.Equals(string.Empty))
			{
				Log.D("Sign required but secret key doesn't set");
			}
			List<byte> list = new List<byte>();
			foreach (KeyValuePair<string, string> parameter in parameters)
			{
				list.AddRange(Encoding.UTF8.GetBytes(string.Format("{0}={1}", parameter.Key, parameter.Value)));
			}
			if (postData != null)
			{
				list.AddRange(postData);
			}
			list.AddRange(Encoding.UTF8.GetBytes(secret));
			return MD5Helper.GetMd5(list.ToArray());
		}
	}
}
