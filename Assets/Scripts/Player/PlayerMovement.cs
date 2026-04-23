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

        // Check for a passable cell in the moving direction
        CellData nextCellData = _board.GetCellData(nextCellTarget);
        if (nextCellData != null && !nextCellData.passable)
            return false;

        // Player can move to a cell that doesn't have a cell object
        CellObject obj = nextCellData.containedObject;
        if (obj == null)
        {
            MoveTo(nextCellTarget);
            AudioManager.Instance.PlayAudio(
                AudioManager.Instance.SoundLibrary.sfxFootSteps, transform, 1f, false);
        }

        // Check the condition for the player to occupy a cell containing an object
        else if (obj.PlayerWantsToEnter())
        {
            MoveTo(nextCellTarget);
            obj.PlayerEntered();
        }

        return true;
        // GameManager.Instance.TurnManager.Tick();
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
