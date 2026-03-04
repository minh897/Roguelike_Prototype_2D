using UnityEngine;

public class Enemy : CellObject
{
    [SerializeField] private int maxHealth;

    private int _health;

    void OnEnable()
    {
        GameManager.Instance.TurnManager.OnTick += TurnHappened;
    }

    void OnDestroy()
    {
        GameManager.Instance.TurnManager.OnTick -= TurnHappened;
    }

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        _health = maxHealth;
    }

    public override bool PlayerWantsToEnter()
    {
        _health -= 1;
        if (_health > 0)
        {
            GameManager.Instance.GetPlayer().PlayAttack();
            return false;
        }

        Destroy(gameObject);
        return true;
    }

    // Enemy allways try to move toward the player whenever a turn happened
    // lest it surrounded by unpassable cell
    private void TurnHappened()
    {
        var playerCoord = GameManager.Instance.GetPlayer().GetCellPosition();
        
        int xDist = playerCoord.x - _cell.x;
        int yDist = playerCoord.y - _cell.y;

        // Ignore the differences in orientation
        var absXDist = Mathf.Abs(xDist);
        var absYDist = Mathf.Abs(yDist);

        // Enemy stop moving and attack when close to the player
        if (absXDist == 1 && yDist == 0
             || absYDist == 1 && xDist == 0)
        {
            GameManager.Instance.ChangeFood(-2);
            return;
        }

        if (absYDist >= absXDist)
        {
            MoveInDirection(yDist, Vector2Int.up);
        }
        else
        {
            MoveInDirection(xDist, Vector2Int.right);
        }
    }

    private void MoveInDirection(int dist, Vector2Int direction)
    {
        // Pick a direction base on the distance from the player
        // if it's positive then move along the original direction
        // else move along the opposite
        var moveDir = dist > 0 ? direction : -direction;
        TryMoveTo(_cell + moveDir);
    }

    private void TryMoveTo(Vector2Int coord)
    {
        var board = GameManager.Instance.GetBoard();
        var targetCell = board.GetCellData(coord);

        if (!targetCell.passable || 
            targetCell == null || 
            targetCell.containedObject != null)
        {
            return;
        }

        // Remove the enemy from the current cell
        var currentCell = board.GetCellData(_cell);
        currentCell.containedObject = null;

        // Add it to the next cell
        targetCell.containedObject = this;
        _cell = coord;
        transform.position = board.CellToWorld(coord);
    }

}
