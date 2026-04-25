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
    private VisualElement _winPanel;
    private Label _foodAmountLabel;
    private Label _gameOverMessageLabel;
    private Label _enemyStatLabel;
    private Label _foodStatLabel;
    private Label _levelStatLabel;

    private int _traveledLevel;
    private int _foodAmount;
    private int _enemyDefeated;
    public bool _victory { get; private set; }

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
        
        _foodAmountLabel = uiDoc.rootVisualElement.Q<Label>("FoodAmount");
        _gameOverPanel = uiDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
        _winPanel = uiDoc.rootVisualElement.Q<VisualElement>("VictoryPanel");

        _gameOverMessageLabel = _gameOverPanel.Q<Label>("GameOverMessage");

        _enemyStatLabel = _winPanel.Q<Label>("EnemyStat");
        _foodStatLabel = _winPanel.Q<Label>("FoodStat");
        _levelStatLabel = _winPanel.Q<Label>("LevelStat");
    }

    // void OnEnable()
    // {
    //     TurnManager.OnTick += OnTurnHappen;
    // }

    // void OnDisable()
    // {
    //     TurnManager.OnTick -= OnTurnHappen;
    // }

    void Start()
    {
        StartNewGame();
    }

#region PUBLIC
    public void ChangeFood(int amount)
    {
        _foodAmount += amount;
        _foodAmountLabel.text = _foodAmount.ToString();

        // Game over condition
        if (_foodAmount <= 0)
        {
            TriggerGameOver();
        }
    }

    [ContextMenu("Start new game")]
    public void StartNewGame()
    {
        TurnManager.OnTick += OnTurnHappen;

        _gameOverPanel.style.visibility = Visibility.Hidden;
        _winPanel.style.visibility = Visibility.Hidden;

        // Gameplay rule: always start over at level 1, and reset traveled level to 0
        _traveledLevel = 0;
        _enemyDefeated = 0;
        _foodAmount = startFoodAmount;

        _foodAmountLabel.text = _foodAmount.ToString();

        NewLevel();
    }

    public void TriggerVictory()
    {
        _traveledLevel++;

        _enemyStatLabel.text = "Enemy defeated: " + _enemyDefeated;
        _foodStatLabel.text= "Food remain: " + _foodAmount;
        _levelStatLabel.text= "Level traveled: " + _traveledLevel;
        _winPanel.style.visibility = Visibility.Visible;
        
        TurnManager.OnTick -= OnTurnHappen;

        OnVictory?.Invoke();
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
        ChangeFood(-1);
    }

    private void TriggerGameOver()
    {
        // Set game over ui to visible
        _gameOverPanel.style.visibility = Visibility.Visible;
        _gameOverMessageLabel.text = "Game Over!\n\nYou traveled through\n" + _traveledLevel + " levels";

        TurnManager.OnTick -= OnTurnHappen;

        OnGameOver?.Invoke();
    }

    [ContextMenu("New Level")]
    private void NewLevel()
    {
        board.CleanBoard();
        board.Init();
        player.Init(playerInitialPos);
    }
#endregion
}
