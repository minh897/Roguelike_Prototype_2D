using System;
using UnityEngine;
using InputActions;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public event Action<Vector2> OnMovePressed;
    public event Action OnRestartPressed;
    public event Action OnProgressPressed;

    private PlayerInputActions _inputActions;

    private InputAction _moveAction;
    private InputAction _restartAction;
    private InputAction _progressAction;

    void Awake()
    {
        _inputActions = new();
        _moveAction = _inputActions.Player.Move;
        _restartAction = _inputActions.Player.Restart;
        _progressAction = _inputActions.Player.Progress;
    }

    void OnEnable()
    {
        _moveAction.started += OnMove;
        _restartAction.started += OnRestart;
        _progressAction.started += OnProgress;

        _inputActions.Player.Enable();
    }

    void OnDisable()
    {
        _moveAction.started -= OnMove;
        _restartAction.started -= OnRestart;
        _progressAction.started -= OnProgress;

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

    private void OnProgress(InputAction.CallbackContext context)
    {
        OnProgressPressed?.Invoke();
    }
}
