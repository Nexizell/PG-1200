using UnityEngine;

namespace Rilisoft
{
	public class LobbyItemZoomOutStateBehaviour : StateMachineBehaviour
	{
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateEnter(animator, stateInfo, layerIndex);
			animator.SetBool("INTERNAL_InItemsSM", false);
			if (LobbyCraftController.Instance != null)
			{
				LobbyCraftController.Instance.OnZoomOut();
			}
		}
	}
}
