using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleObject : CellObject
{
    [SerializeField] private int maxHealth;
    [SerializeField] private Tile obstacleTile;
    [SerializeField] private Tile damagedTile;

    private int _healthPoint;
    private Tile _originalTile;
    private SpriteRenderer sRenderer;
  
    public override void Init(Vector2Int cell)
    {
        base.Init(cell);

        _healthPoint = maxHealth;
        sRenderer = GetComponent<SpriteRenderer>();

        // cache the ground tile from the board as orignal tile 
        // before setting the current cell with an obstacle tile
        _originalTile = GameManager.Instance.GetBoard().GetCellTile(cell);
        // GameManager.Instance.GetBoard().SetCellTile(cell, obstacleTile);
    }

    public override bool PlayerWantsToEnter()
    {
        _healthPoint -= 1;
        
        if (_healthPoint > 0)
        {
            if (_healthPoint == 1)
            {
                // Replace the sprite with the damagedTile sprite
                sRenderer.sprite = damagedTile.sprite;
            }
            return false;
        }

        GameManager.Instance.GetBoard().SetCellTile(_cell, _originalTile);
        Destroy(gameObject);
        return true;
    }
}
