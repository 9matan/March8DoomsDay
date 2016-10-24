using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUIHealth : UIFilledField
{

	protected override void Update()
	{
		base.Update();

		_UpdateCurrentHp();
	}

	protected void _UpdateCurrentHp()
	{
		float r = Mathf.Min(1.0f, (float)GlobalDataHolder.player.health / (float)GlobalDataHolder.player.max_health);
		SetFill(r);
	}

}
