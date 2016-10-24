using UnityEngine;
using System.Collections;

public class DialogItem : MonoBehaviour
{

	public int number;

	[SerializeField]
	protected GameObject[] _dialogs;
	[SerializeField]
	protected string _sceneName;

	protected int _currentDialog = -1;		 

	void OnEnable()
	{
		NextDialog();
    }

	public bool NextDialog()
	{
		if (++_currentDialog == _dialogs.Length) return false;

		_dialogs[Mathf.Max(0, _currentDialog - 1)].Hide();
		_dialogs[_currentDialog].Show();

		return true;
	}

	public void OnSkip()
	{
		if (!NextDialog())
			LoadScene();
    }

	public void LoadScene()
	{
		++App.dialogNumber;
		ScenesManager.LoadLevel(_sceneName, false);
	}

}
