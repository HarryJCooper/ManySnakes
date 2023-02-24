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
            beenEaten = true;
        }
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

    void AddSegment()
    {
        GameObject segment = Instantiate(
            snake.tailPrefab, 
            this.transform.position,
            this.transform.rotation
        );
        segment.transform.parent = snake.transform;
        segment.GetComponent<Segment>().direction = segments[segments.Count - 1].direction;
        snake.snakeVals.segments.Add(segment.GetComponent<Segment>());
        snake.snakeVals.segmentPositions = new Vector3[snake.snakeVals.segments.Count];
        foreach (Segment seg in snake.snakeVals.segments){
            snake.snakeVals.segmentPositions[snake.snakeVals.segments.IndexOf(seg)] = seg.transform.position;
        }

        this.transform.position = new Vector3(
            Random.Range(-10, 10),
            0,
            Random.Range(-10, 10)
        );

        snake.snakeVals.snakeSpeed -= snake.snakeVals.snakeSpeedIncrease;
    }
}
