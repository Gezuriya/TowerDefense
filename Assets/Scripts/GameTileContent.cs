using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTileContent : MonoBehaviour
{
    [SerializeField]
    GameTileContentType _type;
    public GameTileContentType Type => _type;
    public GameTileContFactory OriginFactory { get; set; }

    public void Recycle()
    {
        OriginFactory.Reclaim(this);
    }
}
public enum GameTileContentType
{
    Empty,
    Destination,
    Wall
}
