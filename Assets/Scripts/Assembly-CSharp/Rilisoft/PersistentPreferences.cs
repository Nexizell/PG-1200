using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Rilisoft
{
	internal sealed class PersistentPreferences : Preferences
	{
		private readonly XDocument _doc;

		private static readonly string _path;

		private const string KeyElement = "Key";

		private const string PreferenceElement = "Preference";

		private const string RootElement = "Preferences";

		private const string ValueElement = "Value";

		public override ICollection<string> Keys
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override ICollection<string> Values
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override int Count
		{
			get
			{
				return _doc.Root.Elements().Count();
			}
		}

		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		internal static string Path
		{
			get
			{
				return _path;
			}
		}

		static PersistentPreferences()
		{
			_path = "com.P3D.Pixlgun.PersistentPreferences.xml";
		}

		public PersistentPreferences()
		{
			try
			{
				_doc = XDocument.Load(_path);
			}
			catch (Exception)
			{
				_doc = new XDocument(new XElement("Preferences"));
			}
		}

		protected override void AddCore(string key, string value)
		{
			XElement xElement = _doc.Root.Elements("Preference").FirstOrDefault((XElement e) => e.Element("Key") != null && e.Element("Key").Value.Equals(key));
			if (xElement != null)
			{
				xElement.Remove();
			}
			XElement content = new XElement("Preference", new XElement("Key", key), new XElement("Value", value));
			_doc.Root.Add(content);
		}

		protected override bool ContainsKeyCore(string key)
		{
			return _doc.Root.Elements("Preference").Any((XElement e) => e.Element("Key") != null && e.Element("Key").Value.Equals(key));
		}

		protected override void CopyToCore(KeyValuePair<string, string>[] array, int arrayIndex)
		{
			throw new NotSupportedException();
		}

		protected override bool RemoveCore(string key)
		{
			XElement xElement = _doc.Root.Elements("Preference").FirstOrDefault((XElement e) => e.Element("Key") != null && e.Element("Key").Value.Equals(key));
			if (xElement != null)
			{
				xElement.Remove();
				return true;
			}
			return false;
		}

		protected override bool TryGetValueCore(string key, out string value)
		{
			XElement xElement = _doc.Root.Elements("Preference").FirstOrDefault((XElement e) => e.Element("Key") != null && e.Element("Key").Value.Equals(key));
			if (xElement != null)
			{
				XElement xElement2 = xElement.Element("Value");
				if (xElement2 != null)
				{
					value = xElement2.Value;
					return true;
				}
			}
			value = null;
			return false;
		}

		public override void Save()
		{
		}

		public override void Clear()
		{
			_doc.Root.RemoveNodes();
		}

		public override IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			throw new NotSupportedException();
		}
	}
}
