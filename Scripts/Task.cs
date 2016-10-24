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
  public class KillEnemyOfType : Task
  {
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
        if(_done >= needed)
        {
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

    private void ProgressChecker(FieldUnit unit)
    {
      if(unit is Enemy)
      {
        Enemy enemy = unit as Enemy;
        if ( enemy.enemy_id == enemy_id )
          done++;
      }
    }
    private KillEnemyOfType(int enemy_id, int needed)
    {
      this.enemy_id = enemy_id;
      this.needed = needed;
      done = 0;
      GlobalEventSystem.OnDeath += ProgressChecker;
    }

    public static KillEnemyOfType Create( int enemy_id, int needed )
    {
      return new KillEnemyOfType( enemy_id, needed );
    }
  }

}