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
    public Light[] lightArray;
    public Color mouseColour, snakeColour;
    int segmentCount;
    float reduction;
    float scale;

    public List<GameObject> snakes = new List<GameObject>();

    void Start()
    {
        mouseColour = mouse.GetComponent<Renderer>().sharedMaterial.color;
        snakeColour = snakePrefab.GetComponent<Renderer>().sharedMaterial.color;
    }

    public void AddSegment(GameObject snake)
    {
        if (!IsServer) return;
        snake.GetComponent<Snake>().segments.Add(segmentArray[segmentCount]);
        snake.GetComponent<Snake>().segmentPositions.Add(mouse.transform.position);
        segmentCount++;
        float snakeLength = (float)snake.GetComponent<Snake>().segments.Count;
        reduction = snakeLength / 100;
        scale = (1 - reduction);
        snake.GetComponent<Snake>().segments[segmentCount].transform.localScale = new Vector3(scale, scale, scale);
    }

    [ServerRpc (RequireOwnership = false)]
    public void CreateSnakeServerRpc(ulong clientId, Color snakeColour)
    {
        foreach (GameObject snk in snakes){
            if (snk.GetComponent<Snake>().clientId == clientId){
                return;
            }
        }
        GameObject spawnedSnake = Instantiate(snakePrefab, this.transform);
        Snake snake = spawnedSnake.GetComponent<Snake>();
        spawnedSnake.GetComponent<NetworkObject>().Spawn();
        snake.snakeColour = snakeColour;
        foreach(GameObject segment in snake.segments) segment.GetComponent<Renderer>().material.color = snake.snakeColour;
        snake.clientId = clientId;
        snake.snakeSpeed = 3;
        snakes.Add(spawnedSnake);
        snake.segments.Add(spawnedSnake);
        snake.segmentPositions.Add(spawnedSnake.transform.position);
        snake.snakeColour = snakeColour;
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

        if (Vector3.Distance(snake.segments[0].transform.position, mouse.transform.position) < 0.35f){
            mouse.GetComponent<Renderer>().material.color = snake.GetComponent<Renderer>().material.color;
            foreach(GameObject segment in snake.segments) segment.GetComponent<Renderer>().material.color = mouseColour;
            // could animate mouth opening here
        }

        if (Vector3.Distance(snake.segments[snake.segments.Count - 1].transform.position, mouse.transform.position) < 0.35f){
            mouse.GetComponent<Renderer>().material.color = mouseColour;
            foreach(GameObject segment in snake.segments) segment.GetComponent<Renderer>().material.color = snakeColour;
            mouse.transform.position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            StartCoroutine(Light(snake));
            AddSegment(snake.gameObject);
        }
    }

    IEnumerator Light(Snake snake)
    {
        foreach (Light light in lightArray){
            light.color = snake.snakeColour;
            yield return new WaitForSeconds(0.25f);
            light.color = Color.white;
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

    [ServerRpc (RequireOwnership = false)]
    public void ChangeSnakeColorServerRpc(Color color, ulong clientId)
    {
        Debug.Log("Changing snake color to: " + color + " for client: " + clientId);
        foreach (GameObject snake in snakes){
            Snake snk = snake.GetComponent<Snake>();
            if (snk.clientId == clientId){
                Debug.Log("TWO! Changing snake color to: " + color + " for client: " + clientId);
                snk.snakeColour = color;
                foreach(GameObject segment in snk.segments){
                    Debug.Log("THREE! Changing segment color to: " + color + " for client: " + clientId);
                    segment.GetComponent<Renderer>().sharedMaterial.SetColor("_EmissionColor", color);
                    segment.GetComponent<Renderer>().sharedMaterial.SetColor("_Albedo", color);
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
