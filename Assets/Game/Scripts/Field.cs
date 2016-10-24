using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;



public class Field
{

  public class Tile
  {
    public enum TileTypes
    {
      EMPTY, // порожня
      WALL, // стіна
      ROAD, // дорога
      STRUCTURE, // будівля
      SPAWN, // точка спавна
      STORK, // лелека
      CURTAIN, // штора
      TURNSTILE, // турнікет
      EXIT // вихід
    }





    #region Data
    private TileTypes _type;
    public int x;
    public int y;

    public FieldUnit unit;
    public Field field;
    public Item item;

    public int enemy_to_spawn;

    public TileView tileView;
    #endregion
    #region Properties
    public Tile this[Field.Directions direction]
    {
      get
      {
        switch ( direction )
        {
          case Field.Directions.UP:
            return up;
          case Field.Directions.DOWN:
            return down;
          case Field.Directions.LEFT:
            return left;
          case Field.Directions.RIGHT:
            return right;
          default:
            return null;
        }
      }
    }

    public bool passable
    {
      get
      {
        return ( type == TileTypes.ROAD || type == TileTypes.CURTAIN );
      }
    }

    public int road_count
    {
      get
      {
        int cnt = 0;
        if ( left != null && left.type == TileTypes.ROAD )
          ++cnt;
        if ( right != null && right.type == TileTypes.ROAD )
          ++cnt;
        if ( up != null && up.type == TileTypes.ROAD )
          ++cnt;
        if ( down != null && down.type == TileTypes.ROAD )
          ++cnt;
        return cnt;
      }
    }
    public Tile random_no_unit_road
    {
      get
      {
        List<Tile> tiles = new List<Tile>();
        if ( left != null && left.type == TileTypes.ROAD && left.unit == null )
          tiles.Add( left );
        if ( right != null && right.type == TileTypes.ROAD && right.unit == null )
          tiles.Add( right );
        if ( up != null && up.type == TileTypes.ROAD && up.unit == null )
          tiles.Add( up );
        if ( down != null && down.type == TileTypes.ROAD && down.unit == null )
          tiles.Add( down );
        if ( tiles.Count == 0 )
          return null;
        return tiles[UnityEngine.Random.Range(0, tiles.Count )];
      }
    }
    public Directions random_direction
    {
      get
      {
        List<Directions> tiles = new List<Directions>();
          tiles.Add( Directions.LEFT);
          tiles.Add( Directions.RIGHT);
          tiles.Add( Directions.UP);
          tiles.Add( Directions.DOWN);
        return tiles[UnityEngine.Random.Range( 0, tiles.Count )];
      }
    }

    public TileTypes type
    {
      get
      {
        return _type;
      }

      set
      {
        if ( _type == TileTypes.SPAWN )
        {
          field.OnTurnTick -= SpawnRoutine;
        }
        if ( value == TileTypes.SPAWN )
        {
          field.OnTurnTick += SpawnRoutine;
          enemy_to_spawn = GlobalDataHolder.next_enemy_id;
        }
        if ( value == TileTypes.EXIT )
        {
          field.exit = this;
        }
        _type = value;
      }
    }

    public Tile left
    {
      get
      {
        return field[x - 1, y];
      }
    }
    public Tile right
    {
      get
      {
        return field[x + 1, y];
      }
    }
    public Tile up
    {
      get
      {
        return field[x, y - 1];
      }
    }
    public Tile down
    {
      get
      {
        return field[x, y + 1];
      }
    }
    #endregion

    public int GetNoEqMask( TileTypes type )
    {
      int mask = 0;

      if (left != null && left.type != type )
        mask |= 1;
      if (up != null && up.type != type )
        mask |= 8;
      if (right != null && right.type != type )
        mask |= 4;
      if (down != null && down.type != type )
        mask |= 2;

      return mask;
    }

    public void Activate()
    {
      GlobalEventSystem.ActivateTile( this );
      if ( unit != null )
      {
        GlobalEventSystem.ActivateUnit( unit );
        return;
      }
      if ( item != null )
        item.PickUp();
    }
    public void Attacked( int damage )
    {
      if ( unit != null )
        unit.Attacked( damage );
    }


    #region find player & leha
    public Tile path_to_player
    {
      get
      {
        int len = -1;
        Tile tile = null;
        if ( left != null && (left.passable ) && left.unit == null )
        {
          len = left.player_far;
          tile = left;
        }
        if ( right != null && (right.passable ) && right.unit == null && ( len > right.player_far || len == -1 ) )
        {
          len = right.player_far;
          tile = right;
        }
        if ( up != null && (up.passable ) && up.unit == null && ( len > up.player_far || len == -1 ) )
        {
          len = up.player_far;
          tile = up;
        }
        if ( down != null &&( down.passable ) && down.unit == null && ( len > down.player_far || len == -1 ) )
        {
          len = down.player_far;
          tile = down;
        }
        if ( len > player_far )
          return null;
        return tile;
      }
    }

    public int player_far = -1;

    public void RunPlayerFromHere( int length = 0 )
    {
      if ( length == 0 )
      {
        foreach ( var tile in field.tiles )
        {
          tile.player_far = -1;
        }
        player_far = -1;
        if ( left != null && ( left.passable ) )
          left.RunPlayerFromHere( length + 1 );
        if ( right != null && ( right.passable ) )
          right.RunPlayerFromHere( length + 1 );
        if ( up != null && ( up.passable ) )
          up.RunPlayerFromHere( length + 1 );
        if ( down != null && ( down.passable ) )
          down.RunPlayerFromHere( length + 1 );
        return;
      }
      if ( player_far == -1 || player_far > length )
      {
        player_far = length;
      }
      else
        return;
      if ( left != null && ( left.passable ) )
        left.RunPlayerFromHere( length + 1 );
      if ( right != null && ( right.passable ) )
        right.RunPlayerFromHere( length + 1 );
      if ( up != null && ( up.passable ) )
        up.RunPlayerFromHere( length + 1 );
      if ( down != null && ( down.passable ) )
        down.RunPlayerFromHere( length + 1 );
    }

    public Tile path_to_leha
    {
      get
      {
        int len = -1;
        Tile tile = null;
        if ( left != null && (left.passable ) && left.unit == null )
        {
          len = left.leha_far;
          tile = left;
        }
        if ( right != null && (right.passable ) && right.unit == null && ( len > right.leha_far || len == -1 ) )
        {
          len = right.leha_far;
          tile = right;
        }
        if ( up != null && (up.passable ) && up.unit == null && ( len > up.leha_far || len == -1 ) )
        {
          len = up.leha_far;
          tile = up;
        }
        if ( down != null && (down.passable ) && down.unit == null && ( len > down.leha_far || len == -1 ) )
        {
          len = down.leha_far;
          tile = down;
        }
        if ( len > leha_far )
          return null;
        return tile;
      }
    }
    public int leha_far = -1;
    public void RunLehaFromHere( int length = 0 )
    {
      if ( length == 0 )
      {
        foreach ( var tile in field.tiles )
        {
          tile.leha_far = -1;
        }
        leha_far = -1;
        if ( left != null && ( left.passable ) )
          left.RunLehaFromHere( length + 1 );
        if ( right != null && ( right.passable ) )
          right.RunLehaFromHere( length + 1 );
        if ( up != null && ( up.passable ) )
          up.RunLehaFromHere( length + 1 );
        if ( down != null && ( down.passable ) )
          down.RunLehaFromHere( length + 1 );
        return;
      }
      if ( leha_far == -1 || leha_far > length )
      {
        leha_far = length;
      }
      else
        return;
      if ( left != null && ( left.passable ) )
        left.RunLehaFromHere( length + 1 );
      if ( right != null && ( right.passable ) )
        right.RunLehaFromHere( length + 1 );
      if ( up != null && ( up.passable ) )
        up.RunLehaFromHere( length + 1 );
      if ( down != null && ( down.passable ) )
        down.RunLehaFromHere( length + 1 );
    }
    #endregion


    public List<Tile> SearchTile( Tile tile, int depth = 10 )
    {
      
      if ( tile == this )
        return new List<Tile>() { this };
      if ( depth == 0 )
        return null;
      List<Tile> rez = null;
      List<Tile> tmp = null;
      if ( left != null && left.passable )
        rez = left.SearchTile( tile, depth - 1 );
      if ( right != null && right.passable )
      {
        tmp = right.SearchTile( tile, depth - 1 );
        if ( tmp != null && ( rez == null || tmp.Count < rez.Count ) )
          rez = tmp;
      }
      if ( up != null && up.passable )
      {
        tmp = up.SearchTile( tile, depth - 1 );
        if ( tmp != null && ( rez == null || tmp.Count < rez.Count ) )
          rez = tmp;
      }
      if ( down != null && down.passable )
      {
        tmp = down.SearchTile( tile, depth - 1 );
        if ( tmp != null && ( rez == null || tmp.Count < rez.Count ) )
          rez = tmp;
      }
      if ( rez != null )
        rez.Add( this );
      return rez;
    }


    #region Spawn
    void SpawnRoutine()
    {
      if ( UnityEngine.Random.Range( 0, 100 ) <= 15 && spawned_count < GlobalDataHolder.max_units_per_spawn )
      {
        Enemy prototype = GlobalDataHolder.FindEnemy( enemy_to_spawn );
        if ( prototype == null )
          return;
        FieldUnit spawned = prototype.Spawn( random_no_unit_road );
        if ( spawned != null )
        {
          GlobalEventSystem.EnemySpawned( spawned );
          spawned.OnDeath += SpawnedUnitDied;
          ++spawned_count;
        }
      }
    }
    int spawned_count = 0;
    void SpawnedUnitDied(FieldUnit unit)
    {
      --spawned_count;
    }
    #endregion

    #region Generation
    public void RunRoad()
    {
      type = TileTypes.ROAD;
      //field.Print();
      foreach ( var tile in empty_neighbours )
      {
        if ( tile.road_count <= 1 )
          tile.RunRoad();
        else
          tile.type = TileTypes.STRUCTURE;
      }
    }


    
        
        
    public void BlowUp( int power )
    {
      if ( !field.LineContainsTurnstule( this ) )
        return;
      if ( UnityEngine.Random.Range( 0, 100 ) <= power )
      {
        type = TileTypes.ROAD;
        if ( left != null && left.type != TileTypes.WALL )
          left.BlowUp( power / 2 );
        if ( right != null && right.type != TileTypes.WALL )
          right.BlowUp( power / 2 );
        if ( up != null && up.type != TileTypes.WALL )
          up.BlowUp( power / 2 );
        if ( down != null && down.type != TileTypes.WALL )
          down.BlowUp( power / 2 );
      }
    }
    public Tile[] empty_neighbours
    {
      get
      {
        List<Tile> tiles = new List<Tile>();
        if ( left != null && left.type == TileTypes.EMPTY )
          tiles.Add( left );
        if ( right != null && right.type == TileTypes.EMPTY )
          tiles.Add( right );
        if ( up != null && up.type == TileTypes.EMPTY )
          tiles.Add( up );
        if ( down != null && down.type == TileTypes.EMPTY )
          tiles.Add( down );
        return tiles.OrderBy( x => UnityEngine.Random.value ).ToArray();
      }
    }

    


    #endregion

    public Tile( Field field, int x, int y )
    {
      this.field = field;
      this.x = x;
      this.y = y;
      type = TileTypes.EMPTY;
      unit = null;
    }

  }

  
  public event GlobalEventSystem.NoArgsEvent OnTurnTick;

  public void RaiseTurnTick()
  {

    if ( OnTurnTick != null )
      OnTurnTick();
  }
  public enum Directions
  {
    UP,
    DOWN,
    LEFT,
    RIGHT,
    NONE
  }

  Tile[,] tiles;
  private int _size_x;
  private int _size_y;
  public Tile exit;

  public List<Enemy> enemies;

  #region Properties
  public Tile this[int x, int y]
  {
    get
    {
      if ( tiles == null )
        return null;
      if ( x < 0 || y < 0 || y >= size_y || x >= size_x )
        return null;
      return tiles[x, y];
    }
  }
  public int size_x
  {
    get
    {
      return _size_x;
    }

    private set
    {
      _size_x = value;
    }
  }

  public int size_y
  {
    get
    {
      return _size_y;
    }

    private set
    {
      _size_y = value;
    }
  }

  public Tile random_road
  {
    get
    {
      List<Tile> rez = new List<Tile>();
      foreach ( var tile in tiles )
      {
        if ( tile.type == Tile.TileTypes.ROAD )
          rez.Add( tile );
      }
      if ( rez.Count == 0 )
        return null;

      
      return rez[UnityEngine.Random.Range(0,rez.Count-1)];
    }
  }
  public Tile random_no_unit_road
  {
    get
    {
      List<Tile> rez = new List<Tile>();
      foreach ( var tile in tiles )
      {
        if ( tile.type == Tile.TileTypes.ROAD && tile.unit == null )
          rez.Add( tile );
      }
      if ( rez.Count == 0 )
        return null;
      return rez[UnityEngine.Random.Range( 0, rez.Count )];
    }
  }
  public Tile random_no_item_road
  {
    get
    {
      List<Tile> rez = new List<Tile>();
      foreach ( var tile in tiles )
      {
        if ( tile.type == Tile.TileTypes.ROAD && tile.item == null )
          rez.Add( tile );
      }
      if ( rez.Count == 0 )
        return null;
      return rez[UnityEngine.Random.Range( 0, rez.Count )];
    }
  }
  public Tile random_structure
  {
    get
    {
      List<Tile> rez = new List<Tile>();
      foreach ( var tile in tiles )
      {
        if ( tile.type == Tile.TileTypes.STRUCTURE )
          rez.Add( tile );
      }
      if ( rez.Count == 0 )
        return null;
      return rez[UnityEngine.Random.Range( 0, rez.Count )];
    }
  }
  public Tile random_structure_near_road
  {
    get
    {
      List<Tile> rez = new List<Tile>();
      foreach ( var tile in tiles )
      {
        if ( tile.type == Tile.TileTypes.STRUCTURE && tile.road_count > 0 )
          rez.Add( tile );
      }
      if ( rez.Count == 0 )
        return null;
      return rez[UnityEngine.Random.Range( 0, rez.Count )];
    }
  }
  #endregion



  #region Generation
  private bool is_generation_valid
  {
    get
    {
      int struct_count = 0;
      foreach ( var tile in tiles )
      {
        if ( tile.type == Tile.TileTypes.STRUCTURE )
          ++struct_count;
      }
      return struct_count < size_x * size_y / 2 && !has_closed_areas;
    }
  }
  
  private bool has_closed_areas
  {
    get
    {
      
      random_road.RunPlayerFromHere();
      foreach ( var tile in tiles )
      {
        if ( tile.type == Tile.TileTypes.ROAD && tile.player_far == -1 )
        {
          return false;
        }
      }
      return false;
    }
  }
  private void PlaceBlowUps(int count)
  {
    for ( int i = 0; i < count; i++ )
    {
      Tile tile = random_structure;
      if ( tile == null )
        break;
      tile.BlowUp( ( size_x * size_y ) / ( ( size_x + size_y ) / 10 + 1 ) );
    }
  }
  private bool LineContainsTurnstule( Tile tile )
  {
      for ( int d = 1; d < size_x - 1; d++ )
      {
        if ( tiles[d, tile.y].type == Tile.TileTypes.TURNSTILE )
          return false;
      }
      for ( int d = 1; d < size_x - 1; d++ )
      {
        if ( tiles[tile.x, d].type == Tile.TileTypes.TURNSTILE )
          return false;
      }

    return true;
  }
  private bool TurnstuleAllowed(Tile tile, bool horisontal)
  {
    if(horisontal)
    {
      for ( int d = 1; d < size_x - 1; d++ )
      {
        if ( tiles[d, tile.y + 1].type == Tile.TileTypes.TURNSTILE || tiles[d, tile.y - 1].type == Tile.TileTypes.TURNSTILE )
          return false;
      }
    }
    else
    {
      for ( int d = 1; d < size_x - 1; d++ )
      {
        if ( tiles[tile.x + 1, d].type == Tile.TileTypes.TURNSTILE || tiles[tile.x - 1, d].type == Tile.TileTypes.TURNSTILE )
          return false;
      }

    }
    
    return true;
  }
  private void PlaceTurnstule(int count)
  {
    bool horisontal = true;
    for ( int i = 0; i < count; i++ )
    {
      Tile tile = null;
      do
      {
        tile = random_structure_near_road;

      } while ( !TurnstuleAllowed( tile, horisontal ) || tile.x < 2 || tile.x>size_x - 3 || tile.y < 2 || tile.y > size_y - 3 );
      
      
      if ( horisontal )
      {
        for ( int j = 1; j < size_x - 1; j++ )
        {
          tiles[j, tile.y].type = Tile.TileTypes.STRUCTURE;
          if ( tile.y + 1 < size_y - 1 )
            tiles[j, tile.y + 1].type = Tile.TileTypes.ROAD;
          if ( tile.y - 1 > 0 )
            tiles[j, tile.y - 1].type = Tile.TileTypes.ROAD;
        }
      }
      else
      {
        for ( int j = 1; j < size_y - 1; j++ )
        {
          tiles[tile.x, j].type = Tile.TileTypes.STRUCTURE;
          if ( tile.x + 1 < size_x - 1 )
            tiles[tile.x + 1, j].type = Tile.TileTypes.ROAD;
          if ( tile.x - 1 > 0 )
            tiles[tile.x - 1, j].type = Tile.TileTypes.ROAD;
        }
      }
      tile.type = Tile.TileTypes.TURNSTILE;

    }
  }
  private void PlaceSpawns(int count)
  {
    for ( int i = 0; i < count; i++ )
    {
      Tile tile = random_structure_near_road;
      if ( tile == null )
        break;
      tile.type = Tile.TileTypes.SPAWN;

    }
  }
  private void PlaceStorks(int count)
  {
    for ( int i = 0; i < count; i++ )
    {
      Tile tile = random_structure_near_road;
      if ( tile == null )
        break;
      tile.type = Tile.TileTypes.STORK;
    }
  }
  private void PlaceFlowers(int count)
  {
    for ( int i = 0; i < count; i++ )
    {
      Tile tile = random_no_item_road;
      if ( tile == null )
        break;
      Item item = Item.CreateFlower();
      while ( !item.Spawn( tile ) )
      {
        tile = random_no_item_road;
      }
    }
  }
  private void PlaceCoins( int count )
  {
    for ( int i = 0; i < count; i++ )
    {
      Tile tile = random_no_item_road;
      if ( tile == null )
        break;
      Item item = Item.CreateCoin();
      while ( !item.Spawn( tile ) )
      {
        tile = random_no_item_road;
      }
    }
  }
  private void PlaceCurtain(int count)
  {
    for ( int i = 0; i < count; i++ )
    {
      Tile tile = random_structure_near_road;
      if ( tile == null )
        break;
      tile.type = Tile.TileTypes.CURTAIN;
    }
  }
  private void PlaceExit()
  {
    Tile tile = random_structure_near_road;
    if ( tile != null )
      tile.type = Tile.TileTypes.EXIT;
  }
  public void Generate( int size_x, int size_y )
  {
    this.size_x = size_x;
    this.size_y = size_y;
    tiles = new Tile[size_x, size_y];

    for ( int i = 0; i < size_x; i++ )
    {
      for ( int j = 0; j < size_y; j++ )
      {
        tiles[i, j] = new Tile( this, i, j );
      }
    }

    for ( int i = 0; i < size_x; i++ )
    {
      tiles[i, 0].type = Tile.TileTypes.WALL;
      tiles[i, size_y - 1].type = Tile.TileTypes.WALL;
    }
    for ( int i = 0; i < size_y; i++ )
    {
      tiles[0, i].type = Tile.TileTypes.WALL;
      tiles[size_x - 1, i].type = Tile.TileTypes.WALL;
    }


    int x = UnityEngine.Random.Range(0, size_x);
    int y = UnityEngine.Random.Range(0, size_y);
    tiles[x, y].RunRoad();


    for ( int i = 0; i < size_x; i++ )
    {
      for ( int j = 0; j < size_y; j++ )
      {
        if ( tiles[i, j].type == Tile.TileTypes.EMPTY )
        {
          tiles[i, j].type = Tile.TileTypes.STRUCTURE;
        }
      }
    }


    //Math.Sqrt( size_x + size_y )
    //Math.Sqrt( Math.Sqrt( size_x + size_y ) ) + 1

    if ( ScenesManager.GetLevelNumber() == 2 )
    {
      PlaceTurnstule( (int)( Math.Sqrt( size_x + size_y ) / 3 ) );
      PlaceBlowUps( (int)Math.Sqrt( size_x + size_y ) / 4 );
    }
    else
      PlaceBlowUps( (int)Math.Sqrt( size_x + size_y ) );

    for ( int i = 0; i < size_x; i++ )
    {
      tiles[i, 0].type = Tile.TileTypes.WALL;
      tiles[i, size_y - 1].type = Tile.TileTypes.WALL;
    }
    for ( int i = 0; i < size_y; i++ )
    {
      tiles[0, i].type = Tile.TileTypes.WALL;
      tiles[size_x - 1, i].type = Tile.TileTypes.WALL;
    }

    PlaceSpawns( (int)Math.Sqrt( size_x + size_y ) );
    PlaceCurtain( (int)( Math.Sqrt( size_x + size_y ) / 3 ) );
    PlaceStorks( (int)Math.Sqrt( Math.Sqrt( size_x + size_y ) ) + 1 );
    PlaceExit();
    PlaceFlowers( (int)Math.Sqrt( size_x + size_y ) );
    PlaceCoins( (int)Math.Sqrt( size_x + size_y ) );

    



    while ( !is_generation_valid )
      Generate( size_x, size_y );
  }
  #endregion
  public void SpawnPlayer()
  {
    if ( GlobalDataHolder.player == null )
      return;
    Tile tile = random_road;
    while ( exit.SearchTile( tile ) != null )
    {
      tile = random_road;
    }
    GlobalDataHolder.player.Spawn( tile, false );
    GlobalEventSystem.OnTurnTick += RaiseTurnTick;
  }
}
