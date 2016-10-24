using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFilledField : MonoBehaviour {

	[SerializeField]
	protected Image _curImage;

	[SerializeField]
	protected Image _backImage;

	public void SetFill(float r)
	{
		_curImage.rectTransform.SetWidth
			(Mathf.Lerp(0.0f, _backImage.rectTransform.GetWidth(), r));
	}

	protected virtual void Update()
	{

	}

}
