using UnityEngine;
using System.Collections;

public interface IDOScriptGeneratorTemplate
{
	DOScriptGenerator generator { get; }

	void Initialize(DOScriptGenerator __generator);
	string Replace (string text);
}

public abstract class DOScriptGeneratorTemplate : MonoBehaviour, IDOScriptGeneratorTemplate {

	public int seqNumber {
		get { return _seqNumber; }
		set { _seqNumber = value; }
	}

	public string templateName {
		get { return _templateName; }
		set { _templateName = value; }
	}

	[SerializeField] protected int 		_seqNumber;
	[SerializeField] protected string 	_templateName;

	public DOScriptGenerator generator { get; protected set; }


	virtual public void Initialize(DOScriptGenerator __generator)
	{
		generator = __generator;
	}

	virtual public string Replace (string text)
	{
		string newString = this._GetNewString ();
		return text.Replace (_templateName, newString);
	}

	protected abstract string _GetNewString();

}
