using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelEndUI : MonoBehaviour
{

	[SerializeField]
	protected Text _uiScore;

	[SerializeField]
	protected Text _uiCoins;

	[SerializeField]
	protected Text _uiTime;

	public int score
	{
		get
		{
			return _score;
		}

		set
		{
			_score = value;
			_uiScore.text = _score.ToString();
		}
	}

	public int coins
	{
		get
		{
			return _coins;
		}

		set
		{
			_coins = value;
			_uiCoins.text = _coins.ToString();
		}
	}

	public int time
	{
		get
		{
			return _time;
		}

		set
		{
			_time = value;
			_uiTime.text = _time.ToString();
		}
	}

	protected int _score;
	protected int _coins;
	protected int _time;



}
