using UnityEngine;
using System.Collections;

public class ItemViewFactory : MonoBehaviour {

	static public ItemViewFactory i
	{
		get { return _instance; }
	}

	static protected ItemViewFactory _instance;

	[SerializeField]
	protected DOGOMemFactory _flowerFactory;
	[SerializeField]
	protected DOGOMemFactory _coinFactory;
	[SerializeField]
	protected DOGOMemFactory _questItemFactory;

	protected void Awake()
	{
		_instance = this;
	}

	public ItemView CreateItemView(Item.EItemType type)
	{
		ItemView itemView = null;

		switch(type)
		{
			case Item.EItemType.FLOWER:
				itemView = _flowerFactory.Allocate().GetComponent<ItemView>();
                break;
			case Item.EItemType.COIN:
				itemView = _coinFactory.Allocate().GetComponent<ItemView>();
				break;
			case Item.EItemType.QUEST_ITEM:
				itemView = _questItemFactory.Allocate().GetComponent<ItemView>();
				break;
		}

		return itemView;
    }

	public void FreeItemView(ItemView itemView)
	{
		switch (itemView.item.type)
		{
			case Item.EItemType.FLOWER:
				_flowerFactory.Free(itemView.gameObject);
				break;
			case Item.EItemType.COIN:
				_coinFactory.Free(itemView.gameObject);
				break;
			case Item.EItemType.QUEST_ITEM:
				_questItemFactory.Free(itemView.gameObject);
				break;
		}
	}

}
