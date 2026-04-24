using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitObject : CellObject
{
    public Tile exitTile;

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        BoardManager.Instance.SetCellTile(cell, exitTile);
    }

    public override void PlayerEntered()
    {
        GameManager.Instance.GetPlayer().EnterGameStopState();
        GameManager.Instance.DisplayWinUI();
        AudioManager.Instance.PlayAudio(
            AudioManager.Instance.SoundLibrary.victory, transform, 1f, false);
    }
}
