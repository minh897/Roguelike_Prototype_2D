using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitCellObject : CellObject
{
    public Tile exitTile;

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        GameManager.Instance.GetBoard().SetCellTile(cell, exitTile);
    }

    public override void PlayerEntered()
    {
        Debug.Log("Player entered the exit ");
    }
}
