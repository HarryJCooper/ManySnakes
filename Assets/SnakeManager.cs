using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SnakeManager : NetworkBehaviour
{
    public GameObject snakePrefab;
    public List<GameObject> snakes = new List<GameObject>();

    [ServerRpc (RequireOwnership = false)]
    public void AddSegmentServerRpc(ulong clientId)
    {
        
    }

    [ServerRpc (RequireOwnership = false)]
    public void CreateSnakeServerRpc(ulong clientId)
    {
        GameObject spawnedSnake = Instantiate(snakePrefab);
        spawnedSnake.GetComponent<NetworkObject>().Spawn();
        spawnedSnake.GetComponent<Snake>().clientId = clientId;
        snakes.Add(spawnedSnake);
        Debug.Log("CreateSnakeServerRpc: " + clientId);
    }

    [ServerRpc (RequireOwnership = false)]
    public void DirectSnakeServerRpc(Direction direction, ulong clientId)
    {
        Debug.Log("DirectSnakeServerRpc: " + clientId + " " + direction);
    }
}
