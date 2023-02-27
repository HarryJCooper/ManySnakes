// using System.Collections.Generic;
// using UnityEngine;
// using Unity.Netcode;

// public class Mouse : NetworkBehaviour
// {
//     Snake snake;
//     List<Segment> segments;
//     public SegmentContainer segmentContainer;
//     bool beenEaten;

//     void Start()
//     {
//         segmentContainer = GameObject.Find("SegmentContainer").GetComponent<SegmentContainer>();
//     }

//     void OnTriggerEnter(Collider other)
//     {
//         if (other.gameObject.tag == "Snake")
//         {
//             snake = other.GetComponent<Snake>();
//             segments = snake.segments;
//             beenEaten = true;
//         }
//     }
    
//     void Update()
//     {
//         if (!beenEaten) return;
//         foreach (Segment segment in segments){
//             if (segment.transform.position == this.transform.position){
//                 return;
//             }
//         }
//         beenEaten = false;
//         AddSegment();
//     }

//     void AddSegment()
//     {
//         // loop through our segment array and find the first segment that isn't in use
//         foreach(GameObject segmentObj in segmentContainer.segments){
//             Segment segment = segmentObj.GetComponent<Segment>();
//             if (segment.inUse) continue;
//             segment.inUse = true;
//             // set the direction of the segment to the direction of the last segment
//             segment.direction = segments[segments.Count - 1].direction;
//             // move it from it's storage position to the position of the mouse
//             segment.transform.position = this.transform.position;
//             snake.segments.Add(segment);
//             break;
//         }
        
//         snake.segmentPositions = new Vector3[snake.segments.Count];
        
//         foreach (Segment segment in snake.segments){
//             snake.segmentPositions[snake.segments.IndexOf(segment)] = segment.transform.position;

//         }

//         this.transform.position = new Vector3(
//             Random.Range(-10, 10),
//             0,
//             Random.Range(-10, 10)
//         );

//         snake.snakeSpeed -= snake.snakeSpeedIncrease;
//     }
// }
