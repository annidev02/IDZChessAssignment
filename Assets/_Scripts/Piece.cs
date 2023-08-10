using System.Collections.Generic;
using Chess.Scripts.Core;
using UnityEngine;

public class Piece : MonoBehaviour
{
    // Getting the current position of the piece on the board 
    private Vector2Int currentPos;

    // List for storing all the possible positions of the piece 
    public List<Vector2Int> possiblePositions = new List<Vector2Int>();

    private void OnMouseDown()
    {
        // Clear the available highlights whenever click on the piece 
        ChessBoardPlacementHandler.Instance.ClearHighlights();

        // Determine the current piece placement in grid
        ChessPlayerPlacementHandler placement = GetComponent<ChessPlayerPlacementHandler>();
        currentPos = new Vector2Int(placement.row, placement.column);

        // Determine the current chess piece and calculate the legal positions 
        switch (gameObject.tag)
        {
            case "Pawn":
                CalculatePawnMovePositions();
                break;
            case "Knight":
                CalculateKnightPositions();
                break;
            case "Bishop":
                CalculateBishopPositions();
                break;
            case "Rook":
                CalculateRookPositions();
                break;
            case "Queen":
                CalculateBishopPositions();
                CalculateRookPositions();
                CalculateKingPositions();
                break;
            case "King":
                CalculateKingPositions();
                break;
        }

        // Highlight the available legal possible moves 
        HightlightPositions();

    }

    #region ALL METHODS FOR CALCULATIONS 

    private void CalculatePawnMovePositions()
    {
        for (int i = 1; i <= 2; i++)
        {
            bool obstructed;
            Vector2Int predictedPosition = new Vector2Int(currentPos.x + i, currentPos.y);
            CheckObstruction(predictedPosition, out obstructed);
            if (obstructed) break;
        }
    }

    private void CalculateKnightPositions()
    {
        Vector2Int[] knightMoves = new Vector2Int[]
        {
        new Vector2Int(currentPos.x + 2, currentPos.y + 1),
        new Vector2Int(currentPos.x + 2, currentPos.y - 1),
        new Vector2Int(currentPos.x - 2, currentPos.y + 1),
        new Vector2Int(currentPos.x - 2, currentPos.y - 1),
        new Vector2Int(currentPos.x + 1, currentPos.y + 2),
        new Vector2Int(currentPos.x + 1, currentPos.y - 2),
        new Vector2Int(currentPos.x - 1, currentPos.y + 2),
        new Vector2Int(currentPos.x - 1, currentPos.y - 2)
        };

        foreach (Vector2Int predictedPosition in knightMoves)
        {
            if (Mathf.Sign(predictedPosition.x) != -1 && Mathf.Sign(predictedPosition.y) != -1)
            CheckObstruction(predictedPosition, out bool obstructed);
        }
    }

    private void CalculateKingPositions()
    {
        Vector2Int[] kingMoves = new Vector2Int[]
        {
            new Vector2Int(currentPos.x + 1, currentPos.y),
            new Vector2Int(currentPos.x - 1, currentPos.y),
            new Vector2Int(currentPos.x + 1, currentPos.y + 1),
            new Vector2Int(currentPos.x + 1, currentPos.y - 1),
            new Vector2Int(currentPos.x, currentPos.y + 1),
            new Vector2Int(currentPos.x, currentPos.y - 1),
            new Vector2Int(currentPos.x - 1, currentPos.y + 1),
            new Vector2Int(currentPos.x - 1, currentPos.y -1)
        };
        foreach (Vector2Int predictedPosition in kingMoves)
        {
            if (Mathf.Sign(predictedPosition.x) != -1 && Mathf.Sign(predictedPosition.y) != -1)
                CheckObstruction(predictedPosition, out bool obstructed);
        }
    }

    private void CalculateRookPositions()
    {
        Vector2Int[] directions = {
        new Vector2Int(1, 0),  // Up
        new Vector2Int(-1, 0), // Down
        new Vector2Int(0, -1), // Left
        new Vector2Int(0, 1)   // Right
        };

        foreach (Vector2Int direction in directions)
        {
            for (int distance = 1; distance <= 7; distance++)
            {
                Vector2Int predictedPosition = currentPos + direction * distance;
                bool obstructed = false;
                if (Mathf.Sign(predictedPosition.x) != -1 && Mathf.Sign(predictedPosition.y) != -1)
                    CheckObstruction(predictedPosition, out obstructed);
                if (obstructed) break;
            }
        }
    }

    private void CalculateBishopPositions()
    {
        Vector2Int[] directions = {
        new Vector2Int(1, 1),   // Up-Right
        new Vector2Int(1, -1),  // Up-Left
        new Vector2Int(-1, 1),  // Down-Right
        new Vector2Int(-1, -1)  // Down-Left
    };

        foreach (Vector2Int direction in directions)
        {
            for (int distance = 1; distance <= 7; distance++)
            {
                Vector2Int predictedPosition = currentPos + direction * distance;
                bool obstructed = false;
                if (Mathf.Sign(predictedPosition.x) != -1 && Mathf.Sign(predictedPosition.y) != -1)
                    CheckObstruction(predictedPosition, out obstructed);
                if (obstructed) break;
            }
        }
    }

    private void CheckObstruction(Vector2Int predictedPosition, out bool obstructed)
    {
        obstructed = false;
        foreach (ChessPlayerPlacementHandler pos in MoveCalculator.Instance.pieces)
        {
            Vector2Int otherPiecePosition = new Vector2Int(pos.row, pos.column);
            if (predictedPosition.Equals(otherPiecePosition))
            {
                obstructed = true;
                break;
            }
        }
        if (obstructed) return;
        if (!possiblePositions.Contains(predictedPosition) && predictedPosition.x <=7 && predictedPosition.y <= 7) possiblePositions.Add(predictedPosition);
    }

    private void HightlightPositions()
    {
        for (int i = 0; i < possiblePositions.Count; i++)
        {
            ChessBoardPlacementHandler.Instance.Highlight(possiblePositions[i].x, possiblePositions[i].y);
        }
    }

    #endregion

}
