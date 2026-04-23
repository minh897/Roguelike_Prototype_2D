using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private DamageFlash _damageFlash;
    private PlayerInputHandler _inputHandler;
    private PlayerMovement _playerMovement;

    private bool _isGameStop = false;

#region UNITY
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _damageFlash = GetComponent<DamageFlash>();
        _inputHandler = GetComponent<PlayerInputHandler>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (_isGameStop)
        {
            if (_inputHandler.InputRestart)
            {
                GameManager.Instance.StartNewGame();
            }
            return;
        }
    }
#endregion

#region PUBLIC
    public void Init(BoardManager boardManager, Vector2Int cell)
    {
        _isGameStop = false;
        _playerMovement.Spawn(boardManager, cell);
    }

    public void EnterGameStopState()
    {
        _isGameStop = true;
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


}
