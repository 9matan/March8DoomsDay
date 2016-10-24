using UnityEngine;
using System.Collections;

public class GameStarter : MonoBehaviour {

	protected void Awake()
	{
		App.dialogNumber = 1;
        App.isFirstLevel = true;
		App.nextLevelToLoad = ScenesManager.firstLevel + 1;
        StartCoroutine(IELoadLevel());
	}

	protected IEnumerator IELoadLevel()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();

		ScenesManager.LoadDialog();
	}

}
