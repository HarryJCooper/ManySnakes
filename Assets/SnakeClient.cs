using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SnakeClient : NetworkBehaviour
{
    public ulong clientId;
    public SnakeManager snakeManager; // make snakeManager a sigleton
    Direction currentDirection;
    bool hasSnake;

    void Start()
    {
        clientId = NetworkManager.Singleton.LocalClientId;

        Debug.Log("SnakeClient start: " + clientId);

        snakeManager = GameObject.Find("SnakeManager").GetComponent<SnakeManager>();

        currentDirection = Direction.Forward;
    }

    void Controls()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && currentDirection != Direction.Back && currentDirection != Direction.Forward){
            snakeManager.DirectSnakeServerRpc(Direction.Forward, clientId);
            Debug.Log("Controls: " + clientId);
            currentDirection = Direction.Forward;
        } else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && currentDirection != Direction.Left && currentDirection != Direction.Right){
            snakeManager.DirectSnakeServerRpc(Direction.Right, clientId);
            currentDirection = Direction.Right;
        } else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && currentDirection != Direction.Forward && currentDirection != Direction.Back){
            snakeManager.DirectSnakeServerRpc(Direction.Back, clientId);
            currentDirection = Direction.Back;
        } else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && currentDirection != Direction.Right && currentDirection != Direction.Left){
            snakeManager.DirectSnakeServerRpc(Direction.Left, clientId);
            currentDirection = Direction.Left;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !hasSnake){
            hasSnake = true;
            snakeManager.CreateSnakeServerRpc(clientId);
            snakeManager.AddSegmentServerRpc(clientId);
        }
    }

    

    void Update()
    {
        Controls();
    }
}
