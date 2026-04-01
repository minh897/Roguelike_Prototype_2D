using InputActions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager _board;
    private Vector2Int _cellPosition;

    private PlayerInputActions _inputActions;
    private InputAction _moveAction;
    private InputAction _restartAction;
    private Vector2Int _direction;
    private Vector3 _moveTarget;
    public float moveSpeed = 5f;

    private Animator _animator;

    private bool _canMove = false;
    private bool _isMoving = false;
    public bool _isAttacking = false;
    private bool _isGameStop = false;

    void Awake()
    {
        _inputActions = new();
        _moveAction = _inputActions.Player.Move;
        _restartAction = _inputActions.Player.Interact;
        _animator = GetComponent<Animator>();
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

    void Update()
    {
        if (_isGameStop)
        {
            if (_restartAction.WasPressedThisFrame())
            {
                GameManager.Instance.StartNewGame();
            }
            return;
        }

        Vector2Int nextCellTarget = _cellPosition;

        // Check for a passable cell in the moving direction
        if (_moveAction.WasPressedThisFrame() && _moveAction.IsPressed())
        {
            nextCellTarget += _direction;
            var nextCellData = _board.GetCellData(nextCellTarget);
            if (nextCellData != null && !nextCellData.passable)
            {
                return;
            }

            var obj = nextCellData.containedObject;
            // Player can move to a cell that doesn't have a cell object
            if (obj == null)
            {
                MoveTo(nextCellTarget, false);
            }
            // Check the condition for the player to occupy a cell containing an object
            else if (obj.PlayerWantsToEnter())
            {
                MoveTo(nextCellTarget, false);
                obj.PlayerEntered();
            }

            GameManager.Instance.TurnManager.Tick();
        }

        if (_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, _moveTarget, moveSpeed * Time.deltaTime);
            if (transform.position == _moveTarget)
            {
                _isMoving = false;
                _animator.SetBool("Moving", false);
            }
        }
    }

    public void Init()
    {
        _isGameStop = false;
    }

    public void EnterGameStopState()
    {
        _isGameStop = true;
    }

    // Spawn player character on game board
    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        _board = boardManager;
        MoveTo(cell, true);
    }

    public void MoveTo(Vector2Int cell, bool immediate)
    {
        _cellPosition = cell;
        // Player move immediately without animation
        if (immediate)
        {
            _isMoving = false;
            transform.position = _board.CellToWorld(_cellPosition);
        }
        else
        {
            _isMoving = true;
            _moveTarget = _board.CellToWorld(_cellPosition);
        }
        _animator.SetBool("Moving", _isMoving);
    }

    public Vector2Int GetCellPosition()
    {
        return _cellPosition;
    }

    public void PlayAttack()
    {
        _animator.SetTrigger("Attacking");
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        _direction = new((int)input.x, (int)input.y);
    }

}
