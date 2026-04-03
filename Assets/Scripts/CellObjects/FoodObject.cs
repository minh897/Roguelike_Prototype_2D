public class FoodObject : CellObject
{
    public int foodPoint;

    public override void PlayerEntered()
    {
        GameManager.Instance.ChangeFood(foodPoint);
        GameManager.Instance.GetPlayer().PlayFoodChomp();
        Destroy(gameObject);  
    }
}
