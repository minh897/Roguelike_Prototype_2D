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
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Player.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        direction = new((int)input.x, (int)input.y);
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        direction = Vector2Int.zero;
    }

    void Update()
    {
        Vector2Int newCellTarget = _cellPosition;

        // only set direction for a new target cell once per input
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
                MoveTo(newCellTarget);
            }
        }
    }

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
