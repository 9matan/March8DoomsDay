using UnityEngine;
public abstract class Item
{
  
	public EItemType type { get; protected set; }

	public enum EItemType
	{
		NONE,
		FLOWER,
		COIN,
		QUEST_ITEM
	}

  private class Flower :Item
  {
    int score = 0;
    public override void PickUp()
    {
      GlobalDataHolder.player_score += score;
      base.PickUp();
    }
    public Flower(int score):base()
    {
		type = EItemType.FLOWER;
      this.score = score;
    }
  }
  private class Coin : Item
  {
    int gold = 0;
    public override void PickUp()
    {
      GlobalDataHolder.player_gold += gold;
      base.PickUp();
    }
    public Coin( int gold ) :base()
    {
		type = EItemType.COIN;
      this.gold = gold;
    }
  }
  private class QuestItem : Item
  {
    public QuestItem() : base()
    {
      type = EItemType.QUEST_ITEM;
    }
  }
  public Field.Tile tile { get; protected set; }

  

  public virtual void PickUp()
  {
    GlobalEventSystem.ItemPickedUp( this );
    Die();
  }

  protected virtual void Die()
  {
	tile.tileView.ClearItemView();
    tile.item = null;
    tile = null;
  }

  protected Item()
  {
  }

  public static Item CreateFlower()
  {
    return new Flower( UnityEngine.Random.Range( GlobalDataHolder.min_score_per_flower, GlobalDataHolder.max_score_per_flower ) );
  }
  public static Item CreateCoin()
  {
    return new Coin( UnityEngine.Random.Range( GlobalDataHolder.min_gold_per_coin, GlobalDataHolder.max_gold_per_coin) );
  }

  public static Item CreateQuestItem()
  {
    return new QuestItem();
  }

  public bool Spawn( Field.Tile tile )
  {
    if ( tile.item != null )
      return false;
    tile.item = this;
    this.tile = tile;

	if (tile.tileView != null)
		tile.tileView.UpdateItemView();

	return true;
  }

}