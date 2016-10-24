using UnityEngine;
using System.Collections;

public class UISelftTimerButton : UITimerButton {

	public delegate void OnValidClick(PlayerControlKeysMono key);

	public event OnValidClick onValidClick = delegate { };

	[SerializeField]
	protected PlayerControlKeysMono _key;

	[SerializeField]
	protected float _cdTime;
	protected float _curTime;

	public void OnClick()
	{
		if(ValidTime())
		{
			_curTime = _cdTime;
			onValidClick(_key);
        }
	}

	public bool ValidTime()
	{
		return _curTime <= 0.0f;
	}

	protected virtual void Update()
	{
		_curTime -= Time.deltaTime;
		if (_curTime < 0.0f)
			_curTime = 0.0f;

		SetPercent((_cdTime - _curTime) / _cdTime);
	}

}
