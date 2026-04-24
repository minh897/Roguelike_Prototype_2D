using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public event Action OnWantToEnterFail;

    public Vector2Int CurrentCellPos { get; private set; }

    private Vector2Int _direction;

    public bool TryToMove(Vector2 input, BoardManager board)
    {
        if (!enabled)
            return false;

        _direction = new((int)input.x, (int)input.y);

        Vector2Int nextCellTarget = CurrentCellPos + _direction;
        CellData nextCellData = board.GetCellData(nextCellTarget);

        // Stop player from occupying an unpassable cell (borders)
        if (nextCellData != null && !nextCellData.passable)
            return false;

        CellObject obj = nextCellData.containedObject;
        // The target cell is empty
        if (obj == null)
        {
            MoveTo(nextCellTarget, board);
        }
        // The target cell is not empty and the player wants to enter
        else if (obj.PlayerWantsToEnter())
        {
            MoveTo(nextCellTarget, board);
            obj.PlayerEntered();
        }
        // The target cell is not empty and the player can't enter
        else
        {
            OnWantToEnterFail?.Invoke();
        }

        return true;
    }

    public void MoveTo(Vector2Int cell, BoardManager board)
    {
        CurrentCellPos = cell;
        transform.position = board.CellToWorld(CurrentCellPos);
    }
}
