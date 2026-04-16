using UnityEngine;

public class FoodObject : CellObject
{
    [SerializeField] private int _foodPoint;

    public override void PlayerEntered()
    {
        GameManager.Instance.ChangeFood(_foodPoint);
        GameManager.Instance.GetPlayer().PlayFoodChomp();
        VFXManager.Instance.TriggerFloatingText(_foodPoint.ToString(), transform.position);
        Destroy(gameObject);  
    }
}
