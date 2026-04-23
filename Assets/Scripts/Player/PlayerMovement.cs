using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private BoardManager _board;
    private PlayerInputHandler _inputHandler;

    private Vector2Int _currentCellPos;
    private Vector2Int _direction;

    void Awake()
    {
        _inputHandler = GetComponent<PlayerInputHandler>();
    }

    void OnEnable()
    {
        _inputHandler.OnMovePressed += TryToMove;
    }

    void OnDisable()
    {
        _inputHandler.OnMovePressed -= TryToMove;
    }

    void Update()
    {
        // // Check the condition for the player to occupy a cell containing an object
        // else if (obj.PlayerWantsToEnter())
        // {
        //     MoveTo(nextCellTarget);
        //     obj.PlayerEntered();
        // }

        // if (_moveAction.WasPressedThisFrame() && _moveAction.IsPressed())
        // {
        //     GameManager.Instance.TurnManager.Tick();
        // }
    }

    public void TryToMove()
    {
        Debug.Log("Player tring to move");
        _direction = new(
            (int)_inputHandler.InputMove.x, 
            (int)_inputHandler.InputMove.y);

        Vector2Int nextCellTarget = _currentCellPos + _direction;

        // Check for a passable cell in the moving direction
        CellData nextCellData = _board.GetCellData(nextCellTarget);
        if (nextCellData != null && !nextCellData.passable)
        {
            return;
        }

        // Player can move to a cell that doesn't have a cell object
        CellObject obj = nextCellData.containedObject;
        if (obj == null)
        {
            MoveTo(nextCellTarget);
            AudioManager.Instance.PlayAudio(
                AudioManager.Instance.SoundLibrary.sfxFootSteps, transform, 1f, false);
        }
    }

    public void MoveTo(Vector2Int cell)
    {
        _currentCellPos = cell;
        transform.position = _board.CellToWorld(_currentCellPos);
    }

    public Vector2Int GetCellPosition()
    {
        return _currentCellPos;
    }

    // Spawn the player character on game board
    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        _board = boardManager;
        MoveTo(cell);
    }
}
