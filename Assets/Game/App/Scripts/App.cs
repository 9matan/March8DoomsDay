using UnityEngine;
using System.Collections;

public class App : MonoBehaviour {

	public static int nextLevelToLoad = 1;
	public static GlobalDataHolder.DifficultyLevels difficulty = GlobalDataHolder.DifficultyLevels.EASY;
	public static bool isFirstLevel { get; set; }
	public static int dialogNumber = 1;

	protected void Start()
	{
        ScenesManager.LoadMenu();
	}

}
