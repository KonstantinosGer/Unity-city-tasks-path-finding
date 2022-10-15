using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Agent:MonoBehaviour
{
    public static Agent Instance;

    public int id;
    public int energyPoints;
    public int numberOfCoins;
    public int numberOfEnergyPots;
    public int startX, startY;
    public int routeStartPointX,routeStartPointY;
    public int routeEndPointX,routeEndPointY;
    public GameObject assetPrefab;
    public bool hasDied;

    //agents' plan
    public Dictionary<string, Vector2> plan;
    public Dictionary<string, bool> agentVisitedBuildings;
    public bool executedThePlan;


    public void Awake()
    {
        Instance = this;

        agentVisitedBuildings = new Dictionary<string, bool>();
    }
}
