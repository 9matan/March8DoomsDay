
public class Enemy : FieldUnit
{
  public int enemy_id;
  public int score;
  public int experience;
  void EnemyTick()
  {
    var player_path = tile.PlayerSearch();
    if ( player_path != null && player_path.Count >= 2 )
    {
      if ( player_path.Count == 2 )
      {
        Turn( player_path[0] );
        Attack();
      }
      else
      {
        Move( player_path[player_path.Count - 2] );
      }
      return;
    }
    Wonder();
  }

  void Wonder()
  {
    if ( UnityEngine.Random.Range(0,100) <= 30 )
    {
      Move( tile.random_no_unit_road );
    }
  }
<<<<<<< HEAD
  public override FieldUnit Spawn( Field.Tile position )
=======

  public override bool Spawn( Field.Tile position )
>>>>>>> 553627a36f3597bf7addd92cfaaa707554d0f108
  {
    FieldUnit spawned = base.Spawn( position );
    if ( spawned != null )
      position.field.OnTick += EnemyTick;
    return spawned;
  }

<<<<<<< HEAD
=======
  public Enemy( int health, int damage )
    : base( health, damage )
  { }
>>>>>>> 553627a36f3597bf7addd92cfaaa707554d0f108
}