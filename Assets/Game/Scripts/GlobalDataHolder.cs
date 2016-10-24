using System.Collections;
using UnityEngine;

public class GlobalDataHolder : MonoBehaviour
{
  #region player
  public int _player_score = 0;
  public int _player_gold = 0;
  public int _phone_cells = 0;
  public int _medicine = 0;
  public Player _player;
  public Leha _leha;
  #endregion
  public Field _field;
  public float _turn_tick_time = 5f;
  public int _max_units_per_spawn = 4;

  public float _time_left = 540f;
  [SerializeField]
  protected float _levelTime;

  public Enemy[] enemies;
  public FieldUnit _quest_unit;


  public int _min_gold_per_coin = 1;
  public int _max_gold_per_coin = 5;
  #region Scores
  public int _min_score_per_flower = 5;
  public int _max_score_per_flower = 20;

  public int _score_per_coin = 5;
  public int _score_per_time = 1;
  public int _score_per_phone_cell = 7;
  public int _score_per_medicine = 5;

	public static float levelTime
	{
		get
		{
			if (data != null)
				return data._levelTime;
			return 0.0f;
		}

		set
		{
			if (data != null)
				data._levelTime = value;
        }
	}

  public static int score_from_coins
  {
    get
    {
      return score_per_coin * player_gold;
    }
  }
  public static int score_from_time
  {
    get
    {
      return score_per_time * (int)time_left;
    }
  }
  public static int score_from_phone_cells
  {
    get
    {
      return score_per_phone_cell * phone_cells;
    }
  }
  public static int score_from_medicine
  {
    get
    {
      return score_per_medicine * medicine;
    }
  }
  public static int score_from_everything
  {
    get
    {
      return player_score + score_from_coins + score_from_time + score_from_phone_cells + score_from_phone_cells + score_from_medicine;
    }
  }

  #endregion

  #region shop items characts
  public int _health_for_medicine = 20;
  public int _big_hammer_damage_bonus = 5;
  public float _time_boost_amount = 20f;
  #endregion
  #region curtain
  public const float CURTAIN_TICK_VALUE = 0.7f;
  public const int CURTAIN_HEALTH_PER_TICK = 1;
  #endregion
  #region levelup
  public const int LEVEL_UP_BONUS_HEALTH = 100;
  public const int LEVEL_UP_BONUS_DAMAGE = 6;
  public const float LEVEL_UP_SLOW_TICK_AMOUNT = 0.2f;
  public const int LEVEL_UP_TIME_BONUS_AMOUNT = 20;
  #endregion
  public int _gold_to_pass_turnstile = 5;


  private static GlobalDataHolder data;

	public static void SetField(Field __field)
	{
		data._field = __field;
	}

  public enum DifficultyLevels
  {
    NOT_SET,
    EASY,
    NORMAL,
    HARD,
    UNREAL
  }
  public DifficultyLevels _difficulty = DifficultyLevels.NOT_SET;
  private static void SetDifficultyLevel()
  {
    if ( data == null )
      return;
    switch ( difficulty )
    {
      case DifficultyLevels.EASY:
        phone_cells = 2;
        medicine = 3;
        player_gold = 100;
        player.skill_achieved = true;
        break;
      case DifficultyLevels.NORMAL:
        foreach ( var enemy in data.enemies )
        {
          enemy.max_health = (int)( enemy.max_health * 1.5f );
          enemy.damage = (int)( enemy.damage * 1.5f );
          enemy.experience = (int)( enemy.experience * 0.9f );
          enemy.score = (int)( enemy.score * 1.5f );
        }
        time_left *= 0.7f;
        min_gold_per_coin = (int)( min_gold_per_coin * 1.5f );
        max_gold_per_coin = (int)( max_gold_per_coin * 1.5f );
        min_score_per_flower = (int)( min_score_per_flower * 1.5f );
        max_score_per_flower = (int)( max_score_per_flower * 1.5f );

        score_per_coin = (int)( score_per_coin * 1.5f );
        score_per_medicine = (int)( score_per_medicine * 1.5f );
        score_per_phone_cell = (int)( score_per_phone_cell * 1.5f );
        score_per_time = (int)( score_per_time * 1.5f );

        phone_cells = 1;
        medicine = 2;
        player_gold = 0;
        player.skill_achieved = false;
        break;
      case DifficultyLevels.HARD:
        foreach ( var enemy in data.enemies )
        {
          enemy.max_health = (int)( enemy.max_health * 2f );
          enemy.damage = (int)( enemy.damage * 2f );
          enemy.experience = (int)( enemy.experience * 0.8f );
          enemy.score = (int)( enemy.score * 2f );
        }
        player.max_health = (int)( player.max_health * 0.8f );
        max_units_per_spawn = 5;
        time_left *= 0.6f;
        min_gold_per_coin = (int)( min_gold_per_coin * 1.8f );
        max_gold_per_coin = (int)( max_gold_per_coin * 1.8f );
        min_score_per_flower = (int)( min_score_per_flower * 1.8f );
        max_score_per_flower = (int)( max_score_per_flower * 1.8f );

        score_per_coin = (int)( score_per_coin * 2f );
        score_per_medicine = (int)( score_per_medicine * 2f );
        score_per_phone_cell = (int)( score_per_phone_cell * 2f );
        score_per_time = (int)( score_per_time * 2f );

        phone_cells = 1;
        medicine = 0;
        player_gold = 0;
        player.skill_achieved = false;
        break;
      case DifficultyLevels.UNREAL:
        foreach ( var enemy in data.enemies )
        {
          enemy.max_health = (int)( enemy.max_health * 3f );
          enemy.damage = (int)( enemy.damage * 3f );
          enemy.experience = (int)( enemy.experience * 0.7f );
          enemy.score = (int)( enemy.score * 3f );
        }
        player.max_health = (int)( player.max_health * 0.7f );
        player.damage = (int)( player.damage * 0.7f );
        max_units_per_spawn = 6;
        time_left *= 0.5f;
        min_gold_per_coin = (int)( min_gold_per_coin * 3f );
        max_gold_per_coin = (int)( max_gold_per_coin * 3f );
        min_score_per_flower = (int)( min_score_per_flower * 3f );
        max_score_per_flower = (int)( max_score_per_flower * 3f );

        score_per_coin = (int)( score_per_coin * 3f );
        score_per_medicine = (int)( score_per_medicine * 3f );
        score_per_phone_cell = (int)( score_per_phone_cell * 3f );
        score_per_time = (int)( score_per_time * 3f );

        gold_to_pass_turnstile = (int)( gold_to_pass_turnstile * 3f );

        phone_cells = 0;
        medicine = 0;
        player_gold = 0;
        player.skill_achieved = false;
        break;
      default:
        break;
    }
  }

	public static bool isLevelStart { get; protected set; }

  public static DifficultyLevels difficulty
  {
    get
    {
      if ( data != null )
        return data._difficulty;
      return 0;
    }
    set
    {
      if ( data != null )
      {
        if ( data._difficulty != DifficultyLevels.NOT_SET )
          return;
        data._difficulty = value;
        SetDifficultyLevel();
      }
    }
  }

  public static int gold_to_pass_turnstile
  {
    get
    {
      if ( data != null )
        return data._gold_to_pass_turnstile;
      return 0;
    }
    set
    {
      if ( data != null )
        data._gold_to_pass_turnstile = value;
    }
  }
  public static int score_per_coin
  {
    get
    {
      if ( data != null )
        return data._score_per_coin;
      return 0;
    }
    set
    {
      if ( data != null )
        data._score_per_coin = value;
    }
  }
  public static int score_per_time
  {
    get
    {
      if ( data != null )
        return data._score_per_time;
      return 0;
    }
    set
    {
      if ( data != null )
        data._score_per_time = value;
    }
  }
  public static int score_per_phone_cell
  {
    get
    {
      if ( data != null )
        return data._score_per_phone_cell;
      return 0;
    }
    set
    {
      if ( data != null )
        data._score_per_phone_cell = value;
    }
  }
  public static int score_per_medicine
  {
    get
    {
      if ( data != null )
        return data._score_per_medicine;
      return 0;
    }
    set
    {
      if ( data != null )
        data._score_per_medicine = value;
    }
  }


  public static int min_score_per_flower
  {
    get
    {
      if ( data != null )
        return data._min_score_per_flower;
      return 0;
    }
    set
    {
      if ( data != null )
        data._min_score_per_flower = value;
    }
  }
  public static int max_score_per_flower
  {
    get
    {
      if ( data != null )
        return data._max_score_per_flower;
      return 0;
    }
    set
    {
      if ( data != null )
        data._max_score_per_flower = value;
    }
  }
  public static int min_gold_per_coin
  {
    get
    {
      if ( data != null )
        return data._min_gold_per_coin;
      return 0;
    }
    set
    {
      if ( data != null )
        data._min_gold_per_coin = value;
    }
  }
  public static int max_gold_per_coin
  {
    get
    {
      if ( data != null )
        return data._max_gold_per_coin;
      return 0;
    }
    set
    {
      if ( data != null )
        data._max_gold_per_coin = value;
    }
  }

  public static int max_units_per_spawn
  {
    get
    {
      if ( data != null )
        return data._max_units_per_spawn;
      return 0;
    }
    set
    {
      if ( data != null )
        data._max_units_per_spawn = value;
    }
  }
  public static float turn_tick_time
  {
    get
    {
      if ( data != null )
        return data._turn_tick_time;
      return 5f;
    }
    set
    {
      if ( data != null )
        data._turn_tick_time = value;
    }
  }
  public static FieldUnit quest_unit
  {
    get
    {
      if ( data != null )
        return data._quest_unit;
      return null;
    }
  }
  public static int health_for_medicine
  {
    get
    {
      if ( data != null )
        return data._health_for_medicine;
      return 0;
    }
    set
    {
      if ( data != null )
        data._health_for_medicine = value;
    }
  }
  public static int big_hammer_damage_bonus
  {
    get
    {
      if ( data == null )
        return 0;
      return data._big_hammer_damage_bonus;
    }
  }
  public static float time_boost_amount
  {
    get
    {
      if ( data == null )
        return 0;
      return data._time_boost_amount;
    }
  }
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
      {
        GlobalEventSystem.PlayerScoreChanged( value - data._player_score );
        data._player_score = value;
      }
    }
  }
  public static int player_gold
  {
    get
    {
      if ( data != null )
        return data._player_gold;
      return 0;
    }
    set
    {
      if ( data != null && value >= 0 )
      {
        GlobalEventSystem.PlayerGoldChanged( value - data._player_gold );
        data._player_gold = value;
      }
    }
  }
  public static int phone_cells
  {
    get
    {
      if ( data != null )
        return data._phone_cells;
      return 0;
    }
    set
    {
      if ( data != null )
        data._phone_cells = value;
    }
  }
  public static int medicine
  {
    get
    {
      if ( data != null )
        return data._medicine;
      return 0;
    }
    set
    {
      if ( data != null )
        data._medicine = value;
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
  public static Leha leha
  {
    get
    {
      if ( data != null )
        return data._leha;
      return null;
    }
  }
  public static Field field
  {
    get
    {
      if ( data != null )
        return data._field;
      return null;
    }
  }
  public static Enemy FindEnemy(int enemy_id)
  {
    if ( data == null )
      return null;
    foreach ( var enemy in data.enemies )
    {
      if ( enemy.enemy_id == enemy_id )
        return enemy;
    }
    return null;
  }

  private static int num_enemy = 0;

	private static int[][] _levels_en = new int[][]
		  {
			new int[] {0, 1},
			new int[] {3},
			new int[] {0, 1, 2}
		  };

  public static int next_enemy_id
  {
    get
    {
      if ( data == null || data.enemies.Length == 0 )
        return -1;

			var lvl = ScenesManager.GetLevelNumber() - 1;
			return data.enemies[ _levels_en[lvl][ (num_enemy++) % _levels_en[lvl].Length] ].enemy_id;
 /*     if ( num_enemy >= 2 )
        num_enemy = 0;
//		Debug.LogFormat("lvl: {0} num: {1}", ScenesManager.GetLevelNumber(), num_enemy);
      return data.enemies[(ScenesManager.GetLevelNumber() - 1 + num_enemy++) % data.enemies.Length].enemy_id;
*/
	  }
  }

  public static float time_left
  {
    get
    {
      if ( data != null )
        return data._time_left;
      return 0f;
    }

    set
    {
      if ( data != null )
	  {
        data._time_left = value;
		data._levelTime = Mathf.Max(data._levelTime, data._time_left);
	  }
    }
  }



  private void TimerStart()
  {
	isLevelStart = true;
    GlobalEventSystem.OnRealTimeTick += Timer;
  }
  private void TimerStop()
  {
	GlobalEventSystem.OnRealTimeTick -= Timer;
	isLevelStart = false;
  }
  private void Timer()
  {
    time_left -= Time.deltaTime;
    if (time_left<=0)
    {
      time_left = 0;
      GlobalEventSystem.OnLevelStart -= TimerStart;
      GlobalEventSystem.OnLevelEnd -= TimerStop;
      TimerStop();
      GlobalEventSystem.TimerElapsed();
    }
  }

  void LevelUpTimeBonus()
  {
    time_left += LEVEL_UP_TIME_BONUS_AMOUNT;
  }
  void EnforceEnemy()
  {
    foreach ( var enemy in enemies )
    {
      enemy.max_health = (int)( enemy.max_health * 1.25f );
      enemy.damage = (int)( enemy.damage * 1.25f );
    }
  }

  void Awake()
  {
	data = this;
	difficulty = App.difficulty;
    GlobalEventSystem.OnPlayerLevelUp += LevelUpTimeBonus;
    GlobalEventSystem.OnLevelEnd += EnforceEnemy;

//    _time_left = _levelTime;
  }

  void Update()
  {
    GlobalEventSystem.RaiseRealTimeTick();
  }

	protected void OnLevelWasLoaded(int level)
	{

        if (level == ScenesManager.levelStartInd || ScenesManager.isLoadingScene || ScenesManager.isShopScene || ScenesManager.isDialogScene) return;

		_Reset();

		if (ScenesManager.IsLevel(level))
			_InitAtNewLevel();
		else
			Destroy(this.gameObject);
    }

	protected void _Reset()
	{
        player.Hide();
		leha.Hide();
		isVictory = true;		

		GlobalEventSystem.OnLevelStart += TimerStart;
		GlobalEventSystem.OnLevelEnd += TimerStop;
		GlobalEventSystem.OnGameOver += _OnGameover;
    }

	[Header("Unity")]
	[SerializeField]
	protected EffectManager _effectManager;
	[SerializeField]
	protected TaskManager _taskManager;

	public static bool isVictory { get; protected set; }

	protected void _InitAtNewLevel()
	{
		Debug.Log("Global Init");
		player.Show();
		leha.Show();

		_levelTime = _time_left;
        _effectManager.Initialize();
		_taskManager.Initialize();
		
        GlobalEventSystem.OnLehaDisappear += _OnLehaDisappear;
		
	}

	protected void _OnLehaDisappear()
	{
		leha.transform.position = this.transform.position;
	}

	protected void _OnGameover()
	{
		isVictory = false;
    }

	static public void Clear()
	{
		player.Hide();
		leha.Hide();
	}

}
