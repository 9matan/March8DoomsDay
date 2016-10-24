using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
		
	void LateUpdate()
	{
		if(GlobalDataHolder.player != null)
			SetPosition(GlobalDataHolder.player.transform.position);
	}

	public void SetPosition(Vector2 pos)
	{
		this.transform.position = new Vector3(
			pos.x, pos.y,
			this.transform.position.z
			);
    }

}
