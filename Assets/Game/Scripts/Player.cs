using UnityEngine;

public class Player : FieldUnit
{

	public int level;

	public int exp_to_level
  {
    get
    {
      return level * 70;
    }
  }

  private int _experience;
  public enum LevelUpgradeList
  {
    HEALTH,
    DAMAGE,
    SPEED // slow down TurnTick
  }
  public int experience
  {
    get
    {
      return _experience;
    }

    set
    {
	  GlobalEventSystem.PlayerExperienceChanged( value - _experience );
      _experience = value;
      
      if (_experience > exp_to_level )
      {
        _experience -= exp_to_level;
        ++level;
		GlobalEventSystem.PlayerLevelUp();
        
      }
    }
  }

	public int used_level { get; protected set; }
	public int free_skill_point
	{
		get { return level - used_level; }
	}

	public void UpLevel(LevelUpgradeList up)
	{
		if (used_level >= level) return;

		++used_level;

		switch (up)
		{
			case LevelUpgradeList.HEALTH:
				max_health += GlobalDataHolder.LEVEL_UP_BONUS_HEALTH;
				health += GlobalDataHolder.LEVEL_UP_BONUS_HEALTH;
				break;
			case LevelUpgradeList.DAMAGE:
				damage += GlobalDataHolder.LEVEL_UP_BONUS_DAMAGE;
				break;
			case LevelUpgradeList.SPEED:
				GlobalDataHolder.turn_tick_time += GlobalDataHolder.LEVEL_UP_SLOW_TICK_AMOUNT;
				break;
			default:
				break;
		}
	}

  //TODO
  //change to false
  public bool skill_achieved = true;

  public float skill_reuse_time = 0f;

  public void UseSkill()
  {

    tile[direction].Attacked( damage * 2 );
    if ( tile[direction].type == Field.Tile.TileTypes.STRUCTURE )
    {
      tile[direction].type = Field.Tile.TileTypes.ROAD;
      tile[direction].tileView.UpdateView( Field.Tile.TileTypes.STRUCTURE );
      FieldUpdate();
      GlobalEventSystem.StructureDestroyed( tile[direction] );
    }

  }
  public void UseMedicine()
  {
    if ( GlobalDataHolder.medicine > 0 )
    {
      GlobalDataHolder.medicine--;
      health += GlobalDataHolder.health_for_medicine;
      GlobalEventSystem.PlayerMedicineUsed();
    }
  }
  public bool CallLeha()
  {
    FieldUpdate();
    return GlobalEventSystem.SummonLeha();
  }

  #region Curtain
  public bool uses_curtain = false;
  private float time_to_curtain_tick = 0f;
  private void CurtainTick()
  {
    time_to_curtain_tick += Time.deltaTime;
    if(time_to_curtain_tick >= GlobalDataHolder.CURTAIN_TICK_VALUE )
    {
      time_to_curtain_tick = 0f;
      health += GlobalDataHolder.CURTAIN_HEALTH_PER_TICK;
    }
  }
  private void StartCurtainWork( Field.Tile tile )
  {
    uses_curtain = true;
    OnRealTimeTick += CurtainTick;
  }
  private void EndCurtainWork( Field.Tile tile )
  {
    uses_curtain = false;
    OnRealTimeTick -= CurtainTick;
  }
  #endregion

  public override void Die(bool destroy = true)
  {
    base.Die(false);
    GlobalEventSystem.OnTurnTick -= FieldUpdate;
  }

  public bool movement_disabled = false;

  public override bool Move( Field.Directions direction )
  {
    if(movement_disabled)
    {
      this.direction = direction;
      return false;
    }
    bool is_valid = base.Move( direction );
    if(is_valid)
      FieldUpdate();
    return is_valid;
  }

  private void FieldUpdate()
  {
    tile.RunPlayerFromHere();
  }

	public bool isInit { get; protected set; }
	public string playerName
	{
		get
		{
			if (_playerName == "")
				_playerName = "TORt";
            return _playerName;
		}

		set { _playerName = value; }
	}

	protected string _playerName;

  public override void Initialize()
	{
		if(App.isFirstLevel)
		{
			level = 1;
			_experience = 0;
			used_level = 1;
			playerName = "UnnamedTort";

			App.isFirstLevel = false;
        }

        base.Initialize();
		
		GlobalEventSystem.OnCurtainActivate += StartCurtainWork;
		GlobalEventSystem.OnCurtainLeave += EndCurtainWork;
		GlobalEventSystem.OnTurnTick += FieldUpdate;
		tile.RunPlayerFromHere();

		_ui.Initialize();
		isInit = true;
	}

	[SerializeField]
	protected PlayerUI _ui;

	public PlayerUI ui { get { return _ui; } }

	public override void Reset()
	{
		base.Reset();
		isInit = false;		
	}

	public override bool MoveByDirection(Field.Directions direction)
	{
		var isValid = base.MoveByDirection(direction);

		if (uses_curtain && tile.type != Field.Tile.TileTypes.CURTAIN)
			GlobalEventSystem.RaiseCurtainLeave(tile);

		return isValid;
	}

	public void DebugLevelEnd()
	{
		GlobalEventSystem.DebugRaiseLevelEnd();
	}

}
