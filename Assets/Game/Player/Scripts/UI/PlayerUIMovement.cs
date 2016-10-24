using UnityEngine;
using System.Collections;

public class PlayerUIMovement : MonoBehaviour {

	[SerializeField]
	protected PlayerUI _ui;

	[SerializeField]
	protected float _cdTime = 1.5f;
	protected float _curTime = 0.0f;

	[SerializeField]
	protected UITimerButton[] _buttons;

#if UNITY_EDITOR || UNITY_STANDALONE
	protected void Awake()
	{
		this.Hide();
	}
#endif

	public void OnButtonClick(PlayerControlKeysMono key)
	{
		if(ValidTime())
			_Move(key);
	}

	protected void _Move(PlayerControlKeysMono key)
	{
		_curTime = _cdTime;
		_ui.OnButtonClick(key);
    }

	public bool ValidTime()
	{
		return _curTime <= 0.0f;
    }

	protected void Update()
	{
		_curTime -= Time.deltaTime;
		if (_curTime < 0.0f)
			_curTime = 0.0f;

		for (int i = 0; i < _buttons.Length; ++i)
			_buttons[i].SetPercent((_cdTime-_curTime) / _cdTime);
    }

}
