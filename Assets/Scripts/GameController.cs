using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    Vector2Int _boardSize;

    [SerializeField]
    BoardContr _boardScript;


    void Start()
    {
        _boardScript.Initialize(_boardSize);
    }
}
