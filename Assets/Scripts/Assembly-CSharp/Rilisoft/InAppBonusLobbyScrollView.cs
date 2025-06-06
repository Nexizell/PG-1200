using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	public class InAppBonusLobbyScrollView : InAppBonusLobbyViewBase
	{
		[Header("[ base ]")]
		[SerializeField]
		protected internal UILabel _packsLabel;

		[SerializeField]
		protected internal UILabel _descriptionLabel;

		[SerializeField]
		protected internal UILabel _headerLabel;

		[Header("[ images objects ]")]
		[SerializeField]
		protected internal GameObject _currencyObj;

		[SerializeField]
		protected internal GameObject _leprechauntObj;

		[SerializeField]
		protected internal GameObject _weaponObj;

		[SerializeField]
		protected internal GameObject _petObj;

		[SerializeField]
		protected internal GameObject _gadgetsObj;

		[SerializeField]
		[Header("[ textures ]")]
		protected internal UITexture _weaponTexture;

		[SerializeField]
		protected internal UITexture _petTexture;

		[SerializeField]
		protected internal List<UITexture> _gadgetsTextures;

		public override void UpdateView(bool force = false)
		{
			base.UpdateView(force);
			if (base.Data == null)
			{
				return;
			}
			if (_data.IsTypePack)
			{
				if (force || _prevData.Pack != _data.Pack)
				{
					_prevData.Pack = _data.Pack;
					string term = ((_data.Type == BonusData.BonusType.Currency || _data.Type == BonusData.BonusType.Gadgets) ? "Key_2864" : "Key_2896");
					_packsLabel.text = string.Format(LocalizationStore.Get(term), new object[1] { _data.Pack });
				}
			}
			else if (force || _prevData.End != _data.End)
			{
				_prevData.End = _data.End;
				string text = ((_data.End >= 86400) ? string.Format("{0} {1}", new object[2]
				{
					LocalizationStore.Get("Key_1125"),
					RiliExtensions.GetTimeStringDays(_data.End)
				}) : RiliExtensions.GetTimeString(_data.End));
				_packsLabel.text = text;
			}
			if (_data.Type == BonusData.BonusType.Weapons && (force || _prevData.WeaponId != _data.WeaponId))
			{
				_weaponTexture.mainTexture = ItemDb.GetItemIcon(_data.WeaponId, (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(_data.WeaponId));
			}
			else if (_data.Type == BonusData.BonusType.Pets && (force || _prevData.PetId != _data.PetId))
			{
				PetInfo info = Singleton<PetsManager>.Instance.GetInfo(_data.PetId);
				if (info != null)
				{
					_descriptionLabel.text = string.Format(LocalizationStore.Get("Key_2901"), new object[1] { LocalizationStore.Get(info.Lkey) });
					_petTexture.mainTexture = ItemDb.GetItemIcon(info.IdWithoutUp, ShopNGUIController.CategoryNames.PetsCategory);
				}
			}
			else if (_data.Type == BonusData.BonusType.Gadgets && (force || (_data.Gadgets.Any() && _gadgetsTextures.Any() && _data.Gadgets.Any((string g) => _prevData.Gadgets.All((string pg) => pg != g)))))
			{
				List<string> first = GadgetsInfo.AvailableForBuyGadgets(ExpController.GetOurTier()).ToList();
				first = first.Intersect(_data.Gadgets).ToList();
				for (int i = 0; i < _gadgetsTextures.Count; i++)
				{
					if (i < first.Count())
					{
						string key = first[i];
						if (GadgetsInfo.info.ContainsKey(key))
						{
							GadgetInfo gadgetInfo = GadgetsInfo.info[key];
							_gadgetsTextures[i].mainTexture = ItemDb.GetItemIcon(gadgetInfo.Id, (ShopNGUIController.CategoryNames)gadgetInfo.Category);
						}
					}
				}
			}
			if (_prevData.Type != _data.Type || force)
			{
				_currencyObj.SetActiveSafe(false);
				_leprechauntObj.SetActiveSafe(false);
				_weaponObj.SetActiveSafe(false);
				_petObj.SetActiveSafe(false);
				_gadgetsObj.SetActiveSafe(false);
				if (_data.Type == BonusData.BonusType.Currency)
				{
					_currencyObj.SetActiveSafe(true);
					_headerLabel.text = LocalizationStore.Get("Key_2875");
					_descriptionLabel.text = LocalizationStore.Get("Key_2906");
				}
				else if (_data.Type == BonusData.BonusType.Weapons)
				{
					_weaponObj.SetActiveSafe(true);
					_headerLabel.text = LocalizationStore.Get("Key_2892");
					_descriptionLabel.text = LocalizationStore.Get("Key_2893");
				}
				else if (_data.Type == BonusData.BonusType.Pets)
				{
					_petObj.SetActiveSafe(true);
					_headerLabel.text = LocalizationStore.Get("Key_2882");
				}
				else if (_data.Type == BonusData.BonusType.Leprechaunt)
				{
					_leprechauntObj.SetActiveSafe(true);
					_headerLabel.text = LocalizationStore.Get("Key_2900");
					_descriptionLabel.text = LocalizationStore.Get("Key_2899");
				}
				else if (_data.Type == BonusData.BonusType.Gadgets)
				{
					_gadgetsObj.SetActiveSafe(true);
					_headerLabel.text = LocalizationStore.Get("Key_2883");
					_descriptionLabel.text = LocalizationStore.Get("Key_2897");
				}
			}
		}
	}
}
