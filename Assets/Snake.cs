using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Snake : NetworkBehaviour
{
    public int snakeSpeed = 20;
    public int snakeSpeedIncrease = 1;
    public float timer = 0;
    public bool hasMoved;
    public ulong clientId;
    public List<GameObject> segments = new List<GameObject>();
    public List<Vector3> segmentPositions = new List<Vector3>(1);
}
