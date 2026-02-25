using UnityEngine;
using UnityEngine.Tilemaps;

public class WallObject : CellObject
{
    public Tile obstacleTile;
  
    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        GameManager.Instance.GetBoard().SetCellTile(cell, obstacleTile);
    }

    public override bool PlayerWantsToEnter()
    {
        return false;
    }
}
