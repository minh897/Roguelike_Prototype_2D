using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static event Action OnGameOver;
    public static event Action OnVictory;

    public static GameManager Instance { get; private set; }
    public TurnManager TurnManager { get; private set; }

    [SerializeField] private UIDocument uiDoc; 
    [SerializeField] private BoardManager board;
    [SerializeField] private PlayerController player;
    [SerializeField] private Vector2Int playerInitialPos;
    [SerializeField] private int startFoodAmount;

    private VisualElement _gameOverPanel;
    private VisualElement _victoryPanel;
    private Label _foodAmountLabel;
    private Label _enemyStatLabel;
    private Label _foodStatLabel;

    private int _currentFood;
    private int _foodCollected;
    private int _enemyDefeated;

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
        
        _gameOverPanel = uiDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
        _victoryPanel = uiDoc.rootVisualElement.Q<VisualElement>("VictoryPanel");

        _foodAmountLabel = uiDoc.rootVisualElement.Q<Label>("FoodAmount");
        _enemyStatLabel = _gameOverPanel.Q<Label>("EnemyStat");
        _foodStatLabel = _gameOverPanel.Q<Label>("FoodStat");
    }

    void Start()
    {
        StartNewGame();
    }

#region PUBLIC
    public void ChangeCurrentFood(int amount)
    {
        _currentFood += amount;
        _foodAmountLabel.text = _currentFood.ToString();

        // Game over condition
        if (_currentFood <= 0)
        {
            TriggerGameOver();
        }
    }

    public void IncreaseTotalFood(int amount)
    {
        // Total food collected stat can't be affected by
        // the changes of current food. That's why it needs a separate method
        _foodCollected += amount;
    }

    [ContextMenu("New Level")]
    public void NewLevel()
    {
        board.CleanBoard();
        board.Init();
        player.Init(playerInitialPos);

        TurnManager.OnTick += OnTurnHappen;

        _victoryPanel.style.visibility = Visibility.Hidden;
    }

    [ContextMenu("Start new game")]
    public void StartNewGame()
    {
        // Reset every stat
        _enemyDefeated = 0;
        _foodCollected = 0;
        _currentFood = startFoodAmount;

        _foodAmountLabel.text = _currentFood.ToString();
        _gameOverPanel.style.visibility = Visibility.Hidden;

        NewLevel();
    }

    public void TriggerVictory()
    {
        OnVictory?.Invoke();
        TurnManager.OnTick -= OnTurnHappen;

        _victoryPanel.style.visibility = Visibility.Visible;
    }

    public void IncreaseEnemyDefeated()
    {
        _enemyDefeated++;
    }

    public BoardManager GetBoard() => board;

    public PlayerController GetPlayer() => player;

#endregion

#region PRIVATE
    private void OnTurnHappen()
    {
        ChangeCurrentFood(-1);
    }

    private void TriggerGameOver()
    {
        OnGameOver?.Invoke();
        TurnManager.OnTick -= OnTurnHappen;

        // Set game over ui to visible
        _foodStatLabel.text= "Food collected: " + _foodCollected;
        _enemyStatLabel.text = "Enemy defeated: " + _enemyDefeated;
        _gameOverPanel.style.visibility = Visibility.Visible;
    }
#endregion
}
