using UnityEngine;

namespace Rilisoft
{
	public class LobbyItemZoomInStateBehaviour : StateMachineBehaviour
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			animator.SetBool("INTERNAL_InItemsSM", true);
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			if (LobbyCraftController.Instance != null)
			{
				LobbyCraftController.Instance.OnZoomIn();
			}
		}
	}
}
