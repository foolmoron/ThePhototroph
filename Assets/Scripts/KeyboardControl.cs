using System.Collections;
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

    Phototroph phototroph;
    GravityVulnerable gravityVulnerable;
    SphereCollider collider;
    
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
        if (phototroph.Orbs.Count > 0 && Input.GetButtonDown("Jump")) {
            RigidbodyToUse.AddForce(-BaseJumpForce * gravityVulnerable.CurrentDown, ForceMode.VelocityChange);
            gravityDisableTime = JumpGravityDisableTime;
            phototroph.UseOrb();
        }

        // gravity disable
        if (gravityDisableTime > 0) {
            gravityDisableTime -= Time.deltaTime;
        }
        gravityVulnerable.GravityModifier = gravityDisableTime > 0 ? 0 : 1;
    }
}
