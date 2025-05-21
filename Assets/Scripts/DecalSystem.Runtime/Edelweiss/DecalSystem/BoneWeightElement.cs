using System;

namespace Edelweiss.DecalSystem
{
	internal struct BoneWeightElement : IComparable<BoneWeightElement>
	{
		public int index;

		public float weight;

		public int CompareTo(BoneWeightElement a_Other)
		{
			return -weight.CompareTo(a_Other.weight);
		}
	}
}
