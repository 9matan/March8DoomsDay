using UnityEngine;
using System.Collections.Generic;

public class FieldUnit : MonoBehaviour
{

	static protected HashSet<FieldUnit> _allUnits = new HashSet<FieldUnit>();

	static public void ClearAllUnits()
	{
		foreach (var un in _allUnits)
			un.Clear();

		_allUnits.Clear();
    }

	//
	// < Unity >
	//

	[SerializeField]
	protected FieldUnitFace _face;

	public virtual bool MoveByDirection(Field.Directions direction)
	{
		var isvalid = Move(direction);

		if (isvalid)
			_SetPosition(tile.tileView.transform.position, true);

		return isvalid;
	}

  public virtual bool ForceMoveByDirection( Field.Directions direction )
  {
    var isvalid = ForceMove( direction );
    _face.UpdateDirectionView( direction );

    if ( isvalid )
      _SetPosition( tile.tileView.transform.position, true );

    return isvalid;
  }

  //
  // </ Unity >
  //

  public event GlobalEventSystem.NoArgsEvent OnRealTimeTick;
  public event GlobalEventSystem.NoArgsEvent OnTurnTick;
  public event GlobalEventSystem.FieldDamageEvent OnDamageRecieved;
  public event GlobalEventSystem.FieldDamageEvent OnHealingRecieved;
  public event GlobalEventSystem.FieldUnitEvent OnDeath;

  protected void RaiseRealTimeTick()
  {
    if ( OnRealTimeTick != null )
      OnRealTimeTick();
  }
  protected void RaiseTurnTick()
  {
    if ( OnTurnTick != null )
      OnTurnTick();
  }
  protected void RaiseDeath()
  {
    if ( OnDeath != null )
      OnDeath(this);
  }
  protected void RaiseDamageRecieved(int damage)
  {
    if ( OnDamageRecieved != null )
      OnDamageRecieved( this, damage );
  }
  protected void RaiseHealingRecieved( int amount )
  {
    if (OnHealingRecieved != null )
      OnHealingRecieved( this, amount );
  }
  [SerializeField]
  protected DOGOMemFactory _factory;

  public Field.Tile tile;
  public Field.Directions direction;

  public int max_health;
  private int _health;
  public int damage;

  public int health
  {
    get
    {
      return _health;
    }

    protected set
    {
	  if(_health > value)
      {
        RaiseDamageRecieved( _health - value );
      }
      else
      {
        if( _health < Mathf.Min(value, max_health))
        {
          RaiseHealingRecieved(Mathf.Min(value, max_health) - _health );
        }
      }      
      _health = Mathf.Min(value, max_health);
    }
  }

  public virtual void Attacked( int damage )
  {
    health -= damage;
    if(this is Player)
      Debug.Log( "Player damaged by " + damage + "; health: " + health );
    else
      Debug.Log( "Unit at ("+tile.x +";"+tile.y+") damaged by " + damage + "; health: " + health );
    
    if ( health <= 0 )
      Die();
  }
  public virtual void Die(bool destroy = true)
  {
    //TODO
    RaiseDeath();
    tile.unit = null;
    Reset();

	if(destroy)
	{
		DestroyUnit();
    }
  }

	protected void DestroyUnit()
	{
		if (_factory != null)
			_factory.Free(this.gameObject);
		else
			Destroy(this.gameObject);
	}

	public void Clear()
	{
		DestroyUnit();
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

		_face.UpdateDirectionView(direction);
		return true;
  }

  public virtual bool ForceMove( Field.Directions direction )
  {
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
    if ( target != null )
    {
      if ( target.unit == null )
      {
        target.unit = this;
        tile.unit = null;
        tile = target;
        return true;
      }
    }
    return false;
  }
  public virtual bool Move( Field.Directions direction )
  {
    //Debug.Log( "Move called by " + GetInstanceID() );
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
    if ( target != null )
    {
      if(this == GlobalDataHolder.player)
        target.Activate();
      if ( (target.passable ) && target.unit == null )
      {
        //Debug.Log( "Moved from (" + tile.x.ToString() + ";" + tile.y.ToString() + ") to (" + target.x.ToString() + ";" + target.y.ToString() +")");
        target.unit = this;
        tile.unit = null;
        tile = target;
        return true;
      }
    }
    return false;
  }
  public virtual bool Move( Field.Tile target )
  {
    
    if ( !Turn( target ) )
      return false;
    return MoveByDirection( direction );
  }

  public virtual FieldUnit Spawn( Field.Tile position, bool new_inst = true )
  {
    
    if ( position == null )
      return null;


    if ( position.unit == null && position.passable )
    {
      //TODO
      //Instantiation
      FieldUnit fu = this;
      if ( new_inst )
      {
        if ( _factory != null )
        {
          fu = _factory.Allocate().GetComponent<FieldUnit>();
          fu.Show();
        }
        else
          fu = Instantiate( this );

		_allUnits.Add(fu);
      }
      fu.direction = Field.Directions.LEFT;
      position.unit = fu;
      fu.tile = position;
      fu._SetPosition( position.tileView.transform.position );
      fu.Initialize();
	  GlobalEventSystem.RaiseUnitSpawned(this);

	  return fu;
    }
    return null;
  }



	protected const float _INTERPOLATION_TIME = 0.3f;

	protected bool _isInterpolation = false;
	protected float _curInterTime;

	protected Vector2 _startPos;
	protected Vector2 _targetPos;

	protected void _UpdateInterpol()
	{
		if (_curInterTime >= _INTERPOLATION_TIME) _isInterpolation = false;
		if (!_isInterpolation) return;

		_curInterTime += Time.deltaTime;		

		var pos = Vector2.Lerp(_startPos, _targetPos, _curInterTime / _INTERPOLATION_TIME);
		_SetPosition(pos);
	}

	protected void _SetPosition(Vector2 pos, bool interpol = false)
	{
		if (interpol)
		{
			_isInterpolation = true;
			_curInterTime = 0;
			_startPos = this.transform.position;
			_targetPos = pos;
		}
		else
		{
            this.transform.position = new Vector3(
					pos.x, pos.y,
					this.transform.position.z
				);
		}
	}

  public virtual void Reset()
  {
    OnDeath = null;
    OnRealTimeTick = null;
    OnTurnTick = null;
	OnDamageRecieved = null;
	OnHealingRecieved = null;

		_face.HideAll();
		_isInterpolation = false;
  }


  public virtual void Initialize()
  {
    Reset();
	_face.UpdateDirectionView(direction);
	GlobalEventSystem.OnTurnTick += RaiseTurnTick;
    health = max_health;
  }

	void Update()
	{
		_UpdateInterpol();
		RaiseRealTimeTick();
	}

}