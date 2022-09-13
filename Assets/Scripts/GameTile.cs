using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    [SerializeField]
    Transform _arrow;

    private GameTile _north, _east, _west, _south, _nextTile;
    int _distance;

    public bool HasPath => _distance != int.MaxValue;
    public bool isAlternative {get; set;}

    private Quaternion _northRot = Quaternion.Euler(90, -90, 0);
    private Quaternion _eastRot = Quaternion.Euler(90, 0, 0);
    private Quaternion _southRot = Quaternion.Euler(90, 90, 0);
    private Quaternion _westRot = Quaternion.Euler(90, 180, 0);

    private GameTileContent _content;

    public GameTileContent Content
    {
        get => _content;
        set
        {
            if(_content != null)
            {
                _content.Recycle();
            }

            _content = value;
            _content.transform.localPosition = transform.localPosition;
        }
    }

    public static void MakeEastWestNeighbors(GameTile east, GameTile west)
    {
        west._east = east;
        east._west = west;
    }

    public static void MakeSouthNorthNeighbors(GameTile south, GameTile north)
    {
        north._south = south;
        south._north = north;
    }

    public void ClearPath()
    {
        _distance = int.MaxValue;
        _nextTile = null;
    }
    public void BecomeDestination()
    {
        _distance = 0;
        _nextTile = null;
    }

    private GameTile GrowPathTo(GameTile neighbor)
    {
        if(!HasPath || neighbor == null || neighbor.HasPath)
        {
            return null;
        }
        neighbor._distance = _distance + 1;
        neighbor._nextTile = this;
        return neighbor.Content.Type !=  GameTileContentType.Wall ? neighbor : null;
    }

    public GameTile GrowPathNorth() => GrowPathTo(_north); 
    public GameTile GrowPathEast() => GrowPathTo(_east);
    public GameTile GrowPathSouth() => GrowPathTo(_south);
    public GameTile GrowPathWest() => GrowPathTo(_west);

    public void ShowPath()
    {
        if(_distance == 0)
        {
            _arrow.gameObject.SetActive(false);
            return;
        }
        _arrow.gameObject.SetActive(true);
        _arrow.localRotation =
            _nextTile == _north ? _northRot:
            _nextTile == _east ? _eastRot:
            _nextTile == _south ? _southRot:
            _westRot;
    }

}
