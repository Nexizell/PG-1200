using System;
using UnityEngine;

namespace Rilisoft
{
	[Serializable]
	public class PlayerStepsSoundsData 
	{
		public AudioClip Walk;

		public AudioClip Jump;

		public AudioClip JumpDown;

		public AudioClip WalkMech;

		public AudioClip WalkMechBear;

		public void SetTo(SkinName data)
		{
			data.walkAudio = Walk;
			data.jumpAudio = Jump;
			data.jumpDownAudio = JumpDown;
			data.walkMech = WalkMech;
			data.walkMechBear = WalkMechBear;
		}

		public bool IsSettedTo(SkinName data)
		{
			if (data.walkAudio == Walk && data.jumpAudio == Jump && data.jumpDownAudio == JumpDown && data.walkMech == WalkMech)
			{
				return data.walkMechBear == WalkMechBear;
			}
			return false;
		}

		public static PlayerStepsSoundsData Create(SkinName data)
		{
			return new PlayerStepsSoundsData
			{
				Walk = data.walkAudio,
				Jump = data.jumpAudio,
				JumpDown = data.jumpDownAudio,
				WalkMech = data.walkMech,
				WalkMechBear = data.walkMechBear
			};
		}
	}
}
