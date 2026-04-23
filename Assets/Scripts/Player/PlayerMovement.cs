using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private BoardManager _board;

    private Vector2Int _currentCellPos;
    private Vector2Int _direction;

    public bool TryToMove(Vector2 input)
    {
        if (!enabled)
            return false;

        _direction = new((int)input.x, (int)input.y);

        Vector2Int nextCellTarget = _currentCellPos + _direction;
        CellData nextCellData = _board.GetCellData(nextCellTarget);

        // Stop player from occupying an impassable cell
        if (nextCellData != null && !nextCellData.passable)
            return false;

        // Player can move to a cell that doesn't have a cell object
        CellObject obj = nextCellData.containedObject;
        if (obj == null)
        {
            MoveTo(nextCellTarget);
        }

        // Check the condition for the player to occupy a cell containing an object
        else if (obj.PlayerWantsToEnter())
        {
            MoveTo(nextCellTarget);
            obj.PlayerEntered();
        }

        return true;
    }

    // Spawn the player character on game board
    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        _board = boardManager;
        MoveTo(cell);
    }

    private void MoveTo(Vector2Int cell)
    {
        _currentCellPos = cell;
        transform.position = _board.CellToWorld(_currentCellPos);
    }

    private Vector2Int GetCellPosition()
    {
        return _currentCellPos;
    }
}
