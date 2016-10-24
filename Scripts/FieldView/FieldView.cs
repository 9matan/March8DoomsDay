using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldView : MonoBehaviour {		

	[SerializeField]
	protected FieldViewInfo _info;
	[SerializeField]
	protected int _width;
	[SerializeField]
	protected int _height;

	protected List<TileView>	_tiles = new List<TileView>();
	protected Field				_filed = new Field();

	protected void Start()
	{
		Initilize();
    }

	public void Initilize()
	{
		_Generate();
		_RenderField();
    }

	protected void _Generate()
	{
		_filed.Generate(_width, _height);
	}

	//
	// < Render field >
	//

	protected Vector3 _startPos;
	protected Vector3 _deltaPos;

	protected void _InitRenderPos()
	{
		_deltaPos = _info.spacing + _info.size;

		_startPos = new Vector3(
			-(_filed.size_x / 2) * (_info.spacing.x + _info.size.x),
			-(_filed.size_y / 2) * (_info.spacing.y + _info.size.y),
			this.transform.position.z
		);
    }

	protected void _RenderField()
	{
		_InitRenderPos();
		Vector3 pos = _startPos;

		for (int i = 0; i < _filed.size_x; ++i)
		{
			pos.y = _startPos.y;

			for (int j = 0; j < _filed.size_y; ++j)
			{
				_RenderTile(_filed[i, j], pos);
				pos.y += _deltaPos.y;
			}

			pos.x += _deltaPos.x;
		}
    }

	protected void _RenderTile(Field.Tile tile, Vector3 pos)
	{
		var tileView = _CreateTile(tile.type);

		tileView.SetTile(tile);
        tileView.Show();

		tileView.transform.parent = this.transform;
        tileView.transform.localPosition = pos;
    }

	protected TileView _CreateTile(Field.Tile.TileTypes type)
	{
		TileView tile = null;

        switch (type)
		{
			case Field.Tile.TileTypes.ROAD:
				tile = _info.roadFactory.Allocate().GetComponent<TileView>();
                break;
			case Field.Tile.TileTypes.WALL:
				tile = _info.wallFactory.Allocate().GetComponent<TileView>();
				break;
			case Field.Tile.TileTypes.SPAWN:
				tile = _info.spawnFactory.Allocate().GetComponent<TileView>();
				break;
			case Field.Tile.TileTypes.STORK:
				tile = _info.storkFactory.Allocate().GetComponent<TileView>();
				break;
			case Field.Tile.TileTypes.STRUCTURE:
				tile = _info.structureFactory.Allocate().GetComponent<TileView>();
				break;
		}

		return tile;
	}

	//
	// </ Render tile >
	//

	public void Clear()
	{
		_ClearTiles();
    }

	protected void _ClearTiles()
	{
		for (int i = 0; i < _tiles.Count; ++i)
		{
			_tiles[i].Clear();
			_FreeTile(_tiles[i]);
        }

		_tiles.Clear();
	}

	protected void _FreeTile(TileView tile)
	{
		switch (tile.tile.type)
		{
			case Field.Tile.TileTypes.ROAD:
				_info.roadFactory.Free(tile.gameObject);
				break;
			case Field.Tile.TileTypes.WALL:
				_info.wallFactory.Free(tile.gameObject);
				break;
			case Field.Tile.TileTypes.SPAWN:
				_info.spawnFactory.Free(tile.gameObject);
				break;
			case Field.Tile.TileTypes.STORK:
				_info.storkFactory.Free(tile.gameObject);
				break;
			case Field.Tile.TileTypes.STRUCTURE:
				_info.structureFactory.Free(tile.gameObject);
				break;
		}
    }

}
