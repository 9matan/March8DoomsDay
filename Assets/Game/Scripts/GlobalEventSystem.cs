using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

public abstract class GlobalEventSystem
{
  public delegate void NoArgsEvent();
  public delegate void IntArgEvent(int val);
  public delegate void FieldUnitEvent( FieldUnit unit );
  public delegate void FieldDamageEvent( FieldUnit unit, int damage );
  public delegate void TileActionEvent( Field.Tile tile );
  public delegate void StorkEvent( Field.Tile tile, Task task );
  public delegate void TaskEvent( Task task );
  public delegate void ItemEvent( Item item );
  public delegate void PlayerLevelUpEvent();

  public static event IntArgEvent OnScoreChanged;
  public static event IntArgEvent OnGoldChanged;
  public static event IntArgEvent OnExperienceChanged;
  public static event FieldUnitEvent OnSpawn;
  public static event FieldUnitEvent OnDeath;
  public static event NoArgsEvent OnGameOver;
  public static event NoArgsEvent OnLevelStart;
  public static event NoArgsEvent OnLevelEnd;
  public static event NoArgsEvent OnTurnTick;
  public static event NoArgsEvent OnRealTimeTick;
  public static event NoArgsEvent OnLehaAppear;
  public static event NoArgsEvent OnLehaDisappear;
  public static event StorkEvent OnStorkTalk;
  public static event TileActionEvent OnStructureDestroyed;
  public static event TileActionEvent OnCurtainActivate;
  public static event TileActionEvent OnCurtainLeave;
  public static event TileActionEvent OnTileActivate;
  public static event FieldUnitEvent OnUnitActivate;
  public static event PlayerLevelUpEvent OnPlayerLevelUp;
  public static event TaskEvent OnTaskDone;
  public static event ItemEvent OnItemPickedUp;
  public static event FieldUnitEvent OnPlayerSkillUsed;

  public static event FieldUnitEvent OnPlayerMedicineUsed;


  #region UnitFuctions
  public static void RaiseUnitSpawned( FieldUnit unit )
  {
    if ( OnSpawn != null )
      OnSpawn( unit );
  }
  private static void RaiseUnitDied( FieldUnit unit )
  {
    if ( OnDeath != null )
      OnDeath( unit );
  }
  private static void RaiseLehaDisappear()
  {
    if ( OnLehaDisappear != null )
      OnLehaDisappear();
  }

	public static void ListenPlayerDeath()
	{
		GlobalDataHolder.player.OnDeath += UnitDeathAngel;
    }

  private static void UnitDeathAngel( FieldUnit unit )
  {

    if ( unit is Player )
    {
      //TODO
      Debug.Log( "Player died!" );
      RaiseGameOver();
      return;
    }
    if ( unit is Leha )
    {
      Debug.Log( "Leha dissapear" );
      RaiseLehaDisappear();
      return;
    }
    if ( unit is Enemy )
    {
      Enemy enemy = unit as Enemy;
      RaiseUnitDied( unit );
      GlobalDataHolder.player_score += enemy.score;
	  GlobalDataHolder.player.experience += enemy.experience;	
    }
  }

  public static void EnemySpawned( FieldUnit unit )
  {
    RaiseUnitSpawned( unit );
    unit.OnDeath += UnitDeathAngel;
  }


  #endregion
  public static void RaiseTurnTick()
  {
    if ( OnTurnTick != null )
      OnTurnTick();
    //Debug.Log( Time.realtimeSinceStartup );
  }
  public static void RaiseRealTimeTick()
  {
    if ( OnRealTimeTick != null )
      OnRealTimeTick();
  }


  private static void UnassignEvents()
  {
    OnScoreChanged = null;
    OnGoldChanged = null;
    OnExperienceChanged = null;
    OnSpawn = null;
    OnDeath = null;
    OnTurnTick = null;
    OnRealTimeTick = null;
    OnLehaAppear = null;
    OnLehaDisappear = null;
    OnStorkTalk = null;
    OnCurtainActivate = null;
    OnCurtainLeave = null;
    OnTileActivate = null;
    OnUnitActivate = null;
    OnPlayerLevelUp = null;
    OnTaskDone = null;
    OnItemPickedUp = null;
    OnPlayerSkillUsed = null;
    OnPlayerMedicineUsed = null;
	OnLevelStart = null;
	OnLevelEnd = null;
  }
  //TODO
  private static void RaiseGameOver()
  {
    if ( OnGameOver != null )
      OnGameOver();
	RaiseLevelEnd();
  }
  public static void RaiseLevelStart()
  {
    if ( OnLevelStart != null )
      OnLevelStart();
  }
  private static void RaiseLevelEnd()
  {
    if ( OnLevelEnd != null )
      OnLevelEnd();
	UnassignEvents();
  }

  public static void TimerElapsed()
  {
    RaiseGameOver();
  }

  #region Stork
  private static void RaiseStorkTalk( Field.Tile tile )
  {
    Task task = Task.FindTask( tile );
    if ( OnStorkTalk != null )
      OnStorkTalk( tile, task);
  }

  #endregion

  #region FieldFunctions
  private static void RaiseCurtainActivate( Field.Tile tile )
  {
    if ( OnCurtainActivate != null )
      OnCurtainActivate( tile );
  }
  public static void RaiseCurtainLeave( Field.Tile tile )
  {
    if ( OnCurtainLeave != null )
      OnCurtainLeave( tile );
  }
  private static void RaiseTileActivate( Field.Tile tile )
  {
    if ( OnTileActivate != null )
      OnTileActivate( tile );
  }
  public static void StructureDestroyed( Field.Tile tile )
  {
    if ( OnStructureDestroyed != null )
      OnStructureDestroyed(tile);
  }
  public static void ActivateTile( Field.Tile tile )
  {
    if ( tile.type == Field.Tile.TileTypes.STORK )
    {
      RaiseStorkTalk( tile );
      return;
    }
    if ( tile.type == Field.Tile.TileTypes.CURTAIN )
    {
      RaiseCurtainActivate( tile );
      return;
    }
    if(tile.type == Field.Tile.TileTypes.TURNSTILE )
    {
      if(GlobalDataHolder.player_gold >= GlobalDataHolder.gold_to_pass_turnstile )
      {
        GlobalDataHolder.player.tile.unit = null;
        GlobalDataHolder.player.tile = tile;
        tile.unit = GlobalDataHolder.player;
        GlobalDataHolder.player_gold -= GlobalDataHolder.gold_to_pass_turnstile;
        GlobalDataHolder.player.ForceMoveByDirection( GlobalDataHolder.player.direction );
      }
      return;
    }
    if(tile.type == Field.Tile.TileTypes.EXIT)
    {
      RaiseLevelEnd();
      return;
    }
    RaiseTileActivate( tile );
  }
  public static void ActivateUnit( FieldUnit unit )
  {
    if ( OnUnitActivate != null )
      OnUnitActivate( unit );
  }
  public static void ItemPickedUp( Item item )
  {
    if ( OnItemPickedUp != null )
      OnItemPickedUp( item );
  }
  #endregion

  #region PlayerFunctuion
  public static void PlayerScoreChanged(int value)
  {
    if ( OnScoreChanged != null )
      OnScoreChanged( value );
  }
  public static void PlayerGoldChanged( int value )
  {
    if ( OnGoldChanged != null )
      OnGoldChanged( value );
  }
  public static void PlayerExperienceChanged( int value )
  {
    if ( OnExperienceChanged != null )
      OnExperienceChanged( value );
  }
  public static void PlayerLevelUp()
  {
    if ( OnPlayerLevelUp != null )
		OnPlayerLevelUp();
  }
  public static void PlayerSkillUsed()
  {
    if ( OnPlayerSkillUsed != null )
      OnPlayerSkillUsed( GlobalDataHolder.player );
  }
  public static void PlayerMedicineUsed()
  {
    if ( OnPlayerMedicineUsed != null )
      OnPlayerMedicineUsed( GlobalDataHolder.player );
  }
  public static bool SummonLeha()
  {
    if ( GlobalDataHolder.phone_cells > 0 )
    {
      FieldUnit leha = GlobalDataHolder.leha.Spawn( GlobalDataHolder.player.tile.random_no_unit_road, false );
      if ( leha != null )
      {
        GlobalDataHolder.phone_cells--;
        leha.OnDeath += UnitDeathAngel;
        if ( OnLehaAppear != null )
          OnLehaAppear();
        Debug.Log( "Leha apears" );
        return true;
      }

    }
    return false;
  }

  #endregion


  public static void TaskDone(Task task)
  {
    if ( OnTaskDone != null )
      OnTaskDone( task );
  }
  
  
  

  public enum ControllKeys
  {
    UP,
    DOWN,
    LEFT,
    RIGHT,
    ATTACK,
    SKILL,
    LEHA,
    MEDICINE
  }
  public static void ButtonClicked( ControllKeys key )
  {   
    switch ( key )
    {
      case ControllKeys.UP:
        GlobalDataHolder.player.MoveByDirection( Field.Directions.UP );
        break;
      case ControllKeys.DOWN:
        GlobalDataHolder.player.MoveByDirection( Field.Directions.DOWN );
        break;
      case ControllKeys.LEFT:
        GlobalDataHolder.player.MoveByDirection( Field.Directions.LEFT );
        break;
      case ControllKeys.RIGHT:
        GlobalDataHolder.player.MoveByDirection( Field.Directions.RIGHT );
        break;
      case ControllKeys.ATTACK:
        GlobalDataHolder.player.Attack();
        break;
      case ControllKeys.SKILL:
        GlobalDataHolder.player.UseSkill();
        break;
      case ControllKeys.LEHA:
        GlobalDataHolder.player.CallLeha();
        break;
      case ControllKeys.MEDICINE:
        GlobalDataHolder.player.UseMedicine();
        break;
      default:
        break;
    }
  }

	public static void DebugRaiseLevelEnd()
	{
		RaiseLevelEnd();
	}

}
