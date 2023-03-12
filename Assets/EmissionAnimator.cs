using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionAnimator : MonoBehaviour
{
    [SerializeField] Material mat;

    void Update(){
        GetComponent<Renderer>().sharedMaterial.SetColor("_EmissionColor", Color.Lerp(Color.red, Color.blue, Mathf.PingPong(Time.time, 1)));
    }
}
