using InputActions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager _board;
    private Vector2Int _cellPosition;

    private PlayerInputActions _inputActions;
    private InputAction _moveAction;
    private Vector2Int _direction;

    private bool _hasMoved = false;

    void Awake()
    {
        _inputActions = new();
        _moveAction = _inputActions.Player.Move;
    }

    void OnEnable()
    {
        _moveAction.started += OnMove;
        _moveAction.canceled += OnMove;
        _inputActions.Player.Enable();
    }

    void OnDisable()
    {
        _moveAction.started -= OnMove;
        _moveAction.canceled -= OnMove;
        _inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        _direction = new((int)input.x, (int)input.y);
    }

    void Update()
    {
        Vector2Int newCellTarget = _cellPosition;

        // Only set direction for a new target cell once per input
        if (_moveAction.WasPressedThisFrame() && _moveAction.IsPressed())
        {
            newCellTarget += _direction;
            _hasMoved = true;
        }

        // Check for a passable tile
        // then move there if it is
        if (_hasMoved)
        {
            CellData cellData = _board.GetCellData(newCellTarget);
            if (cellData != null && cellData.passable)
            {
                GameManager.Instance.TurnManager.Tick();
                MoveTo(newCellTarget);
                if (cellData.containedObject != null)
                {
                    cellData.containedObject.PlayerEntered();
                }
                _hasMoved = false;
            }
        }
    }

    // Spawn player character on game board
    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        _board = boardManager;
        MoveTo(cell);
    }

    public void MoveTo(Vector2Int cell)
    {
        _cellPosition = cell;
        transform.position = _board.CellToWorld(_cellPosition);
    }
}
