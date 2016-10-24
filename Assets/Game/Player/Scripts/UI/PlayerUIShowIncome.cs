using UnityEngine;
using System.Collections;

public class PlayerUIShowIncome : MonoBehaviour
{

	[SerializeField]
	protected PlayerUICoins _coins;

	[SerializeField]
	protected PlayerUIExp _exp;

	[SerializeField]
	protected PlayerUIMedicine _medicine;

	[SerializeField]
	protected PlayerUIPhoneCells _phoneCells;

	[SerializeField]
	protected PlayerUIScore _score;

	[SerializeField]
	protected Color32 _coinsColor;

	[SerializeField]
	protected Color32 _scoreColor;

	public void ShowCoins(int income)
	{
		EffectManager.StartValueEffect(_coins.incomeTransform, _coinsColor, income, Vector3.zero);
	}

	public void ShowExp(int income)
	{
		EffectManager.StartValueEffect(_exp.incomeTransform, _exp.color, income, Vector3.zero);
	}

	public void ShowMedicine(int income)
	{
		EffectManager.StartValueEffect(_medicine.incomeTransform, _medicine.color, income, Vector3.zero);
	}

	public void ShowPhoneCells(int income)
	{
		EffectManager.StartValueEffect(_phoneCells.incomeTransform, _phoneCells.color, income, Vector3.zero);
	}

	public void ShowScore(int income)
	{
		EffectManager.StartValueEffect(_score.incomeTransform, _scoreColor, income, Vector3.zero);
	}

}
