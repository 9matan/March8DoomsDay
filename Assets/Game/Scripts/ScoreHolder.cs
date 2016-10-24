using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

public abstract class ScoreHolder
{

  const string score_file_name = "score.dat";

  static List<Pair<string, int>> list = null;

  public static Pair<string, int> GetElement( int n )
  {
    if ( list == null )
      Deserialize();
    if ( list == null || list.Count <= n )
      return null;
    return list[n];
  }
  /// <summary>
  /// Returns position in new list to enlight
  /// </summary>
  /// <param name="name"></param>
  /// <param name="score"></param>
  /// <returns></returns>
  public static int AddElement( string name, int score )
  {
    if ( list == null )
      Deserialize();
    if ( list == null )
      list = new List<Pair<string, int>>();
    var el = new Pair<string, int>( name, score );
    list.Add( el );
    Serialize();
    return list.FindIndex( x => x == el );
  }

  static void Serialize()
  {
    list = list.OrderBy( x => ( -x.Second ) ).ToList();
    XmlSerializer s = new XmlSerializer( typeof( List<Pair<string, int>> ) );
    TextWriter writer = new StreamWriter( System.IO.Path.Combine( Application.persistentDataPath, score_file_name ) );
    s.Serialize( writer, list );
  }

  static void Deserialize()
  {
    try
    {
      XmlSerializer s = new XmlSerializer( typeof( List<Pair<string, int>> ) );
      using ( TextReader reader = new StreamReader( System.IO.Path.Combine( Application.persistentDataPath, score_file_name ) ) )
      {
        try
        {
          list = (List<Pair<string, int>>)s.Deserialize( reader );
          list = list.OrderBy( x => ( -x.Second ) ).ToList();
        }
        catch
        {
        }
      }
    }
    catch { }
  }
}