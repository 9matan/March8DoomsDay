using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ValueEffect : Effect
{

	public ValueEffect()
	{
		type = EEffectType.VALUE;
	}

	[SerializeField]
	protected Text _text;

	[SerializeField]
	protected float _deltaHeight;

	[SerializeField]
	[Range(0.0f, 1.0f)]
	protected float _targetAlpha;

	[SerializeField]
	[Range(0.0f, 1.0f)]
	protected float _startAlpha = 1.0f;

	protected float _startHeight;
	protected float _targetHeight;

	protected int _val;

	public int val
	{
		get
		{
			return _val;
		}

		set
		{
			_val = value;
			if(_val > 0)
				_text.text = "+" + value.ToString();
			else
				_text.text = value.ToString();
		}
	}

	public Color32 color
	{
		get { return _text.color; }
		set { _text.color = value; }
	}

	public override void StartEffect()
	{
		base.StartEffect();

		_startHeight = this.transform.position.y;
		_targetHeight = _startHeight + _deltaHeight;
    }

	protected override void _OnEffectStep(float ratio)
	{
		base._OnEffectStep(ratio);

		this.transform.position = new Vector3(
			this.transform.position.x,
			Mathf.Lerp(_startHeight, _targetHeight, ratio),
			this.transform.position.z
			);

		_text.color = _text.color.SetAlpha(Mathf.Lerp(_startAlpha, _targetAlpha, ratio));
    }

	public void SetTextScale(Vector3 sc)
	{
		_text.transform.localScale = sc;
	}

}
