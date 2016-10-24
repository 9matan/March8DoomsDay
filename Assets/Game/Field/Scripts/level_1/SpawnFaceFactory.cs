using UnityEngine;
using System.Collections;

public class SpawnFaceFactory : MonoBehaviour {

	static public SpawnFaceFactory i
	{
		get { return _instance; }
	}

	static protected SpawnFaceFactory _instance;

	protected void Awake()
	{
		_instance = this;
	}

	[SerializeField]
	protected DOGOMemFactory _girlFace;
	[SerializeField]
	protected DOGOMemFactory _grannyFace;
	[SerializeField]
	protected DOGOMemFactory _traderFace;
	[SerializeField]
	protected DOGOMemFactory _conductorFace;

	public GameObject CreateFace(Enemy.EEnemyType type)
	{
		GameObject face = null;

		switch (type)
		{
			case Enemy.EEnemyType.GIRL:
				face = _girlFace.Allocate();
                break;
			case Enemy.EEnemyType.GRANNY:
				face = _grannyFace.Allocate();
				break;
			case Enemy.EEnemyType.TRADER:
				face = _traderFace.Allocate();
				break;
			case Enemy.EEnemyType.CONDUCTOR:
				face = _conductorFace.Allocate();
				break;
		}

		return face;
    }

	public void Free(GameObject face, Enemy.EEnemyType type)
	{
		switch (type)
		{
			case Enemy.EEnemyType.GIRL:
				_girlFace.Free(face);
				break;
			case Enemy.EEnemyType.GRANNY:
				_grannyFace.Free(face);
				break;
			case Enemy.EEnemyType.TRADER:
				_traderFace.Free(face);
				break;
			case Enemy.EEnemyType.CONDUCTOR:
				_conductorFace.Free(face);
				break;
		}
	}

}
