using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RatingsList : MonoBehaviour
{

	public struct RatingInfo
	{
		public string name;
		public int score;
		public int place;
	}

	public int ratingsNumber
	{
		get
		{
			return _listRT.childCount;
        }
	}

	[SerializeField]
	protected RectTransform _listRT;

	[SerializeField]
	protected int _maxNumber;

	[SerializeField]
	protected DOGOMemFactory _itemFactory;

	protected float _deltaH = 0.0f;

	protected void Awake()
	{
		_Initialize();
    }

	protected void _Initialize()
	{
		_deltaH = -_GetDelta();
		ShowScores();
    }

	protected float _GetDelta()
	{
		return _itemFactory.creator.prefab.GetComponent<RectTransform>().GetHeight();
    }	

	public void SetRatings(List<RatingInfo> _infoes)
	{
		bool isValid = true;
		for (int i = 0; i < _infoes.Count && !isValid; ++i)
			isValid = SetRating(_infoes[i]);
    }

	public bool SetRating(RatingInfo _info)
	{
		if (ratingsNumber >= _maxNumber) return false;

		var item = _itemFactory.Allocate().GetComponent<RatingsListItem>();
		_LocateItem(item);

		item.score = _info.score;
		item.pname = _info.name;
		item.place = _info.place;

		return true;
	}

	protected void _LocateItem(RatingsListItem item)
	{
		item.Show();

		float y = _deltaH * ratingsNumber;

		item.rectTransform.SetParent(_listRT, true);
		item.transform.parent = _listRT;
        item.rectTransform.localPosition = new Vector3(
			0.0f, y,
			item.rectTransform.localPosition.z
			);

		item.rectTransform.SetDefault();

		_listRT.SetHeight(Mathf.Abs(_deltaH) * ratingsNumber);
    }

	public void ShowScores()
	{
		RatingInfo info = new RatingInfo();
		bool isValid = true;
		int number = 0;

		Pair<string, int> cur = ScoreHolder.GetElement(number++);

		while (cur != null)
		{
			info.place = number;
			info.name = cur.First;
			info.score = cur.Second;

			isValid = SetRating(info);
			if (!isValid) break;

			cur = ScoreHolder.GetElement(number++);
        }
	}

}
