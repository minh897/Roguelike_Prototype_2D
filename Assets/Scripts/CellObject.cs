using UnityEngine;

public class CellObject : MonoBehaviour
{
    protected Vector2Int _cell;

    public virtual void Init(Vector2Int cell)
    {
        _cell = cell;
    }

    //Called when the player enter the cell in which that object is
    public virtual void PlayerEntered()
    {
        // Base method do nothing
    }

    public virtual bool PlayerWantsToEnter()
    {
        return true;
    }
}
