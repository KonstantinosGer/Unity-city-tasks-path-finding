using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class Buildings : MonoBehaviour
{
    public static Buildings Instance;

    [SerializeField] public GameObject bank;
    [SerializeField] public GameObject woodStore;
    [SerializeField] public GameObject toolStore;
    [SerializeField] public GameObject treeSet;
    [SerializeField] public GameObject treeLine;
    [SerializeField] public GameObject lake;
    [SerializeField] public GameObject nonImportantBuilding;


    public List<Tile> buildingsTiles = new List<Tile>();
    Vector2 myVector;

    void Awake()
    {
        Instance = this;

        Instantiate(bank, new Vector3(50, 90, 0), Quaternion.identity);
        Instantiate(woodStore, new Vector3(13, 57, 0), Quaternion.identity);
        Instantiate(toolStore, new Vector3(89, 33, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(20, 80, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(10, 85, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(5, 75, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(12, 71, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(27, 87, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(12, 92, 0), Quaternion.identity);
        Instantiate(lake, new Vector3(15, 35, 0), Quaternion.identity);
        Instantiate(nonImportantBuilding, new Vector3(80, 80, 0), Quaternion.identity);
        Instantiate(nonImportantBuilding, new Vector3(10, 10, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(80, 53, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(75, 45, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(88, 57, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(93, 63, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(65, 90, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(70, 85, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(63, 65, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(30, 10, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(40, 5, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(50, 7, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(65, 10, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(80, 15, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(60, 20, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(90, 5, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(30, 45, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(70, 30, 0), Quaternion.identity);
        Instantiate(treeSet, new Vector3(30, 65, 0), Quaternion.identity);
        Instantiate(treeLine, new Vector3(48, 35, 0), Quaternion.identity);

        for (int i = 0; i < GridManager.Instance._width; i++)
        {
            for (int j = 0; j < GridManager.Instance._height; j++)
            {
                //tiles for bank building
                if (i >= 43 && i <= 57 && j >= 83 && j <= 97)
                {
                    //GridManager.Instance.count++;
                    //print("bank. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for woodstore building
                if (i >= 8 && i <= 18 && j >= 52 && j <= 62)
                {
                    //GridManager.Instance.count++;
                    //print("woodstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for toolstore building
                if (i >= 84 && i <= 94 && j >= 28 && j <= 38)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-1
                if (i >= 16 && i <= 24 && j >= 76 && j <= 84)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-2
                if (i >= 6 && i <= 14 && j >= 81 && j <= 89)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-3
                if (i >= 1 && i <= 9 && j >= 71 && j <= 79)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-4
                if (i >= 8 && i <= 16 && j >= 67 && j <= 75)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-5
                if (i >= 23 && i <= 31 && j >= 83 && j <= 91)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-6
                if (i >= 8 && i <= 16 && j >= 88 && j <= 96)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for lake
                if (i >= 0 && i <= 30 && j >= 23 && j <= 47)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for nonImportant Building-1
                if (i >= 75 && i <= 85 && j >= 75 && j <= 85)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for nonImportant Building-2
                if (i >= 5 && i <= 15 && j >= 5 && j <= 15)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-1
                if (i >= 76 && i <= 84 && j >= 49 && j <= 57)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-2
                if (i >= 71 && i <= 79 && j >= 41 && j <= 49)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-3
                if (i >= 84 && i <= 92 && j >= 53 && j <= 61)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-4
                if (i >= 89 && i <= 97 && j >= 59 && j <= 67)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-5
                if (i >= 61 && i <= 69 && j >= 86 && j <= 94)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-6
                if (i >= 66 && i <= 74 && j >= 81 && j <= 89)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-7
                if (i >= 59 && i <= 67 && j >= 61 && j <= 69)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-8
                if (i >= 26 && i <= 34 && j >= 6 && j <= 14)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-9
                if (i >= 36 && i <= 44 && j >= 1 && j <= 9)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-10
                if (i >= 46 && i <= 54 && j >= 3 && j <= 11)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-11
                if (i >= 61 && i <= 69 && j >= 6 && j <= 14)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-12
                if (i >= 76 && i <= 84 && j >= 11 && j <= 19)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-13
                if (i >= 56 && i <= 64 && j >= 16 && j <= 24)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-14
                if (i >= 86 && i <= 94 && j >= 1 && j <= 9)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-15
                if (i >= 26 && i <= 34 && j >= 41 && j <= 49)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-16
                if (i >= 66 && i <= 74 && j >= 26 && j <= 34)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeSet-17
                if (i >= 26 && i <= 34 && j >= 61 && j <= 69)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
                //tiles for treeLine
                if (i >= 41 && i <= 55 && j >= 28 && j <= 42)
                {
                    //GridManager.Instance.count++;
                    //print("toolstore. Count: " + GridManager.Instance.count);
                    myVector = new Vector2(i, j);
                    buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));

                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }

            }
        }

        AgentMovement.Instance.generateHousesAndAgents();
    }
}
