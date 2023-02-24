using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    public Direction direction;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Snake")
        {
            if (other.gameObject.transform.parent.GetComponent<Snake>() == this.gameObject.transform.parent.GetComponent<Snake>()){
                Debug.Log("You hit yourself");
                return;
            } else {
                Debug.Log("You hit another snake");
            }
        }
    }
}
