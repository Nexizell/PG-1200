using System;
using System.Text;

namespace Rilisoft
{
	public class MailUrlBuilder
	{
		public string to;

		public string subject;

		public string body;

		public string GetUrl()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("mailto:{0}", new object[1] { to });
			string text = Uri.EscapeUriString(subject);
			stringBuilder.AppendFormat("?subject={0}", new object[1] { text });
			string text2 = Uri.EscapeUriString(body);
			stringBuilder.AppendFormat("&body={0}", new object[1] { text2 });
			return stringBuilder.ToString();
		}
	}
}
