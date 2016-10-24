using UnityEngine;
using System.Collections;

public class PlayerKeyControl : MonoBehaviour
{

	[SerializeField]
	protected float _cdTime = 0.35f;

#if !(UNITY_EDITOR || UNITY_STANDALONE)
	protected void Awake()
	{
		this.Hide();
    }
#endif

#if (UNITY_EDITOR || UNITY_STANDALONE)

	protected float _currentTime = 0.0f;

	protected void Update()
	{
		_currentTime += Time.deltaTime;

		Check();
    }

	protected void FixedUpdate()
	{
		Check();
	}


	protected void Check()
	{
		if (_currentTime >= _cdTime)
		{
			if (Input.GetKey(KeyCode.UpArrow))
			{
				GlobalDataHolder.player.MoveByDirection(Field.Directions.DOWN);
				_currentTime = 0.0f;
			}

			if (Input.GetKey(KeyCode.LeftArrow))
			{
				GlobalDataHolder.player.MoveByDirection(Field.Directions.LEFT);
				_currentTime = 0.0f;
			}

			if (Input.GetKey(KeyCode.RightArrow))
			{
				GlobalDataHolder.player.MoveByDirection(Field.Directions.RIGHT);
				_currentTime = 0.0f;
			}

			if (Input.GetKey(KeyCode.DownArrow))
			{
				GlobalDataHolder.player.MoveByDirection(Field.Directions.UP);
				_currentTime = 0.0f;
			}
		}
	}

#endif

}
