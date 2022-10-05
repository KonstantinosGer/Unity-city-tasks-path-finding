using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBehavior : MonoBehaviour
{
    //
    // Grid
    //
    public int rows = 10;
    public int columns = 10;
    public int scale = 1;
    public GameObject gridPrefab;
    public Vector3 leftBottomLocation = new Vector3(0, 0, 0);
    //
    //
    //
    public bool findDistance = true;
    public GameObject[,] gridArray;
    public int startX = 0;
    public int startY = 0;
    public int endX = 2;
    public int endY = 2;
    public List<GameObject> path = new List<GameObject>();

    // public List<GameObject> reversePath = new List<GameObject>();

    //////
    public GameObject player;
    //[SerializeField] private float moveSpeed;
    //public Vector3 target;
    //public bool move = true;


    void Awake()
    {
        //Initialize gridArray (size of grid)
        gridArray = new GameObject[columns, rows];

        //////
        player = GameObject.Find("Sphere");
        // Start possition
        //player.transform.position = new Vector3(0, 0, 0);

        //
        // Grid
        //
        if (gridPrefab)
            GenerateGrid();
        else print("Missing gridPrefab, please assign.");
    }

    // Update is called once per frame
    void Update()
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

        //if (path != null)
        //{
        //    player.GetComponent<PlayerMovement>().MovePlayer3(path[0]);
        //    path.RemoveAt(0);
        //}


        // If list "path" (sortest path) has at least one item
        if(path.Count > 0)
        {
            // Call MovePlayer from PlayerMovement script
            player.GetComponent<PlayerMovement>().MovePlayer(path[0]);
            path.RemoveAt(0);
        }




        // ctrl + k + c : comment / uncomment
        //if (move)
        //{
        //    // Call MovePlayer from PlayerMovement script
        //    //player.GetComponent<PlayerMovement>().MovePlayer(path);
        //    foreach (GameObject item in path)
        //    {
        //        // Message with a GameObject name.
        //        Debug.Log("X: " + item.GetComponent<GridStat>().x);
        //        // Message with a GameObject name.
        //        Debug.Log("Y: " + item.GetComponent<GridStat>().y);


        //        //target = new Vector3(item.GetComponent<GridStat>().x, 0, item.GetComponent<GridStat>().y);
        //        target = new Vector3(7, 0, 7);
        //        player.transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        //    }

        //    move = false;
        //}

    }

    void GenerateGrid()
    {
        //
        // Grid
        //
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject obj = Instantiate(gridPrefab, new Vector3(leftBottomLocation.x + scale * i, leftBottomLocation.y, leftBottomLocation.z + scale * j), Quaternion.identity);
                obj.transform.SetParent(gameObject.transform);
                obj.GetComponent<GridStat>().x = i;
                obj.GetComponent<GridStat>().y = j;


                //
                //
                //
                gridArray[i, j] = obj;
            }
        }
    }

    // Set up the gridArray so we can label the points
    void InitialSetUp()
    {
        foreach(GameObject obj in gridArray)
        {
            // At first, label everything as -1
            obj.GetComponent<GridStat>().visited = -1;
        }
        // Only starting position is labeled with 0
        gridArray[startX, startY].GetComponent<GridStat>().visited = 0;
    }

    bool TestDirection(int x, int y, int step, int direction)
    {
        // int direction tells which case to use.
        // 1 is up, 2 is right, 3 is down and 4 is left
        switch (direction)
        {
            case 4:
                if (x - 1 > -1 && gridArray[x - 1, y] && gridArray[x - 1, y].GetComponent<GridStat>().visited == step)
                    return true;
                else
                    return false;
            case 3:
                if (y - 1 > -1 && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<GridStat>().visited == step)
                    return true;
                else
                    return false;
            case 2:
                if (x + 1 < columns && gridArray[x + 1, y] && gridArray[x + 1, y].GetComponent<GridStat>().visited == step)
                    return true;
                else
                    return false;
            case 1:
                if (y + 1 < rows && gridArray[x, y + 1] && gridArray[x, y + 1].GetComponent<GridStat>().visited == step)
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
        int x = startX;
        int y = startY;
        int[] testArray = new int[rows * columns]; //bigger than any number of steps i will ever do

        for(int step = 1; step < rows*columns; step++)
        {
            // Going through the whole gridArray and checking all the different objects
            foreach(GameObject obj in gridArray)
            {
                if(obj && obj.GetComponent<GridStat>().visited == step - 1)
                {
                    // Checking everything around the object i currently am
                    // As long as it's -1, i'm going to change it based on the step
                    TestFourDirections(obj.GetComponent<GridStat>().x, obj.GetComponent<GridStat>().y, step);
                }
            }
        }
    }

    void SetPath()
    {
        int step;
        int x = endX;
        int y = endY;
        List<GameObject> tempList = new List<GameObject>();
        // At first, clear any path i might already have
        path.Clear();

        // Make sure gridArray[endX, endY] exists
        // And it must be > 0
        // if it's -1, it means we can't get there
        if (gridArray[endX, endY] && gridArray[endX, endY].GetComponent<GridStat>().visited > 0)
        {
            // Add it to our path
            path.Add(gridArray[x, y]);
            // We started from the ending and then we go to the next lowest repeatedly
            step = gridArray[x, y].GetComponent<GridStat>().visited - 1;
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
                tempList.Add(gridArray[x, y + 1]);
            // And now i'm testing all directions (to see if this is going to the right direction)
            if (TestDirection(x, y, step, 2))
                tempList.Add(gridArray[x + 1, y]);
            if (TestDirection(x, y, step, 3))
                tempList.Add(gridArray[x, y - 1]);
            if (TestDirection(x, y, step, 4))
                tempList.Add(gridArray[x - 1, y]);

            // We have a path
            // It finds the shortest object, out of the four arround my current possision (out of all four directions)
            // And then puts the shortest one in the path
            GameObject tempObj = FindClosest(gridArray[endX, endY].transform, tempList);
            path.Add(tempObj);
            x = tempObj.GetComponent<GridStat>().x;
            y = tempObj.GetComponent<GridStat>().y;
            tempList.Clear();
        }

    }

    void SetVisited(int x, int y, int step)
    {
        // if gridArray[x,y] exists
        if (gridArray[x, y])
            gridArray[x, y].GetComponent<GridStat>().visited = step;
    }

    //From all possible paths, we want to keep the optimal (shortest) path
    GameObject FindClosest(Transform targetLocation, List<GameObject> list)
    {
        // Initialize currentDistance with something realy big
        float currentDistance = scale * rows * columns;
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
