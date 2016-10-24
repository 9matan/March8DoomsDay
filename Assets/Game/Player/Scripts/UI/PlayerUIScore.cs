using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUIScore : PlayerUIStatView
{

	public void Update()
	{
		SetVal(GlobalDataHolder.player_score);
	}

}
