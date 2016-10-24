
public class Kid : FieldUnit
{
  public override void Attacked( int damage )
  {
    return;
  }
  protected void Wonder()
  {
    if ( UnityEngine.Random.Range( 0, 100 ) <= 30 )
    {
      Move( tile.random_no_unit_road );
    }
  }

  public override void Initialize()
  {
    base.Initialize();
    OnTurnTick += Wonder;
  }
}