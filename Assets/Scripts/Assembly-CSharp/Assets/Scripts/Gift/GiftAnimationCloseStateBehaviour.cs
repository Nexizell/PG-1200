using UnityEngine;

namespace Assets.Scripts.Gift
{
	public class GiftAnimationCloseStateBehaviour : StateMachineBehaviour
	{
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			animator.SetBool("INTERNAL_InGachaSM", false);
			if (ButOpenGift.instance != null)
			{
				ButOpenGift.instance.CloseGift();
			}
		}
	}
}
