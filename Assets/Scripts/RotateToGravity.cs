using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToGravity : MonoBehaviour {

    [Range(0, 0.1f)]
    public float RotateSpeed = 0.01f;

    GravityVulnerable gravityVulnerable;
    KeyboardControl keyboardControl;
    Rigidbody rigidbody;

    Vector3 prevVelocity = Vector3.forward;

    void Start() {
        gravityVulnerable = GetComponent<GravityVulnerable>();
        keyboardControl = GetComponent<KeyboardControl>();
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        if (rigidbody.velocity != Vector3.zero) {
            prevVelocity = rigidbody.velocity;
        }
        var rotation = Quaternion.LookRotation(prevVelocity, -gravityVulnerable.CurrentDown);
        rotation = Quaternion.Slerp(transform.rotation, rotation, RotateSpeed);
        transform.rotation = rotation;
    }
}
