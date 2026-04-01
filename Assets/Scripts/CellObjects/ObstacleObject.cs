using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleObject : CellObject
{
    [SerializeField] private int maxHealth;
    [SerializeField] private Tile damagedTile;
    // [SerializeField] private Tile obstacleTile;

    private int _healthPoint;
    private SpriteRenderer sRenderer;
    // private Tile _originalTile;
  
    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        sRenderer = GetComponent<SpriteRenderer>();

        _healthPoint = maxHealth;
        // cache the ground tile from the board as orignal tile 
        // _originalTile = GameManager.Instance.GetBoard().GetCellTile(cell);
    }

    public override bool PlayerWantsToEnter()
    {
        _healthPoint -= 1;
        if (_healthPoint == 1)
        {
            sRenderer.sprite = damagedTile.sprite;
        }
        if (_healthPoint > 0)
        {
            GameManager.Instance.GetPlayer().PlayAttack();
            return false;
        }

        // GameManager.Instance.GetBoard().SetCellTile(_cell, _originalTile);
        Destroy(gameObject);
        return true;
    }
}
