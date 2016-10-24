using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUILevelUp : MonoBehaviour {

	[SerializeField]
	protected Text _pointsNumber;

	[SerializeField]
	protected Button _levelUpBtn;

	[SerializeField]
	protected GameObject _upBtns;

	public void Initialize()
	{
		GlobalEventSystem.OnPlayerLevelUp += OnLevelPointsUpdate;
    }

	public void OnLevelPointsUpdate()
	{
		SetPointsNumber(GlobalDataHolder.player.free_skill_point);
    }

	public void SetPointsNumber(int num)
	{
		if (num > 0)
			_levelUpBtn.Show();
		else
		{
			_upBtns.Hide();
            _levelUpBtn.Hide();
		}

		_pointsNumber.text = num.ToString();
    }

	public void OnLevelUpClick()
	{
		if (_upBtns.activeInHierarchy)
		{
			_upBtns.Hide();
		}
		else
			_upBtns.Show();
    }

	public void OnHealthUp()
	{
		GlobalDataHolder.player.UpLevel(Player.LevelUpgradeList.HEALTH);
		_UsePoint();
    }

	public void OnDamageUp()
	{
		GlobalDataHolder.player.UpLevel(Player.LevelUpgradeList.DAMAGE);
		_UsePoint();
	}

	public void OnSpeedUp()
	{
		GlobalDataHolder.player.UpLevel(Player.LevelUpgradeList.SPEED);
		_UsePoint();
	}

	protected void _UsePoint()
	{
		OnLevelPointsUpdate();
    }

}
