using UnityEngine;
using System.Collections;

public class Effect : MonoBehaviour {

	public enum EEffectType
	{
		NONE,
		VALUE
	}

	public DOGOMemFactory factory { get; protected set; }
	public EEffectType type { get; protected set; }
	public bool isActive { get; protected set; }

	[SerializeField]
	protected float _effectTime;
	protected float _currentTime;

	public virtual void Initialize(DOGOMemFactory __factory)
	{
		factory = __factory;
    }

	public virtual void StartEffect()
	{
		isActive = true;
		_currentTime = 0.0f;
    }

	protected virtual void _OnEffectStep(float ratio)
	{

	}

	public virtual void StopEffect()
	{
		isActive = false;
		Clear();
    }

	public virtual void Clear()
	{
		factory.Free(this.gameObject);
    }

	protected virtual void Update()
	{
		if (!isActive) return;

		_currentTime += Time.deltaTime;
		this._OnEffectStep(Mathf.Max(0.0f, _currentTime / _effectTime));

		if (_currentTime >= _effectTime)
			this.StopEffect();
    }

	public void SetPosition(Vector2 pos)
	{
		this.transform.position = new Vector3(
			pos.x, pos.y,
			this.transform.position.z
			);
	}

}
