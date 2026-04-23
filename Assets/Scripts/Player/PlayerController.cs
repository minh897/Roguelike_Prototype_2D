using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private BoardManager _board;
    private Animator _animator;
    private DamageFlash _damageFlash;
    private PlayerInputHandler _inputHandler;
    private PlayerMovement _playerMovement;

    private bool _isGameStop;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _damageFlash = GetComponent<DamageFlash>();
        _inputHandler = GetComponent<PlayerInputHandler>();
        _playerMovement = GetComponent<PlayerMovement>();
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
    public void Init(BoardManager boardManager, Vector2Int cell)
    {
        _isGameStop = false;
        _playerMovement.enabled = true;
        _playerMovement.Spawn(boardManager, cell);
    }

    public void EnterGameStopState()
    {
        _isGameStop = true;
        _playerMovement.enabled = false;
    }

    public void RestartGame()
    {
        if (_isGameStop)
            GameManager.Instance.StartNewGame();
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
    private void CallMovementLogic(Vector2 input)
    {
        if (_playerMovement.TryToMove(input))
        {
            GameManager.Instance.TurnManager.Tick();
            AudioManager.Instance.PlayAudio(
                AudioManager.Instance.SoundLibrary.sfxFootSteps, transform, 1f, false);
        }
    }
#endregion
}
