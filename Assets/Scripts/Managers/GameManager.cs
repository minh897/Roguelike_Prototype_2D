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

    private VisualElement _endPanel;
    private Label _foodAmountLabel;
    private Label _gameOverMessageLabel;
    private Label _victoryMessageLabel;
    private Label _enemyStatLabel;
    private Label _foodStatLabel;

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
        _endPanel = uiDoc.rootVisualElement.Q<VisualElement>("EndPanel");

        _gameOverMessageLabel = _endPanel.Q<Label>("GameOverMessage");
        _victoryMessageLabel = _endPanel.Q<Label>("VictoryMessage");

        _enemyStatLabel = _endPanel.Q<Label>("EnemyStat");
        _foodStatLabel = _endPanel.Q<Label>("FoodStat");
    }

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

        _endPanel.style.visibility = Visibility.Hidden;

        // Gameplay rule: always start over at level 1, and reset traveled level to 0
        _enemyDefeated = 0;
        _foodAmount = startFoodAmount;

        _foodAmountLabel.text = _foodAmount.ToString();

        NewLevel();
    }

    public void TriggerVictory()
    {
        _enemyStatLabel.text = "Enemy defeated: " + _enemyDefeated;
        _foodStatLabel.text= "Food remain: " + _foodAmount;
        _victoryMessageLabel.style.display = DisplayStyle.Flex;
        _gameOverMessageLabel.style.display = DisplayStyle.None;
        _endPanel.style.visibility = Visibility.Visible;
        
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
        _victoryMessageLabel.style.display = DisplayStyle.None;
        _gameOverMessageLabel.style.display = DisplayStyle.Flex;
        _endPanel.style.visibility = Visibility.Visible;

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
