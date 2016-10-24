using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UITimerButton : MonoBehaviour {

	public Image image
	{
		get
		{
			if (_image == null)
				_image = this.GetComponent<Image>();
			return _image;
        }
	}

	protected Image _image = null;

	public void SetPercent(float p)
	{
		image.fillAmount = p;
	}
}
