using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;

public class SnakeManager : NetworkBehaviour
{
    public GameObject snakePrefab;
    public GameObject mouse;
    public GameObject[] segmentArray;
    public Color mouseColour;
    int segmentCount;
    float reduction;
    float scale;

    public List<GameObject> snakes = new List<GameObject>();

    void Start()
    {
        mouseColour = mouse.GetComponent<Renderer>().material.color;
    }

    public void AddSegment(GameObject snake)
    {
        if (!IsServer) return;
        snake.GetComponent<Snake>().segments.Add(segmentArray[segmentCount]);
        snake.GetComponent<Snake>().segmentPositions.Add(mouse.transform.position);
        segmentCount++;
        float snakeLength = (float)snake.GetComponent<Snake>().segments.Count;
        Debug.Log("SnakeLength: " + snakeLength);
        reduction = snakeLength / 100;
        Debug.Log("Reduction: " + reduction);
        scale = (1 - reduction);
        Debug.Log("Scale: " + scale);
        Debug.Log("SegmentCount: " + segmentCount);
        snake.GetComponent<Snake>().segments[segmentCount].transform.localScale = new Vector3(scale, scale, scale);
    }

    [ServerRpc (RequireOwnership = false)]
    public void CreateSnakeServerRpc(ulong clientId)
    {
        foreach (GameObject snk in snakes){
            if (snk.GetComponent<Snake>().clientId == clientId){
                return;
            }
        }
        GameObject spawnedSnake = Instantiate(snakePrefab, this.transform);
        Snake snake = spawnedSnake.GetComponent<Snake>(); 
        spawnedSnake.GetComponent<NetworkObject>().Spawn();
        snake.clientId = clientId;
        snake.snakeSpeed = 4;
        snakes.Add(spawnedSnake);
        snake.segments.Add(spawnedSnake);
        snake.segmentPositions.Add(spawnedSnake.transform.position);
    }

    [ServerRpc (RequireOwnership = false)]
    public void DirectSnakeServerRpc(Direction direction, ulong clientId)
    {
        foreach (GameObject snake in snakes){
            if (snake.GetComponent<Snake>().clientId == clientId){
                snake.GetComponent<Segment>().direction = direction;
            }
        }
    }

    void MoveSegment(Snake snake)
    {
        snake.timer = 0;
        snake.hasMoved = false;

        for (int i = 0; i < snake.segmentPositions.Count; i++){
            snake.segmentPositions[i] = snake.segments[i].transform.position;
        }

        switch (snake.segments[0].GetComponent<Segment>().direction)
        {
            case Direction.Forward:
                snake.segments[0].transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.Right:
                snake.segments[0].transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case Direction.Back:
                snake.segments[0].transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case Direction.Left:
                snake.segments[0].transform.rotation = Quaternion.Euler(0, -90, 0);
                break;
            case Direction.None:
                Debug.Log("Snake: " + snake.clientId + " is dead");
                snake.isDead = true;
                break;
        }
        if (!snake.isDead) snake.segments[0].transform.Translate((Vector3.forward * 0.2f));

        for (int i = 1; i < snake.segments.Count; i++){
            snake.segments[i].transform.position = snake.segmentPositions[i - 1];
        }

        CheckIfHit(snake);

        if (!IsServer) return;
        if (Vector3.Distance(snake.segments[0].transform.position, mouse.transform.position) < 0.35f){
            mouse.GetComponent<Renderer>().material.color = snake.GetComponent<Renderer>().material.color;
            // could animate mouth opening here
        }

        if (Vector3.Distance(snake.segments[snake.segments.Count - 1].transform.position, mouse.transform.position) < 0.35f){
            AddSegment(snake.gameObject);
            mouse.GetComponent<Renderer>().material.color = mouseColour;
            mouse.transform.position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        }
    }

    void CheckIfHit(Snake snake)
    {
        for (int i = 1; i < snake.segments.Count; i++){
            if (snake.segments[0].transform.position == snake.segments[i].transform.position){
                Debug.Log("Snake: " + snake.clientId + " hit itself");
                snake.segments[0].GetComponent<Segment>().direction = Direction.None;
            }
        }
        foreach (GameObject otherSnake in snakes){
            if (otherSnake == snake.gameObject) continue;
            for (int i = 0; i < otherSnake.GetComponent<Snake>().segments.Count; i++){
                if (snake.segments[0].transform.position == otherSnake.GetComponent<Snake>().segments[i].transform.position){
                    Debug.Log("Snake: " + snake.clientId + " hit other snake");
                    snake.segments[0].GetComponent<Segment>().direction = Direction.None;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (!IsServer) return;
        if (snakes.Count == 0) return;
        foreach(GameObject snakeObj in snakes){
            Snake snake = snakeObj.GetComponent<Snake>();
            if (snake.segments.Count == 0) continue;
            snake.timer += 1;
            if (snake.timer >= snake.snakeSpeed && !snake.isDead) MoveSegment(snake);
        }
    }
}
