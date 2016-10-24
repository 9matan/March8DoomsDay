using UnityEngine;
using System.Collections;

public class FieldUnitFace : MonoBehaviour {

	[SerializeField]
	protected GameObject _leftDir;

	[SerializeField]
	protected GameObject _rightDir;

	[SerializeField]
	protected GameObject _topDir;

	[SerializeField]
	protected GameObject _bottomDir;

	public void UpdateDirectionView(Field.Directions direction)
	{
		HideAll();

		switch (direction)
		{
			case Field.Directions.UP:
				_bottomDir.Show();				
                break;
			case Field.Directions.DOWN:
				_topDir.Show();
				break;
			case Field.Directions.LEFT:
				_leftDir.Show();
				break;
			case Field.Directions.RIGHT:
				_rightDir.Show();
				break;
			default:
				break;
		}
	}

	public void HideAll()
	{
		_leftDir.Hide();
		_rightDir.Hide();
		_topDir.Hide();
		_bottomDir.Hide();
	}

}
