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
    [SerializeField] public GameObject lake;


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
            }
        }

        AgentMovement.Instance.generateHousesAndAgents();
    }
}
