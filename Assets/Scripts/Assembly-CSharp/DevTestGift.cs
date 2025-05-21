using UnityEngine;

public class DevTestGift : MonoBehaviour
{
	internal class DroppedSlotInfo
	{
		public GiftCategoryType Category;

		public string GiftId;

		public int GiftCount;

		public int PosInScroll;

		public int DropCount;

		public DroppedSlotInfo(SlotInfo slot)
		{
			Category = slot.category.Type;
			GiftId = ((slot.gift.RootInfo != null) ? slot.gift.RootInfo.Id : slot.gift.Id);
			GiftCount = slot.gift.Count.Value;
			PosInScroll = slot.positionInScroll;
			DropCount = 1;
		}

		public bool Attach(DroppedSlotInfo droppedSlotInfo)
		{
			if (!Equals(droppedSlotInfo))
			{
				return false;
			}
			DropCount += droppedSlotInfo.DropCount;
			return true;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			DroppedSlotInfo droppedSlotInfo = obj as DroppedSlotInfo;
			if (droppedSlotInfo == null)
			{
				return false;
			}
			if (Category == droppedSlotInfo.Category && GiftId == droppedSlotInfo.GiftId)
			{
				return GiftCount == droppedSlotInfo.GiftCount;
			}
			return false;
		}

		public override string ToString()
		{
			return string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"", PosInScroll, Category, GiftId, GiftCount, DropCount);
		}
	}
}
