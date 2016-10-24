using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUIPhoneCells : PlayerUIStatView
{

	public void Update()
	{
		SetVal(GlobalDataHolder.phone_cells);
	}

}
