using InputActions;
using Unity.VisualScripting;
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

    private bool _hasMoved = false;
    private bool _isMoving = false;
    public bool _isAttacking = false;
    private bool _isGameOver = false;

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
        if (_isGameOver)
        {
            if (_restartAction.WasPressedThisFrame())
            {
                GameManager.Instance.StartNewGame();
            }
            return;
        }

        Vector2Int newCellTarget = _cellPosition;

        // Only set direction for a new target cell once per input
        if (_moveAction.WasPressedThisFrame() && _moveAction.IsPressed())
        {
            newCellTarget += _direction;
            _hasMoved = true;
        }
        
        // Check for a passable tile then move there if it is
        if (_hasMoved)
        {
            CellData cellData = _board.GetCellData(newCellTarget);
            if (cellData != null && !cellData.passable)
            {
                return;
            }
            
            // Player can move to a cell that doesn't have a cell object
            if (cellData.containedObject == null)
            {
                MoveTo(newCellTarget, false);
            }
            // Check the condition for the player to occupy a cell containing an object
            else if (cellData.containedObject != null)
            {
                // As of now, only ObstacleObject prevent the player from occupying
                // Therefor if PlayerWantsToEnter returns false, that cell is containing ObstacleObject
                if (!cellData.containedObject.PlayerWantsToEnter())
                {
                    _isAttacking = true;
                }
                else
                {
                    MoveTo(newCellTarget, true);
                    cellData.containedObject.PlayerEntered();
                }
            }
            
            GameManager.Instance.TurnManager.Tick();
            _hasMoved = false;
        }

        if (_isAttacking)
        {
            _animator.SetTrigger("Attacking");
            _isAttacking = false;
        }

        if (_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, _moveTarget, moveSpeed * Time.deltaTime);
            if (transform.position == _moveTarget)
            {
                _isMoving = false;
                _animator.SetBool("Moving", _isMoving);
                var cellData = _board.GetCellData(_cellPosition);
                if (cellData.containedObject != null)
                {
                    cellData.containedObject.PlayerEntered();
                }
            }
            return;
        }
    }

    public void Init()
    {
        _isGameOver = false;
    }

    public void SetGameOver()
    {
        _isGameOver = true;
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

    private void OnMove(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        _direction = new((int)input.x, (int)input.y);
    }

}
