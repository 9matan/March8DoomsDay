using UnityEngine;
using System.Collections;

public class FlowerSpriteFactory : MonoBehaviour {

	static public FlowerSpriteFactory i
	{
		get { return _instance; }
	}

	static protected FlowerSpriteFactory _instance;

	protected void Awake()
	{
		_instance = this;
	}

	[SerializeField]
	protected DOGOMemFactory[] _factories;

	public ItemViewFace CreateFace()
	{
		int rnd = Random.Range(0, _factories.Length);
		var face = _factories[rnd].Allocate().GetComponent<ItemViewFace>();
		face.factoryId = rnd;
		return face;
    }

	public void FreeFace(ItemViewFace ivFace)
	{
		_factories[ivFace.factoryId].Free(ivFace.gameObject);
    }

}
