using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


public class Player : FieldUnit
{

  public int level;

  private int exp_to_level
  {
    get
    {
      return level * 50;
    }
  }

  private int _experience;

  public int experience
  {
    get
    {
      return _experience;
    }

    set
    {
      _experience = value;
      if(_experience > exp_to_level )
      {
        _experience -= exp_to_level;
        ++level;
        GlobalEventSystem.PlayerLevelUp();
      }
    }
  }

  public override void Action( Field.Tile target )
  {
    base.Action( target );
    if(target.type == Field.Tile.TileTypes.STORK)
    {
      GlobalEventSystem.TalkToStork( target );
    }
  }

  public override void Initialize()
  {
    base.Initialize();
    level = 1;
    _experience = 0;

  }
}
