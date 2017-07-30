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
    public bool IsJumping;

    public float GravityMelodyDistanceMax = 50;

    public AudioClip JumpSound;
    public AudioClip LandSound;
    public AudioClip DiveSound;

    Phototroph phototroph;
    GravityVulnerable gravityVulnerable;
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
        cameraEffects = GetComponentInChildren<CameraEffects>();
    }

    void Update() {
        // gravity dive start
        if (Input.GetButton("Jump")) {
            gravityDiveHoldTime += Time.deltaTime;
            if (gravityDiveHoldTime >= GravityDiveHoldTime) {
                if (!IsDiving) {
                    AudioSource.PlayClipAtPoint(DiveSound, Vector3.zero);
                }
                IsDiving = true;
                IsJumping = false;
            }
        }

        // jump
        if (phototroph.Orbs.Count > 0 && Input.GetButtonUp("Jump") && !IsDiving) {
            var currentDownVelocity = Vector3.Project(RigidbodyToUse.velocity, gravityVulnerable.CurrentDown);
            RigidbodyToUse.AddForce(2*(-BaseJumpForce * gravityVulnerable.CurrentDown - currentDownVelocity), ForceMode.VelocityChange);
            gravityDisableTime = JumpGravityDisableTime;
            IsJumping = true;
            phototroph.UseOrb();
            AudioSource.PlayClipAtPoint(JumpSound, Vector3.zero);
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

        // audio
        var speed = RigidbodyToUse.velocity.magnitude;
        AudioManager.Inst.Drums.Current = speed > 30 ? 4 : speed > 15 ? 3 : speed > 7 ? 2 : speed > 2 ? 1 : 0;
        AudioManager.Inst.Synths.Current = IsDiving ? 2 : IsJumping ? 1 : 0;
        AudioManager.Inst.Melodies.Current = gravityVulnerable.CurrentNearestGravityObject.MelodyId;
        AudioManager.Inst.Melodies.MasterVolume = 1 - Mathf.Clamp01(gravityVulnerable.CurrentDistanceToGravity / GravityMelodyDistanceMax);
    }

    void FixedUpdate() {
        // movement force
        if (IsDiving) {
            RigidbodyToUse.velocity = DiveSpeed * Mathf.Min(10, gravityDiveHoldTime - GravityDiveHoldTime) * gravityVulnerable.CurrentDown;
        } else {
            var inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
            var right = DirectionalTransformToUse.right;
            var forward = Vector3.Cross(gravityVulnerable.CurrentDown, right).normalized;
            var direction = right * inputDirection.x + forward * inputDirection.y;
            if (RigidbodyToUse.velocity.magnitude < MaxSpeed) {
                RigidbodyToUse.AddForce(direction * Acceleration, ForceMode.Acceleration);
            }
        }
    }

    void OnCollisionStay(Collision collision) {
        // TODO: change to raycast for planet
        if (collision.transform.GetComponent<Planet>()) {
            IsJumping = false;
            IsDiving = false;
            gravityDiveHoldTime = 0;
            //AudioSource.PlayClipAtPoint(LandSound, Vector3.zero);
        }
    }
}
