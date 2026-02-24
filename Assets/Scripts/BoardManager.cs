using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CellData
{
    public bool passable;
    public GameObject containedObject;
}

public class BoardManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerController player;

    [Header("Prefabs")]
    [SerializeField] private GameObject foodPrefab;

    [Header("Tile Infos")]
    [SerializeField] private int tileWidth;
    [SerializeField] private int tileHeight;
    [SerializeField] private Tile[] groundTiles;
    [SerializeField] private Tile[] wallTiles;

    private Grid _grid;
    private Tilemap _tilemap;
    private CellData[,] _boardData;
    private List<Vector2Int> _emptyCellList;

    void Awake()
    {
        _grid = GetComponentInChildren<Grid>();
        _tilemap = GetComponentInChildren<Tilemap>();
        _emptyCellList = new();
    }

    public void Init()
    {
        // Generate a new game board
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
                    _emptyCellList.Add(new(x, y));
                }
                Vector3Int position = new(x, y, 0);
                _tilemap.SetTile(position, tile);
            }
        }

        // Remove the starting point of the player
        _emptyCellList.Remove(new(1,1));
        // Randomly distributed food
        GenerateFood();
    }

    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return _grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    public CellData GetCellData(Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= tileWidth
            || cellIndex.y < 0 || cellIndex.y >= tileHeight)
        {
            return null;
        }

        return _boardData[cellIndex.x, cellIndex.y];
    }

    void GenerateFood()
    {
        int foodCount = 5;
        for (int i = 0; i < foodCount; ++i)
        {
            int randomIndex = Random.Range(0, _emptyCellList.Count);
            Vector2Int coord = _emptyCellList[randomIndex];
            CellData data = _boardData[coord.x, coord.y];
            GameObject newFood = Instantiate(foodPrefab);
            newFood.transform.position = CellToWorld(coord);
            data.containedObject = newFood;
            // remove from the list because the cell is not empty anymore
            _emptyCellList.RemoveAt(randomIndex); 
        }
    }
}
