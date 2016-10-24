using UnityEngine;
using System.Collections;

public class DialogScene : MonoBehaviour
{

	public DialogItem[] items;

	protected void Awake()
	{
		Debug.Log(App.dialogNumber - 1);
		items[App.dialogNumber-1].Show();
    }

}
