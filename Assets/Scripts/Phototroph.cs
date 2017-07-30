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

    [Range(0, 2)]
    public float FlashDuration = 0.3f;
    Material skyboxMaterial;
    Color skyboxNoise1Original;
    Color skyboxNoise2Original;

    void Start() {
        GetComponentInChildren<CollectLight>().OnCollectLight += AddLight;
        jointRigidbody = GetComponentInChildren<FixedJoint>().GetComponent<Rigidbody>();
        orbMaterial = OrbPrefab.GetComponent<Renderer>().sharedMaterial;
        skyboxMaterial = Camera.main.GetComponent<Skybox>().material;
        skyboxNoise1Original = skyboxMaterial.GetColor("_Noise1");
        skyboxNoise2Original = skyboxMaterial.GetColor("_Noise2");
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
        Light = Mathf.RoundToInt(Mathf.Pow(Orbs.Count - 1 + LightToOrbMin, LightToOrbRootBase));
        StopAllCoroutines();
        StartCoroutine(FlashSkybox(orbMaterial.GetColor("_EmissionColor"), FlashDuration));
    }

    IEnumerator FlashSkybox(Color color, float duration) {
        var time = 0f;
        while (time < duration) {
            var t = Mathf.Sqrt(Mathf.Clamp01(time / duration));
            var noise1Color = Color.Lerp(color, skyboxNoise1Original, Mathf.Lerp(0, 1, t));
            var noise2Color = Color.Lerp(color, skyboxNoise2Original, Mathf.Lerp(0.5f, 1, t));
            skyboxMaterial.SetColor("_Noise1", noise1Color);
            skyboxMaterial.SetColor("_Noise2", noise2Color);
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }
        skyboxMaterial.SetColor("_Noise1", skyboxNoise1Original);
        skyboxMaterial.SetColor("_Noise2", skyboxNoise2Original);
    }
}
