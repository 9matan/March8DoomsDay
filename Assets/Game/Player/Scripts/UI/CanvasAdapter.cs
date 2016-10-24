using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasAdapter : MonoBehaviour
{

	public CanvasScaler canvasScaler
	{
		get
		{
			if (_canvasScaler == null)
				_canvasScaler = this.GetComponent<CanvasScaler>();
			return _canvasScaler;
		}
	}

	protected CanvasScaler _canvasScaler;

	[SerializeField]
	protected int _optWidth;

	[SerializeField]
	protected int _optHeight;

	protected void Awake()
	{
		Adapt();
    }

	public void Adapt()
	{
		float wc = (float)Screen.width / (float)_optWidth;
		float hc = (float)Screen.height/ (float)_optHeight;

		float c = Mathf.Min(wc, hc);

		canvasScaler.scaleFactor = c;
    }

	[ContextMenu("Set current screen")]
	public void SetCurrentScreen()
	{
		_optWidth = Screen.width;
		_optHeight = Screen.height;
	}

}
