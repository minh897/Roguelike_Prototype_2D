using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CellData
{
    public bool passable;
    public CellObject containedObject;
}

public class BoardManager : MonoBehaviour
{

    [Header("Player")]
    [SerializeField] private PlayerController player;

    [Header("Food")]
    [SerializeField] private int minFood;
    [SerializeField] private int maxFood;
    [SerializeField] private List<GameObject> foodPrefabs;

    [Header("Wall")]
    [SerializeField] private int minWall = 6;
    [SerializeField] private int maxWall = 10;
    [SerializeField] private WallObject wallPrefab;

    [Header("Tile")]
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

        // Remove the starting point of the player from the list
        // to prevent wall or food spawning on top of it
        _emptyCellList.Remove(new(1,1));

        GenerateWall();
        GenerateFood();
    }
#region PUBLIC
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

    public void SetCellTile(Vector2Int cellIndex, Tile tile)
    {
        _tilemap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y, 0), tile);
    }
#endregion

#region PRIVATE

    private void AddObject(CellObject obj, Vector2Int coord)
    {
        CellData data = _boardData[coord.x, coord.y];
        obj.transform.position = CellToWorld(coord);
        data.containedObject = obj;
        obj.Init(coord);
    }

    private void GenerateFood()
    {
        int foodCount = Random.Range(minFood, maxFood);
        for (int i = 0; i < foodCount; ++i)
        {
            int cellIndex = Random.Range(0, _emptyCellList.Count); // choose a random empty cell
            int spriteIndex = Random.Range(0, foodPrefabs.Count); // choose a random food sprite
            Vector2Int coord = _emptyCellList[cellIndex];
            FoodObject newFood = Instantiate(foodPrefabs[spriteIndex].GetComponent<FoodObject>());
            _emptyCellList.RemoveAt(cellIndex); // remove from the list, the cell isn't empty anymore
            AddObject(newFood, coord);
        }
    }

    private void GenerateWall()
    {
        int wallCount = Random.Range(minWall, maxWall);
        for (int i = 0; i < wallCount; ++i)
        {
            int cellIndex = Random.Range(0, _emptyCellList.Count);
            Vector2Int coord = _emptyCellList[cellIndex];
            WallObject newWall = Instantiate(wallPrefab);
            _emptyCellList.RemoveAt(cellIndex);
            AddObject(newWall, coord);
        }
    }
#endregion
}
