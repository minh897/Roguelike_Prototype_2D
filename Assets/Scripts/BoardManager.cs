using UnityEngine;
using UnityEngine.Tilemaps;

public class CellData
{
    public bool passable;
}

public class BoardManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerController player;

    [Header("Tile Infos")]
    [SerializeField] private int tileWidth;
    [SerializeField] private int tileHeight;
    [SerializeField] private Tile[] groundTiles;
    [SerializeField] private Tile[] wallTiles;

    private Grid _grid;
    private Tilemap _tilemap;
    private CellData[,] _boardData;

    void Awake()
    {
        _grid = GetComponentInChildren<Grid>();
        _tilemap = GetComponentInChildren<Tilemap>();
    }

    void Start()
    {
        _boardData = new CellData[tileWidth, tileHeight];

        // Generate game board
        for (int y = 0; y < tileHeight; y++)
        {
            for (int x = 0; x < tileWidth; x++)
            {
                Tile tile;
                _boardData[x, y] = new();
                if (x == 0 || y == 0 || x == tileWidth - 1 || y == tileHeight - 1)
                {
                    tile = wallTiles[Random.Range(0, wallTiles.Length)];
                    _boardData[x, y].passable = false;
                }
                else
                {
                    tile = groundTiles[Random.Range(0, groundTiles.Length)];
                    _boardData[x, y].passable = true;
                }
                Vector3Int position = new(x, y, 0);
                _tilemap.SetTile(position, tile);
            }
        }

        // Spawn player character on game board
        player.Spawn(this, new Vector2Int(1, 1));
    }

    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return _grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }
}
