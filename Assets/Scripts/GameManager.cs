using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TurnManager TurnManager { get; private set; }

    [SerializeField] private BoardManager board;
    [SerializeField] private PlayerController player;


    void Awake()
    {
        // Make sure there is only one instance of this class exist
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        TurnManager = new();
        board.Init();
        player.Spawn(board, new Vector2Int(1, 1));
    }
}
