namespace Rilisoft
{
	public class LobbyItemGroup
	{
		public LobbyItemGroupType GroupType { get; private set; }

		public LobbyItemGroupType ParentGroupType { get; private set; }

		public LobbyItemInfo.LobbyItemSlot[] ItemSlots { get; private set; }

		public LobbyItemGroup(LobbyItemGroupType groupType, LobbyItemInfo.LobbyItemSlot[] itemSlots, LobbyItemGroupType rootGroupType)
		{
			GroupType = groupType;
			ItemSlots = itemSlots;
			ParentGroupType = rootGroupType;
		}
	}
}
