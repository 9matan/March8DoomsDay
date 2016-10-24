using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class RatingsManager : MonoBehaviour
{
	public const string fileName = "ratings.dat";
	public const int maxNumber = 20;

	static public SortedDictionary<int, RatingsList.RatingInfo> sortedInfo
	{
		get { return _sortedInfo; }
	}

	static protected List< RatingsList.RatingInfo > _infoes = new List<RatingsList.RatingInfo>();
	static protected SortedDictionary<int, RatingsList.RatingInfo> _sortedInfo = new SortedDictionary<int, RatingsList.RatingInfo>();

	static public void LoadInfo()
	{
		_sortedInfo = new SortedDictionary<int, RatingsList.RatingInfo>();
		_infoes = null;

		Deserialize();
		if (_infoes == null) return;

		for (int i = 0; i < _infoes.Count; ++i)
			_sortedInfo.Add(-_infoes[i].score, _infoes[i]);
    }

	static void Serialize()
	{
		XmlSerializer s = new XmlSerializer(typeof(List<RatingsList.RatingInfo>));
		TextWriter writer = new StreamWriter(fileName);
		s.Serialize(writer, _infoes);
	}

	static void Deserialize()
	{
		try
		{
			XmlSerializer s = new XmlSerializer(typeof(List<RatingsList.RatingInfo>));
			using (TextReader reader = new StreamReader(fileName))
			{
				try
				{
					_infoes = (List<RatingsList.RatingInfo>)s.Deserialize(reader);
				}
				catch
				{
				}
			}
		}
		catch { }
	}

	static public void SaveInfo()
	{
		_infoes = new List<RatingsList.RatingInfo>();
		int number = 0;

		foreach (var p in _sortedInfo)
		{
			if (number >= maxNumber) break;
			_infoes.Add(p.Value);
			++number;
        }
	}

	static public void UpdateInfo(RatingsList.RatingInfo info)
	{
		_sortedInfo.Add(-info.score, info);
	}

}
