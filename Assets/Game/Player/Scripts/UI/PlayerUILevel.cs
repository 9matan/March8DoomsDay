using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUILevel : MonoBehaviour {

	[SerializeField]
	protected Text _uiLevel;

	public void Update()
	{
		if (GlobalDataHolder.player != null)
			_uiLevel.text = GlobalDataHolder.player.level.ToString();
	}

}
