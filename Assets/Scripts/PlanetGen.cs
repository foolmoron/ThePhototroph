using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGen : MonoBehaviour {

    public GameObject PlanetPrefab;

    [Range(0, 100)]
    public int Planets = 50;
    [Range(0, 1000)]
    public float PlanetRange = 800;
    
    void Awake() {
        for (int i = 0; i < Planets; i++) {
            var sizeish = Random.value;
            var dist = Mathf.Lerp(30, PlanetRange, sizeish);
            var pos = Random.onUnitSphere * dist;
            var scale = Mathf.Lerp(5, 100, sizeish) * Mathf.Lerp(0.66f, 1.5f, Random.value);
            var planet = Instantiate(PlanetPrefab, pos, Quaternion.identity);
            planet.transform.localScale = Vector3.one * scale;
        }
    }
}
