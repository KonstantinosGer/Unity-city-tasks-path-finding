using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePrefabs : MonoBehaviour
{
    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    [SerializeField] private GameObject agent;
    
    private float constant = 0.35f;

    // This script will simply instantiate the Prefab when the game starts.
    void Start()
    {
        Instantiate(agent, new Vector3(0, (float)(0 + constant), 0), Quaternion.identity);
    }
}
