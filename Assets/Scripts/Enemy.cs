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

    private void TurnHappened()
    {
        // Get the player's cell position in X and Y on the board
        var playerCell = GameManager.Instance.GetPlayer().GetCellPosition();
        Debug.Log("Player position: " + playerCell);

        // Get the enemy's cell position in X and Y on the board
        var enemyCell = _cell;
        Debug.Log("Enemy position: " + enemyCell);

        // Calculate the distance between the enemy and the player in X and Y
        var enemyToPlayer = enemyCell - playerCell;
        Debug.Log("Distance to player: " + enemyToPlayer);

        // If the Y >= X distance (in number only) then move along Y axis
        if (Mathf.Abs(enemyToPlayer.y) >= Mathf.Abs(enemyToPlayer.x))
        {
            // If the player is below enemy
            if (enemyToPlayer.y > 0)
            {
                var targerCell = enemyCell + Vector2Int.down;
                Debug.Log("Move to: " + targerCell);
            }
            // If the player is above enemy
            else if (enemyToPlayer.y < 0)
            {
                var targerCell = enemyCell + Vector2Int.up;
                Debug.Log("Move to: " + targerCell);
            }
        }

        // If the X > Y distance then move along X axis
        if (Mathf.Abs(enemyToPlayer.x) > Mathf.Abs(enemyToPlayer.y))
        {
            // if the player is to the left of enemy
            if (enemyToPlayer.x > 0)
            {
                var targerCell = enemyCell + Vector2Int.left;
                Debug.Log("Move to: " + targerCell);
            }
            // if the player is to the right of enemy
            else if (enemyToPlayer.x < 0)
            {
                var targerCell = enemyCell + Vector2Int.right;
                Debug.Log("Move to: " + targerCell);
            }
        }
    }
}
