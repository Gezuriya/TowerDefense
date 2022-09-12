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

    public void Initialize(Vector2Int size)
    {
        _size = size;
        _ground.localScale = new Vector3(size.x, size.y, 1f);

        Vector2 Offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);

        _tiles = new GameTile[size.x * size.y];
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
            }
        }
        FindPath();
    }

    public void FindPath()
    {
        foreach (var tile in _tiles)
        {
            tile.ClearPath();
        }

        int destinationIndex = _tiles.Length / 2;
        _tiles[destinationIndex].BecomeDestination();
        _searchFrontier.Enqueue(_tiles[destinationIndex]);

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
            t.ShowPath();
        }
    }
}