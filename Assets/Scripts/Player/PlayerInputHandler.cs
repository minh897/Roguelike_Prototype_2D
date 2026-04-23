using System;
using UnityEngine;
using InputActions;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public event Action<Vector2> OnMovePressed;
    public event Action OnRestartPressed;

    private PlayerInputActions _inputActions;
    private InputAction _moveAction;
    private InputAction _restartAction;

    void Awake()
    {
        _inputActions = new();
        _moveAction = _inputActions.Player.Move;
        _restartAction = _inputActions.Player.Restart;
    }

    void OnEnable()
    {
        _moveAction.started += OnMove;
        _restartAction.started += OnRestart;

        _inputActions.Player.Enable();
    }

    void OnDisable()
    {
        _moveAction.started -= OnMove;
        _restartAction.started -= OnRestart;

        _inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        OnMovePressed?.Invoke(input);
    }

    private void OnRestart(InputAction.CallbackContext context)
    {
        OnRestartPressed?.Invoke();
    }
}
