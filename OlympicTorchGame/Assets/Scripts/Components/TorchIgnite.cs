﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchIgnite : MonoBehaviour
{
    public GameObject torch;
    public GameObject lighter;

    private void Start()
    {
        torch = GameObject.Find("Olympic Torch");
        lighter = GameObject.Find("Lighting Point");
    }

    private void Update()
    {
        
    }
}
