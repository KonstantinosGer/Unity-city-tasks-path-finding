using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //////
    public GameObject player;
    [SerializeField] private float moveSpeed;
    //////
    public Vector3 target;

    // Start is called before the first frame update
    void Start()
    {

        //controller = GetComponent<CharacterController>();
        player = GameObject.Find("Sphere");

        // Start possition
        player.transform.position = new Vector3(0, 0, 0);

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

        while (player.transform.position != target)
            player.transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        //Target Reached
        Debug.Log(player.transform.position);
    }

}
