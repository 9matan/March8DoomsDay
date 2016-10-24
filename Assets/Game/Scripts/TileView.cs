using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TileView : MonoBehaviour {

	public FieldView fieldView;
	public GameObject face { get; protected set; }
	public Field.Tile tile { get; protected set; }

	public Enemy.EEnemyType enemyType
	{
		get
		{
			if(tile.type == Field.Tile.TileTypes.SPAWN)
			{
				return GlobalDataHolder.FindEnemy(tile.enemy_to_spawn).type;
			}

			return Enemy.EEnemyType.NONE;
		}
	}

	protected int _structureMask = 0;

	public void UpdateView(Field.Tile.TileTypes prevType)
	{
		fieldView.UpdateTile(prevType, this);
    }

	public virtual void ResetTile()
	{
		if (tile.type == Field.Tile.TileTypes.STRUCTURE)
			SetTile(tile);
    }

	public virtual void SetTile(Field.Tile __tile)
	{
		tile = __tile;

		if (tile.type == Field.Tile.TileTypes.STRUCTURE)
		{
			_ClearFace();
			SetFace(
				StructureFaceFactory.i.CreateFace(
					_structureMask = tile.GetNoEqMask(Field.Tile.TileTypes.STRUCTURE)));
        }
		else if(tile.type == Field.Tile.TileTypes.SPAWN)
		{	
			SetFace(
				SpawnFaceFactory.i.CreateFace(
					enemyType));
        }

		if (tile.item != null)
			UpdateItemView();
    }

	public void SetFace(GameObject __face)
	{
		face = __face;
		face.Show();
        face.transform.parent = this.transform;
		face.transform.localPosition = Vector3.zero;
    }

	//
	// < item view >
	//

	protected ItemView _itemView;

	public void UpdateItemView()
	{
		ClearItemView();

		_itemView = _CreateItemView(tile.item.type);

		_itemView.Show();
        _itemView.Initialize(tile.item);
		_itemView.transform.parent = this.transform;
        _itemView.SetPosition(this.transform.position);
    }

	protected ItemView _CreateItemView(Item.EItemType type)
	{
		return ItemViewFactory.i.CreateItemView(type);
	}

	public void ClearItemView()
	{
		if (_itemView == null) return;
		_itemView.Clear();
        ItemViewFactory.i.FreeItemView(_itemView);
    }

	//
	// </ item view >
	//

	public virtual void Clear()
	{
		_ClearFace();
		ClearItemView();
    }

	protected void _ClearFace()
	{
		if (face == null) return;

		if (tile.type == Field.Tile.TileTypes.STRUCTURE)
			StructureFaceFactory.i.Free(_structureMask, face);
		else if (tile.type == Field.Tile.TileTypes.SPAWN)
			SpawnFaceFactory.i.Free(face, enemyType);
	}


	public override int GetHashCode()
	{
		return tile.x + 10007 * tile.y;
	}

	protected void Update()
	{
//		UpdateDebug();
    }

	//
	// Debug
	//

	public bool debug = false;

	[SerializeField]
	protected Text _debug_text;

	void UpdateDebug()
	{
		if (!debug) return;

		if (tile != null)
		{
			if (tile.player_far == -1)
				_debug_text.text = "";
			else
				_debug_text.text = tile.player_far.ToString();
		}
    }

	//

}
