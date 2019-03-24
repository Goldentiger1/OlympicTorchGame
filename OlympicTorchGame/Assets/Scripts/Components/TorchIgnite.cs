using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchIgnite : MonoBehaviour
{
    public GameObject torch;
    public GameObject lighter;
    public Color newColor;
    public Renderer rend;

    private void Start()
    {
        torch = GameObject.Find("Olympic Torch");
        lighter = GameObject.Find("Lighting Point");
    }

    private void Update()
    {
        newColor = Color.green;
        rend = lighter.GetComponentInChildren<Renderer>();
    }

    public void LightingOlympicFire()
    {
        rend.material.shader 
        
    }

}
