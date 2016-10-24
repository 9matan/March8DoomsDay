using UnityEngine;
using System.Collections;

public class MenuScene : MonoBehaviour
{

	public void StartGame(DifficultTypeMono type)
	{
		App.dialogNumber = 1;
        App.difficulty = type.type;
		ScenesManager.LoadStartGame();
	}

}
