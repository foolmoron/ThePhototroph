﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControl : MonoBehaviour {

    public Transform DirectionalTransformToUse;
    public Rigidbody RigidbodyToUse;
    public float Acceleration = 5;
    public float MaxSpeed = 20;
    public float BaseJumpForce = 10;
    public float JumpGravityDisableTime = 1;
    float gravityDisableTime;

    public float GravityDiveHoldTime = 0.5f;
    float gravityDiveHoldTime;
    public float DiveSpeed = 5;
    public bool IsDiving;

    Phototroph phototroph;
    GravityVulnerable gravityVulnerable;
    SphereCollider collider;
    CameraEffects cameraEffects;
    
    void Start() {
        if (!DirectionalTransformToUse) {
            DirectionalTransformToUse = Camera.main.transform;
        }
        if (!RigidbodyToUse) {
            RigidbodyToUse = GetComponent<Rigidbody>();
        }
        phototroph = GetComponent<Phototroph>();
        gravityVulnerable = GetComponent<GravityVulnerable>();
        collider = GetComponent<SphereCollider>();
        cameraEffects = GetComponentInChildren<CameraEffects>();
    }

    void FixedUpdate() {
        // gravity dive start
        if (Input.GetButton("Jump")) {
            gravityDiveHoldTime += Time.deltaTime;
            if (gravityDiveHoldTime >= GravityDiveHoldTime) {
                IsDiving = true;
            }
        }

        // movement force
        if (IsDiving) {
            //transform.rotation = Quaternion.Euler(new Vector3(1,2,3));
            RigidbodyToUse.velocity = DiveSpeed * Mathf.Min(10, gravityDiveHoldTime - GravityDiveHoldTime) * gravityVulnerable.CurrentDown;
        } else {
            var inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
            var right = DirectionalTransformToUse.right;
            var forward = Vector3.Cross(gravityVulnerable.CurrentDown, right).normalized;
            var direction = right * inputDirection.x + forward * inputDirection.y;
            // TODO: clamp speed properly?
            if (RigidbodyToUse.velocity.magnitude < MaxSpeed) {
                RigidbodyToUse.AddForce(direction * Acceleration, ForceMode.Acceleration);
            }
        }

        // jump
        if (phototroph.Orbs.Count > 0 && Input.GetButtonUp("Jump") && !IsDiving) {
            // TODO: cancel all momentum towards CurrentDown
            RigidbodyToUse.AddForce(-BaseJumpForce * gravityVulnerable.CurrentDown, ForceMode.VelocityChange);
            gravityDisableTime = JumpGravityDisableTime;
            phototroph.UseOrb();
        }

        // gravity disable
        if (gravityDisableTime > 0) {
            gravityDisableTime -= Time.deltaTime;
        }
        gravityVulnerable.GravityModifier = gravityDisableTime > 0 ? 0 : 1;
        
        // gravity dive end
        if (!Input.GetButton("Jump")) {
            IsDiving = false;
            gravityDiveHoldTime = 0;
        }

        // camera effects intensity
        cameraEffects.Intensity = gravityDiveHoldTime / 4;
    }

    void OnCollisionStay(Collision collision) {
        // TODO: change to raycast for planet
        if (collision.transform.GetComponent<Planet>()) {
            IsDiving = false;
            gravityDiveHoldTime = 0;
        }
    }
}
