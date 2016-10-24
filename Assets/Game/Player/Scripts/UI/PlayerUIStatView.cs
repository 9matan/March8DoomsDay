using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUIStatView : MonoBehaviour
{

	[SerializeField]
	protected Transform _incomeTransform;

	public Transform incomeTransform
	{
		get { return _incomeTransform; }
	}

	public Color32 color
	{
		get { return _uiText.color; }
	}

	[SerializeField]
	protected Text _uiText;

	public void SetVal(int val)
	{
		_uiText.text = "x" + val.ToString();
    }

}
