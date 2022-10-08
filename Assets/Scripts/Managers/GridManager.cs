using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private int _width, _height;
    public int scale = 1;

    [SerializeField] private Tile _grassTile, _mountainTile;

    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles;

    public bool findDistance = true;
    public List<Tile> path = new List<Tile>();
    public int startX = 0;
    public int startY = 0;
    public int endX = 5;
    public int endY = 5;
    Vector2 myVector;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateGrid();
        //Debug.Log(GetTileAtPosition(myVector));
    }

    private void Update()
    {
        if (findDistance)
        {

            InitialSetUp();


            // Propagate all the numbers
            SetDistance();
            // Give us an array
            SetPath();




            path.Reverse();
            // Call MovePlayer from PlayerMovement script
            //player.GetComponent<PlayerMovement>().MovePlayer(path);



            // Set findDistance to false so it doesn't keep doing this over and over again
            findDistance = false;
        }
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

                spawnedTile.transform.SetParent(gameObject.transform);
                spawnedTile.x = i;
                spawnedTile.y = j;

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


    // Set up the gridArray so we can label the points
    void InitialSetUp()
    {
        foreach (var tile in _tiles)
        {
            // At first, label everything as -1
            tile.Value.visited = -1;
            //print("tile: "+tile+ ", visited: "+tile.Value.visited);
        }
        // Only starting position is labeled with 0
        myVector = new Vector2(startX, startY);
        _tiles[myVector].visited = 0;
    }

    bool TestDirection(int x, int y, int step, int direction)
    {
        // int direction tells which case to use.
        // 1 is up, 2 is right, 3 is down and 4 is left
        switch (direction)
        {
            case 4:
                myVector = new Vector2(x - 1, y);
                if (x - 1 > -1 && _tiles[myVector] && _tiles[myVector].visited == step)
                    return true;
                else
                    return false;
            case 3:
                myVector = new Vector2(x, y-1);
                if (y - 1 > -1 && _tiles[myVector] && _tiles[myVector].visited == step)
                    return true;
                else
                    return false;
            case 2:
                myVector = new Vector2(x+1, y);
                if (x + 1 < _width && _tiles[myVector] && _tiles[myVector].visited == step)
                    return true;
                else
                    return false;
            case 1:
                myVector = new Vector2(x, y+1);
                if (y + 1 < _height && _tiles[myVector] && _tiles[myVector].visited == step)
                    return true;
                else
                    return false;
        }
        return false;
    }

    void TestFourDirections(int x, int y, int step)
    {
        if (TestDirection(x, y, -1, 1))
            SetVisited(x, y + 1, step);
        if (TestDirection(x, y, -1, 2))
            SetVisited(x + 1, y, step);
        if (TestDirection(x, y, -1, 3))
            SetVisited(x, y - 1, step);
        if (TestDirection(x, y, -1, 4))
            SetVisited(x - 1, y, step);
    }

    void SetDistance()
    {
        //int x = 0;
        //int y = 0;
        //int[] testArray = new int[_height * _width]; //bigger than any number of steps i will ever do

        for (int step = 1; step < _height * _width; step++)
        {
            // Going through the whole gridArray and checking all the different objects
            foreach (var tile in _tiles)
            {
                if (tile.Value.visited == step - 1)
                {
                    // Checking everything around the object i currently am
                    // As long as it's -1, i'm going to change it based on the step
                    TestFourDirections(tile.Value.x, tile.Value.y, step);
                }
            }
        }
    }


    void SetPath()
    {
        int step;
        int x = endX;
        int y = endY;
        List<Tile> tempList = new List<Tile>();
        // At first, clear any path i might already have
        path.Clear();

        // Make sure gridArray[endX, endY] exists
        // And it must be > 0
        // if it's -1, it means we can't get there
        myVector = new Vector2(endX, endY);
        if (_tiles[myVector] && _tiles[myVector].visited > 0)
        {
            // Add it to our path
            myVector = new Vector2(x, y);
            path.Add(_tiles[myVector]);
            // We started from the ending and then we go to the next lowest repeatedly
            step = _tiles[myVector].visited - 1;

            //print("tile: " + _tiles[myVector] + ", visited: " + _tiles[myVector].visited);
            //print("path: " + path);
        }
        else
        {
            // if there isn't something equal to -1
            // It means we can't get to final destination (endX, endY)
            print("Can't reach the desired location");
            return;
        }

        // Starting at the last spot and going recursively back to 0
        for (int i = step; step > -1; step--)
        {
            // First if -> i'm testing up
            if (TestDirection(x, y, step, 1))
            {
                myVector = new Vector2(x, y+1);
                tempList.Add(_tiles[myVector]);
            } 
            // And now i'm testing all directions (to see if this is going to the right direction)
            if (TestDirection(x, y, step, 2))
            {
                myVector = new Vector2(x+1, y);
                tempList.Add(_tiles[myVector]);
            }
            if (TestDirection(x, y, step, 3))
            {
                myVector = new Vector2(x, y-1);
                tempList.Add(_tiles[myVector]);
            }   
            if (TestDirection(x, y, step, 4))
            {
                myVector = new Vector2(x-1, y);
                tempList.Add(_tiles[myVector]);
            }   

            // We have a path
            // It finds the shortest object, out of the four arround my current possision (out of all four directions)
            // And then puts the shortest one in the path
            myVector = new Vector2(endX, endY);
            Tile tempTile = FindClosest(_tiles[myVector].transform, tempList);
            path.Add(tempTile);
            x = tempTile.x;
            y = tempTile.y;
            tempList.Clear();

            //print("tempTile: " + tempTile);
        }

    }

    void SetVisited(int x, int y, int step)
    {
        // if gridArray[x,y] exists
        myVector = new Vector2(x, y);
        if (_tiles[myVector])
            _tiles[myVector].visited = step;
    }

    //From all possible paths, we want to keep the optimal (shortest) path
    Tile FindClosest(Transform targetLocation, List<Tile> list)
    {
        // Initialize currentDistance with something realy big
        float currentDistance = scale * _height * _width;
        int indexNumber = 0;

        // Check which of them has shortest distance
        for (int i = 0; i < list.Count; i++)
        {
            // Checking between the end goal and the current place
            // if it's less than the currentDistance, then it's closer and then that's the one we want to use
            if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
            {
                currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                indexNumber = i;
            }
        }
        return list[indexNumber];
    }
}
