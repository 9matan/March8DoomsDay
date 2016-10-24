using UnityEngine;
using System.Collections;

public class ItemView : MonoBehaviour
{

	[SerializeField]
	protected ItemViewFace _face;

	public Item item { get; protected set; }

	public void Initialize(Item __item)
	{
		item = __item;

//		Debug.Log("Lolka: " + FlowerSpriteFactory.i.name);

		if (item.type == Item.EItemType.FLOWER)
			SetFace(
				FlowerSpriteFactory.i.CreateFace());
    }

	public void SetFace(ItemViewFace __face)
	{
		_face = __face;
		_face.Show();
		_face.transform.parent = this.transform;
		_face.transform.localPosition = Vector3.zero;
	}

	public void SetPosition(Vector2 pos)
	{
		this.transform.position = new Vector3(
			pos.x, pos.y,
			this.transform.position.z
			);
	}

	public void Clear()
	{
		if (item.type == Item.EItemType.FLOWER)
			FlowerSpriteFactory.i.FreeFace(_face);
    }

}
