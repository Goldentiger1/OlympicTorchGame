using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchIgnite : MonoBehaviour
{
    public GameObject torch;
    public GameObject lighter;
    public Color newColor;

    private void Start()
    {
        torch = GameObject.Find("Olympic Torch");
        lighter = GameObject.Find("Lighting Point");
    }

    public void LightingOlympicFire()
    {

    }

}
