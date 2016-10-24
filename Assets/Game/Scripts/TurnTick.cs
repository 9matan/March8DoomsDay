using UnityEngine;
using System.Collections;

public class TurnTick : MonoBehaviour
{

	protected float _curTime = 0.0f;

	protected void Update()
	{
		if (!GlobalDataHolder.isLevelStart) return;

		_curTime += Time.deltaTime;

		if (_curTime >= GlobalDataHolder.turn_tick_time)
			_Tick();
    }

	protected void _Tick()
	{
		GlobalEventSystem.RaiseTurnTick();
		_curTime = 0.0f;
    }

}
