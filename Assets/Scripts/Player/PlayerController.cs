using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private BoardManager _board;
    private Vector2Int _cellPosition;

    private Animator _animator;
    private DamageFlash _damageFlash;

    private bool _isGameStop = false;

#region UNITY
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _damageFlash = GetComponent<DamageFlash>();
    }

    void Update()
    {
        // if (_isGameStop)
        // {
        //     if (_restartAction.WasPressedThisFrame())
        //     {
        //         GameManager.Instance.StartNewGame();
        //     }
        //     return;
        // }

        // Vector2Int nextCellTarget = _cellPosition;

        // // Check for a passable cell in the moving direction
        // if (_moveAction.WasPressedThisFrame() && _moveAction.IsPressed())
        // {
        //     nextCellTarget += _direction;
        //     CellData nextCellData = _board.GetCellData(nextCellTarget);
        //     if (nextCellData != null && !nextCellData.passable)
        //     {
        //         return;
        //     }

        //     CellObject obj = nextCellData.containedObject;
        //     // Player can move to a cell that doesn't have a cell object
        //     if (obj == null)
        //     {
        //         MoveTo(nextCellTarget);
        //         AudioManager.Instance.PlayAudio(
        //             AudioManager.Instance.SoundLibrary.sfxFootSteps, transform, 1f, false);
        //     }
        //     // Check the condition for the player to occupy a cell containing an object
        //     else if (obj.PlayerWantsToEnter())
        //     {
        //         MoveTo(nextCellTarget);
        //         obj.PlayerEntered();
        //     }

        //     GameManager.Instance.TurnManager.Tick();
        // }
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
