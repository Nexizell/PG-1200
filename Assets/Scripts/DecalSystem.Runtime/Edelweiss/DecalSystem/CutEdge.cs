using System;

namespace Edelweiss.DecalSystem
{
	internal struct CutEdge : IComparable<CutEdge>
	{
		public int vertex1Index;

		public int vertex2Index;

		public int newVertex1Index;

		public int newVertex2Index;

		public int SmallerIndex
		{
			get
			{
				int num = vertex1Index;
				if (vertex2Index < num)
				{
					num = vertex2Index;
				}
				return num;
			}
		}

		public int GreaterIndex
		{
			get
			{
				int num = vertex1Index;
				if (vertex2Index > num)
				{
					num = vertex2Index;
				}
				return num;
			}
		}

		public int ModifiedIndex
		{
			get
			{
				if (vertex1Index == newVertex1Index)
				{
					return newVertex2Index;
				}
				return newVertex1Index;
			}
		}

		public CutEdge(int a_Vertex1Index, int a_Vertex2Index)
		{
			vertex1Index = a_Vertex1Index;
			vertex2Index = a_Vertex2Index;
			newVertex1Index = vertex1Index;
			newVertex2Index = vertex2Index;
		}

		public int CompareTo(CutEdge a_Other)
		{
			int num = SmallerIndex.CompareTo(a_Other.SmallerIndex);
			if (num == 0)
			{
				num = GreaterIndex.CompareTo(a_Other.GreaterIndex);
			}
			return num;
		}
	}
}
