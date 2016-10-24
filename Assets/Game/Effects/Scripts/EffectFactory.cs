using UnityEngine;
using System.Collections;

public class EffectFactory : MonoBehaviour {

	static public EffectFactory i
	{
		get { return _instance; }
	}

	static protected EffectFactory _instance;

	protected void Awake()
	{
		_instance = this;
	}

	[SerializeField]
	protected DOGOMemFactory _damageFactory;

	public Effect CreateEffect(Effect.EEffectType type)
	{
		var factory = GetFactory(type);
		var effect = factory.Allocate().GetComponent<Effect>();

		effect.Initialize(factory);
		return effect;
    }

	public DOGOMemFactory GetFactory(Effect.EEffectType type)
	{
		DOGOMemFactory factory = null;

		switch (type)
		{
			case Effect.EEffectType.VALUE:
				factory = _damageFactory;
                break;
		}

		return factory;
    }

}
