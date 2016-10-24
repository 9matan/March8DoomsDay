using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public class Pair<A, B>
{
  [SerializeField]
  A first;
  [SerializeField]
  B second;

  public A First
  {
    get
    {
      return first;
    }
    set
    {
      first = value;
    }
  }
  public B Second
  {
    get
    {
      return second;
    }
    set
    {
      second = value;
    }
  }

  public Pair( A first, B second )
  {
    this.first = first;
    this.second = second;
  }
  public Pair()
  {
  }

  public static bool operator ==( Pair<A, B> a, Pair<A, B> b )
  {
    if ( (object)a == null && (object)b == null )
      return true;
    if ( (object)a == null && (object)b != null || (object)a != null && (object)b == null )
      return false;
    return ( (object)a.first == (object)b.first && (object)a.second == (object)b.second );
  }
  public static bool operator !=( Pair<A, B> a, Pair<A, B> b )
  {
    if ( (object)a == null && (object)b == null )
      return false;
    if ( (object)a == null && (object)b != null || (object)a != null && (object)b == null )
      return true;
    return !( (object)a.first == (object)b.first && (object)a.second == (object)b.second );
  }
}
[System.Serializable]
public class Treplet<A, B, C>
{
  [SerializeField]
  A first;
  [SerializeField]
  B second;
  [SerializeField]
  C third;

  protected A First
  {
    get
    {
      return first;
    }
    set
    {
      first = value;
    }
  }
  protected B Second
  {
    get
    {
      return second;
    }
    set
    {
      second = value;
    }
  }
  protected C Third
  {
    get
    {
      return third;
    }
    set
    {
      third = value;
    }
  }

  public Treplet( A first, B second, C third )
  {
    this.first = first;
    this.second = second;
    this.third = third;
  }

  public static bool operator ==( Treplet<A, B, C> a, Treplet<A, B, C> b )
  {
    if ( (object)a == null && (object)b == null )
      return true;
    if ( (object)a == null && (object)b != null || (object)a != null && (object)b == null )
      return false;
    return ( (object)a.first == (object)b.first && (object)a.second == (object)b.second && (object)a.third == (object)b.third );
  }
  public static bool operator !=( Treplet<A, B, C> a, Treplet<A, B, C> b )
  {
    if ( (object)a == null && (object)b == null )
      return false;
    if ( (object)a == null && (object)b != null || (object)a != null && (object)b == null )
      return true;
    return !( (object)a.first == (object)b.first && (object)a.second == (object)b.second && (object)a.third == (object)b.third );
  }
}

public interface IHasID
{
  int id
  {
    get;
  }
}

[Serializable]
public class IdList<A>
  where A : class, IHasID
{
  [SerializeField]
  List<A> data;
  public IdList()
  {
    data = new List<A>();
  }
  public A FindById( int id )
  {
    foreach ( A a in data )
    {
      if ( a.id == id )
        return a;
    }
    return null;
  }
  public A this[int i]
  {
    get
    {
      if ( data.Count <= i )
        return null;
      return data[i];
    }
    set
    {
      if ( data.Count <= i )
        return;
      data[i] = value;
    }

  }
  public int Count
  {
    get
    {
      return data.Count;
    }
  }
  public bool Add( A element )
  {
    if ( FindById( element.id ) != null )
      return false;
    data.Add( element );
    return true;
  }
  public void Remove( A element )
  {
    data.Remove( element );
  }
  
  public void Clear()
  {
    data.Clear();
  }
  public void CopyTo( IdList<A> target )
  {
    if(target!= null && target.data != null)
    {
      foreach ( A item in data )
      {
        target.data.Add( item );
      }
    }
  }

  public int next_id
  {
    get
    {
      int max = -1;
      foreach ( A item in data )
      {
        if ( item.id > max )
          max = item.id;
      }
      return max + 1;
    }
  }
}

[Serializable]
public class Set<A>
{
  const int INIT_SLOT_COUNT = 12;
  [SerializeField]
  A[] slots = null;
  [SerializeField]
  bool[] used = null;

  [SerializeField]
  public Set()
  {
    slots = new A[INIT_SLOT_COUNT];
    used = new bool[INIT_SLOT_COUNT];
  }
  public bool Contains( A element )
  {
    for ( int i = 0; i < slots.Length; ++i )
    {
      if ( used[i] && slots[i].Equals( element ) )
        return true;
    }
    return false;
  }
  public void Add( A element )
  {
    if ( Contains( element ) )
      return;
    int i = 0;
    for ( ; i < slots.Length; ++i )
    {
      if ( !used[i] )
      {
        slots[i] = element;
        used[i] = true;
        return;
      }
    }
    if ( slots.Length <= i )
    {
      Array.Resize( ref slots, i * 2 );
      Array.Resize( ref used, i * 2 );
    }
    slots[i] = element;
    used[i] = true;
  }
  public void Remove( A element )
  {
    for ( int i = 0; i < slots.Length; ++i )
    {
      if ( used[i] && slots[i].Equals( element ) )
      {
        used[i] = false;
      }
    }
  }

}
[Serializable]
public class EnumSet<A> where A : struct, IConvertible, IComparable, IFormattable
{
  //TODO
  [SerializeField]
  public A[] slots = null;

  [SerializeField]
  public bool[] used = null;

  [SerializeField]
  public EnumSet()
  {
    if ( !typeof( A ).IsEnum )
    {
      throw new ArgumentException( "T must be an enumerated type" );
    }
    Array elems = Enum.GetValues( typeof( A ) );
    slots = new A[elems.Length];
    used = new bool[elems.Length];
    int i = 0;
    foreach ( A item in elems )
    {
      slots[i] = item;
      ++i;
    }
  }
  public void Update()
  {
    Array elems = Enum.GetValues( typeof( A ) );
    Array.Resize( ref slots, elems.Length );
    Array.Resize( ref used, elems.Length );
    int i = 0;
    foreach ( A item in elems )
    {
      slots[i] = item;
      ++i;
    }
  }
  public bool Contains( A element )
  {
    for ( int i = 0; i < slots.Length; ++i )
    {
      if ( used[i] && slots[i].Equals( element ) )
        return true;
    }
    return false;
  }
  public void Add( A element )
  {
    int i = 0;
    for ( ; i < slots.Length; ++i )
    {
      if ( slots[i].Equals( element ) )
      {
        used[i] = true;
        return;
      }
    }
  }
  public void Remove( A element )
  {
    for ( int i = 0; i < slots.Length; ++i )
    {
      if ( used[i] && slots[i].Equals( element ) )
      {
        used[i] = false;
      }
    }
  }


}

[Serializable]
public class MyDictionary<TKey, TValue> where TKey : class
{

  [SerializeField]
  private List<TKey> keys;
  [SerializeField]
  private List<TValue> values;

  public TKey[] Keys
  {
    get
    {
      return keys.ToArray();
    }
  }
  public TValue this[TKey key]
  {
    get
    {
      for ( int i = 0; i < keys.Count; i++ )
      {
        if ( keys[i].Equals( key ) )
          return values[i];
      }
      throw new Exception( "No such key!" );
    }
    set
    {
      for ( int i = 0; i < keys.Count; i++ )
      {
        if ( keys[i].Equals( key ) )
        {
          values[i] = value;
          return;
        }
      }
      keys.Add( key );
      values.Add( value );
    }
  }
  public Pair<TKey, TValue> this[int num]
  {
    get
    {
      if ( keys.Count > num && num >= 0 )
      {
        return new Pair<TKey, TValue>( keys[num], values[num] );
      }
      return null;
    }
    set
    {
      if ( keys.Count > num && num >= 0 )
      {
        keys[num] = value.First;
        values[num] = value.Second;
      }
    }
  }
  public int Count
  {
    get
    {
      if ( keys == null )
        return 0;
      return keys.Count;
    }
  }
  public MyDictionary()
  {
    Clear();
  }
  public void Clear()
  {
    keys = new List<TKey>();
    values = new List<TValue>();
  }
  public void Remove( TKey key )
  {
    for ( int i = 0; i < keys.Count; i++ )
    {
      if ( keys[i].Equals( key ) )
      {
        keys.RemoveAt( i );
        values.RemoveAt( i );
        return;
      }
    }
  }
  public void Remove( Pair<TKey, TValue> entry )
  {
    for ( int i = 0; i < keys.Count; i++ )
    {
      if ( keys[i].Equals( entry.First ) )
      {
        keys.RemoveAt( i );
        values.RemoveAt( i );
        return;
      }
    }
  }
  public void CopyTo( MyDictionary<TKey, TValue> target )
  {
    if ( target == null )
      return;
    for ( int i = 0; i < Count; i++ )
    {
      target[keys[i]] = values[i];
    }
  }
}

[Serializable]
public class DoubleDictionary<TKey1, TKey2, TValue> 
{
	[Serializable]
	public class Key:Pair<TKey1, TKey2>
	{
		public Key(TKey1 key1, TKey2 key2) :base(key1,key2)
		{
		}
	}
	[SerializeField]
	private List<Key> keys;
	[SerializeField]
	private List<TValue> values;

	public Pair<TKey1,TKey2>[] Keys
	{
		get
		{
			return keys.ToArray();
		}
	}
	public TValue this[TKey1 key1, TKey2 key2]
	{
		get
		{
			for ( int i = 0; i < keys.Count; i++ )
			{
				if ( keys[i].First.Equals( key1 ) && keys[i].Second.Equals( key2 ) )
					return values[i];
			}
			throw new Exception( "No such key!" );
		}
		set
		{
			for ( int i = 0; i < keys.Count; i++ )
			{
				if ( keys[i].First.Equals( key1 ) && keys[i].Second.Equals( key2 ) )
				{
					values[i] = value;
					return;
				}
			}
			keys.Add( new Key( key1, key2) );
			values.Add( value );
		}
	}
	public TValue this[Key key]
	{
		get
		{
			for ( int i = 0; i < keys.Count; i++ )
			{
				if ( keys[i].First.Equals( key.First ) && keys[i].Second.Equals( key.Second ) )
					return values[i];
			}
			throw new Exception( "No such key!" );
		}
		set
		{
			for ( int i = 0; i < keys.Count; i++ )
			{
				if ( keys[i].First.Equals( key.First ) && keys[i].Second.Equals( key.Second ) )
				{
					values[i] = value;
					return;
				}
			}
			keys.Add( new Key( key.First, key.Second ) );
			values.Add( value );
		}
	}
	public Pair< Key, TValue> this[int num]
	{
		get
		{
			if ( keys.Count > num && num >= 0 )
			{
				return new Pair<Key, TValue>( keys[num], values[num] );
			}
			return null;
		}
		set
		{
			if ( keys.Count > num && num >= 0 )
			{
				keys[num] = value.First;
				values[num] = value.Second;
			}
		}
	}
	public int Count
	{
		get
		{
			if ( keys == null )
				return 0;
			return keys.Count;
		}
	}
	public DoubleDictionary()
	{
		Clear();
	}
	public void Clear()
	{
		keys = new List<Key>();
		values = new List<TValue>();
	}
	public void Remove( TKey1 key1, TKey2 key2 )
	{
		for ( int i = 0; i < keys.Count; i++ )
		{
			if ( keys[i].First.Equals( key1 ) && keys[i].Second.Equals( key2 ) )
			{
				keys.RemoveAt( i );
				values.RemoveAt( i );
				return;
			}
		}
	}
	public void Remove( Pair<Key, TValue> entry )
	{
		for ( int i = 0; i < keys.Count; i++ )
		{
			if ( keys[i].Equals( entry.First ) )
			{
				keys.RemoveAt( i );
				values.RemoveAt( i );
				return;
			}
		}
	}
	public void CopyTo( DoubleDictionary<TKey1, TKey2, TValue> target )
	{
		if ( target == null )
			return;
		for ( int i = 0; i < Count; i++ )
		{
			target[keys[i]] = values[i];
		}
	}
}