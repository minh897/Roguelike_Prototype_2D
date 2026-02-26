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
    [SerializeField] private int minWall;
    [SerializeField] private int maxWall;
    [SerializeField] private List<WallObject> wallPrefabs;

    [Header("Exit")]
    [SerializeField] private ExitCellObject exitPrefab;

    [Header("Board")]
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Tile[] groundTiles;
    [SerializeField] private Tile[] wallTiles;
    
    private Grid _grid;
    private Tilemap _tilemap;
    private CellData[,] _boardData;
    private List<Vector2Int> _emptyCellList;

#region UNITY
    void Awake()
    {
        _grid = GetComponentInChildren<Grid>();
        _tilemap = GetComponentInChildren<Tilemap>();
        _emptyCellList = new();
    }
#endregion

#region PUBLIC
    public void Init()
    {
        // Generate a new game board
        _boardData = new CellData[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Tile tile;
                _boardData[x, y] = new();
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
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

        GenerateExit();
        GenerateWall();
        GenerateFood();
    }

    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return _grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    public CellData GetCellData(Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= width
            || cellIndex.y < 0 || cellIndex.y >= height)
        {
            return null;
        }

        return _boardData[cellIndex.x, cellIndex.y];
    }

    public void SetCellTile(Vector2Int cellIndex, Tile tile)
    {
        _tilemap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y, 0), tile);
    }

    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return _tilemap.GetTile<Tile>(new(cellIndex.x, cellIndex.y, 0));
    }

    [ContextMenu("Clean Board")]
    public void CleanBoard()
    {
        // First time initializing the game board
        if (_boardData == null)
        {
            return;
        }
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var cellContainedObj = _boardData[x, y].containedObject;
                if (cellContainedObj != null)
                {
                    Destroy(cellContainedObj.gameObject);
                }

                Vector3Int position = new(x, y, 0);
                _tilemap.SetTile(position, null);
                _boardData[x, y] = new();
            }
        }
        _emptyCellList = new();
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
            AddObject(newFood, coord);
            _emptyCellList.RemoveAt(cellIndex); // remove from the list, the cell isn't empty anymore
        }
    }

    private void GenerateWall()
    {
        int wallCount = Random.Range(minWall, maxWall);
        for (int i = 0; i < wallCount; ++i)
        {
            int cellIndex = Random.Range(0, _emptyCellList.Count);
            int spriteIndex = Random.Range(0, wallPrefabs.Count);
            Vector2Int coord = _emptyCellList[cellIndex];
            WallObject newWall = Instantiate(wallPrefabs[spriteIndex]);
            AddObject(newWall, coord);
            _emptyCellList.RemoveAt(cellIndex);
        }
    }

    private void GenerateExit()
    {
        // the exit tile is placed in the upper-right corner of the tilemap
        Vector2Int endCoord = new(width - 2, height - 2);
        ExitCellObject exitCell = Instantiate(exitPrefab);
        AddObject(exitCell, endCoord);
        _emptyCellList.Remove(endCoord);
    }
#endregion
}
