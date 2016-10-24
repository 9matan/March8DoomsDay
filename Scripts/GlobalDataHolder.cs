using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

public class GlobalDataHolder
{
  private int _player_score = 0;
  private Player _player;


  private static GlobalDataHolder data;


  public static int player_score
  {
    get
    {
      if ( data != null )
        return data._player_score;
      return 0;
    }
    set
    {
      if ( data != null )
        data._player_score = value;
    }
  }
  public static Player player
  {
    get
    {
      if ( data != null )
        return data._player;
      return null;
    }
  }


  public static void Initialize( Player player )
  {
    data = new GlobalDataHolder( player );
  }

  private GlobalDataHolder( Player player)
  {
    _player = player;
    _player_score = 0;
  }
}
