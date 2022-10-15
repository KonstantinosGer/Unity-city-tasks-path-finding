using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

    public Vector3 target;
    private float constant;
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
    public bool moveAgents = false;

    public Dictionary<string, Vector2> coordinates;

    public Vector2 BANK_COORDINATES; 
    public Vector2 WOOD_STORE_COORDINATES; 
    public Vector2 TOOL_STORE_COORDINATES; 

    public Dictionary<string, bool> isBuildingVisited;

    public int i;

    Agent currentAgent = new Agent();

    public Dictionary<int, Agent> agents;

    public int numberOfAgentsFinished;

    public int previousRandomBuilding;

    public bool[,] savedDataArray = new bool[10,3];

    //x -> agentID, y -> trip
    public int[,] steps = new int[10, 4];
    public int[] trips = new int[10];

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        coordinates = new Dictionary<string, Vector2>();
        isBuildingVisited = new Dictionary<string, bool>();
        agents = new Dictionary<int, Agent>();

        BANK_COORDINATES = new(53, 82);
        WOOD_STORE_COORDINATES = new(19, 52);
        TOOL_STORE_COORDINATES = new(83, 28);
        constant = 0.35f;
        i = 0;
        numberOfAgentsFinished = 0;
        previousRandomBuilding = -1;

        for(int k = 0; k < 10; k++)
        {
            trips[k] = 0;
            for(int j = 0; j < 4; j++)
            {
                steps[k,j] = 0;
            }
        }
    }

    public void generateHousesAndAgents()
    {
        for (int i = 0; i < numberOfAgents; i++)
        {
            do
            {
                int randomX = Random.Range(2, 97);
                int randomY = Random.Range(2, 98);

                houseX = randomX;
                houseY = randomY;

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

            //
            // Instantiate prefabs
            //
            agent = Instantiate(agent, new Vector3(agentStartX, (float)(agentStartY + constant)), Quaternion.identity);
            Instantiate(house, new Vector3(houseX, houseY, 0), Quaternion.identity);


            //
            // Setting tiles inside buildings as non walkable
            //
            for (int x = bottomLeftX; x <= topRightX; x++)
            {
                for (int y = bottomLeftY; y <= topRightY; y++)
                {
                    myVector = new Vector2(x, y);
                    Buildings.Instance.buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(myVector));
                    GridManager.Instance.GetTileAtPosition(myVector)._isWalkable = false;
                }
            }
            agentVector = new Vector2(agentStartX, agentStartY);
            Buildings.Instance.buildingsTiles.Add(GridManager.Instance.GetTileAtPosition(agentVector));


            //
            // Choosing a random bulding for each agent to start his plan
            // Bank = 0, Woodstore = 1, Toolstore = 2
            //
            int destinationX;
            int destinationY;
            int randomBuilding;
            do
            {
                randomBuilding = Random.Range(0, 2);
            } while (randomBuilding == previousRandomBuilding);
            previousRandomBuilding = randomBuilding;
             
            coordinates["home"] = new Vector2(agentStartX, agentStartY);

            if (randomBuilding == 0)
            {
                coordinates["bank"] = new Vector2(53, 82);
                coordinates["woodStore"] = new Vector2(0, 0);
                coordinates["toolStore"] = new Vector2(0, 0);

                destinationX = 53;
                destinationY = 82;
            }
            else if (randomBuilding == 1)
            {
                coordinates["bank"] = new Vector2(0, 0);
                coordinates["woodStore"] = new Vector2(19, 52);
                coordinates["toolStore"] = new Vector2(0, 0);

                destinationX = 19;
                destinationY = 52;
            }
            else
            {
                coordinates["bank"] = new Vector2(0, 0);
                coordinates["woodStore"] = new Vector2(0, 0);
                coordinates["toolStore"] = new Vector2(83, 28);

                destinationX = 83;
                destinationY = 28;
            }

            //
            // Initializing agent's attributes
            //
            isBuildingVisited["home"] = true;
            isBuildingVisited["bank"] = false;
            isBuildingVisited["woodStore"] = false;
            isBuildingVisited["toolStore"] = false;

            //
            // Creating a new Agent object
            //
            currentAgent = new()
            {
                id = i,
                energyPoints = 100,
                numberOfCoins = 0,
                numberOfEnergyPots = 0,
                startX = agentStartX,
                startY = agentStartY,
                routeStartPointX = agentStartX,
                routeStartPointY = agentStartY,
                routeEndPointX = destinationX,
                routeEndPointY = destinationY,
                assetPrefab = agent,
                plan = coordinates,
                executedThePlan = false,
                agentVisitedBuildings = isBuildingVisited,
                hasDied = false
            };

            agents[i] = currentAgent;

            savedDataArray[i, 0] = false; //bank
            savedDataArray[i, 1] = false; //woodstore
            savedDataArray[i, 2] = false; //toolstore

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
    void FixedUpdate()
    {
        if (moveAgents && !agents[i].executedThePlan)
        {
            //
            // Move player to target position
            //
            bool agentMadeItToDestination = MovePlayer(GridManager.Instance.agentsPaths[i], agents[i].assetPrefab, agents[i] , trips[i]);
            

            if (agentMadeItToDestination)
            {
                trips[i]++;

                //
                // Updating agents' plan
                // Assigning new destination for each agent 
                //
                agents[i].routeStartPointX = agents[i].routeEndPointX;
                agents[i].routeStartPointY = agents[i].routeEndPointY;
                Vector2 currentPosition = new(agents[i].routeStartPointX, agents[i].routeStartPointY);
                if (currentPosition == BANK_COORDINATES)
                {
                    // Keeping previous visited booleans for each agent
                    agents[i].agentVisitedBuildings["bank"] = true;
                    agents[i].agentVisitedBuildings["woodStore"] = savedDataArray[i, 1];
                    agents[i].agentVisitedBuildings["toolStore"] = savedDataArray[i, 2];
                    savedDataArray[i, 0] = true;

                    print("Agent " + i + " arrived at the bank!");
                    if (agents[i].agentVisitedBuildings["woodStore"] == false)
                    {
                        agents[i].plan["woodStore"] = WOOD_STORE_COORDINATES;
                        agents[i].routeEndPointX = (int)WOOD_STORE_COORDINATES.x;
                        agents[i].routeEndPointY = (int)WOOD_STORE_COORDINATES.y;
                    }
                    else if (agents[i].agentVisitedBuildings["toolStore"] == false)
                    {
                        agents[i].plan["toolStore"] = TOOL_STORE_COORDINATES;
                        agents[i].routeEndPointX = (int)TOOL_STORE_COORDINATES.x;
                        agents[i].routeEndPointY = (int)TOOL_STORE_COORDINATES.y;
                    }
                    else
                    {
                        agents[i].routeEndPointX = agents[i].startX;
                        agents[i].routeEndPointY = agents[i].startY;
                    }
                }
                else if (currentPosition == WOOD_STORE_COORDINATES)
                {
                    // Keeping previous visited booleans for each agent
                    agents[i].agentVisitedBuildings["woodStore"] = true;
                    agents[i].agentVisitedBuildings["bank"] = savedDataArray[i, 0];
                    agents[i].agentVisitedBuildings["toolStore"] = savedDataArray[i, 2];
                    savedDataArray[i, 1] = true;

                    print("Agent " + i + " arrived at the woodStore!");
                    if (agents[i].agentVisitedBuildings["bank"] == false)
                    {
                        agents[i].plan["bank"] = BANK_COORDINATES;
                        agents[i].routeEndPointX = (int)BANK_COORDINATES.x;
                        agents[i].routeEndPointY = (int)BANK_COORDINATES.y;
                    }
                    else if (agents[i].agentVisitedBuildings["toolStore"] == false)
                    {
                        agents[i].plan["toolStore"] = TOOL_STORE_COORDINATES;
                        agents[i].routeEndPointX = (int)TOOL_STORE_COORDINATES.x;
                        agents[i].routeEndPointY = (int)TOOL_STORE_COORDINATES.y;
                    }
                    else
                    {
                        agents[i].routeEndPointX = agents[i].startX;
                        agents[i].routeEndPointY = agents[i].startY;
                    }
                }
                else if (currentPosition == TOOL_STORE_COORDINATES)
                {
                    // Keeping previous visited booleans for each agent
                    agents[i].agentVisitedBuildings["toolStore"] = true;
                    agents[i].agentVisitedBuildings["woodStore"] = savedDataArray[i, 1];
                    agents[i].agentVisitedBuildings["bank"] = savedDataArray[i, 0];
                    savedDataArray[i, 2] = true;

                    print("Agent " + i + " arrived at the toolStore!");
                    if (agents[i].agentVisitedBuildings["bank"] == false)
                    {
                        agents[i].plan["bank"] = BANK_COORDINATES;
                        agents[i].routeEndPointX = (int)BANK_COORDINATES.x;
                        agents[i].routeEndPointY = (int)BANK_COORDINATES.y;
                    }
                    else if (agents[i].agentVisitedBuildings["woodStore"] == false)
                    {
                        agents[i].plan["woodStore"] = WOOD_STORE_COORDINATES;
                        agents[i].routeEndPointX = (int)WOOD_STORE_COORDINATES.x;
                        agents[i].routeEndPointY = (int)WOOD_STORE_COORDINATES.y;
                    }
                    else
                    {
                        agents[i].routeEndPointX = agents[i].startX;
                        agents[i].routeEndPointY = agents[i].startY;
                    }
                }
                else if (currentPosition == new Vector2(agents[i].startX, agents[i].startY))
                {
                    // Keeping previous visited booleans for each agent
                    agents[i].agentVisitedBuildings["bank"] = savedDataArray[i, 0];
                    agents[i].agentVisitedBuildings["woodStore"] = savedDataArray[i, 1];
                    agents[i].agentVisitedBuildings["toolStore"] = savedDataArray[i, 2];

                    if (agents[i].agentVisitedBuildings["bank"] == true && agents[i].agentVisitedBuildings["woodStore"] == true && agents[i].agentVisitedBuildings["toolStore"] == true)
                    {
                        print("Agent " + i + " returned home. Plan executed!");
                        agents[i].executedThePlan = true;
                        numberOfAgentsFinished++;
                    }
                }
                else
                {
                    print("Agent " + i + " is in position: " + currentPosition);
                }


                //
                // When all agents have completed all of their tasks
                //
                if (numberOfAgentsFinished == numberOfAgents)
                {
                    for (int j = 0; j < numberOfAgents; j++)
                    {
                        target = new Vector2(agents[j].startX, agents[j].startY);
                        agents[j].assetPrefab.transform.position = target;
                    }
                    print("All agents completed their tasks");
                }
            }


            //
            // Calling pathfinding when each agents has finished his trip
            //
            if (i == numberOfAgents - 1)
            {
                i = 0;
                moveAgents = false;
                GridManager.Instance.findDistance = true;
            }
            else
            {
                i++;
            }
        }
    }

    //
    // Moving agent on shortest path
    //
    public bool MovePlayer(List<Tile> path, GameObject myAgent, Agent agent, int trip)
    {
        // Foreach tile in list (list = shortest path)
        foreach (Tile tile in path)
        {
            if (!agent.hasDied)
            {
                target = new Vector3(tile.x, tile.y + constant, 0);
                
                while (myAgent.transform.position != target)
                {
                    myAgent.transform.position = target;
                    agent.energyPoints--;
                    steps[agent.id, trip]++;
                }

                // Found tile with gold or energy pot
                if (tile.OccupiedUnit != null)
                {
                    float tempX = target.x;
                    float tempY = target.y - constant;
                    Vector2 temp = new(tempX, tempY);
                    // Found energy pot
                    if (UnitManager.Instance.energyPotTiles.Contains(tile))
                    {
                        print("I am in coordinates: " + temp + ".I found an energy pot.");
                        agent.numberOfEnergyPots++;
                    }
                    // Found gold coin
                    else if (UnitManager.Instance.goldTiles.Contains(tile))
                    {
                        print("I am in coordinates: " + temp + ".I found a gold coin.");
                        agent.numberOfCoins++;
                    }
                }

                if (agent.energyPoints <= 25 && agent.energyPoints > 0)
                {
                    if (agent.numberOfEnergyPots >= 1)
                    {
                        agent.numberOfEnergyPots--;
                        agent.energyPoints += 20;
                        print("Agent's " + agent.id + " health was " + (agent.energyPoints - 20) + ". He consumed one energy pot and now his health is " + agent.energyPoints + ".");
                    }
                    else
                    {
                        print("Agent's " + agent.id + " health is deteriorating dramatically.");
                    }
                }
                else if (agent.energyPoints == 0)
                {
                    print("Agent's " + agent.id + " has died at "+ myAgent.transform.position+".");
                    agent.hasDied = true;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }
}
