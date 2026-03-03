using System.Threading.Tasks;
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
        var playerToEnemy = playerCoord - _cell;

        // Ignore the differences in orientation
        var absXDist = Mathf.Abs(playerToEnemy.x);
        var absYDist = Mathf.Abs(playerToEnemy.y);

        // Check if the target cell is passable first
        // then do the actual moving if it is
        if (absYDist >= absXDist)
        {
            // If the player is below enemy
            if (playerToEnemy.y > 0)
            {
                TryToMove(_cell + Vector2Int.up);
            }
            // If the player is above enemy
            else if (playerToEnemy.y < 0)
            {
                TryToMove(_cell + Vector2Int.down);
            }
        }
    }

    private void TryToMove(Vector2Int coord)
    {
        var board = GameManager.Instance.GetBoard();
        var targetCell = board.GetCellData(coord);

        if (!targetCell.passable || 
            targetCell == null || 
            targetCell.containedObject != null)
        {
            return;
        }

        // remove the enemy from the current cell
        var currentCell = board.GetCellData(_cell);
        currentCell.containedObject = null;

        // add it to the next cell
        targetCell.containedObject = this;
        _cell = coord;
        transform.position = board.CellToWorld(coord);
    }

}
