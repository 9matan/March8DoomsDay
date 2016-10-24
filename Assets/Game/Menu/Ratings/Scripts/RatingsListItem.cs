using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RatingsListItem : MonoBehaviour
{

	public RectTransform rectTransform
	{
		get
		{
			if (_rt == null)
				_rt = GetComponent<RectTransform>();

			return _rt;
		}
	}

	protected RectTransform _rt;

	[SerializeField]
	protected Text _uiPlace;

	public int place
	{
		get { return _place; }
		set
		{
			_place = value;
			_uiPlace.text = _place.ToString() + ".";
        }
	}

	protected int _place;

	[SerializeField]
	protected Text _uiName;

	public string pname
	{
		get { return _pname; }
		set
		{
			_pname = value;
			_uiName.text = _pname;
		}
	}

	protected string _pname;

	[SerializeField]
	protected Text _uiScore;

	public int score
	{
		get { return _score; }
		set
		{
			_score = value;
			_uiScore.text = _score.ToString();
		}
	}

	protected int _score;


}
