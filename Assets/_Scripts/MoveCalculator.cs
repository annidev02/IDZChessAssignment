using UnityEngine;
using Chess.Scripts.Core;

public class MoveCalculator : MonoBehaviour
{
    #region SINGELTON
    public static MoveCalculator Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public ChessPlayerPlacementHandler[] pieces;
}

