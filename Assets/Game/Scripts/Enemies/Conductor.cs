
public class Conductor : Enemy
{
	public Conductor(): base()
	{
		type = EEnemyType.CONDUCTOR;
	}

	public override void Attack()
  {
    base.Attack();
    GlobalDataHolder.player.movement_disabled = true;
    
  }
  public override void Die(bool destroy = true)
	{
    GlobalDataHolder.player.movement_disabled = false;
    base.Die(destroy);
  }

}