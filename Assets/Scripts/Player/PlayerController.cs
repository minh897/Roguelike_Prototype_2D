using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private DamageFlash _damageFlash;
    private PlayerInputHandler _inputHandler;
    private PlayerMovement _movement;
    
    private bool _isGameStop;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _damageFlash = GetComponent<DamageFlash>();
        _inputHandler = GetComponent<PlayerInputHandler>();
        _movement = GetComponent<PlayerMovement>();
    }

    void OnEnable()
    {
        _inputHandler.OnMovePressed += CallMovementLogic;
        _inputHandler.OnRestartPressed += RestartGame;
    }

    void OnDisable()
    {
        _inputHandler.OnMovePressed -= CallMovementLogic;
        _inputHandler.OnRestartPressed -= RestartGame;
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

    public void PlayAttack()
    {
        _animator.SetTrigger("Attacking");
        AudioManager.Instance.PlayAudio(
            AudioManager.Instance.SoundLibrary.sfxAttacks, transform, 1f, false);
    }

    public void PlayFoodChomp()
    {
        AudioManager.Instance.PlayAudio(
            AudioManager.Instance.SoundLibrary.sfxEatFoods, transform, 1f, false);
    }

    public void PlayPlayerDown()
    {
        AudioManager.Instance.PlayAudio(
            AudioManager.Instance.SoundLibrary.sfxPlayerDowns, transform, 1f, false);
    }

    public void PlayPlayerDamage()
    {
        _damageFlash.PlayDamageFlash();
    }
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
#endregion
}
