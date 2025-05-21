using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Linq.Expressions
{
	public abstract class IndexExpression : Expression
	{
		public abstract Collection<Expression> Arguments { get; }

		public abstract bool CanReduce { get; }

		public abstract PropertyInfo Indexer { get; }

		public abstract Expression Object { get; }

		public IndexExpression()
			: base(ExpressionType.ArrayIndex, typeof(object))
		{
		}
	}
}
