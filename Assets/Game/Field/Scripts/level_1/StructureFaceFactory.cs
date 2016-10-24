using UnityEngine;
using System.Collections;

public class StructureFaceFactory : MonoBehaviour {

	static public StructureFaceFactory i
	{
		get { return _instance; }
	}

	static protected StructureFaceFactory _instance;

	protected void Awake()
	{
		_instance = this;
	}

	[SerializeField]
	// mask 0-15
	protected DOGOMemFactory[] _factories;

	public GameObject CreateFace(int mask)
	{
		return _factories[mask].Allocate();
	}

	public void Free(int mask, GameObject go)
	{
		_factories[mask].Free(go);
    }

}
