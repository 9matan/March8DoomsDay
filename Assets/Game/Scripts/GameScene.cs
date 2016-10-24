using UnityEngine;
using System.Collections;

public class GameScene : MonoBehaviour
{
	


	[SerializeField]
	protected FieldController _fieldController;

	public void Start()
	{
		Debug.Log("Start level: " + ScenesManager.GetLevelNumber());

		_fieldController.Initialize();
		GlobalDataHolder.SetField(_fieldController.field);
		_fieldController.field.SpawnPlayer();

		GlobalEventSystem.ListenPlayerDeath();
        GlobalEventSystem.RaiseLevelStart();

		GlobalDataHolder.player.ui.onGameOverNext += _OnLevelEnd;
		GlobalDataHolder.player.ui.onGameWin += _OnGameWin;
	}

	protected void _OnLevelEnd()
	{
		StartCoroutine(IELoadNextScene());		
	}
	
	protected IEnumerator IELoadNextScene()
	{
		yield return new WaitForEndOfFrame();

		if (ScenesManager.IsLastLevel() && GlobalDataHolder.isVictory)
		{
			GlobalDataHolder.player.ui.ShowGameWin();
        }
		else
		{
			_Clear();

			if (GlobalDataHolder.isVictory && !ScenesManager.IsLastLevel())
			{
				ScenesManager.LoadShop();
			}

			if (!GlobalDataHolder.isVictory)
			{
				ScoreHolder.AddElement(GlobalDataHolder.player.playerName, GlobalDataHolder.player_score);
				ScenesManager.LoadMenu();
			}
		}
    }

	protected void _OnGameWin( )
	{
		_Clear();
		Debug.LogFormat("Win: {0}. Score: {1}", GlobalDataHolder.player.playerName, GlobalDataHolder.player_score);
		ScoreHolder.AddElement(GlobalDataHolder.player.playerName, GlobalDataHolder.score_from_everything);
		ScenesManager.LoadDialog();
	}


	protected void _Clear()
	{
		FieldUnit.ClearAllUnits();
		_fieldController.Clear();
		Task.ClearTasks();
		GlobalDataHolder.player.ui.Clear();
		GlobalDataHolder.Clear();
	}


}
