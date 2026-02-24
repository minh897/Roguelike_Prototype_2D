using UnityEngine;

public class FoodObject : CellObject
{
    public int foodPoint;

    public override void PlayerEntered()
    {
        // Debug.Log("Food increased");
        GameManager.Instance.ChangeFood(foodPoint);
        Destroy(gameObject);  
    }
}
