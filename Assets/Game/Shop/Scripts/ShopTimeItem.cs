using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopTimeItem : ShopItem
{

	[SerializeField]
	protected Text _uiNumber;

	protected override void Start()
	{
		base.Start();

		_uiNumber.text = "+" + GlobalDataHolder.time_boost_amount.ToString();
    }

}
