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
    [SerializeField] private List<FoodObject> foodPrefabs;

    [Header("Wall")]
    [SerializeField] private int minWall;
    [SerializeField] private int maxWall;
    [SerializeField] private List<ObstacleObject> obstaclePrefabs;

    [Header("Exit")]
    [SerializeField] private ExitObject exitCellPrefab;

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
        GenerateRandomly(obstaclePrefabs, minWall, maxWall);
        GenerateRandomly(foodPrefabs, minFood, maxFood);
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

    private void GenerateRandomly(IReadOnlyList<CellObject> prefabList, int min, int max)
    {
        int count = Random.Range(min, max);
        for (int i = 0; i < count; i++)
        {
            int cellIndex = Random.Range(0, _emptyCellList.Count);
            int prefabIndex = Random.Range(0, prefabList.Count);
            CreateCellObject(_emptyCellList[cellIndex], prefabList[prefabIndex]);
        }
    }

    private void GenerateExit()
    {
        // Gameplay rule: the exit cell will always be placed at the 
        // most upper-right corner of the game board 2 tiles in
        Vector2Int coord = new(width - 2, height - 2);
        CreateCellObject(coord, exitCellPrefab);
    }

    private void CreateCellObject(Vector2Int cellCoord, CellObject cellObject)
    {
        Vector2Int coord = cellCoord;
        CellObject cell  = Instantiate(cellObject);
        AddObject(cell, coord);
        _emptyCellList.Remove(coord); // remove from the list, the cell isn't empty anymore
    }
#endregion
}
