using UnityEngine;
using System.Collections;

public class DOScriptGeneratorStringTemplate : DOScriptGeneratorTemplate {

	[SerializeField] DOStringValue _newString;

	protected override string _GetNewString ()
	{
		return _newString.value;
	}

}
