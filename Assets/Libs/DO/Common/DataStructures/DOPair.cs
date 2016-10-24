using UnityEngine;
using System.Collections;

[System.Serializable]
public struct DOPair<T1, T2> {

	public T1 first { get; set; }
	public T2 second { get; set; }

	public DOPair(T1 __first, T2 __second)
	{
		first = __first; second = __second;
	}

	public override string ToString ()
	{
		return string.Format ("({0}; {1})", first, second);
	}

}
