using UnityEngine;
using System.Collections;

public class MySuperUnityClass : MonoBehaviour {

	void Awake()
	{
		Debug.Log("Awake");
	}

	void Start()
	{
		Debug.Log("Start");
	}

	void Update()
	{
		Debug.Log("Update");
	}

	void FixedUpdate()
	{
		Debug.Log("FixedUpdate");
	}

	void OnEnable()
	{
		Debug.Log("Enable");
	}

	void OnDisable()
	{
		Debug.Log("OnDisable");
	}
}
