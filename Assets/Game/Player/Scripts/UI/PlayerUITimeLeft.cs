using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUITimeLeft : MonoBehaviour {

	[SerializeField]
	protected Text _uiMedicine;

	public void Update()
	{
		if (GlobalDataHolder.player != null)
			_uiMedicine.text = "x" + ((int)(GlobalDataHolder.time_left)).ToString();
	}

}
