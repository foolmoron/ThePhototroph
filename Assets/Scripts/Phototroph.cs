using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phototroph : MonoBehaviour {

    public GameObject OrbPrefab;

    public int Light;
    [Range(1, 5)]
    public float LightToOrbRootBase = 3;
    [Range(0, 10)]
    public int LightToOrbMin = 3;

    [Range(0, 1)]
    public float OrbHueBase = 0/360f;
    [Range(0, 1)]
    public float OrbHueStep = 60/360f;
    public float OrbSat = 0.65f;
    public float OrbVal = 1.5f;

    public List<GameObject> Orbs = new List<GameObject>(10);

    Rigidbody jointRigidbody;
    Material orbMaterial;

    void Start() {
        GetComponentInChildren<CollectLight>().OnCollectLight += AddLight;
        jointRigidbody = GetComponentInChildren<FixedJoint>().GetComponent<Rigidbody>();
        orbMaterial = OrbPrefab.GetComponent<Renderer>().sharedMaterial;
    }

    void Update() {
        var color = Color.HSVToRGB((OrbHueBase + OrbHueStep * Orbs.Count) % 1, OrbSat, OrbVal);
        orbMaterial.SetColor("_EmissionColor", color);
    }

    void FixedUpdate() {
        var desiredOrbs = Mathf.FloorToInt(Mathf.Max(0, Mathf.Pow(Light, 1 / LightToOrbRootBase) - LightToOrbMin));
        while (desiredOrbs < Orbs.Count) {
            Destroy(Orbs[0]);
            Orbs.RemoveAt(0);
        }
        while (desiredOrbs > Orbs.Count) {
            var orb = Instantiate(OrbPrefab);
            orb.GetComponent<SpringJoint>().connectedBody = jointRigidbody;
            orb.GetComponentInChildren<CollectLight>().OnCollectLight += AddLight;
            Orbs.Add(orb);
        }
    }

    public void AddLight(int light) {
        Light += light;
    }

    public void UseOrb() {
        var prevLight = Light;
        Light = Mathf.RoundToInt(Mathf.Pow(Orbs.Count - 1 + LightToOrbMin, LightToOrbRootBase));
        Debug.Log(prevLight + " -> " + Light);
    }
}
