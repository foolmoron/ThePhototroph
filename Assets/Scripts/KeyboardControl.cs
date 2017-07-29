﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControl : MonoBehaviour {

    public Transform DirectionalTransformToUse;
    public Rigidbody RigidbodyToUse;
    public float Acceleration = 5;
    public float MaxSpeed = 20;
    public float JumpForce = 10;

    GravityVulnerable gravityVulnerable;
    SphereCollider collider;
    
    void Start() {
        if (!DirectionalTransformToUse) {
            DirectionalTransformToUse = Camera.main.transform;
        }
        if (!RigidbodyToUse) {
            RigidbodyToUse = GetComponent<Rigidbody>();
        }
        gravityVulnerable = GetComponent<GravityVulnerable>();
        collider = GetComponent<SphereCollider>();
    }

    void FixedUpdate() {
        // movement force
        var inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        var right = DirectionalTransformToUse.right;
        var forward = Vector3.Cross(gravityVulnerable.CurrentDown, right).normalized;
        var direction = right * inputDirection.x + forward * inputDirection.y;
        if (RigidbodyToUse.velocity.magnitude < MaxSpeed) {
            RigidbodyToUse.AddForce(direction * Acceleration, ForceMode.Acceleration);
        }

        // jump
        var canJump = Physics.Raycast(transform.position, gravityVulnerable.CurrentDown, collider.radius * transform.lossyScale.x * 1.1f);
        if (canJump && Input.GetButtonDown("Jump")) {
            RigidbodyToUse.AddForce(-JumpForce * gravityVulnerable.CurrentDown, ForceMode.VelocityChange);
        }
    }
}
