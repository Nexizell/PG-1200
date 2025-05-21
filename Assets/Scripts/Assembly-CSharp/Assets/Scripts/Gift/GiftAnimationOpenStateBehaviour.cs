using UnityEngine;

namespace Assets.Scripts.Gift
{
	public class GiftAnimationOpenStateBehaviour : StateMachineBehaviour
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			animator.SetBool("INTERNAL_InGachaSM", true);
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			if (GiftBannerWindow.instance != null)
			{
				GiftBannerWindow.instance.OpenBannerWindow();
			}
		}
	}
}
