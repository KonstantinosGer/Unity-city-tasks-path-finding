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

    public int index;

    Agent currentAgent = new Agent();

    public Dictionary<int, Agent> agents;

    public int numberOfAgentsFinished;

    public int previousRandomBuilding;

    public bool[,] savedDataArray = new bool[10,3];

    //x -> agentID, y -> trip
    public int[,] steps = new int[10, 4];
    public int[] trips = new int[10];
    public int[] energyPots = new int[10];
    public int[] goldCoins = new int[10];
    public int[] healthPoints = new int[10];

    public int messageCount;

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
        index = 0;
        numberOfAgentsFinished = 0;
        previousRandomBuilding = -1;

        messageCount = 1;
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



            //
            // Keep previous attributes for each agent in temporary arrays
            //
            savedDataArray[i, 0] = false; //bank
            savedDataArray[i, 1] = false; //woodstore
            savedDataArray[i, 2] = false; //toolstore



            trips[i] = 0;
            energyPots[i] = 0;
            goldCoins[i] = 0;
            healthPoints[i] = 100;

            for (int j = 0; j < 4; j++)
            {
                steps[i, j] = 0;
            }

        }
        GridManager.Instance.findDistance = true;
        //GridManager.Instance.PathFinding();
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
        if (moveAgents && !agents[index].executedThePlan)
        {

            if (!agents[index].hasDied)
            {
                //
                // Move player to target position
                //
                //print(messageCount + ". Agent's " + index + " trip from (" + agents[index].routeStartPointX + ", " + agents[index].routeStartPointY + ") to (" + agents[index].routeEndPointX + ", " + agents[index].routeEndPointY + ") starts now!");
                //messageCount++;
                bool agentMadeItToDestination = MovePlayer(GridManager.Instance.agentsPaths[index], agents[index].assetPrefab, agents[index], trips[index]);


                if (agentMadeItToDestination)
                {
                    trips[index]++;

                    //
                    // Updating agents' plan
                    // Assigning new destination for each agent 
                    //
                    agents[index].routeStartPointX = agents[index].routeEndPointX;
                    agents[index].routeStartPointY = agents[index].routeEndPointY;
                    Vector2 currentPosition = new(agents[index].routeStartPointX, agents[index].routeStartPointY);
                    if (currentPosition == BANK_COORDINATES)
                    {
                        // Keeping previous visited booleans for each agent
                        agents[index].agentVisitedBuildings["bank"] = true;
                        agents[index].agentVisitedBuildings["woodStore"] = savedDataArray[index, 1];
                        agents[index].agentVisitedBuildings["toolStore"] = savedDataArray[index, 2];
                        savedDataArray[index, 0] = true;

                        print(messageCount + ". Agent " + index + " arrived at the bank!\nHis health is " + agents[index].energyPoints + ".");
                        messageCount++;

                        if (agents[index].agentVisitedBuildings["woodStore"] == false)
                        {
                            agents[index].plan["woodStore"] = WOOD_STORE_COORDINATES;
                            agents[index].routeEndPointX = (int)WOOD_STORE_COORDINATES.x;
                            agents[index].routeEndPointY = (int)WOOD_STORE_COORDINATES.y;
                        }
                        else if (agents[index].agentVisitedBuildings["toolStore"] == false)
                        {
                            agents[index].plan["toolStore"] = TOOL_STORE_COORDINATES;
                            agents[index].routeEndPointX = (int)TOOL_STORE_COORDINATES.x;
                            agents[index].routeEndPointY = (int)TOOL_STORE_COORDINATES.y;
                        }
                        else
                        {
                            agents[index].routeEndPointX = agents[index].startX;
                            agents[index].routeEndPointY = agents[index].startY;
                        }
                    }
                    else if (currentPosition == WOOD_STORE_COORDINATES)
                    {
                        // Keeping previous visited booleans for each agent
                        agents[index].agentVisitedBuildings["woodStore"] = true;
                        agents[index].agentVisitedBuildings["bank"] = savedDataArray[index, 0];
                        agents[index].agentVisitedBuildings["toolStore"] = savedDataArray[index, 2];
                        savedDataArray[index, 1] = true;

                        print(messageCount + ". Agent " + index + " arrived at the woodStore!\nHis health is " + agents[index].energyPoints + ".");
                        messageCount++;

                        if (agents[index].agentVisitedBuildings["bank"] == false)
                        {
                            agents[index].plan["bank"] = BANK_COORDINATES;
                            agents[index].routeEndPointX = (int)BANK_COORDINATES.x;
                            agents[index].routeEndPointY = (int)BANK_COORDINATES.y;
                        }
                        else if (agents[index].agentVisitedBuildings["toolStore"] == false)
                        {
                            agents[index].plan["toolStore"] = TOOL_STORE_COORDINATES;
                            agents[index].routeEndPointX = (int)TOOL_STORE_COORDINATES.x;
                            agents[index].routeEndPointY = (int)TOOL_STORE_COORDINATES.y;
                        }
                        else
                        {
                            agents[index].routeEndPointX = agents[index].startX;
                            agents[index].routeEndPointY = agents[index].startY;
                        }
                    }
                    else if (currentPosition == TOOL_STORE_COORDINATES)
                    {
                        // Keeping previous visited booleans for each agent
                        agents[index].agentVisitedBuildings["toolStore"] = true;
                        agents[index].agentVisitedBuildings["woodStore"] = savedDataArray[index, 1];
                        agents[index].agentVisitedBuildings["bank"] = savedDataArray[index, 0];
                        savedDataArray[index, 2] = true;

                        print(messageCount + ". Agent " + index + " arrived at the toolStore!\nHis health is " + agents[index].energyPoints + ".");
                        messageCount++;

                        if (agents[index].agentVisitedBuildings["bank"] == false)
                        {
                            agents[index].plan["bank"] = BANK_COORDINATES;
                            agents[index].routeEndPointX = (int)BANK_COORDINATES.x;
                            agents[index].routeEndPointY = (int)BANK_COORDINATES.y;
                        }
                        else if (agents[index].agentVisitedBuildings["woodStore"] == false)
                        {
                            agents[index].plan["woodStore"] = WOOD_STORE_COORDINATES;
                            agents[index].routeEndPointX = (int)WOOD_STORE_COORDINATES.x;
                            agents[index].routeEndPointY = (int)WOOD_STORE_COORDINATES.y;
                        }
                        else
                        {
                            agents[index].routeEndPointX = agents[index].startX;
                            agents[index].routeEndPointY = agents[index].startY;
                        }
                    }
                    else if (currentPosition == new Vector2(agents[index].startX, agents[index].startY))
                    {
                        // Keeping previous visited booleans for each agent
                        agents[index].agentVisitedBuildings["bank"] = savedDataArray[index, 0];
                        agents[index].agentVisitedBuildings["woodStore"] = savedDataArray[index, 1];
                        agents[index].agentVisitedBuildings["toolStore"] = savedDataArray[index, 2];

                        if (agents[index].agentVisitedBuildings["bank"] == true && agents[index].agentVisitedBuildings["woodStore"] == true && agents[index].agentVisitedBuildings["toolStore"] == true)
                        {
                            print(messageCount + ". Agent " + index + " returned home. His health is " + agents[index].energyPoints + ".\nPlan executed!");
                            messageCount++;

                            agents[index].executedThePlan = true;
                            numberOfAgentsFinished++;
                        }
                    }
                    //else
                    //{
                    //    print(messageCount + ". Agent " + index + " is in position: " + currentPosition);
                    //    messageCount++;
                    //}


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
                        print(messageCount + ". All agents completed their tasks");
                        messageCount++;
                    }
                }
            }


            //
            // Calling pathfinding when each agents has finished his trip
            //
            if (index == numberOfAgents - 1)
            {
                index = 0;
                moveAgents = false;
                GridManager.Instance.findDistance = true;
            }
            else
            {
                index++;
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

                //print(messageCount + ". Agent " + agent.id + " is at "+ myAgent.transform.position);
                //messageCount++;

                //
                // Move agent to next tile
                //
                target = new Vector3(tile.x, tile.y + constant, 0);
                //print(messageCount + ". Agent " + agent.id + " will move to (" + tile.x + ", " + tile.y +")");

                while (myAgent.transform.position != target)
                {
                    myAgent.transform.position = target;


                    //print(messageCount + ". Agent " + agent.id + " moved to " + myAgent.transform.position);
                    //messageCount++;

                    healthPoints[agent.id]--;
                    agent.energyPoints = healthPoints[agent.id];

                    steps[agent.id, trip]++;
                }

                //
                // Found tile with gold or energy pot
                //
                if (tile.OccupiedUnit != null)
                {
                    float tempX = target.x;
                    float tempY = target.y - constant;
                    Vector2 tempVector = new(tempX, tempY);
                    // Found energy pot
                    // Keep it if you have less than three on you
                    if (UnitManager.Instance.energyPotTiles.Contains(tile) && agent.numberOfEnergyPots < 2)
                    {
                        energyPots[agent.id]++;
                        agent.numberOfEnergyPots = energyPots[agent.id];
                        UnitManager.Instance.energyPotTiles.Remove(tile);

                        print(messageCount + ". Agent " + agent.id + " found an energy pot! Now he has " + agent.numberOfEnergyPots + "\nHis health is " + agent.energyPoints + ".");
                        messageCount++;
                    }
                    // Found gold coin
                    // Keep it if you have less than five on you
                    else if (UnitManager.Instance.goldTiles.Contains(tile) && agent.numberOfCoins < 5)
                    {
                        goldCoins[agent.id]++;
                        agent.numberOfCoins = goldCoins[agent.id];
                        UnitManager.Instance.energyPotTiles.Remove(tile);

                        print(messageCount + ". Agent " + agent.id + " found a gold coin! Now he has " + agent.numberOfCoins + ".");
                        messageCount++;
                    }
                }


                if (agent.energyPoints <= 25 && agent.energyPoints > 0)
                {
                    if (agent.numberOfEnergyPots == 0)
                    {
                        print(messageCount + ". Agent's " + agent.id + " health is deteriorating dramatically.");
                        messageCount++;
                    }

                    while (agent.numberOfEnergyPots > 0)
                    {
                        //Consume an energy pot
                        energyPots[agent.id]--;
                        agent.numberOfEnergyPots = energyPots[agent.id];

                        healthPoints[agent.id] += 20;
                        agent.energyPoints = healthPoints[agent.id];
                        
                        print(messageCount + ". Agent's " + agent.id + " health was " + (agent.energyPoints - 20) + ". He consumed one energy pot and now his health is " + agent.energyPoints + ".");
                        messageCount++;

                        if (agent.energyPoints > 80)
                        {
                            break;
                        }
                    }
                }
                else if (agent.energyPoints == 0)
                {
                    print(messageCount + ". Agent " + agent.id + " died!");
                    messageCount++;

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
