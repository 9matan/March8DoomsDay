using System.Collections.Generic;
using System.Linq;
public class Girl : Enemy
{

	public Girl(): base()
	{
		type = EEnemyType.GIRL;
	}

  protected override void ListenToLehaTick()
  {
    if ( tile.leha_far > 1 )
    {
      Move( tile.path_to_leha );
    }
    else
    {
      Turn( GlobalDataHolder.leha.tile );
      Attack();
      return;
    }
  }




  public override void Initialize()
  {
    base.Initialize();
  }




}