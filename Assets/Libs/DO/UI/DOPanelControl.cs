using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DOPanelControl : MonoBehaviour {

	public EDOInitState initState = EDOInitState.AWAKE;

	protected Dictionary<string, GameObject> _gobjs = new Dictionary<string, GameObject>();

	protected virtual void Awake()
	{
		if(initState == EDOInitState.AWAKE)
			Initialize ();
	}

	virtual public void Initialize()
	{
		this._InitializeGameObjects ();
		this._ActivateFirst ();
	}

	private void _ActivateFirst()
	{
		int cnum = this.transform.childCount;
		if(cnum <= 0) return;

		if(!this.transform.GetChild(0).gameObject.activeSelf)
			this.transform.GetChild(0).gameObject.SetActive(true);

		for(int i = 1; i < cnum; ++i)
		{
			this.transform.GetChild(i).gameObject.SetActive(false);
		}
	}

	private void _InitializeGameObjects()
	{
		int cnum = this.transform.childCount;

		for(int i = 0; i < cnum; ++i)
		{
			var tr = this.transform.GetChild(i);
			_gobjs.Add(tr.name, tr.gameObject);
		}
	}

	virtual public void SetActive(string name)
	{
		this.DeactivateAll ();
		_gobjs [name].SetActive (true);
	}

	virtual public void DeactivateAll()
	{
		foreach (var gobj in _gobjs)
			gobj.Value.SetActive (false);
	}

	public virtual void Clear()
	{
		_gobjs.Clear ();
	}

}
