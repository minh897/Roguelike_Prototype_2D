using InputActions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    private InputAction _moveAction;
    private InputAction _restartAction;

    private BoardManager _board;
    private Vector2Int _cellPosition;
    private Vector2Int _direction;

    private Animator _animator;
    private SoundLibrary _sounds;

    private bool _isGameStop = false;

#region UNITY
    void Awake()
    {
        _inputActions = new();
        _moveAction = _inputActions.Player.Move;
        _restartAction = _inputActions.Player.Interact;
        _animator = GetComponent<Animator>();
        _sounds = AudioManager.Instance.SoundLibrary;
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
            CellData nextCellData = _board.GetCellData(nextCellTarget);
            if (nextCellData != null && !nextCellData.passable)
            {
                return;
            }

            CellObject obj = nextCellData.containedObject;
            // Player can move to a cell that doesn't have a cell object
            if (obj == null)
            {
                MoveTo(nextCellTarget);
                AudioManager.Instance.PlayAudio(_sounds.sfxFootSteps, transform, 1f, false);
            }
            // Check the condition for the player to occupy a cell containing an object
            else if (obj.PlayerWantsToEnter())
            {
                MoveTo(nextCellTarget);
                obj.PlayerEntered();
            }

            GameManager.Instance.TurnManager.Tick();
        }
    }
#endregion

#region PUBLIC
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
        MoveTo(cell);
    }

    public void MoveTo(Vector2Int cell)
    {
        _cellPosition = cell;
        transform.position = _board.CellToWorld(_cellPosition);
    }

    public Vector2Int GetCellPosition()
    {
        return _cellPosition;
    }

    public void PlayAttack()
    {
        _animator.SetTrigger("Attacking");
        AudioManager.Instance.PlayAudio(_sounds.sfxAttacks, transform, 1f, false);
    }

    public void PlayFoodChomp()
    {
        AudioManager.Instance.PlayAudio(_sounds.sfxEatFoods, transform, 1f, false);
    }

    public void PlayPlayerDown()
    {
        AudioManager.Instance.PlayAudio(_sounds.sfxPlayerDowns, transform, 1f, false);
    }
#endregion

#region PRIVATE
    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        _direction = new((int)input.x, (int)input.y);
    }
#endregion
}
