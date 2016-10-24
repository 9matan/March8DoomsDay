using UnityEngine;
using System.Collections;

public class PlayerUISkillButton : UISelftTimerButton
{

	protected override void Update()
	{
		_cdTime = GlobalDataHolder.player.skill_reuse_time;
        base.Update();
	}

}
