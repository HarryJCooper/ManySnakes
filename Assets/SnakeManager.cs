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
    int segmentCount;

    public List<GameObject> snakes = new List<GameObject>();

    public void Start()
    {
        if (IsServer) Debug.Log("SnakeManager is server");
        if (IsHost) Debug.Log("SnakeManager is host");
        if (IsClient) Debug.Log("SnakeManager is client");
    }

    public void AddSegment(GameObject snake)
    {
        if (!IsServer) return;
        Debug.Log("AddSegment: " + snake.GetComponent<Snake>().clientId);
        snake.GetComponent<Snake>().segments.Add(segmentArray[segmentCount]);
        snake.GetComponent<Snake>().segmentPositions.Add(mouse.transform.position);
        segmentCount++;
    }

    [ServerRpc (RequireOwnership = false)]
    public void CreateSnakeServerRpc(ulong clientId)
    {
        foreach (GameObject snk in snakes){
            if (snk.GetComponent<Snake>().clientId == clientId){
                Debug.Log("Snake already exists: " + clientId);
                return;
            }
        }
        GameObject spawnedSnake = Instantiate(snakePrefab, this.transform);
        Snake snake = spawnedSnake.GetComponent<Snake>(); 
        spawnedSnake.GetComponent<NetworkObject>().Spawn();
        snake.clientId = clientId;
        snake.snakeSpeed = 30;
        snakes.Add(spawnedSnake);
        snake.segments.Add(spawnedSnake);
        snake.segmentPositions.Add(spawnedSnake.transform.position);
        Debug.Log("CreatedSnake: " + clientId);
    }

    [ServerRpc (RequireOwnership = false)]
    public void DirectSnakeServerRpc(Direction direction, ulong clientId)
    {
        Debug.Log("DirectSnakeServerRpc: " + clientId + " " + direction);
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
                snake.segments[0].transform.Translate(Vector3.forward);
                break;
            case Direction.Right:
                snake.segments[0].transform.Translate(Vector2.right);
                break;
            case Direction.Back:
                snake.segments[0].transform.Translate(Vector3.back);
                break;
            case Direction.Left:
                snake.segments[0].transform.Translate(Vector2.left);
                break;
        }

        for (int i = 1; i < snake.segments.Count; i++){
            snake.segments[i].transform.position = snake.segmentPositions[i - 1];
        }

        if (snake.segments[snake.segments.Count - 1].transform.position == mouse.transform.position){
            if (!IsServer) return;
            AddSegment(snake.gameObject);
            mouse.transform.position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Debug.Log("Snake: " + snake.clientId + " ate mouse");
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
            if (snake.timer >= snake.snakeSpeed) MoveSegment(snake);
        }
    }
}
