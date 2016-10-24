using UnityEngine;
using System.Collections;

public class DOS<T>
{
	static public T i {
		get { return _instance; }
	}

	static private T _instance;

	static public void Initialize(T instance)
	{
		_instance = instance;
	}

}
