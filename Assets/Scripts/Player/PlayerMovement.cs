using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2Int _currentCellPos;
    private Vector2Int _direction;

    public bool TryToMove(Vector2 input, BoardManager board)
    {
        if (!enabled)
            return false;

        _direction = new((int)input.x, (int)input.y);

        Vector2Int nextCellTarget = _currentCellPos + _direction;
        CellData nextCellData = board.GetCellData(nextCellTarget);

        // Stop player from occupying an impassable cell (borders)
        if (nextCellData != null && !nextCellData.passable)
            return false;

        // Check the condition for the player to occupy a passable cell
        CellObject obj = nextCellData.containedObject;
        if (obj == null)
        {
            MoveTo(nextCellTarget, board);
        } 
        else if (obj.PlayerWantsToEnter())
        {
            MoveTo(nextCellTarget, board);
            obj.PlayerEntered();
        }

        return true;
    }

    public void MoveTo(Vector2Int cell, BoardManager board)
    {
        _currentCellPos = cell;
        transform.position = board.CellToWorld(_currentCellPos);
    }

    private Vector2Int GetCellPosition()
    {
        return _currentCellPos;
    }
}
