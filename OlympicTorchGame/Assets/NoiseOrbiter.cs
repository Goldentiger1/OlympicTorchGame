using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseOrbiter : MonoBehaviour {
    public float radius;
    public float frequency = 1;
    
    void Update() {
        var t = Time.time / frequency;
        float noise = Mathf.PerlinNoise(t, t) * 2 - 1;
        var v = Vector3.forward * radius;
        var rot = Quaternion.Euler(0, noise * 180, 0);
        transform.localPosition = rot * v;
    }
}
