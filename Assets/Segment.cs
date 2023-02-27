using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    public Direction direction;
    public bool inUse;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Snake" && inUse)
        {
            // if (other.gameObject.GetComponent<Snake>() == this.gameObject.transform.parent.GetComponent<Snake>()){
            //     Debug.Log("You hit yourself");
            //     return;
            // } else {
            //     Debug.Log("You hit another snake");
            // }
        }
    }

    void FixedUpdate()
    {
        if (!inUse) transform.position = new Vector3(0, 1000, 0);
    }
}
