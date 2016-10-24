using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUICoins : PlayerUIStatView
{

	public void Update()
	{
		SetVal( GlobalDataHolder.player_gold );
	}

}
