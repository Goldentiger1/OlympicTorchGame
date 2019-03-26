using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OlympicFire : MonoBehaviour
{
    TorchIgnite flame;

    private void Start()
    {
        GameObject g = GameObject.Find("Olympic Torch");
        flame = g.GetComponent<TorchIgnite>();
    }
    
     void OnTriggerEnter(Collider other)
    {
        flame.LightingOlympicFire();
    }
    
}