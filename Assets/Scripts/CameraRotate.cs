using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour {

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

    void OnDrawGizmos() {
        if (!DirectionalTransformToUse) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + 5 * DirectionalTransformToUse.forward);
        Gizmos.DrawWireSphere(transform.position + 5 * DirectionalTransformToUse.forward, 0.25f);
        var right = DirectionalTransformToUse.right;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + 5 * right);
        Gizmos.DrawWireSphere(transform.position + 5 * right, 0.25f);
        var forward = Vector3.Cross(gravityVulnerable.CurrentDown, right).normalized;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + 5 * forward);
        Gizmos.DrawWireSphere(transform.position + 5 * forward, 0.25f);
    }
}
