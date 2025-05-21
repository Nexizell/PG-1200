using System;
using System.Globalization;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public struct CapeMemento : IEquatable<CapeMemento>
	{
		[SerializeField]
		public long id;

		[SerializeField]
		public string cape;

		private int? _capeHashCode;

		public long Id
		{
			get
			{
				return id;
			}
		}

		public string Cape
		{
			get
			{
				return cape ?? string.Empty;
			}
		}

		public CapeMemento(long id, string cape)
		{
			this.id = id;
			this.cape = cape ?? string.Empty;
			_capeHashCode = null;
		}

		public bool Equals(CapeMemento other)
		{
			if (Id != other.Id)
			{
				return false;
			}
			if (GetCapeHashCode() != other.GetCapeHashCode())
			{
				return false;
			}
			if (Cape != other.Cape)
			{
				return false;
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is SkinMemento))
			{
				return false;
			}
			SkinMemento skinMemento = (SkinMemento)obj;
			return Equals(skinMemento);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode() ^ GetCapeHashCode();
		}

		public override string ToString()
		{
			string text = ((Cape.Length <= 4) ? Cape : Cape.Substring(Cape.Length - 4));
			return string.Format(CultureInfo.InvariantCulture, "{{ \"id\":{0},\"cape\":\"{1}\" }}", Id, text);
		}

		internal static CapeMemento ChooseCape(CapeMemento left, CapeMemento right)
		{
			if (string.IsNullOrEmpty(left.Cape) && string.IsNullOrEmpty(right.Cape))
			{
				if (left.Id > right.Id)
				{
					return left;
				}
				return right;
			}
			if (!string.IsNullOrEmpty(left.Cape) && !string.IsNullOrEmpty(right.Cape))
			{
				if (left.Id > right.Id)
				{
					return left;
				}
				return right;
			}
			if (!string.IsNullOrEmpty(left.Cape))
			{
				return left;
			}
			return right;
		}

		private int GetCapeHashCode()
		{
			if (!_capeHashCode.HasValue)
			{
				_capeHashCode = Cape.GetHashCode();
			}
			return _capeHashCode.Value;
		}
	}
}
