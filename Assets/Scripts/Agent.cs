using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    //public static Agent Instance;

    public int id;
    public int energyPoints;
    public int numberOfCoins;
    public int numberOfEnergyPots;
    public int startX, startY;
    public int routeStartPointX,routeStartPointY;
    public int routeEndPointX,routeEndPointY;

    //agents' plan
    public Dictionary<string, Vector2> plan;

    public Agent(int id, int energyPoints, int numberOfCoins, int numberOfEnergyPots, int startX, int startY, int routeStartPointX, int routeStartPointY, int routeEndPointX, int routeEndPointY)
    {
        this.id = id;
        this.energyPoints = energyPoints;
        this.numberOfCoins = numberOfCoins;
        this.numberOfEnergyPots = numberOfEnergyPots;
        this.startX = startX;
        this.startY = startY;
        this.routeStartPointX = routeStartPointX;
        this.routeStartPointY = routeStartPointY;
        this.routeEndPointX = routeEndPointX;
        this.routeEndPointY = routeEndPointY;
    }

    void Start()
    {
        energyPoints = 100;
        numberOfCoins = 0;
        numberOfEnergyPots = 0;

        plan = new Dictionary<string, Vector2>();
        plan["bank"] = new Vector2(53,82);
        plan["woodStore"] = new Vector2(0,0);
        plan["toolStore"] = new Vector2(0,0);
    }

    void Update()
    {
        
    }
}
