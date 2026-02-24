using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TurnManager TurnManager { get; private set; }

    [SerializeField] private UIDocument uiDoc; 
    [SerializeField] private BoardManager board;
    [SerializeField] private PlayerController player;
    [SerializeField] private int foodAmount;

    private Label _labelFoodAmount;

    void Awake()
    {
        // Make sure there is only one instance of this class exist
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        TurnManager = new();
        
        // Find the label with the name FoodAmount within the root of UIDocument
        _labelFoodAmount = uiDoc.rootVisualElement.Q<Label>("FoodAmount");
    }

    void OnEnable()
    {
        TurnManager.OnTick += OnTurnHappen;
    }

    void OnDisable()
    {
        TurnManager.OnTick -= OnTurnHappen;
    }

    void Start()
    {
        board.Init();
        player.Spawn(board, new Vector2Int(1, 1));

        _labelFoodAmount.text = foodAmount.ToString();
    }

    public void ChangeFood(int amount)
    {
        foodAmount += amount;
        _labelFoodAmount.text = foodAmount.ToString();
    }

    private void OnTurnHappen()
    {
        ChangeFood(-1);
    }
}
