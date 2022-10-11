using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class AgentMovement : MonoBehaviour
{
    public static AgentMovement Instance;

    [SerializeField] public int numberOfAgents;
    [SerializeField] public GameObject agent;
    [SerializeField] public GameObject house;
    [SerializeField] private float moveSpeed;

    //public List<Tile> housesList = new();
    public Vector3 target;
    private float constant = 0.35f;
    Vector2 myVector;
    Vector2 agentVector;
    int bottomLeftX;
    int bottomLeftY;
    int topRightX;
    int topRightY;
    int agentStartX;
    int agentStartY;
    int houseX;
    int houseY;

    //public Dictionary<int, Agent> agents;

    //public Dictionary<int, List<int>> agentsAttributes;
    //public Dictionary<int, GameObject> agentsGObj;

    public GameObject[] agentsGObj = new GameObject[10];
    public int[,] agentsAttributes = new int[10, 9];


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        //controller = GetComponent<CharacterController>();
        //player = GameObject.Find("Sphere");

        // Start possition
        //agent.transform.position = new Vector3(0, 0, 0);
        //house.transform.position= new Vector3(5, 0, 5);
    }

    public void generateHousesAndAgents()
    {
        for (int i = 0; i < numberOfAgents; i++)
        {
            do
            {
                int randomX = Random.Range(2, 97);
                //print("randomX:" + randomX);
                int randomY = Random.Range(2, 98);
                //print("randomY:" + randomY);
                houseX = randomX;
                houseY = randomY;

                //calculating the integer part of [randomX/2]
                //double halfRandomX = randomX / 2;
                //calculating the integer part of [randomY/2]
                //double halfRandomY = randomY / 2;

                //
                //Bottom-Left Tile
                //
                bottomLeftX = randomX - 2;
                bottomLeftY = randomY - 2;

                //
                //Top-Right Tile
                //
                topRightX = randomX + 2;
                topRightY = randomY + 2;

                //Agent Position
                agentStartX = topRightX + 1;
                agentStartY = bottomLeftY;

            } while (isHouseInsideAnotherBuilding(bottomLeftX, bottomLeftY, topRightX, topRightY) || isAgentStartPositionInvalid(agentStartX, agentStartY));


            Instantiate(agent, new Vector3(agentStartX, (float)(agentStartY + constant), 0), Quaternion.identity);
            Instantiate(house, new Vector3(houseX, houseY, 0), Quaternion.identity);

            for (int x = bottomLeftX; x <= topRightX; x++)
            {
                for (int y = bottomLeftY; y <= topRightY; y++)
                {
                    //GridManager.Instance.count++;
                    //print("House. Count: " + GridManager.Instance.count +", X: "+x+", Y: "+y);
                    myVector = new Vector2(x, y);
                    Buildings.Instance.buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));
                    //GridManager.Instance.GetTileAtPosition(myVector).visited = 0;
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;

                    /*
                    if (GridManager.Instance.GetTileAtPosition(myVector)._isWalkable.Equals(false))
                    {

                        //change color
                        //GridManager.Instance._tiles[myVector].mountainTileColor = GetComponent<Renderer>();
                        //GridManager.Instance._tiles[myVector].mountainTileColor.material.color = new Color(1f, 0f, 1f, 1f);

                    }
                    else
                    {
                        GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                    }
                    */
                }
            }
            agentVector = new Vector2(agentStartX, agentStartY);
            Buildings.Instance.buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(agentVector));

            // TODO -> print agent for debugging
            //Agent currentAgent = new(i, 100, 0, 0, agentStartX, agentStartY, agentStartX, agentStartY, 53, 82, agent);
            //Agent currentAgent = gameObject.AddComponent<Agent>();
            //currentAgent.id = i;j
            //currentAgent.energyPoints = 100;
            //currentAgent.numberOfCoins = 0;
            //currentAgent.numberOfEnergyPots = 0;
            //currentAgent.startX = agentStartX;
            //currentAgent.startY = agentStartY;
            //currentAgent.routeStartPointX = agentStartX;
            //currentAgent.routeStartPointY = agentStartY;
            //currentAgent.routeEndPointX = 53;
            //currentAgent.routeEndPointY = 82;
            //currentAgent.assetPrefab = agent;
            //print(currentAgent);
            //agents.Add(i, currentAgent);
            //print(agents[i]);

            /*
            print(agent);
            agentsGObj[i] = agent;
            var attributes = new List<int>();
            attributes.Add(100);
            attributes.Add(0);
            attributes.Add(0);
            attributes.Add(agentStartX);
            attributes.Add(agentStartY);
            attributes.Add(agentStartX);
            attributes.Add(agentStartY);
            attributes.Add(53);
            attributes.Add(82);
            agentsAttributes[i] = attributes;
            print(agentsAttributes[i]);
            */

            agentsGObj[i] = agent;
            agentsAttributes[i, 0] = 100;
            agentsAttributes[i, 1] = 0;
            agentsAttributes[i, 2] = 0;
            agentsAttributes[i, 3] = agentStartX;
            agentsAttributes[i, 4] = agentStartY;
            agentsAttributes[i, 5] = agentStartX;
            agentsAttributes[i, 6] = agentStartY;
            agentsAttributes[i, 7] = 53;
            agentsAttributes[i, 8] = 82;


        }
        GridManager.Instance.findDistance = true;
    }

    public bool isHouseInsideAnotherBuilding(int bottomLeftX, int bottomLeftY, int topRightX, int topRightY)
    {
        for (int x = bottomLeftX; x < topRightX; x++)
        {
            for (int y = bottomLeftY; y < topRightY; y++)
            {
                myVector = new Vector2(x, y);
                if (Buildings.Instance.buildingsTiles.Contains(GridManager.Instance.GetTileAtPosition(myVector))){
                    return true;
                }
            }
        }
        return false;
    }
    public bool isAgentStartPositionInvalid(int agentStartX, int agentStartY)
    {
        //Checking for collisions with buildings
        myVector = new Vector2(agentStartX, agentStartY);
        if (Buildings.Instance.buildingsTiles.Contains(GridManager.Instance.GetTileAtPosition(myVector)))
        {
            return true;
        }

        //Checking for forbiddenTiles
        if (GridManager.Instance._tiles[myVector]._isWalkable.Equals(false))
        {
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        //target = new Vector3(0, 0, 9);
        //player.transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        // Debug.Log(player.transform.position);
    }

    //public void MovePlayer(List<GameObject> path)
    //{
    //    // Foreach tile in list (list = shortest path)
    //    foreach (GameObject item in path)
    //    {
    //        // Message with a GameObject name.
    //        Debug.Log("X: " + item.GetComponent<GridStat>().x + ", Y: " + item.GetComponent<GridStat>().y);

    //        target = new Vector3(item.GetComponent<GridStat>().x, 0, item.GetComponent<GridStat>().y);
    //        while (player.transform.position != target)
    //            player.transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

    //        //Target Reached
    //        Debug.Log(player.transform.position);
    //    }
    //}


    public void MovePlayer(Tile element, GameObject myAgent)
    {
        //Target
        Debug.Log("X: " + element.x + ", Y: " + element.y);

        target = new Vector2(element.x,element.y);

        while (myAgent.transform.position != target)
            myAgent.transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        //Target Reached
        Debug.Log(myAgent.transform.position);
    }

}
