using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private int tileWidth;
    [SerializeField] private int tileHeight;
    [SerializeField] private Tile[] groundTiles;
    [SerializeField] private Tile[] wallTiles;

    private Tilemap _tilemap;

    void Awake()
    {
        _tilemap = GetComponentInChildren<Tilemap>();
    }

    void Start()
    {
        for (int y = 0; y < tileHeight; y++)
        {
            for (int x = 0; x < tileWidth; x++)
            {
                Tile tile;

                if (x == 0 || y == 0 || x == tileWidth - 1 || y == tileHeight - 1)
                {
                    tile = wallTiles[Random.Range(0, wallTiles.Length)];
                }
                else
                {
                    tile = groundTiles[Random.Range(0, groundTiles.Length)];
                }

                Vector3Int position = new(x, y, 0);
                _tilemap.SetTile(position, tile);
            }
        }
    }
}
