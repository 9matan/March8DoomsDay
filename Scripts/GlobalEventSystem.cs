using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

public abstract class GlobalEventSystem
{
  public delegate void NoArgsEvent();
  public delegate void FieldUnitEvent( FieldUnit unit );
  public delegate void TileActionEvent( Field.Tile tile );
  public delegate void TaskEvent( Task task );

  public static event FieldUnitEvent OnSpawn;
  public static event FieldUnitEvent OnDeath;
  public static event NoArgsEvent OnGameOver;
  public static event TileActionEvent OnStorkTalk;
  public static event FieldUnitEvent OnPlayerLevelUp;
  public static event TaskEvent OnTaskDone;

  private static void RaiseUnitSpawned( FieldUnit unit )
  {
    if ( OnSpawn != null )
      OnSpawn( unit );
  }

  private static void RaiseUnitDied( FieldUnit unit )
  {
    if ( OnDeath != null )
      OnDeath( unit );
  }
  
  private static void RaiseGameOver()
  {
    if ( OnGameOver != null )
      OnGameOver();
  }

  private static void RaiseStorkTalk( Field.Tile tile )
  {
    if ( OnStorkTalk != null )
      OnStorkTalk( tile );
  }

  public static void EnemySpawned( FieldUnit unit )
  {
    RaiseUnitSpawned( unit );
    unit.OnDeath += UnitDeathAngel;
  }

  public static void TaskDone(Task task)
  {
    if ( OnTaskDone != null )
      OnTaskDone( task );
  }
  public static void TalkToStork( Field.Tile tile )
  {
    RaiseStorkTalk( tile );
  }
  public static void PlayerLevelUp()
  {
    if ( OnPlayerLevelUp != null )
      OnPlayerLevelUp( GlobalDataHolder.player );
  }
  private static void UnitDeathAngel( FieldUnit unit )
  {
    if(unit is Player)
    {
      //TODO
      Debug.Log("Player died!");
      RaiseGameOver();
      return;
    }
    if(unit is Enemy)
    {
      Enemy enemy = unit as Enemy;
      GlobalDataHolder.player_score += enemy.score;
    }
  }

}
