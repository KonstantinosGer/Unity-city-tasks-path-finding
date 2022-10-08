using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Buildings : MonoBehaviour
{
    [SerializeField] public GameObject bank;
    [SerializeField] public GameObject woodStore;
    [SerializeField] public GameObject toolStore;

    void Start()
    {
        Instantiate(bank, new Vector3(80, 5, 0), Quaternion.identity);
        Instantiate(woodStore, new Vector3(10, 50, 0), Quaternion.identity);
        Instantiate(toolStore, new Vector3(40, 50, 0), Quaternion.identity);
    }
}
