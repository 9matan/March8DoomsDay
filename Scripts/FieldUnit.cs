public class FieldUnit
{
  public event GlobalEventSystem.NoArgsEvent OnTick;
  public event GlobalEventSystem.FieldUnitEvent OnDeath;

  protected void RaiseTick()
  {
    if ( OnTick != null )
      OnTick();
  }
  protected void RaiseDeath()
  {
    if ( OnDeath != null )
      OnDeath(this);
  }



  public Field.Tile tile;
  public Field.Directions direction;

  public int max_health;
  protected int health;
  public int damage;

  public virtual void Attacked( int damage )
  {
    health -= damage;
    if ( health <= 0 )
      Die();
  }
  public virtual void Die()
  {
    //TODO
    RaiseDeath();
    tile.unit = null;
  }
  public virtual void Action( Field.Tile target )
  {
    //TODO
  }
  public virtual void Attack()
  {
    tile[direction].Attacked( damage );
  }

  public bool Turn( Field.Tile target )
  {
    if ( tile == null || target == null )
      return false;
    if ( target == tile.left )
    {
      direction = Field.Directions.LEFT;
    }
    else
    if ( target == tile.right )
    {
      direction = Field.Directions.RIGHT;
    }
    else
    if ( target == tile.up )
    {
      direction = Field.Directions.UP;
    }
    else
    if ( target == tile.down )
    {
      direction = Field.Directions.DOWN;
    }
    else
      return false;
    return true;
  }
  public virtual bool Move( Field.Directions direction )
  {
    if ( tile == null )
      return false;
    Field.Tile target;
    switch ( direction )
    {
      case Field.Directions.UP:
        target = tile.up;
        break;
      case Field.Directions.DOWN:
        target = tile.down;
        break;
      case Field.Directions.LEFT:
        target = tile.left;
        break;
      case Field.Directions.RIGHT:
        target = tile.right;
        break;
      default:
        target = null;
        break;
    }
    this.direction = direction;
    if ( target != null && target.unit == null )
    {
      if ( target.type == Field.Tile.TileTypes.ROAD )
      {
        target.unit = this;
        tile.unit = null;
        tile = target;
        return true;
      }
      else
      {
        Action( target );
        return true;
      }
    }
    return false;
  }
  public virtual bool Move( Field.Tile target )
  {
    if ( !Turn( target ) )
      return false;
    if ( target != null && target.unit == null )
    {
      if ( target.type == Field.Tile.TileTypes.ROAD )
      {
        target.unit = this;
        tile.unit = null;
        tile = target;
        return true;
      }
      else
      {
        Action( target );
        return true;
      }
    }
    return false;
  }

  public virtual FieldUnit Spawn( Field.Tile position )
  {
    
    if ( position == null )
      return null;
    
    
    if ( position.unit == null && position.type == Field.Tile.TileTypes.ROAD )
    {
      //TODO
      //Instantiation
      FieldUnit fu = new FieldUnit();
      fu.direction = Field.Directions.LEFT;
      position.unit = fu;
      fu.tile = position;
      return fu;
    }
<<<<<<< HEAD
    return null;
=======
    return false;	
>>>>>>> 553627a36f3597bf7addd92cfaaa707554d0f108
  }

  public virtual void Initialize()
  {
    health = max_health;
  }

}