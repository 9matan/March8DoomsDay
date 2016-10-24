using UnityEngine;
using System.Collections;

public class ShopScene : MonoBehaviour {

	static public ShopScene i { get { return _instance; } }

	static protected ShopScene _instance;

	public delegate void OnItemBuy();
	public event OnItemBuy onItemBuy = delegate { };

	[SerializeField]
	protected ShopItem _bigHammerItem;

	protected void Awake()
	{
		_instance = this;

		CheckHammer();
    }

	public void CheckHammer()
	{
		if (GlobalDataHolder.player.skill_achieved)
			_bigHammerItem.Hide();
	}

	public void LoadNextLevel()
	{
		ScenesManager.LoadNextLevel();
	}

	public void BuyItem(Shop.ItemsToBuy type, int price)
	{
        Shop.BuyItem(type);
		CheckHammer();
		onItemBuy();
    }

}
