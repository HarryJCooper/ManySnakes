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
    public Vector3[] segmentPositions = new Vector3[1];
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
        snakeVals.snakeSpeed = 20;
        snakeVals.snakeSpeedIncrease = 1;
        snakeVals.segments[0].direction = Direction.Forward;
    }

    void Controls()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && snakeVals.segments[0].direction != Direction.Back){
            snakeVals.segments[0].direction = Direction.Forward;
        } else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && snakeVals.segments[0].direction != Direction.Left){
            snakeVals.segments[0].direction = Direction.Right;
        } else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && snakeVals.segments[0].direction != Direction.Forward){
            snakeVals.segments[0].direction = Direction.Back;
        } else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && snakeVals.segments[0].direction != Direction.Right){
            snakeVals.segments[0].direction = Direction.Left;
        }
    }

    void MoveSegment()
    {
        timer = 0;
        for (int i = 0; i < snakeVals.segmentPositions.Length; i++){
            snakeVals.segmentPositions[i] = snakeVals.segments[i].transform.position;
        }

        switch (snakeVals.segments[0].direction)
        {
            case Direction.Forward:
                snakeVals.segments[0].transform.Translate(Vector3.forward);
                break;
            case Direction.Right:
                snakeVals.segments[0].transform.Translate(Vector2.right);
                break;
            case Direction.Back:
                snakeVals.segments[0].transform.Translate(Vector3.back);
                break;
            case Direction.Left:
                snakeVals.segments[0].transform.Translate(Vector2.left);
                break;
        }

        for (int i = 1; i < snakeVals.segments.Count; i++){
            snakeVals.segments[i].transform.position = snakeVals.segmentPositions[i - 1];
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
