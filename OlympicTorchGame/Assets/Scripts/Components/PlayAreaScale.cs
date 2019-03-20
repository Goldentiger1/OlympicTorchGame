using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAreaScale : MonoBehaviour 
{
    private GameObject Height;
    private GameObject Width;
    private GameObject Length;
    public bool LockArea = false;
    public Vector3 PlayAreaSize;
    public List<GameObject> SizeObjects = new List<GameObject>();

    private void Awake()
    {
        Height = GameObject.Find("Height");
        Width = GameObject.Find("Width");
        Length = GameObject.Find("Length");
        SizeObjects.Add(Height);
        SizeObjects.Add(Width);
        SizeObjects.Add(Length);
    }

    private void Update()
    {
        Vector3 A = SizeObjects[0].transform.GetChild(0).position;
        Vector3 B = SizeObjects[0].transform.GetChild(1).position;
    }
}