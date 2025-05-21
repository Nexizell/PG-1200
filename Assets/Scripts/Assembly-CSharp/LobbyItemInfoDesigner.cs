using UnityEngine;

public class LobbyItemInfoDesigner : MonoBehaviour
{
	public string Lkey;

	public string EffectLocKey;

	public LobbyItemInfo.LobbyItemSlot slot;

	public Material[] materials;

	public LobbyItemInfo.LobbyItemBuff[] Buffs = new LobbyItemInfo.LobbyItemBuff[0];
}
