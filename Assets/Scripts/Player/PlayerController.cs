using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputHandler _inputHandler;
    private PlayerMovement _movement;
    private PlayerFeedback _feedback;

    void Awake()
    {
        _inputHandler = GetComponent<PlayerInputHandler>();
        _movement = GetComponent<PlayerMovement>();
        _feedback = GetComponent<PlayerFeedback>();
    }

    void OnEnable()
    {
        _inputHandler.OnMovePressed += CallMovementLogic;
        _movement.OnWantToEnterFail += _feedback.PlayAttack;
        GameManager.OnGameOver += EnterGameOverState;
        GameManager.OnVictory += EnterVictoryState;
    }

    void OnDisable()
    {
        _inputHandler.OnMovePressed -= CallMovementLogic;
        _movement.OnWantToEnterFail -= _feedback.PlayAttack;
        GameManager.OnGameOver -= EnterGameOverState;
        GameManager.OnVictory -= EnterVictoryState;
    }
#region PUBLIC
    public void Init(Vector2Int cell)
    {
        _movement.enabled = true;

        // Spawn the player character on the game board
        _movement.MoveTo(cell, BoardManager.Instance); 
    }

    public void DisablePlayer()
    {
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
        GameManager.Instance.StartNewGame();
        _inputHandler.OnRestartPressed -= RestartGame;
    }

    private void ProgressLevel()
    {
        GameManager.Instance.NewLevel();
        _inputHandler.OnProgressPressed -= ProgressLevel;
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
        DisablePlayer();
        _feedback.PlayPlayerDown();
        _inputHandler.OnRestartPressed += RestartGame;
    }

    private void EnterVictoryState()
    {
        DisablePlayer();
        _inputHandler.OnProgressPressed += ProgressLevel;
    }
#endregion
}
