using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TurnManager TurnManager { get; private set; }

    [SerializeField] private BoardManager board;
    [SerializeField] private PlayerController player;
    [SerializeField] private int _foodAmount;

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
    }

    void OnEnable()
    {
        TurnManager.OnTick += OnTurnHappen;
    }

    void OnDisable()
    {
        TurnManager.OnTick += OnTurnHappen;
    }

    void Start()
    {
        board.Init();
        player.Spawn(board, new Vector2Int(1, 1));
    }

    private void OnTurnHappen()
    {
        _foodAmount -= 1;
        Debug.Log("Current amount of food: " + _foodAmount);
    }
}
