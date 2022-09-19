using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]

public class GameTileContFactory : GameObjectFactory
{
    [SerializeField]
    GameTileContent _destPrefab, _emptyPrefab, _wallPrefab, _spawnPointPrefab;
    public void Reclaim(GameTileContent content)
    {
        Destroy(content.gameObject);
    }
    public GameTileContent Get(GameTileContentType type)
    {
        switch (type)
        {
            case GameTileContentType.Destination:
                return Get(_destPrefab);
            case GameTileContentType.Empty:
                return Get(_emptyPrefab);
            case GameTileContentType.Wall:
                return Get(_wallPrefab);
            case GameTileContentType.SpawnPoint:
                return Get(_spawnPointPrefab);
        }

        return null;
    }
    private GameTileContent Get(GameTileContent prefab)
    {
        GameTileContent instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        return instance;
    }
}
