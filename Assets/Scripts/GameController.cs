using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    Vector2Int _boardSize;

    [SerializeField]
    BoardContr _boardScript;

    [SerializeField]
    GameTileContFactory _contentFactory;

    [SerializeField]
    Camera _camera;
    private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouch();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleAlternative();
        }
    }
    void HandleTouch()
    {
        GameTile tile = _boardScript.GetTile(TouchRay);
        if(tile != null)
        {
            _boardScript.ToggleWall(tile);
        }
    }
    void HandleAlternative()
    {
        GameTile tile = _boardScript.GetTile(TouchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _boardScript.ToggleDestination(tile);
            }
            else
            {
                _boardScript.ToggleSpawners(tile);
            }
        }
    }
    void Start()
    {
        _boardScript.Initialize(_boardSize, _contentFactory);
    }
}
