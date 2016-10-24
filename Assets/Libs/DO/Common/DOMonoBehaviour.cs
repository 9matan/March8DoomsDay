using UnityEngine;
using System.Collections;

public class DOMonoBehaviour : MonoBehaviour
{

	[Header("Mono behaviour")]
	public bool debug = false;

	//
	// < Initialize >
	//

	public EDOInitState initState;

	protected virtual void Awake()
	{
		if (initState == EDOInitState.AWAKE)
			this.Initialize();
	}

	protected virtual void Start()
	{
		if (initState == EDOInitState.START)
			this.Initialize();
	}

	public virtual void Initialize()
	{

	}

	//
	// </ Initialize >
	//

	public virtual void Log(object mess)
	{
		if (debug)
			Debug.Log(this.gameObject.name + " " + mess);
	}

}
