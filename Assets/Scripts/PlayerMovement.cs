using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public int numberOfAgents;
    [SerializeField] public GameObject agent;
    [SerializeField] public GameObject house;
    [SerializeField] public GameObject bank;
    [SerializeField] public GameObject woodStore;
    [SerializeField] public GameObject toolStore;
    [SerializeField] private float moveSpeed;

    public List<Tile> housesList = new();
    public Vector3 target;
    private float constant = 0.35f;


    // Start is called before the first frame update
    void Start()
    {

        //controller = GetComponent<CharacterController>();
        //player = GameObject.Find("Sphere");

        // Start possition
        //agent.transform.position = new Vector3(0, 0, 0);
        //house.transform.position= new Vector3(5, 0, 5);
        Instantiate(agent, new Vector3(0, (float)(0 + constant), 0), Quaternion.identity);
        Instantiate(house, new Vector3(5, 5, 0), Quaternion.identity);
        


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


    //public void MovePlayer2(GameObject element)
    //{
    //    //Target
    //    Debug.Log("X: " + element.GetComponent<GridStat>().x + ", Y: " + element.GetComponent<GridStat>().y);

    //    target = new Vector3(element.GetComponent<GridStat>().x, 0, element.GetComponent<GridStat>().y);

    //    while (player.transform.position != target)
    //        player.transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

    //    //Target Reached
    //    Debug.Log(player.transform.position);
    //}


    public void MovePlayer(GameObject element)
    {
        //Target
        Debug.Log("X: " + element.GetComponent<GridStat>().x + ", Y: " + element.GetComponent<GridStat>().y);

        target = new Vector3(element.GetComponent<GridStat>().x, 0, element.GetComponent<GridStat>().y);

        while (agent.transform.position != target)
            agent.transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        //Target Reached
        Debug.Log(agent.transform.position);
    }

}
