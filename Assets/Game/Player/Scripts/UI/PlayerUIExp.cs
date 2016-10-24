using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUIExp : UIFilledField
{

	[SerializeField]
	protected Transform _incomeTransform;

	public Transform incomeTransform
	{
		get { return _incomeTransform; }
	}

	public Color32 color
	{
		get { return _curImage.color; }
	}

	protected override void Update()
	{
		base.Update();

		_UpdateCurrentExp();
    }

	protected void _UpdateCurrentExp()
	{
		float r = Mathf.Min(1.0f, (float)GlobalDataHolder.player.experience / (float)GlobalDataHolder.player.exp_to_level );
		SetFill(r);
	}

}
