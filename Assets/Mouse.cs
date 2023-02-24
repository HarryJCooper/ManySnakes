using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    Snake snake;
    List<Segment> segments;
    bool beenEaten;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Snake")
        {
            snake = other.transform.parent.GetComponent<Snake>();
            SnakeVals snakeVals = snake.snakeVals;
            segments = snakeVals.segments;
            Debug.Log("Snake ate the mouse");
            beenEaten = true;
        }
    }

    void AddSegment()
    {
        Debug.Log(1);
        GameObject segment = Instantiate(
            snake.tailPrefab, 
            this.transform.position,
            this.transform.rotation
        );
        Debug.Log(2);
        segment.transform.parent = snake.transform;
        segment.GetComponent<Segment>().direction = segments[segments.Count - 1].direction;
        snake.snakeVals.segments.Add(segment.GetComponent<Segment>());
        Destroy(this.gameObject);
    }
    
    void Update()
    {
        if (!beenEaten) return;
        foreach (Segment segment in segments){
            if (segment.transform.position == this.transform.position){
                return;
            }
        }
        beenEaten = false;
        AddSegment();
    }
}
