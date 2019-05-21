using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtScript : MonoBehaviour {
    public Transform target;

    void Update() {
        if (target != null) {
            transform.rotation =
                Quaternion.LookRotation(target.position - transform.position);
        }
    }
}
