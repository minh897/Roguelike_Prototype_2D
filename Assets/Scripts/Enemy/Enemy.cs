using UnityEngine;

public class Enemy : CellObject
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int damageDeal;
    [SerializeField] private float moveSpeed;

    private Animator _animator;
    private DamageFlash _damageFlash;

    private int _health;
    private int _damage;

#region UNITY
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _damageFlash = GetComponent<DamageFlash>();
    }

    void OnEnable()
    {
        GameManager.Instance.TurnManager.OnTick += TurnHappened;
    }

    void OnDestroy()
    {
        GameManager.Instance.TurnManager.OnTick -= TurnHappened;
    }
#endregion

#region PUBLIC
    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        _health = maxHealth;
        _damage = damageDeal;
    }

    public override bool PlayerWantsToEnter()
    {
        _health -= 1;
        if (_health > 0)
        {
            GameManager.Instance.GetPlayer().PlayAttack();
            _damageFlash.PlayDamageFlash();
            return false;
        }
        Destroy(gameObject);
        return true;
    }
#endregion

#region PRIVATE
    // Enemy allways try to move toward the player whenever a turn happened
    // lest it surrounded by unpassable cell
    private void TurnHappened()
    {
        Vector2Int playerCoord = GameManager.Instance.GetPlayer().GetCellPosition();
        
        int xDist = playerCoord.x - _cell.x;
        int yDist = playerCoord.y - _cell.y;

        // Ignore the differences in orientation
        int absXDist = Mathf.Abs(xDist);
        int absYDist = Mathf.Abs(yDist);

        // Enemy stop moving and attack when close to the player
        bool adjacentHorizontally = absXDist == 1 && yDist == 0;
        bool adjacentVertically = absYDist == 1 && xDist == 0;
        if (adjacentHorizontally || adjacentVertically)
        {
            GameManager.Instance.ChangeFood(_damage);
            GameManager.Instance.GetPlayer().PlayPlayerDamage();
            _animator.SetTrigger("Attacking");
            return;
        }

        if (absYDist >= absXDist)
        {
            if (!MoveTowardAxis(yDist, Vector2Int.up))
            {
                MoveTowardAxis(xDist, Vector2Int.right);
            }
        }
        else
        {
            if (!MoveTowardAxis(xDist, Vector2Int.right))
            {
                MoveTowardAxis(yDist, Vector2Int.up);
            }
        }
    }

    private bool MoveTowardAxis(int dist, Vector2Int positiveDirection)
    {
        // Pick a direction base on the distance from the player
        // if it's positive then move along the original direction
        // else move along the opposite
        Vector2Int moveDir = dist > 0 ? positiveDirection : -positiveDirection;
        return TryMoveTo(_cell + moveDir);
    }

    private bool TryMoveTo(Vector2Int coord)
    {
        BoardManager board = GameManager.Instance.GetBoard();
        CellData targetCell = board.GetCellData(coord);

        // Can't move into cell containing an object
        if (targetCell == null || 
            !targetCell.passable || 
            targetCell.containedObject != null)
        {
            return false;
        }

        // Remove the enemy from the current cell
        CellData currentCell = board.GetCellData(_cell);
        currentCell.containedObject = null;

        // Add it to the next cell
        targetCell.containedObject = this;
        _cell = coord;

        return true;
    }
#endregion
}
