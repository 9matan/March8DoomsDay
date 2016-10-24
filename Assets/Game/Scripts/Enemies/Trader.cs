
public class Trader : Enemy
{
  public int gold;

	public Trader(): base()
	{
		type = EEnemyType.TRADER;
	}

	public override void Attack()
  {
    base.Attack();
    if ( GlobalDataHolder.player_gold > 0 )
    {
      GlobalDataHolder.player_gold -= 1;
      ++gold;
    }
    
  }
  public override void Die(bool destroy = true)
	{
    base.Die(destroy);
    GlobalDataHolder.player_gold += gold;
  }

}