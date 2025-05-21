using UnityEngine;

public class MirrorGadgetButton : MonoBehaviour
{
	public UIAnchor targetUiAnchor;

	private void Update()
	{
		base.transform.localScale = ((targetUiAnchor.side == UIAnchor.Side.BottomRight) ? Vector3.one : new Vector3(-1f, 1f, 1f));
	}
}
