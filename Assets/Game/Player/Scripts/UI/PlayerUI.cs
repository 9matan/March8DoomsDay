using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUI : MonoBehaviour {

	public event GlobalEventSystem.NoArgsEvent onGameOverNext;
	public event GlobalEventSystem.NoArgsEvent onGameWin;

	[SerializeField]
	protected UISelftTimerButton[] _buttons;

	[SerializeField]
	protected UISelftTimerButton _skillButton;
	[SerializeField]
	protected UISelftTimerButton _medicineButton;
	[SerializeField]
	protected UISelftTimerButton _lehaButton;

	protected void Awake()
	{
		for (int i = 0; i < _buttons.Length; ++i)
			_buttons[i].onValidClick += OnButtonClick;
    }

	public void OnButtonClick(PlayerControlKeysMono key)
	{
		GlobalEventSystem.ButtonClicked(key.key);
    }

	protected void Update()
	{
		if (GlobalDataHolder.player != null)
		{
			UpdateSkill();
			UpdateMedicine();
			UpdateLeha();
        }
    }

	protected void UpdateSkill()
	{
		if (GlobalDataHolder.player.skill_achieved)
			_skillButton.Show();
		else
			_skillButton.Hide();
	}

	protected void UpdateMedicine()
	{
		if (GlobalDataHolder.medicine > 0)
			_medicineButton.Show();
		else
			_medicineButton.Hide();
	}

	protected void UpdateLeha()
	{
		if (GlobalDataHolder.phone_cells > 0)
			_lehaButton.Show();
		else
			_lehaButton.Hide();
	}

	[SerializeField]
	protected Animator _mCompleteAnimator;
	[SerializeField]
	protected string _mCompleteAnimState = "start";

	public void MissionComplete()
	{
		_mCompleteAnimator.Play(_mCompleteAnimState);
    }


	[SerializeField]
	protected GameObject _gameUI;

	[SerializeField]
	protected PlayerUILevelUp _levelUp;

	public void Initialize()
	{
		this.Show();
		this.transform.SetParent(GlobalDataHolder.player.transform.parent);
		onGameOverNext = delegate { };
		onGameWin = delegate { };
        GlobalEventSystem.OnLevelEnd += _OnLevelEnd;

		_gameUI.Show();
        _onVictory.Hide();
		_onDefeat.Hide();
		_onGameWin.Hide();

		_levelUp.Initialize();
    }

	public void Clear()
	{
		this.Hide();
	}

	protected void _OnLevelEnd()
	{
		if (GlobalDataHolder.isVictory)
			_OnVictory();
		else
			_OnDefeat();
    }

	[SerializeField]
	protected LevelEndUI _onVictory;

	protected void _OnVictory()
	{
		_SetLevelEnd(_onVictory);
    }

	[SerializeField]
	protected LevelEndUI _onDefeat;

	protected void _OnDefeat()
	{
		_SetLevelEnd(_onDefeat);
	}

	protected void _SetLevelEnd(LevelEndUI levelEnd)
	{
		_gameUI.Hide();
        levelEnd.Show();

		levelEnd.score = GlobalDataHolder.player_score;
		levelEnd.coins = GlobalDataHolder.player_gold;
		levelEnd.time = (int)GlobalDataHolder.time_left;
    }

	public void OnNextClick()
	{
		onGameOverNext();
    }

	[SerializeField]
	protected GameObject _onGameWin;

	public void ShowGameWin()
	{
		_onVictory.Hide();
		_onDefeat.Hide();
        _onGameWin.Show();
    }

	public void OnGameWinClick()
	{
		onGameWin();
	}

	[SerializeField]
	protected PlayerUIShowIncome _showIncome;

	public PlayerUIShowIncome showIncome
	{
		get { return _showIncome; }
	}

	public void SetPlayerName(InputField uiName)
	{
		GlobalDataHolder.player.playerName = uiName.text;
	}

}
