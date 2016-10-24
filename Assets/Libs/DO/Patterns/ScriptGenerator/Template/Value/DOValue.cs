using UnityEngine;

public interface IDOValue
{
	System.Object GetValue();
}

[System.Serializable]
public class DOValue<T> : MonoBehaviour, IDOValue
{

	public T value;

	public System.Object GetValue()
	{
		return (System.Object)value;
	}

}
