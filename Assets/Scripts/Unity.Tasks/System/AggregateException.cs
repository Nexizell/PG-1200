using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace System
{
	public class AggregateException : Exception
	{
		public ReadOnlyCollection<Exception> InnerExceptions { get; private set; }

		public AggregateException(IEnumerable<Exception> innerExceptions)
		{
			InnerExceptions = new ReadOnlyCollection<Exception>(innerExceptions.ToList());
		}

		public AggregateException Flatten()
		{
			List<Exception> list = new List<Exception>();
			foreach (Exception innerException in InnerExceptions)
			{
				AggregateException ex = innerException as AggregateException;
				if (ex != null)
				{
					list.AddRange(ex.Flatten().InnerExceptions);
				}
				else
				{
					list.Add(innerException);
				}
			}
			return new AggregateException(list);
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.ToString());
			foreach (Exception innerException in InnerExceptions)
			{
				stringBuilder.AppendLine("\n-----------------");
				stringBuilder.AppendLine(innerException.ToString());
			}
			return stringBuilder.ToString();
		}
	}
}
