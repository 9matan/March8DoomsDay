using System.Collections.Generic;
using System.Linq;
public class Enemy : FieldUnit
{

	public enum EEnemyType
	{
		NONE,
		GIRL,
		GRANNY,
		TRADER,
    CONDUCTOR
	}

	public EEnemyType type { get; protected set; }

  public int enemy_id;
  public string enemy_name;
  public int score;
  public int experience;
  protected virtual void EnemyTick()
  {
    if ( tile.player_far > 1 )
    {
      if(tile.player_far > 3 )
      {
        Wonder();
        return;
      }
      Move( tile.path_to_player );
      return;
    }
    else
    {
      if ( tile.player_far != -1 )
      {
        Turn( GlobalDataHolder.player.tile );
        Attack();
      }
    }
    
  }

  protected virtual void ListenToLehaTick()
  {
    if ( tile.leha_far > 1 )
    {
      Move( tile.path_to_leha );
    }
    else
    {
      Turn( tile.path_to_leha );
      return;
    }
    Wonder();
  }

  protected virtual void LehaIsHere()
  {
    OnTurnTick -= EnemyTick;
    OnTurnTick += ListenToLehaTick;
  }
  protected virtual void LehaIsGone()
  {
    OnTurnTick -= ListenToLehaTick;
    OnTurnTick += EnemyTick;
  }

  protected void Wonder()
  {
    if ( UnityEngine.Random.Range(0,100) <= 30 )
    {
      Move( tile.random_no_unit_road );
    }
  }
  public override void Die(bool destroy = true)
  {
    base.Die(destroy);
    switch ( GlobalDataHolder.difficulty )
    {
      case GlobalDataHolder.DifficultyLevels.EASY:
        GlobalDataHolder.player_score += score;
        break;
      case GlobalDataHolder.DifficultyLevels.NORMAL:
        GlobalDataHolder.player_score += (int)(score * 1.5f);
        break;
      case GlobalDataHolder.DifficultyLevels.HARD:
        GlobalDataHolder.player_score += (int)( score * 2f );
        break;
      case GlobalDataHolder.DifficultyLevels.UNREAL:
        GlobalDataHolder.player_score += (int)( score * 3f );
        break;
      default:
        break;
    }
//		UnityEngine.Debug.Log("ExP: " + experience);
    GlobalDataHolder.player.experience += experience;
  }
  public override void Initialize()
  {
    base.Initialize();
    OnTurnTick += EnemyTick;
    GlobalEventSystem.OnLehaAppear += LehaIsHere;
    GlobalEventSystem.OnLehaDisappear += LehaIsGone;
  }




}