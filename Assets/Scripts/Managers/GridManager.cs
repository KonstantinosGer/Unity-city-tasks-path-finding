using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] public int _width, _height;
    public int scale;

    [SerializeField] private Tile _grassTile, _forbiddenTile;
    [SerializeField] public GameObject rock;

    [SerializeField] private Transform _cam;

    public Dictionary<Vector2, Tile> _tiles;

    public Dictionary<int, List<Tile>> agentsPaths; // store path for each of (numberOfAgents) agent

    public bool findDistance = false;
    public List<Tile> path = new();
    Vector2 myVector;

    public int index;




    void Awake()
    {
        Instance = this;
        GenerateGrid();
        agentsPaths = new Dictionary<int, List<Tile>>();

        scale = 1;
        index = 0;
    }


    private void Update()
    {
        //
        // Starting Pathfinding algorithm
        //

        if (findDistance && !AgentMovement.Instance.agents[index].executedThePlan)
        {

            if (!AgentMovement.Instance.agents[index].hasDied)
            {
                    InitialSetUp(index);

                    // Propagate all the numbers
                    SetDistance();
                    // Give us an array
                    SetPath(index);


                    path.Reverse();
                agentsPaths[index] = path;



            }

            if (index == AgentMovement.Instance.numberOfAgents - 1)
                {
                    index = 0;
                    // Set findDistance to false so it doesn't keep doing this over and over again
                    findDistance = false;

                    AgentMovement.Instance.moveAgents = true;

            }
            else
            {
                index++;
            }
        }
    }


    //
    // Checking coordinates are the entrace of Bank/Woodstore/Toolstore
    //
    public bool isBuildingEntrance(int x, int y)
    {
        if(x == 53 && y == 82)
        {
            return true;
        }
        if(x == 83 && y == 28)
        {
            return true;
        }
        if(x == 19 && y == 52)
        {
            return true;
        }
        return false;
    }

    //
    // Checking if we are inside Bank/Woodstore/Toolstore
    //
    public bool isBuildingArea(int x, int y)
    {
        if(x >= 43 && x <= 57 && y >= 83 && y <= 97)
        {
            return true;
        }
        if(x >= 8 && x <= 18 && y >= 52 && y <= 62)
        {
            return true;
        }
        if(x >= 84 && x <= 94 && y >= 28 && y <= 38)
        {
            return true;
        }
        return false;
    }


    //
    // Generating a dynamic grid consisting of walkable tiles and forbidden tiles
    //
    public void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                var randomTile = Random.Range(0, 12) == 3 && (!isBuildingEntrance(i,j) && !isBuildingArea(i,j))? _forbiddenTile : _grassTile;  // if random generator returns 3 then assign a forbiddenTile, else grasstile
                var spawnedTile = Instantiate(randomTile, new Vector3(i, j), Quaternion.identity);

                if(randomTile == _forbiddenTile)
                {
                    Instantiate(rock, new Vector3(i, j), Quaternion.identity);
                }

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

    }

    
    //
    // GetSpawnTile for gold and energyPot
    //
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


    // Setting up the gridArray so we can label the points
    void InitialSetUp(int agentId)
    {
        foreach (var tile in _tiles)
        {
            // At first, label everything as -1
            tile.Value.visited = -1;
        }
        // Only starting position is labeled with 0
        myVector = new Vector2(AgentMovement.Instance.agents[agentId].routeStartPointX, AgentMovement.Instance.agents[agentId].routeStartPointY);
        _tiles[myVector].visited = 0;
    }


    //
    // Checking (on each direction) for the optimum next tile of path 
    //
    bool TestDirection(int x, int y, int step, int direction)
    {
        // int direction tells which case to use.
        // 1 is up, 2 is right, 3 is down and 4 is left
        switch (direction)
        {
            case 4:
                if(x-1 >= 0)
                {
                    myVector = new Vector2(x - 1, y);
                }
                else
                {
                    return false;
                }
                if (_tiles[myVector] && _tiles[myVector]._isWalkable && x - 1 > -1 && _tiles[myVector].visited == step)
                    return true;
                else
                    return false;
            case 3:
                if(y-1 >= 0)
                {
                    myVector = new Vector2(x, y - 1);
                }
                else
                {
                    return false;
                }
                if (_tiles[myVector] && _tiles[myVector]._isWalkable && y - 1 > -1 && _tiles[myVector].visited == step)
                    return true;
                else
                    return false;
            case 2:
                if (x+1 <= 99)
                {
                    myVector = new Vector2(x + 1, y);
                }
                else
                {
                    return false;
                }
                if (_tiles[myVector] && _tiles[myVector]._isWalkable && x + 1 < _width && _tiles[myVector].visited == step)
                    return true;
                else
                    return false;
            case 1:
                if (y + 1 <= 99)
                {
                    myVector = new Vector2(x, y + 1);
                }
                else
                {
                    return false;
                }
                if (_tiles[myVector] && _tiles[myVector]._isWalkable && y + 1 < _height && _tiles[myVector].visited == step)
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

    //
    // Iterrating over grid and calling TestFourDirections for next tile in path
    //
    void SetDistance()
    {
        for (int step = 1; step < _height * _width; step++)
        {
            // Going through the whole gridArray and checking all the different objects
            foreach (var tile in _tiles)
            {
                if (tile.Value.visited == step - 1)
                {
                    // Checking everything around the object we currently are
                    // As long as it's -1, we are going to change it based on the step
                    TestFourDirections(tile.Value.x, tile.Value.y, step);
                }
            }
        }
    }

    //
    // Setting the shortest path for destination
    //
    void SetPath(int agentId)
    {
        int step;
        int x = AgentMovement.Instance.agents[agentId].routeEndPointX;
        int y = AgentMovement.Instance.agents[agentId].routeEndPointY;
        List<Tile> tempList = new();

        // At first, clear any path we might already have
        path.Clear();


        // if it's -1, it means we can't get there
        myVector = new Vector2(x, y);
        if (_tiles[myVector] && _tiles[myVector].visited > 0)
        {
            // Add it to our path
            myVector = new Vector2(x, y);
            path.Add(_tiles[myVector]);
            // We started from the ending and then we go to the next lowest repeatedly
            step = _tiles[myVector].visited - 1;
        }
        else
        {
            // if there isn't something equal to -1
            // It means we can't get to final destination (endX, endY)
            print("Can't reach the desired location");
            print("My vector: "+myVector);
            print("_tiles[myVector].visited = "+_tiles[myVector].visited);
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
            myVector = new Vector2(x, y);
            Tile tempTile = FindClosest(_tiles[myVector].transform, tempList);
            //print(tempTile);
            path.Add(tempTile);
            x = tempTile.x;
            y = tempTile.y;
            tempList.Clear();
        }
        
    }

    void SetVisited(int x, int y, int step)
    {
        myVector = new Vector2(x, y);
        if (_tiles[myVector])
            _tiles[myVector].visited = step;
    }

    //
    // From all possible paths, we keep the optimal (shortest) path
    //
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
