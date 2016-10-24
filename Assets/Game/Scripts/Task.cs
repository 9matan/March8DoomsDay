using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;




public abstract class Task
{
  public Field.Tile task_giver;
  public virtual bool is_done
  {
    get
    {
      return false;
    }
  }

  public int reward_exp;
  public int reward_gold;
  public int reward_score;
  public float reward_time;

  public virtual string message
  {
    get
    {
      return "Ты сделал все что мне нужно.";
    }
  }
  public virtual string short_message
  {
    get
    {
      return "";
    }
  }
  public virtual void GiveReward()
  {
    if ( reward_exp > 0 )
      GlobalDataHolder.player.experience += reward_exp;
    if ( reward_gold > 0 )
      GlobalDataHolder.player_gold += reward_gold;
    if ( reward_gold > 0 )
      GlobalDataHolder.player_score += reward_score;
    if ( reward_time > 0 )
      GlobalDataHolder.time_left += reward_time;

  }
  public Task( Field.Tile task_giver, int reward_exp, int reward_gold, int reward_score, float reward_time )
  {
    this.task_giver = task_giver;
    this.reward_exp = reward_exp;
    this.reward_gold = reward_gold;
    this.reward_score = reward_score;
    this.reward_time = reward_time;

  }


  private class KillEnemyOfType : Task
  {
    public const int MINIMUM_TO_KILL = 3;
    public const int MAXIMUM_TO_KILL = 8;
    public int enemy_id;
    public int needed;
    private int _done;



    public int done
    {
      get
      {
        return _done;
      }

      set
      {
        _done = value;
        if ( _done >= needed )
        {
          GlobalEventSystem.OnDeath -= ProgressChecker;
          GlobalEventSystem.TaskDone( this );
        }
      }
    }
    public override bool is_done
    {
      get
      {
        return done >= needed;
      }
    }

    public override string short_message
    {
      get
      {
        return "Убить " + GlobalDataHolder.FindEnemy( enemy_id ).enemy_name.ToString() + ": "+done+"/"+needed;
      }
    }
    public override string message
    {
      get
      {
        if ( is_done )
          return base.message;
        return "Иди убей "+(needed-done).ToString()+" "+GlobalDataHolder.FindEnemy(enemy_id).enemy_name+"!";
      }
    }

    private void ProgressChecker( FieldUnit unit )
    {
      if ( unit is Enemy )
      {
        Enemy enemy = unit as Enemy;
        if ( enemy.enemy_id == enemy_id )
          done++;
      }
    }
    public KillEnemyOfType( Field.Tile task_giver, int reward_exp, int reward_gold, int reward_score, int enemy_id, int needed )
      : base( task_giver, reward_exp, reward_gold, reward_score, 0 )
    {
      this.enemy_id = enemy_id;
      this.needed = needed;
      done = 0;
      GlobalEventSystem.OnDeath += ProgressChecker;
    }

    
  }
  private class FindItem : Task
  {
    public Item item;
    private bool _done;

    public void Done(Item item)
    {
      if ( item == this.item )
      {
        _done = true;
        GlobalEventSystem.OnItemPickedUp -= Done;
        GlobalEventSystem.TaskDone( this );
      }
    }
    public override bool is_done
    {
      get
      {
        return _done;
      }
    }

    public override string short_message
    {
      get
      {
        return "Найти супер цветок";
      }
    }
    public override string message
    {
      get
      {
        if ( is_done )
          return base.message;
        return "Найди супер-цветок!";
      }
    }

    public FindItem( Field.Tile task_giver, int reward_exp, int reward_gold, int reward_score )
      : base( task_giver, reward_exp, reward_gold, reward_score, 0 )
    {
      _done = false;
      item = Item.CreateQuestItem();
      GlobalEventSystem.OnItemPickedUp += Done;
    }
       
  }
  private class FindUnit : Task
  {
    public FieldUnit unit_to_interact;
    private bool _done;
    
    public override bool is_done
    {
      get
      {
        return _done;
      }
    }
    public override string short_message
    {
      get
      {
        return "Найти ребенка";
      }
    }
    public override string message
    {
      get
      {
        if ( is_done )
          return base.message;
        return "Найди ребенка!";
      }
    }
    private void ProgressChecker( FieldUnit unit )
    {
      if ( unit == unit_to_interact )
      {
        unit.Die();
        _done = true;
        GlobalEventSystem.TaskDone( this );
        GlobalEventSystem.OnUnitActivate -= ProgressChecker;
      }
    }
    public FindUnit( Field.Tile task_giver, int reward_exp, int reward_gold, int reward_score, float reward_time )
      : base( task_giver, reward_exp, reward_gold, reward_score, reward_time )
    {
      _done = false;
      unit_to_interact = GlobalDataHolder.quest_unit.Spawn( GlobalDataHolder.field.random_no_unit_road );
      GlobalEventSystem.OnUnitActivate += ProgressChecker;
    }


  }



  public static Task CreateFindItem( Field.Tile task_giver, int reward_exp, int reward_gold, int reward_score )
  {
    FindItem task = new FindItem( task_giver, reward_exp, reward_gold, reward_score );
    if ( !task.item.Spawn( GlobalDataHolder.field.random_no_item_road ) )
    {
      return null;
    }
    return task;
  }
  public static Task CreateFindUnit( Field.Tile task_giver, int reward_exp, int reward_gold, int reward_score, float reward_time )
  {
    FindUnit task = new FindUnit( task_giver, reward_exp, reward_gold, reward_score, reward_time );
    if ( task.unit_to_interact== null )
      return null;
    return task;
  }
  public static Task CreateKillEnemyOfType( Field.Tile task_giver, int reward_exp, int reward_gold, int reward_score, int enemy_id, int needed )
  {
    return new KillEnemyOfType( task_giver, reward_exp, reward_gold, reward_score, enemy_id, needed );
  }


  private static Dictionary<Field.Tile, Task> tasks = new Dictionary<Field.Tile, Task>();

  public static void ClearTasks()
  {
    tasks = new Dictionary<Field.Tile, Task>();
  }
  public static Task FindTask( Field.Tile giver, bool create_new_if_not_found = true, int reward_exp = 50, int reward_gold = 50, int reward_score = 1000, float reward_time = 40f )
  {
    if(tasks.ContainsKey(giver))
    {
      return tasks[giver];
    }
    else
    {
      if(create_new_if_not_found)
      {
        int rnd = UnityEngine.Random.Range( 0, 3 );
        Task task = null;
        switch ( rnd )
        {
          case 0:
            {
              int count_to_kill = UnityEngine.Random.Range( KillEnemyOfType.MINIMUM_TO_KILL, KillEnemyOfType.MAXIMUM_TO_KILL );
              float mult = ((float)count_to_kill) / KillEnemyOfType.MINIMUM_TO_KILL;
              task = CreateKillEnemyOfType( giver, 
                UnityEngine.Mathf.CeilToInt(reward_exp * mult), 
                UnityEngine.Mathf.CeilToInt( reward_gold * mult), 
                UnityEngine.Mathf.CeilToInt( reward_score * mult), 
                GlobalDataHolder.next_enemy_id, count_to_kill );
            }break;
          case 1:
            {
              task = CreateFindItem( giver, reward_exp, reward_gold, reward_score );
            }
            break;
          case 2:
            {
              task = CreateFindUnit( giver, reward_exp, reward_gold, reward_score, reward_time );
            }
            break;
          default:
            break;
        }
        if(task != null) tasks[giver] = task;
        return task;
      }
      return null;
    }
  }

  public static List<string> tasks_messages
  {
    get
    {
      List<string> messages = new List<string>();
      foreach ( var item in tasks )
      {
        if(!item.Value.is_done)
          messages.Add( item.Value.short_message );
      }
      return messages;
    }
  }
}