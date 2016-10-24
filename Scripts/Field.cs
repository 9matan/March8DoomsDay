using System;
using System.Collections.Generic;
using System.Linq;
<<<<<<< HEAD
using System.Text;
using System.Threading;
using UnityEngine;


=======
>>>>>>> 553627a36f3597bf7addd92cfaaa707554d0f108

public class Field
{

  public class Tile
  {
    public enum TileTypes
    {
      EMPTY,
      WALL,
      ROAD,
      STRUCTURE,
      SPAWN,
      STORK
    }
    #region Data
    private TileTypes _type;
    public int x;
    public int y;

    public FieldUnit unit;
    public Field field;
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
        var arr = tiles.OrderBy( x => UnityEngine.Random.value ).ToArray();
        if ( arr.Length == 0 )
          return null;
        return arr[0];
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
          field.OnTick -= SpawnRoutine;
        }
        if ( value == TileTypes.SPAWN )
        {
          field.OnTick += SpawnRoutine;
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


    public void Attacked( int damage )
    {
      if ( unit != null )
        unit.Attacked( damage );
    }

    public List<Tile> PlayerSearch( int depth = 3 )
    {
      if ( unit == field.player )
        return new List<Tile>() { this };
      if ( depth == 0 )
        return null;
      List<Tile> rez = null;
      List<Tile> tmp = null;
      if ( left != null && left.type == TileTypes.ROAD )
        rez = left.PlayerSearch( depth - 1 );
      if ( right != null && right.type == TileTypes.ROAD )
      {
        tmp = right.PlayerSearch( depth - 1 );
        if ( tmp != null && ( rez == null || tmp.Count < rez.Count ) )
          rez = tmp;
      }
      if ( up != null && up.type == TileTypes.ROAD )
      {
        tmp = up.PlayerSearch( depth - 1 );
        if ( tmp != null && ( rez == null || tmp.Count < rez.Count ) )
          rez = tmp;
      }
      if ( down != null && down.type == TileTypes.ROAD )
      {
        tmp = down.PlayerSearch( depth - 1 );
        if ( tmp != null && ( rez == null || tmp.Count < rez.Count ) )
          rez = tmp;
      }
      if ( rez != null )
        rez.Add( this );
      return rez;
    }


    public Enemy prototype = null;
    void SpawnRoutine()
    {
      if ( UnityEngine.Random.Range(0,100) <= 15 )
      {
        FieldUnit spawned = prototype.Spawn( random_no_unit_road );
        if(spawned != null)
        {
          GlobalEventSystem.EnemySpawned( spawned );
        }
      }
    }


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
      if ( UnityEngine.Random.Range(0,100) <= power )
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

  
  public event GlobalEventSystem.NoArgsEvent OnTick;

  public void RaiseTick()
  {

    if ( OnTick != null )
      OnTick();
  }
  public enum Directions
  {
    UP,
    DOWN,
    LEFT,
    RIGHT
  }

  Tile[,] tiles;
  private int _size_x;
  private int _size_y;

  public Player player;

  public List<Enemy> enemies;

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

    for ( int i = 0; i < Math.Sqrt( size_x + size_y ); i++ )
    {
      x = UnityEngine.Random.Range( 0, size_x);
      y = UnityEngine.Random.Range(0, size_y);
      if ( tiles[x, y].type == Tile.TileTypes.STRUCTURE )
      {
        tiles[x, y].BlowUp( ( size_x * size_y ) / ( ( size_x + size_y ) / 10 + 1 ) );
      }
      else
        --i;
    }

    for ( int i = 0; i < Math.Sqrt( size_x + size_y ); i++ )
    {
      x = UnityEngine.Random.Range( 0, size_x );
      y = UnityEngine.Random.Range( 0, size_y );
      if ( tiles[x, y].type == Tile.TileTypes.STRUCTURE && tiles[x, y].road_count > 0 )
      {
        tiles[x, y].type = Tile.TileTypes.SPAWN;
      }
      else
        --i;
    }
    for ( int i = 0; i < Math.Sqrt( Math.Sqrt( size_x + size_y ) ) + 1; i++ )
    {
      x = UnityEngine.Random.Range( 0, size_x );
      y = UnityEngine.Random.Range( 0, size_y );
      if ( tiles[x, y].type == Tile.TileTypes.STRUCTURE && tiles[x, y].road_count > 0 )
      {
        tiles[x, y].type = Tile.TileTypes.STORK;
      }
      else
        --i;
    }

  }

  public void SpawnPlayer()
  {
    if ( player == null )
      return;
    int x = UnityEngine.Random.Range(0, size_x);
    int y = UnityEngine.Random.Range(0, size_y);
    while ( tiles[x, y].type != Tile.TileTypes.ROAD )
    {
      x = UnityEngine.Random.Range( 0, size_x );
      y = UnityEngine.Random.Range( 0, size_y );
    }
    player.Spawn( tiles[x, y] );
  }
  /*
  public static void ClearCurrentConsoleLine()
  {
    int currentLineCursor = Console.CursorTop;
    Console.SetCursorPosition( 0, Console.CursorTop );
    Console.Write( new string( ' ', Console.WindowWidth ) );
    Console.SetCursorPosition( 0, currentLineCursor );
  }

  public void Print()
  {
    if ( player == null )
    {
      Console.Clear();
      Console.WriteLine( "GAME OVER" );
    }
    Console.SetCursorPosition( 0, 0 );
    for ( int i = 0; i < size_y; i++ )
    {
      for ( int j = 0; j < size_x; j++ )
      {
        if ( tiles[j, i].unit != null )
        {
          if ( tiles[j, i].unit == player )
            Console.Write( "^" );
          else
            Console.Write( "+" );
          continue;
        }
        switch ( tiles[j, i].type )
        {
          case Tile.TileTypes.EMPTY:
            Console.Write( " " );
            break;
          case Tile.TileTypes.WALL:
            Console.Write( "|" );
            break;
          case Tile.TileTypes.ROAD:
            Console.Write( "#" );
            break;
          case Tile.TileTypes.STRUCTURE:
            Console.Write( " " );
            break;
          case Tile.TileTypes.SPAWN:
            Console.Write( "S" );
            break;
          case Tile.TileTypes.STORK:
            Console.Write( "A" );
            break;
          default:
            break;
        }

      }
      Console.WriteLine();
    }



    //Thread.Sleep(5);
  }*/
}
