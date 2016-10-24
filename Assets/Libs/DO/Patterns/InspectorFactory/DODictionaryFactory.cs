using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DODictionaryFactory<T1, T2>
{	
	
	[SerializeField] private T1[] _keys;
	[SerializeField] private T2[] _values;

	public SortedDictionary<T1, T2> Create()
	{
		int len = Mathf.Min (_keys.Length, _values.Length);
		SortedDictionary<T1, T2> _dict = new SortedDictionary<T1, T2> ();

		for (int i = 0; i < len; ++i)
			_dict.Add (_keys[i], _values[i]);

		return _dict;
	}
}
