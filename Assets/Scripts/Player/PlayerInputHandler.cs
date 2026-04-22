using UnityEngine;
using InputActions;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 InputMove { get; private set; }
    public bool InputRestart { get; private set; }

    private PlayerInputActions _inputActions;
    private InputAction _moveAction;
    private InputAction _restartAction;

    // private Vector2Int _direction;

    void Awake()
    {
        _inputActions = new();
        _moveAction = _inputActions.Player.Move;
        _restartAction = _inputActions.Player.Interact;
    }

    void OnEnable()
    {
        _moveAction.started += OnMove;
        _moveAction.canceled += OnMove;
        _restartAction.started += OnRestart;
        _inputActions.Player.Enable();
    }

    void OnDisable()
    {
        _moveAction.started -= OnMove;
        _moveAction.canceled -= OnMove;
        _restartAction.started -= OnRestart;
        _inputActions.Player.Disable();
    }

#region PRIVATE
    private void OnMove(InputAction.CallbackContext context)
    {
        InputMove = context.ReadValue<Vector2>();
        // _direction = new((int)input.x, (int)input.y);
    }

    private void OnRestart(InputAction.CallbackContext context)
    {
        InputRestart = context.ReadValueAsButton();
    }
#endregion
}
