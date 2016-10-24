using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

public abstract class DialogHolder
{

  const string dialog_file_name = "russian.dat";
  public interface IDialog
  {
    int length
    {
      get;
    }
    Pair<string, string> next_message
    {
      get;
    }
    bool is_over
    {
      get;
    }
  }
  [Serializable]
  public class Dialog : IDialog
  {
    public List<Pair<string, string>> list = null;
    public Pair<string, string> this[int n]
    {
      get
      {
        if ( list.Count <= n )
          return null;
        return list[n];
      }
    }
    public int length
    {
      get
      {
        if ( list == null )
          return 0;
        return list.Count;
      }
    }
    private int current = 0;
    public Pair<string, string> next_message
    {
      get
      {
        if ( is_over )
          return null;
        return this[current++];
      }
    }
    public bool is_over
    {
      get
      {
        return current >= length;
      }
    }
    public Dialog()
    {
      list = null;
      current = 0;
    }
    public Dialog( List<Pair<string, string>> list )
    {
      this.list = list;
      current = 0;
    }
  }
  static List<Dialog> list = null;

  public static IDialog GetElement( int n )
  {
    if ( list == null )
      Deserialize();
    if ( list == null || list.Count <= n )
      return null;
    return list[n];
  }

  static void Serialize()
  {
    XmlSerializer s = new XmlSerializer( typeof( List<Dialog> ) );
    TextWriter writer = new StreamWriter(System.IO.Path.Combine(Application.persistentDataPath, dialog_file_name) );
    s.Serialize( writer, list );
  }

  static void Deserialize()
  {
    XmlSerializer s = new XmlSerializer( typeof( List<Dialog> ) );
    try
    {
      using ( TextReader reader = new StreamReader( System.IO.Path.Combine( Application.persistentDataPath, dialog_file_name ) ) )
      {
        try
        {
          list = (List<Dialog>)s.Deserialize( reader );
        }
        catch
        {
        }
      }
    }
    catch { }
  }

  public static void Test()
  {
		DialogHolder.list = new List<Dialog>();

		List<Pair<string, string>> list = new List<Pair<string, string>>();
    list.Add( new Pair<string, string>( "", "*Утро, шТОРа подходит к ТОРту*" ) );
    list.Add( new Pair<string, string>( "шТОРа", "О, уже 8 марта. Дорогой, а что ты на сегодня приготовил?" ) );
    list.Add( new Pair<string, string>( "ТОРт", "Даааа… Конечно…. Я только спущусь в машину и принесу его." ) );
		Dialog d = new Dialog(list);

		DialogHolder.list.Add(d);
		list = new List<Pair<string, string>>();
		list.Add( new Pair<string, string>( "", "*Улица, ТОРт звонит Лехе*" ) );
    list.Add( new Pair<string, string>( "ТОРт", "Леха, что делать?! Сегодня 8 марта а у меня нет подарка!" ) );
    list.Add( new Pair<string, string>( "Леха", "Срочно нужно что-то предпринять! Я помогу тебе!" ) );
    list.Add( new Pair<string, string>( "ТОРт", "Прикрой меня от 13-летних упырей, если что." ) );
    list.Add( new Pair<string, string>( "Леха", "Ты знаешь, как меня вызвать!" ) );
		d = new Dialog( list );
   
    DialogHolder.list.Add( d );
    list = new List<Pair<string, string>>();
    list.Add( new Pair<string, string>( "ТОРт", "*отдышка*" ) );
    list.Add( new Pair<string, string>( "шТОРа", "Дорогой, где ты был?" ) );
    list.Add( new Pair<string, string>( "ТОРт", "Бегал." ) );
    list.Add( new Pair<string, string>( "шТОРа", "Странно, но твоя футболка сухая и совсем не пахнет!" ) );
    list.Add( new Pair<string, string>( "ТОРт", "Лучше цветочки понюхай!" ) );
    list.Add( new Pair<string, string>( "ТОРт", "С праздником тебя, дорогая!" ) );
    list.Add( new Pair<string, string>( "", "*цем-цем-цем*" ) );
    d = new Dialog( list );
    DialogHolder.list.Add( d );
    Serialize();

  }
}