using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitObject : CellObject
{
    public Tile exitTile;

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        GameManager.Instance.GetBoard().SetCellTile(cell, exitTile);
    }

    public override void PlayerEntered()
    {
        GameManager.Instance.DisplayWinUI();
        GameManager.Instance.GetPlayer().EnterGameStopState();
    }
}
