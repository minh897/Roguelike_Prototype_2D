using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputHandler _inputHandler;
    private PlayerMovement _movement;
    private PlayerFeedback _feedback;
    
    private bool _isGameStop;

    void Awake()
    {
        _inputHandler = GetComponent<PlayerInputHandler>();
        _movement = GetComponent<PlayerMovement>();
        _feedback = GetComponent<PlayerFeedback>();
    }

    void OnEnable()
    {
        _inputHandler.OnMovePressed += CallMovementLogic;
        _inputHandler.OnRestartPressed += RestartGame;
        _movement.OnWantToEnterFail += _feedback.PlayAttack;
        GameManager.OnGameOver += EnterGameOverState;
    }

    void OnDisable()
    {
        _inputHandler.OnMovePressed -= CallMovementLogic;
        _inputHandler.OnRestartPressed -= RestartGame;
        _movement.OnWantToEnterFail -= _feedback.PlayAttack;
        GameManager.OnGameOver -= EnterGameOverState;
    }

#region PUBLIC
    public void Init(Vector2Int cell)
    {
        _isGameStop = false;
        _movement.enabled = true;

        // Spawn the player character on the game board
        _movement.MoveTo(cell, BoardManager.Instance); 
    }

    public void EnterGameStopState()
    {
        _isGameStop = true;
        _movement.enabled = false;
    }

    public void GotAttacked()
    {
        _feedback.PlayPlayerDamage();
    }

    public void Eat()
    {
        _feedback.PlayFoodChomp();
    }

    public Vector2Int GetCellPosition() => _movement.CurrentCellPos;
#endregion

#region PRIVATE
    private void RestartGame()
    {
        if (_isGameStop)
            GameManager.Instance.StartNewGame();
    }

    private void CallMovementLogic(Vector2 input)
    {
        // Tick only happens when a movement is made
        // not when the input is called
        if (_movement.TryToMove(input, BoardManager.Instance))
        {
            GameManager.Instance.TurnManager.Tick();
            AudioManager.Instance.PlayAudio(
                AudioManager.Instance.SoundLibrary.sfxFootSteps, transform, 1f, false);
        }
    }

    private void EnterGameOverState()
    {
        _feedback.PlayPlayerDown();
        EnterGameStopState();
    }
#endregion
}
