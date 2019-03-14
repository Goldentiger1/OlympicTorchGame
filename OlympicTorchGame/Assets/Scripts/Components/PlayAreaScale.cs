using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAreaScale : MonoBehaviour 
{
    private GameObject Height;
    private GameObject Width;
    private GameObject Length;
    public Vector3 PlayAreaSize;

    private void Awake()
    {
        Height = GameObject.Find("Height");
        Width = GameObject.Find("Width");
        Length = GameObject.Find("Length");
    }

    private void Start()
    {

    }
}