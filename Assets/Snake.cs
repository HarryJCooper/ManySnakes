using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Snake : NetworkBehaviour
{
    // public int snakeSpeed;
    // public int snakeSpeedIncrease;    
    // float timer = 0;
    // bool hasMoved;
    public ulong clientId;
    
    // void Start()
    // {
    //     if (!globalAssets) globalAssets = GameObject.Find("GlobalAssets").GetComponent<GlobalAssets>();

    //     segments.Add(GetComponent<Segment>());
    //     snakeSpeed = 20;
    //     snakeSpeedIncrease = 1;
    //     segments[0].direction = Direction.Forward;
    // }

    // void Controls()
    // {
    //     if (hasMoved) return;
    //     if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && segments[0].direction != Direction.Back && segments[0].direction != Direction.Forward){
    //         segments[0].direction = Direction.Forward;
    //         hasMoved = true;
    //     } else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && segments[0].direction != Direction.Left && segments[0].direction != Direction.Right){
    //         segments[0].direction = Direction.Right;
    //         hasMoved = true;
    //     } else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && segments[0].direction != Direction.Forward && segments[0].direction != Direction.Back){
    //         segments[0].direction = Direction.Back;
    //         hasMoved = true;
    //     } else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && segments[0].direction != Direction.Right && segments[0].direction != Direction.Left){
    //         segments[0].direction = Direction.Left;
    //         hasMoved = true;
    //     }
    // }

    // void MoveSegment()
    // {
    //     timer = 0;
    //     hasMoved = false;

    //     for (int i = 0; i < segmentPositions.Length; i++){
    //         if (!segments[i].gameObject.activeInHierarchy) break;
    //         segmentPositions[i] = segments[i].transform.position;
    //     }

    //     switch (segments[0].direction)
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
    //         if (!segments[i].gameObject.activeInHierarchy) break;
    //         segments[i].transform.position = segmentPositions[i - 1];
    //     }
    // }

    // void Update()
    // {
    //     if (IsOwner) Controls();
    // }

    // void FixedUpdate()
    // {
    //     timer += 1;
    //     if (timer >= snakeSpeed) MoveSegment();
    // }
}
