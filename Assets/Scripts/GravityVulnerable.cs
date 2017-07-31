﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityVulnerable : MonoBehaviour {

    public Rigidbody RigidbodyToUse;
    public float GravityModifier = 1;

    public GravityObject CurrentNearestGravityObject;
    public Vector3 CurrentDown = Vector3.down;
    public float CurrentDistanceToGravity;

    void Start() {
        if (!RigidbodyToUse) {
            RigidbodyToUse = GetComponent<Rigidbody>();
        }
    }

    void FixedUpdate() {
        CurrentNearestGravityObject = null;
        var strongestA = float.NegativeInfinity;
        foreach (var gravityObject in GravityObject.AllInCurrentScene) {
            // add gravity
            var vectorToGravity = gravityObject.transform.position - RigidbodyToUse.position;
            var dir = vectorToGravity.normalized;
            var r2 = vectorToGravity.sqrMagnitude;
            var a = GravityModifier * gravityObject.GravitationalMass / r2;
            var radiusScale = Mathf.Clamp01(r2 - (gravityObject.Radius * gravityObject.Radius));
            RigidbodyToUse.AddForce(a * radiusScale * dir, ForceMode.Acceleration);

            // check for nearest object
            if (a > strongestA) {
                CurrentNearestGravityObject = gravityObject;
                strongestA = a;
            }
        }
        if (CurrentNearestGravityObject != null) {
            CurrentDown = (CurrentNearestGravityObject.transform.position - transform.position).normalized;
            CurrentDistanceToGravity = (CurrentNearestGravityObject.transform.position - transform.position).magnitude - CurrentNearestGravityObject.Radius;
        }
    }
}
