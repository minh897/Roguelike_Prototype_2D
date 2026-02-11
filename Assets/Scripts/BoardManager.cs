using UnityEngine;
using UnityEngine.Tilemaps;

public class CellData
{
    public bool passable;
}

public class BoardManager : MonoBehaviour
{
    [SerializeField] private int tileWidth;
    [SerializeField] private int tileHeight;
    [SerializeField] private Tile[] groundTiles;
    [SerializeField] private Tile[] wallTiles;

    private Tilemap _tilemap;
    private CellData[,] _boardData;

    void Awake()
    {
        _tilemap = GetComponentInChildren<Tilemap>();
    }

    void Start()
    {
        _boardData = new CellData[tileWidth, tileHeight];

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
    }
}
