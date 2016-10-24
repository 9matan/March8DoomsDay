
public class Granny : Enemy
{

	public Granny(): base()
	{
		type = EEnemyType.GRANNY;//
	}

  void NarkomanChtole( FieldUnit unit, int distance )
  {
    unit.Attacked( damage / distance );
  }
  protected override void ListenToLehaTick()
  {
      Turn( tile.path_to_leha );
  }
  protected override void EnemyTick()
  {
    if ( tile.player_far > 1 && tile.player_far < 4 )
    {
      NarkomanChtole( GlobalDataHolder.player, tile.player_far );
    }
    else
    {
      if ( tile.player_far == 1 )
      {
        Turn( GlobalDataHolder.player.tile );
        Attack();
      }
    }
  }


  public override void Initialize()
  {
    base.Initialize();

  }




}