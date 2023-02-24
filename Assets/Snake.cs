using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Forward, Right, Back, Left
}

public class SnakeVals
{
    public int playerNumber;
    public int snakeSpeed;
    public int snakeSpeedIncrease;
    public List<Segment> segments = new List<Segment>();
    public List<(float, float, Direction)> turnPositions = new List<(float, float, Direction)>();
}

public class Snake : MonoBehaviour
{
    public GameObject tailPrefab;
    public GlobalAssets globalAssets;
    // this would get passed to the server, and then the server would send it to the other clients
    public SnakeVals snakeVals = new SnakeVals();
    float timer = 0;
    
    void Start()
    {
        if (!globalAssets) globalAssets = GameObject.Find("GlobalAssets").GetComponent<GlobalAssets>();

        snakeVals.playerNumber = globalAssets.snakeVals.Count;
        snakeVals.segments.Add(transform.GetChild(0).gameObject.GetComponent<Segment>());
        snakeVals.snakeSpeed = 60;
        snakeVals.snakeSpeedIncrease = -1;
        snakeVals.segments[0].direction = Direction.Forward;
    }

    void Controls()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && snakeVals.segments[0].direction != Direction.Back){
            snakeVals.segments[0].direction = Direction.Forward;
            snakeVals.turnPositions.Add((snakeVals.segments[0].transform.position.x, snakeVals.segments[0].transform.position.z, snakeVals.segments[0].direction));
        } else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && snakeVals.segments[0].direction != Direction.Left){
            snakeVals.segments[0].direction = Direction.Right;
            snakeVals.turnPositions.Add((snakeVals.segments[0].transform.position.x, snakeVals.segments[0].transform.position.z, snakeVals.segments[0].direction));
        } else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && snakeVals.segments[0].direction != Direction.Forward){
            snakeVals.segments[0].direction = Direction.Back;
            snakeVals.turnPositions.Add((snakeVals.segments[0].transform.position.x, snakeVals.segments[0].transform.position.z, snakeVals.segments[0].direction));
        } else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && snakeVals.segments[0].direction != Direction.Right){
            snakeVals.segments[0].direction = Direction.Left;
            snakeVals.turnPositions.Add((snakeVals.segments[0].transform.position.x, snakeVals.segments[0].transform.position.z, snakeVals.segments[0].direction));
        }
    }

    void MoveSegment()
    {
        timer = 0;
        foreach (Segment segment in snakeVals.segments){
            switch (segment.direction)
            {
                case Direction.Forward:
                    segment.transform.Translate(Vector3.forward);
                    break;
                case Direction.Right:
                    segment.transform.Translate(Vector2.right);
                    break;
                case Direction.Back:
                    segment.transform.Translate(Vector3.back);
                    break;
                case Direction.Left:
                    segment.transform.Translate(Vector2.left);
                    break;
            }

            if (snakeVals.turnPositions.Count > 0) ChangeSegmentDirection(snakeVals.turnPositions, segment);
        }
    }

    void ChangeSegmentDirection(List<(float, float, Direction)> turnPositions, Segment segment)
    {
        foreach ((float, float, Direction) turnPosition in turnPositions){
            if (turnPosition.Item1 == segment.transform.position.x && turnPosition.Item2 == segment.transform.position.z){
                segment.direction = turnPosition.Item3;
            }
        }
    }

    void Update()
    {
        Controls();
    }

    void FixedUpdate()
    {
        timer += 1;
        if (timer >= snakeVals.snakeSpeed) MoveSegment();
    }
}
