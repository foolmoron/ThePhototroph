using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start : MonoBehaviour {

    float timer;
    
    void Awake() {
    }

    void Update() {
        timer += Time.deltaTime;
        if (timer > 2 && Input.anyKeyDown) {
            Destroy(gameObject);
        } else if (timer > 10) {
            Destroy(gameObject);
        }
    }
}
