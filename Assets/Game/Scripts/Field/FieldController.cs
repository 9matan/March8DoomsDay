using UnityEngine;
using System.Collections;

public class FieldController : MonoBehaviour {

	public Field field
	{
		get { return _fieldView.field; }
	}

	[SerializeField]
	protected FieldView _fieldView;

	public void Initialize()
	{
		_InitializeFieldView();
    }

	protected void _InitializeFieldView()
	{
		_fieldView.Initilize();
    }

	public void Clear()
	{
		_fieldView.Clear();
    }

}
