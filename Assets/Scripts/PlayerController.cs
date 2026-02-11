using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private BoardManager _board;
    private Vector2Int _cellPosition;

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        _board = boardManager;
        _cellPosition = cell;
        // move the player to the right position
        transform.position = _board.CellToWorld(cell);
    }
}
