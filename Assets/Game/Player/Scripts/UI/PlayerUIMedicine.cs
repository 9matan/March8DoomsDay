using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUIMedicine : PlayerUIStatView
{

	public void Update()
	{
		SetVal(GlobalDataHolder.medicine);
	}

}
