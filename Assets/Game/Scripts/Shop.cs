using UnityEngine;
public class Shop : MonoBehaviour
{
  public int _big_hammer_cost;
  
  public int _phone_cell_cost;
  public int _medicine_cost;
  public int _time_boost_cost;

  private static Shop data;


	protected void Awake()
	{
		data = this;
	}

  public static int big_hammer_cost
  {
    get
    {
      if ( data == null )
        return 0;
      return data._big_hammer_cost;
    }
  }
  public static int phone_cell_cost
  {
    get
    {
      if ( data == null )
        return 0;
      return data._phone_cell_cost;
    }
  }
  public static int medicine_cost
  {
    get
    {
      if ( data == null )
        return 0;
      return data._medicine_cost;
    }
  }
  public static int time_boost_cost
  {
    get
    {
      if ( data == null )
        return 0;
      return data._medicine_cost;
    }
  }


  public enum ItemsToBuy
  {
    BIG_HAMMER,
    PHONE_CELL,
    MEDICINE,
    TIME_BOOST
  }
  public static bool BuyItem(ItemsToBuy item)
  {
    switch ( item )
    {
      case ItemsToBuy.BIG_HAMMER:
        if(GlobalDataHolder.player_gold >= big_hammer_cost)
        {
          GlobalDataHolder.player_gold -= big_hammer_cost;
          GlobalDataHolder.player.damage += GlobalDataHolder.big_hammer_damage_bonus;
          GlobalDataHolder.player.skill_achieved = true;
          return true;
        }
        break;
      case ItemsToBuy.PHONE_CELL:
        if ( GlobalDataHolder.player_gold >= phone_cell_cost )
        {
          GlobalDataHolder.player_gold -= phone_cell_cost;
          GlobalDataHolder.phone_cells++;
          return true;
        }
        break;
      case ItemsToBuy.MEDICINE:
        if ( GlobalDataHolder.player_gold >= medicine_cost )
        {
          GlobalDataHolder.player_gold -= medicine_cost;
          GlobalDataHolder.medicine++;
          return true;
        }
        break;
      case ItemsToBuy.TIME_BOOST:
        if ( GlobalDataHolder.player_gold >= time_boost_cost)
        {
          GlobalDataHolder.player_gold -= time_boost_cost;
          GlobalDataHolder.time_left += GlobalDataHolder.time_boost_amount;
          return true;
        }
        break;
      default:
        break;
    }
    return false;
  }

	public static int GetCostByEnum(ItemsToBuy type)
	{
		switch (type)
		{
			case ItemsToBuy.BIG_HAMMER:
				return big_hammer_cost;
			case ItemsToBuy.PHONE_CELL:
				return phone_cell_cost;
			case ItemsToBuy.MEDICINE:
				return medicine_cost;
			case ItemsToBuy.TIME_BOOST:
				return time_boost_cost;
			default:
				break;
		}

		return 0;
	}

}