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

    private int _currentLevel;

    private Label _labelFoodAmount;
    private Label _gameOverMessage;
    private VisualElement _gameOverPanel;

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
        
        _labelFoodAmount = uiDoc.rootVisualElement.Q<Label>("FoodAmount");
        _gameOverPanel = uiDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
        _gameOverMessage = _gameOverPanel.Q<Label>("GameOverMessage");

        _gameOverPanel.style.visibility = Visibility.Hidden;
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

        if (foodAmount <= 0)
        {
            _gameOverPanel.style.visibility = Visibility.Visible;
            _gameOverMessage.text = "Game Over!\n\nYou traveled through\n" + _currentLevel + " levels";
        }
    }

    [ContextMenu("New Level")]
    public void NewLevel()
    {
        _currentLevel++;
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
