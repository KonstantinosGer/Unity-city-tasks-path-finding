using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Buildings : MonoBehaviour
{
    [SerializeField] private GameObject house;

    void Start()
    {
        // Instantiate at position (0, 0, 0) and zero rotation.
        Instantiate(house, new Vector3(11, 85, 0), Quaternion.identity);
        Instantiate(house, new Vector3(11, 50, 0), Quaternion.identity);
    }
}
