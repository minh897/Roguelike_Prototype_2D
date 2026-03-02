using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TurnManager TurnManager { get; private set; }

    [SerializeField] private UIDocument uiDoc; 
    [SerializeField] private BoardManager board;
    [SerializeField] private PlayerController player;
    [SerializeField] private Vector2Int playerInitialPos;
    [SerializeField] private int startFoodAmount;

    private Label _labelFoodAmount;
    private Label _gameOverMessage;
    private VisualElement _gameOverPanel;

    private int _currentLevel;
    private int _foodAmount;

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
        StartNewGame();
    }
#endregion

#region PUBLIC
    public BoardManager GetBoard() => board;

    public PlayerController GetPlayer() => player;

    public void ChangeFood(int amount)
    {
        _foodAmount += amount;
        _labelFoodAmount.text = _foodAmount.ToString();

        // Game over condition
        if (_foodAmount <= 0)
        {
            player.EnterGameOverState();
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

    public void StartNewGame()
    {
        _gameOverPanel.style.visibility = Visibility.Hidden;

        // Gameplay rule: always start over at level 1
        _currentLevel = 1; 
        _foodAmount = startFoodAmount;
        _labelFoodAmount.text = _foodAmount.ToString();

        board.CleanBoard();
        board.Init();

        player.Init();
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
