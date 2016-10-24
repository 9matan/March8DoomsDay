using UnityEngine;
using System.Collections;

public class EffectManager : MonoBehaviour {

	[SerializeField]
	protected Color32 _damageColor;

	[SerializeField]
	protected Color32 _healColor;

	[SerializeField]
	protected Color32 _coinColor;

	[SerializeField]
	protected Color32 _expColor;

	[SerializeField]
	protected Color32 _scoreColor;

	protected void Awake()
	{
	
	}

	public void Initialize()
	{
		GlobalEventSystem.OnSpawn += _OnUnitSpawn;
		GlobalEventSystem.OnScoreChanged += _OnScoreChanged;
		GlobalEventSystem.OnGoldChanged += _OnCoinChanged;
		GlobalEventSystem.OnExperienceChanged += _OnExpChanged;

		StartCoroutine(IEInitOnPlayer());
    }

	protected IEnumerator IEInitOnPlayer()
	{
		while (GlobalDataHolder.player == null || !GlobalDataHolder.isLevelStart)
			yield return null;

		GlobalDataHolder.player.OnHealingRecieved += _OnHeal;		
    }

	protected void _OnUnitSpawn(FieldUnit unit)
	{
		unit.OnDamageRecieved += _OnDamageRecieved;
    }

	protected void _OnDamageRecieved(FieldUnit unit, int damage)
	{
		StartValueEffect(unit.transform, _damageColor, -damage, Vector3.zero, false);
	}

	protected void _OnHeal(FieldUnit unit, int val)
	{
		StartValueEffect(unit.transform, _healColor, val, Vector3.zero, false);
    }

	static protected Vector3 _uiScaled = new Vector3(0.4f, 0.4f, 0.4f);
	static protected Vector3 _noUiScale = new Vector3(0.025f, 0.025f, 0.025f);

	public static void StartValueEffect(Transform parent, Color32 color, int val, Vector3 delta, bool isUI = true)
	{
		var effect = (ValueEffect)EffectFactory.i.CreateEffect(Effect.EEffectType.VALUE);

        effect.color = color;
		effect.val = val;
		effect.SetPosition(parent.transform.position + delta);
        effect.Show();
		effect.StartEffect();
		effect.transform.SetParent(parent, true);
		((RectTransform)(effect.transform)).SetDefault();

		if (isUI)
		{			
			effect.SetTextScale(_uiScaled);			
		}
		else
		{
			effect.transform.SetParent(null, true);
			effect.SetTextScale(_noUiScale);
		}

		effect.transform.localPosition = new Vector3(
			effect.transform.localPosition.x,
			effect.transform.localPosition.y,
			-6.0f // bad code
			);
    }

	protected void _OnCoinChanged(int delta)
	{
		GlobalDataHolder.player.ui.showIncome.ShowCoins(delta);
    }

	protected void _OnExpChanged(int delta)
	{
		GlobalDataHolder.player.ui.showIncome.ShowExp(delta);
	}

	protected void _OnScoreChanged(int delta)
	{
		GlobalDataHolder.player.ui.showIncome.ShowScore(delta);
	}

}
