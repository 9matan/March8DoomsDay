using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameWinUI : MonoBehaviour
{

	[SerializeField]
	protected InputField _uiName;

	[SerializeField]
	protected Text _uiScore;

	public void OnNextClick()
	{
		GlobalDataHolder.player.playerName = _uiName.text;
		GlobalDataHolder.player.ui.OnGameWinClick();
    }

	protected void OnEnable()
	{
		_uiScore.text = GlobalDataHolder.score_from_everything.ToString();
    }

}
