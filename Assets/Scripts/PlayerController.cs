using InputActions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager _board;
    private Vector2Int _cellPosition;

    private PlayerInputActions inputActions;
    private Vector2Int direction;

    private bool hasMoved = false;

    void Awake()
    {
        inputActions = new();
    }

    void OnEnable()
    {
        inputActions.Player.Move.started += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Move.started -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            var input = context.ReadValue<Vector2>();
            direction = new((int)input.x, (int)input.y);
        }

        if (context.canceled)
        {
            direction = Vector2Int.zero;
        }
    }

    void Update()
    {
        Vector2Int newCellTarget = _cellPosition;

        if (direction.sqrMagnitude != 0 && !hasMoved)
        {
            hasMoved = true;
            newCellTarget += direction;
        }

        if (direction.sqrMagnitude == 0)
        {
            hasMoved = false;
        }

        // check for a passable tile
        // then move there if it is
        if (hasMoved)
        {
            CellData cellData = _board.GetCellData(newCellTarget);
            if (cellData != null && cellData.passable)
            {
                _cellPosition = newCellTarget;
                transform.position = _board.CellToWorld(_cellPosition);
            }
        }
    }

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        _board = boardManager;
        _cellPosition = cell;
        // move the player to the right position
        transform.position = _board.CellToWorld(cell);
    }
}
