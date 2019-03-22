using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsAttack : MonoBehaviour
{
    public List<GameObject> EnemyBirds = new List<GameObject>();
    public List<GameObject> EnemySpawnpoints = new List<GameObject>();
    public GameObject AllyTorch;

    void Start()
    {
        AllyTorch = GameObject.Find("Olympic Torch"); 
    }
    void Update()
    {

    }
}
