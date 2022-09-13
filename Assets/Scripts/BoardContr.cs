using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardContr : MonoBehaviour
{
    [SerializeField] Transform _ground;

    [SerializeField] GameTile _tilePref;

    Vector2Int _size;
    private GameTile[] _tiles;

    private Queue<GameTile> _searchFrontier = new Queue<GameTile>();
    private GameTileContFactory _contentFactory;
    public void Initialize(Vector2Int size, GameTileContFactory contentFactory)
    {
        _size = size;
        _ground.localScale = new Vector3(size.x, size.y, 1f);

        Vector2 Offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);

        _tiles = new GameTile[size.x * size.y];
        _contentFactory = contentFactory;
        for(int i= 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                GameTile tile = _tiles[i] = Instantiate(_tilePref);
                tile.transform.SetParent(transform, false);
                tile.transform.localPosition = new Vector3(x - Offset.x, 0, y - Offset.y);

                if (x > 0)
                {
                    GameTile.MakeEastWestNeighbors(tile, _tiles[i - 1]);

                }
                if (y > 0)
                {
                    GameTile.MakeSouthNorthNeighbors(_tiles[i-size.x], tile);
                }
                tile.isAlternative = (x & 1) == 0;
                if((y&1) == 0)
                {
                    tile.isAlternative = !tile.isAlternative;
                }

                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            }
        }
        ToggleDestination(_tiles[_tiles.Length/2]);
    }

    public bool FindPath()
    {
        foreach (var t in _tiles)
        {
            if (t.Content.Type == GameTileContentType.Destination)
            {
                t.BecomeDestination();
                _searchFrontier.Enqueue(t);
            }
            else
            {
                t.ClearPath();
            }
        }

        if(_searchFrontier.Count == 0)
        {
            return false;
        }

        while(_searchFrontier.Count > 0)
        {
            GameTile tile = _searchFrontier.Dequeue();

            if(tile != null)
            {
                if (tile.isAlternative)
                {
                    _searchFrontier.Enqueue(tile.GrowPathNorth());
                    _searchFrontier.Enqueue(tile.GrowPathSouth());
                    _searchFrontier.Enqueue(tile.GrowPathEast());
                    _searchFrontier.Enqueue(tile.GrowPathWest());
                }
                else
                {
                    _searchFrontier.Enqueue(tile.GrowPathWest());
                    _searchFrontier.Enqueue(tile.GrowPathEast());
                    _searchFrontier.Enqueue(tile.GrowPathSouth());
                    _searchFrontier.Enqueue(tile.GrowPathNorth());
                }
            }
        }
        foreach(var t in _tiles)
        {
            if (!t.HasPath)
            {
                return false;
            }
        }
        foreach(var t in _tiles)
        {
            t.ShowPath();
        }
        return true;
    }

    public void ToggleDestination(GameTile tile)
    {
        if(tile.Content.Type == GameTileContentType.Destination)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            if (!FindPath())
            {
                tile.Content = _contentFactory.Get(GameTileContentType.Destination);
                FindPath();
            }
        }
        else if(tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Destination);
            FindPath();
        }
    }
    public void ToggleWall(GameTile tile)
    {
        if(tile.Content.Type == GameTileContentType.Wall)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Wall);
            FindPath();
        }
        else if(tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Wall);
            if (!FindPath())
            {
                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
                FindPath();
            }
        }
    }
    public GameTile GetTile(Ray ray)
    {
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            int x = (int)(hit.point.x + _size.x * 0.5f);
            int y = (int)(hit.point.z + _size.y * 0.5f);
            if( x >= 0 && x < _size.x && y >=0 && y< _size.y)
            {
                return _tiles[x + y * _size.x];
            }
        }
        return null;
    }
}
