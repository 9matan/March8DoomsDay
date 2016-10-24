using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScenesManager : MonoBehaviour {

	public const int levelStartInd = 1;
	public const int levelNumber = 3;
	public const int firstLevel = levelStartInd + 1;

	protected const string _loadingLevel = "loading";
	protected const string _menuLevel = "menu";
	protected const string _shopLevel = "shop";
	protected const string _dialogLevel = "dialog";

	static public int loadedLevel { get; protected set; }
	static public string loadedLevelName { get; protected set; }
	static public bool isLoadByNumber { get; protected set; }

	static public bool isLoadingScene
	{
		get
		{
			return currentScene.name == _loadingLevel;
        }
	}

	static public bool isShopScene
	{
		get
		{
			return currentScene.name == _shopLevel;
		}
	}

	static public bool isDialogScene
	{
		get
		{
			return currentScene.name == _dialogLevel;
		}
	}

	public static void LoadLevel(string level, bool loading = true)
	{
		isLoadByNumber = false;
		loadedLevelName = level;

		if (loading)
			UnityEngine.SceneManagement.SceneManager.LoadScene(_loadingLevel);
		else
			UnityEngine.SceneManagement.SceneManager.LoadScene(loadedLevelName);
	}

	public static void LoadLevel(int level, bool loading = true)
	{
		isLoadByNumber = true;
		loadedLevel = level;

		if (loading)
			UnityEngine.SceneManagement.SceneManager.LoadScene(_loadingLevel);
		else
			UnityEngine.SceneManagement.SceneManager.LoadScene(loadedLevel);
	}

	public static bool IsLevel(int level)
	{
		return (level > levelStartInd && level <= levelStartInd + levelNumber);
	}

	public static int GetLevelNumber()
	{
		if (!IsLevel(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)) return -1;

		return UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - levelStartInd;
    }

	public static void LoadLoadedScene()
	{
		if(isLoadByNumber)
			LoadLevel(loadedLevel, false);
		else
			LoadLevel(loadedLevelName, false);
	}

	//
	// < Scenes >
	//

	public static void LoadStartGame(bool loading = true)
	{
		LoadLevel(levelStartInd, loading);
    }

	public static void LoadFirstLevel(bool loading = true)
	{
		App.nextLevelToLoad = firstLevel + 1;
        LoadLevel(firstLevel, loading);
	}

	public static void LoadMenu(bool loading = true)
	{
		LoadLevel(_menuLevel, loading);
    }

	public static void LoadNextLevel(bool loading = true)
	{
		LoadLevel(App.nextLevelToLoad++, loading);
	}

	public static void LoadShop(bool loading = true)
	{
		LoadLevel(_shopLevel, loading);
	}

	public static void LoadDialog(bool loading = true)
	{
		LoadLevel(_dialogLevel, loading);
    }

	//
	// </ Scenes >
	//

	public static bool IsLastLevel()
	{
		return (GetLevelNumber() == levelNumber);
    }

	public static Scene currentScene
	{
		get
		{
			return SceneManager.GetActiveScene();
		}
	}

}
