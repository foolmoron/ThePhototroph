using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityVulnerable : MonoBehaviour {

    public Rigidbody RigidbodyToUse;
    public float GravityModifier = 1;

    public GravityObject CurrentNearestGravityObject;

    void Start() {
        if (!RigidbodyToUse) {
            RigidbodyToUse = GetComponent<Rigidbody>();
        }
    }

    void Update() {
        CurrentNearestGravityObject = null;
        var nearestR2 = float.PositiveInfinity;
        foreach (var gravityObject in GravityObject.AllInCurrentScene) {
            // add gravity
            var vectorToGravity = gravityObject.transform.position - RigidbodyToUse.position;
            var dir = vectorToGravity.normalized;
            var r2 = vectorToGravity.sqrMagnitude;
            var a = GravityModifier * gravityObject.GravitationalMass / r2;
            var radiusScale = Mathf.Clamp01(r2 - (gravityObject.MinRadius * gravityObject.MinRadius));
            RigidbodyToUse.AddForce(a * radiusScale * dir, ForceMode.Acceleration);

            // check for nearest object
            if (r2 < nearestR2) {
                CurrentNearestGravityObject = gravityObject;
                nearestR2 = r2;
            }
        }
    }
}
