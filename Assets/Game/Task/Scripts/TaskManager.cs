using UnityEngine;
using System.Collections;

public class TaskManager : MonoBehaviour {

	public bool debug = true;

	public void Log(object mess)
	{
		if (debug)
			Debug.Log(mess);
	}

	public void Initialize()
	{
		GlobalEventSystem.OnStorkTalk += _OnStorkTalk;
		GlobalEventSystem.OnTaskDone += _OnTaskDone;
    }

	protected void _OnStorkTalk(Field.Tile tile, Task task)
	{		
        tile.tileView.ResetTile();
		((StorkTileView)tile.tileView).ShowQuest(task.message);
        Log("Task: " + task.message);
	}

	protected void _OnTaskDone(Task task)
	{
		GlobalDataHolder.player.ui.MissionComplete();
		task.GiveReward();
		Log("Task has been done: " + task.message);
	}

	public void Clear()
	{

	}

}
