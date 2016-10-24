using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldView : MonoBehaviour {		

	public bool isInit { get; protected set; }

	public Vector2 realSize
	{
		get
		{
			return new Vector2(
				(_filed.size_x) * (_info.spacing.x + _info.size.x) + _info.size.x,
				(_filed.size_y) * (_info.spacing.y + _info.size.y) + _info.size.y
				);
		}
	}
	
	public Field field
	{
		get { return _filed; }
	}

	[SerializeField]
	protected FieldViewInfo _info;
	[SerializeField]
	protected int _width;
	[SerializeField]
	protected int _height;
	[SerializeField]
	protected int _outTileRadius = 5;

	public int fullSizeX
	{
		get { return 2 * _outTileRadius + _filed.size_x; }
	}

	public int fullSizeY
	{
		get { return 2 * _outTileRadius + _filed.size_y; }
	}

	protected HashSet<TileView>	_tiles = new HashSet<TileView>();
	protected Field				_filed = new Field();
	protected HashSet<GameObject> _outTiles = new HashSet<GameObject>();

	public void Initilize()
	{
		_Generate();
		_RenderField();

		isInit = true;
	}

	[ContextMenu("Reinit")]
	public void Reinit()
	{
		if (isInit)
		{
			Clear();
			Initilize();
		}
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
			-((fullSizeX - 1) * (_info.spacing.x + _info.size.x)) / 2.0f,
			-((fullSizeY - 1) * (_info.spacing.y + _info.size.y)) / 2.0f,
			this.transform.position.z
		);
    }

	protected void _RenderField()
	{
		_InitRenderPos();
		_tiles.Clear();
		Vector3 pos = _startPos;

		for (int i = 0; i < fullSizeX; ++i)
		{
			pos.y = _startPos.y;

			for (int j = 0; j < fullSizeY; ++j)
			{
				if (i >= _outTileRadius && i < fullSizeX - _outTileRadius &&
					j >= _outTileRadius && j < fullSizeY - _outTileRadius)
				{
					_RenderBack(pos);
					_RenderTile(_filed[i - _outTileRadius, j - _outTileRadius], pos);
				}
				else
					_RenderOutTile(pos);

				pos.y += _deltaPos.y;
			}

			pos.x += _deltaPos.x;
		}
    }

	protected void _RenderBack(Vector3 pos)
	{
		var back = _info.backFactory.Allocate();
		back.Show();
		back.transform.position = new Vector3(
			pos.x, pos.y,
			back.transform.position.z
			);
		_outTiles.Add(back);
	}

	protected void _RenderOutTile(Vector3 pos)
	{
		var outTile = _info.outTileFactory.Allocate();
		outTile.Show();
		outTile.transform.position = pos;
        _outTiles.Add(outTile);
    }

	protected void _RenderTile(Field.Tile tile, Vector3 pos)
	{
		var tileView = _CreateTile(tile.type);
		tile.tileView = tileView;

		tileView.fieldView = this;       
		tileView.SetTile(tile);
        tileView.Show();

		tileView.transform.parent = this.transform;
        tileView.transform.localPosition = pos;

		_tiles.Add(tileView);
	}

	protected TileView _CreateTile(Field.Tile.TileTypes type)
	{
		TileView tile = null;

        switch (type)
		{
			case Field.Tile.TileTypes.ROAD:
				tile = _info.roadFactory.Allocate().GetComponent<TileView>();
                break;
			case Field.Tile.TileTypes.CURTAIN:
				tile = _info.curtainFactory.Allocate().GetComponent<TileView>();
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
			case Field.Tile.TileTypes.EXIT:
				tile = _info.exitFactory.Allocate().GetComponent<TileView>();
				break;
			case Field.Tile.TileTypes.TURNSTILE:
				tile = _info.turnstileFactory.Allocate().GetComponent<TileView>();
				break;
        }

		return tile;
	}

	public void UpdateTile(Field.Tile.TileTypes prevType, TileView tileView)
	{
		if (!_tiles.Contains(tileView))
			throw new UnityException("Tile view not found!");

		tileView.tile.left.tileView.ResetTile();
		tileView.tile.up.tileView.ResetTile();
		tileView.tile.right.tileView.ResetTile();
		tileView.tile.down.tileView.ResetTile();

		_tiles.Remove(tileView);
		_RenderTile(tileView.tile, tileView.transform.position);
		_ClearTile(prevType, tileView);		
    }

	//
	// </ Render tile >
	//

	public void Clear()
	{
		isInit = false;

		_ClearTiles();
		_ClearOutTiles();
    }

	protected void _ClearOutTiles()
	{
		foreach (var t in _outTiles)
		{
			_info.outTileFactory.Free(t);
		}

		_outTiles.Clear();
    }

	protected void _ClearTiles()
	{
		foreach(var t in _tiles)
		{
			_ClearTile(t.tile.type, t);
        }

		_tiles.Clear();
	}

	protected void _ClearTile(Field.Tile.TileTypes type, TileView tile)
	{
		tile.Clear();
		_FreeTile(type, tile);
	}

	protected void _FreeTile(Field.Tile.TileTypes type, TileView tile)
	{
		switch (type)
		{
			case Field.Tile.TileTypes.ROAD:
				_info.roadFactory.Free(tile.gameObject);
				break;
			case Field.Tile.TileTypes.CURTAIN:
				_info.curtainFactory.Free(tile.gameObject);
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
			case Field.Tile.TileTypes.EXIT:
				_info.exitFactory.Free(tile.gameObject);
				break;
			case Field.Tile.TileTypes.TURNSTILE:
				_info.turnstileFactory.Free(tile.gameObject);
				break;
		}
    }

}
