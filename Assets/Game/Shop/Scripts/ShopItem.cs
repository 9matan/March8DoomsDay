using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class ShopItem : MonoBehaviour {

	public enum EShopItemType
	{
		NONE,
		MEDICINE,
		PHONE_CELL
	}

	[SerializeField]
	protected Shop.ItemsToBuy _type;
	public Shop.ItemsToBuy type { get { return _type; } }

//	[SerializeField]
	protected int _price = 50;

	[SerializeField]
	protected Text _uiPrice;
	[SerializeField]
	protected Button _buyBtn;
	

	protected virtual void Start()
	{
		if (GlobalDataHolder.player == null) return;

		_price = Shop.GetCostByEnum(_type);
        _uiPrice.text = _price.ToString();

		ShopScene.i.onItemBuy += UpdateInfo;
		UpdateInfo();
    }

	public void UpdateInfo()
	{
		if (CanBuy())
			_buyBtn.Show();
		else
			_buyBtn.Hide();
    }

	public void OnBuyClick()
	{
		ShopScene.i.BuyItem(_type, _price);
	}

	public bool CanBuy()
	{
		return _price <= GlobalDataHolder.player_gold;
    }

}
