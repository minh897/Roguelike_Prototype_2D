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
    [SerializeField] private Vector2Int playerInitialPos;

    private Label _labelFoodAmount;

#region UNITY
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
        NewLevel();

        _labelFoodAmount.text = foodAmount.ToString();
    }
#endregion

#region PUBLIC
    public BoardManager GetBoard() => board;

    public void ChangeFood(int amount)
    {
        foodAmount += amount;
        _labelFoodAmount.text = foodAmount.ToString();
    }

    [ContextMenu("New Level")]
    public void NewLevel()
    {
        board.CleanBoard();
        board.Init();
        player.Spawn(board, playerInitialPos);
    }
#endregion

#region PRIVATE
    private void OnTurnHappen()
    {
        ChangeFood(-1);
    }
#endregion
}
