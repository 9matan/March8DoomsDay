using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class PlayerUITaskInfo : MonoBehaviour {

	[SerializeField]
	protected Text _tasksMess;
	[SerializeField]
	protected Button _taskBtn;
	[SerializeField]
	protected GameObject _taskInfo;

	[SerializeField]
	protected float _refreshRate = 1.0f;

	protected float _curTime = 0.0f;

	protected void Update()
	{
		_curTime += Time.deltaTime;

		if (_curTime >= _refreshRate)
		{
			_curTime = 0.0f;
			Check();
        }
    }

	public void Check()
	{
		if(Task.tasks_messages.Count != 0)
		{
			_taskBtn.Show();
			UpdateText(Task.tasks_messages);
        }
		else
		{
			_taskBtn.Hide();
			_taskInfo.Hide();
		}
	}

	public void OnTaskClick()
	{
		if (_taskInfo.activeInHierarchy)
		{
			_taskInfo.Hide();
		}
		else
			_taskInfo.Show();
	}

	public void UpdateText(List<string> tasks)
	{
		_tasksMess.text = "";

		var builder = new System.Text.StringBuilder();

		for (int i = 0; i < tasks.Count; ++i)
		{
			builder.Append((i+1).ToString());
			builder.Append(". ");
			builder.Append(tasks[i]);
			builder.Append('\n');
        }

		_tasksMess.text = builder.ToString();
    }

}
