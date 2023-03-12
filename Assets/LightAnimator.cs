using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAnimator : MonoBehaviour
{
    [SerializeField] Light light;

    void Update(){
        light.intensity = Mathf.Lerp(10, 20, Mathf.PingPong(Time.time, 1.5f));
    }
}
