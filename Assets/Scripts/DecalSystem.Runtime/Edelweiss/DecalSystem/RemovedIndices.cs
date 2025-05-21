using System.Collections.Generic;

namespace Edelweiss.DecalSystem
{
	internal class RemovedIndices
	{
		private List<int> m_RemovedIndices = new List<int>();

		private int m_GreatestUnremovedValue = -1;

		public int Count
		{
			get
			{
				return m_RemovedIndices.Count;
			}
		}

		public void Clear()
		{
			m_RemovedIndices.Clear();
			m_GreatestUnremovedValue = -1;
		}

		public void AddRemovedIndex(int a_Index)
		{
			if (a_Index < Count)
			{
				m_RemovedIndices[a_Index] = -1;
				m_GreatestUnremovedValue = -1;
				for (int i = a_Index + 1; i < Count; i++)
				{
					int num = m_RemovedIndices[i] - 1;
					if (num >= 0)
					{
						m_GreatestUnremovedValue = num;
					}
					m_RemovedIndices[i] = num;
				}
			}
			else
			{
				int num2 = m_GreatestUnremovedValue + 1;
				for (int j = Count; j < a_Index; j++)
				{
					m_RemovedIndices.Add(num2);
					m_GreatestUnremovedValue = num2;
					num2++;
				}
				m_RemovedIndices.Add(-1);
			}
		}

		public bool IsRemovedIndex(int a_Index)
		{
			if (a_Index >= Count)
			{
				return false;
			}
			return m_RemovedIndices[a_Index] < 0;
		}

		public int AdjustedIndex(int a_Index)
		{
			if (a_Index >= Count)
			{
				return m_GreatestUnremovedValue + a_Index - Count + 1;
			}
			return m_RemovedIndices[a_Index];
		}
	}
}
