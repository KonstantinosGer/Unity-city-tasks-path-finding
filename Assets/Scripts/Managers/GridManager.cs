using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _grassTile, _mountainTile;

    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                var randomTile = Random.Range(0, 4) == 3 && (i > _width / 5 && i < _width * 4/5)? _mountainTile : _grassTile;  // if random generator returns 3 then assign a mountainTile, else grasstile
                var spawnedTile = Instantiate(randomTile, new Vector3(i, j), Quaternion.identity);

                spawnedTile.name = $"Tile {i} {j}";

                spawnedTile.Init(i,j);


                _tiles[new Vector2(i, j)] = spawnedTile;
            }
        }

        //adjusting camera view
        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);


        //GameManager.Instance.ChangeState(GameState.SpawnGold);
    }

    //getSpawnTile for gold and energyPot
    public Tile GetSpawnTile()
    {
        return _tiles.Where(t => t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }

    
    public Tile GetNonWalkableTile()
    {
        return _tiles.Where(t => t.Key.x < _width / 4).OrderBy(t => Random.value).First().Value;
    }


    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
}
