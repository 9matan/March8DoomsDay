using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StorkTileView : TileView
{
	[Header("Stork")]
	[SerializeField]
	protected GameObject _taskMark;		 

	public bool hasTask
	{
		get
		{
			return Task.FindTask(tile, false) == null;
		}
	}

	public override void ResetTile()
	{
		SetTile(tile);
    }

	public override void SetTile(Field.Tile __tile)
	{
		base.SetTile(__tile);

		if (hasTask)
			_taskMark.Show();
		else
			_taskMark.Hide();
    }

	[SerializeField]
	protected GameObject _canvas;
	[SerializeField]
	protected Text _questText;
	[SerializeField]
	protected float _shownTime = 2.5f;
	
	public void ShowQuest(string text)
	{
		if (_canvas.activeInHierarchy) return;

		_canvas.Show();
        _questText.text = text;

		Invoke("HideQuest", _shownTime);
    }

	public void HideQuest()
	{
		_canvas.Hide();
		_questText.text = "";
    }

	public override void Clear()
	{
		_taskMark.Hide();
        base.Clear();
	}

}
