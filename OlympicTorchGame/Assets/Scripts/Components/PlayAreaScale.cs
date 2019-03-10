using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAreaScale : MonoBehaviour 
{
    private GameObject PlayAreaTop;
    private GameObject PlayAreaBottom;
    private GameObject PlayAreaPointA;
    private GameObject PlayAreaPointB;
    public Transform PlayAreaHeight;
    public Transform PlayAreaWidth;

    private void Awake()
    {
        PlayAreaTop = GameObject.Find("PlayAreaTop");
        PlayAreaBottom = GameObject.Find("PlayAreaBottom");

    }
}