using UnityEngine;
using System.Collections;

public interface IDOScriptGenerator
{
	void Generate();
}

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class DOScriptGenerator : MonoBehaviour, IDOScriptGenerator {

	[SerializeField] GameObject templatesObject = null;

	virtual public void Generate()
	{
		
	}

	#if UNITY_EDITOR

	virtual protected void Update()
	{

	}

	#else

//	virtual public 

	#endif

}
