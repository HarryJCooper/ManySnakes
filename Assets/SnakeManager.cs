using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Events;

public class SnakeManager : NetworkBehaviour
{
    public GameObject snakePrefab;
    public GameObject segmentPrefab;
    public List<GameObject> snakes = new List<GameObject>();

    public void AddSegment(GameObject snake)
    {
        Debug.Log("AddSegment: " + snake.GetComponent<Snake>().clientId);
        GameObject spawnedSegment = Instantiate(segmentPrefab);
        segmentPrefab.GetComponent<NetworkObject>().Spawn();
        snake.GetComponent<Snake>().segments.Add(spawnedSegment);
    }

    [ServerRpc (RequireOwnership = false)]
    public void CreateSnakeServerRpc(ulong clientId)
    {
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
