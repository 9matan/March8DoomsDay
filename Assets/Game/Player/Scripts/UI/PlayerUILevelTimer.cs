using UnityEngine;
using System.Collections;

public class PlayerUILevelTimer : UITimerButton {

	protected void Update()
	{
		if (!GlobalDataHolder.isLevelStart) return;

		SetPercent(1.0f - GlobalDataHolder.time_left / GlobalDataHolder.levelTime);
	}

}
