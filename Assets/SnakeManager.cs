using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SnakeManager : NetworkBehaviour
{
    public GameObject snakePrefab;
    public GameObject segmentPrefab;
    public List<GameObject> snakes = new List<GameObject>();

    [ServerRpc (RequireOwnership = false)]
    public void AddSegmentServerRpc(ulong clientId)
    {
        Debug.Log("AddSegmentServerRpc: " + clientId);
        foreach (GameObject snake in snakes){
            if (snake.GetComponent<Snake>().clientId == clientId){
                Debug.Log("Spawning segment: " + clientId);
                GameObject spawnedSegment = Instantiate(segmentPrefab);
                segmentPrefab.GetComponent<NetworkObject>().Spawn();
                snake.GetComponent<Snake>().segments.Add(spawnedSegment);
            }
        }
    }

    [ServerRpc (RequireOwnership = false)]
    public void CreateSnakeServerRpc(ulong clientId)
    {
        GameObject spawnedSnake = Instantiate(snakePrefab, this.transform);
        spawnedSnake.GetComponent<NetworkObject>().Spawn();
        spawnedSnake.GetComponent<Snake>().clientId = clientId;
        snakes.Add(spawnedSnake);
        Debug.Log("CreateSnakeServerRpc: " + clientId);
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

    // [ServerRpc (RequireOwnership = false)]
    // void MoveSegmentServerRpc()
    // {
    //     timer = 0;
    //     hasMoved = false;

    //     for (int i = 0; i < segmentPositions.Count; i++){
    //         segmentPositions[i] = segments[i].transform.position;
    //     }

    //     switch (segments[0].GetComponent<Segment>().direction)
    //     {
    //         case Direction.Forward:
    //             segments[0].transform.Translate(Vector3.forward);
    //             break;
    //         case Direction.Right:
    //             segments[0].transform.Translate(Vector2.right);
    //             break;
    //         case Direction.Back:
    //             segments[0].transform.Translate(Vector3.back);
    //             break;
    //         case Direction.Left:
    //             segments[0].transform.Translate(Vector2.left);
    //             break;
    //     }

    //     for (int i = 1; i < segments.Count; i++){
    //         segments[i].transform.position = segmentPositions[i - 1];
    //     }
    // }

    // void FixedUpdate()
    // {
    //     timer += 1;
    //     if (timer >= snakeSpeed) MoveSegmentServerRpc();
    // }
}
