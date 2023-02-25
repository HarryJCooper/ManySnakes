using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMouse : MonoBehaviour
{
    public GameObject mousePrefab;

    void Start()
    {
        GameObject.Instantiate(mousePrefab);
        mousePrefab.transform.position = new Vector3(
            Random.Range(-10, 10),
            0,
            Random.Range(-10, 10)
        );
    }
}
