using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Forward,
    Right,
    Back,
    Left
}

public struct SnakeVals
{
    public int playerNumber;
    public int snakeSpeed;
    public int snakeSpeedIncrease;
    public int snakeLength;
    public Direction snakeDirection;
    public List<(float, float)> snakePositions;
}

public class Snake : MonoBehaviour
{
    List<Transform> tail = new List<Transform>();
    public GameObject tailPrefab;
    public GlobalAssets globalAssets;
    // this would get passed to the server, and then the server would send it to the other clients
    public SnakeVals snakeVals;
    
    void Awake()
    {
        // globalSnake info loads in here
    }

    void Start()
    {
        if (!globalAssets) globalAssets = GameObject.Find("GlobalAssets").GetComponent<GlobalAssets>();

        snakeVals.playerNumber = globalAssets.snakeVals.Count;
        snakeVals.snakeSpeed = 1;
        snakeVals.snakeSpeedIncrease = 1;
        snakeVals.snakeLength = 1;
        snakeVals.snakeDirection = Direction.Forward;
        snakeVals.snakePositions.Add((this.transform.position.x, this.transform.position.y));
    }

    void Controls()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            snakeVals.snakeDirection = Direction.Forward;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            snakeVals.snakeDirection = Direction.Right;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            snakeVals.snakeDirection = Direction.Back;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            snakeVals.snakeDirection = Direction.Left;
        }
    }

    void MovePlayer()
    {
        switch (snakeVals.snakeDirection)
        {
            case Direction.Forward:
                transform.Translate(Vector3.forward * snakeVals.snakeSpeed * Time.deltaTime);
                break;
            case Direction.Right:
                transform.Translate(Vector2.right * snakeVals.snakeSpeed * Time.deltaTime);
                break;
            case Direction.Back:
                transform.Translate(Vector3.back * snakeVals.snakeSpeed * Time.deltaTime);
                break;
            case Direction.Left:
                transform.Translate(Vector2.left * snakeVals.snakeSpeed * Time.deltaTime);
                break;
        }
    }

    void Update()
    {
        Controls();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }
}
