using UnityEngine;
using System.Collections;

public class TileView : MonoBehaviour {

	[SerializeField]
	protected SpriteRenderer _sprite;

	public Field.Tile tile { get; protected set; }

	public void SetTile(Field.Tile __tile)
	{
		tile = __tile;
    }

	public void Clear()
	{
		
    }

}
