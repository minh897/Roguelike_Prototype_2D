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
    private Label _playerStats;
    private VisualElement _gameOverPanel;
    private VisualElement _winPanel;

    private int _currentLevel;
    private int _traveledLevel;
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
        _winPanel = uiDoc.rootVisualElement.Q<VisualElement>("WinStatsPanel");
        _gameOverMessage = _gameOverPanel.Q<Label>("GameOverMessage");
        _playerStats = _winPanel.Q<Label>("PlayerStats");
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
            TriggerGameOver();
        }
    }

    public void DisplayWinUI()
    {
        _winPanel.style.visibility = Visibility.Visible;
    }

    [ContextMenu("New Level")]
    public void NewLevel()
    {
        _currentLevel++;
        _traveledLevel++;

        board.CleanBoard();
        board.Init();

        player.Spawn(board, playerInitialPos);
    }

    public void StartNewGame()
    {
        _gameOverPanel.style.visibility = Visibility.Hidden;
        _winPanel.style.visibility = Visibility.Hidden;

        // Gameplay rule: always start over at level 1, and reset traveled level to 0
        _currentLevel = 1;
        _traveledLevel = 0;
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

    private void TriggerGameOver()
    {
        player.PlayPlayerDown();
        player.EnterGameStopState();
        
        // Set game over ui to visible
        _gameOverPanel.style.visibility = Visibility.Visible;
        _gameOverMessage.text = "Game Over!\n\nYou traveled through\n" + _traveledLevel + " levels";
    }
#endregion
}
