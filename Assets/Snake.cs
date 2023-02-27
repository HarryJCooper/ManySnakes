using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Snake : NetworkBehaviour
{
    public int snakeSpeed;
    public int snakeSpeedIncrease;    
    float timer = 0;
    bool hasMoved;
    public ulong clientId;
    public List<GameObject> segments = new List<GameObject>();
    public List<Vector3> segmentPositions = new List<Vector3>(1);
    
    void Start()
    {
        snakeSpeed = 20;
        snakeSpeedIncrease = 1;
        segments[0].GetComponent<Segment>().direction = Direction.Forward;
    }

    [ServerRpc (RequireOwnership = false)]
    void MoveSegmentServerRpc()
    {
        timer = 0;
        hasMoved = false;

        for (int i = 0; i < segmentPositions.Count; i++){
            segmentPositions[i] = segments[i].transform.position;
        }

        switch (segments[0].GetComponent<Segment>().direction)
        {
            case Direction.Forward:
                segments[0].transform.Translate(Vector3.forward);
                break;
            case Direction.Right:
                segments[0].transform.Translate(Vector2.right);
                break;
            case Direction.Back:
                segments[0].transform.Translate(Vector3.back);
                break;
            case Direction.Left:
                segments[0].transform.Translate(Vector2.left);
                break;
        }

        for (int i = 1; i < segments.Count; i++){
            segments[i].transform.position = segmentPositions[i - 1];
        }
    }

    void FixedUpdate()
    {
        timer += 1;
        if (timer >= snakeSpeed) MoveSegmentServerRpc();
    }
}
