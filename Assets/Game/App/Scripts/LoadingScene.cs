using UnityEngine;
using System.Collections;

public class LoadingScene : MonoBehaviour {

	protected void Awake()
	{
		Debug.Log("Loading...");
		StartCoroutine(IELoadScene());
    }

	protected IEnumerator IELoadScene()
	{
		yield return new WaitForEndOfFrame();
		ScenesManager.LoadLoadedScene();
	}

}
