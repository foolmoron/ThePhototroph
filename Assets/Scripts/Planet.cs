using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    GravityObject gravityObject;
    ParticleSystem particles;
    ParticleBoost boost;
    Renderer renderer;

    void Start() {
        gravityObject = GetComponent<GravityObject>();
        particles = GetComponentInChildren<ParticleSystem>();
        boost = GetComponentInChildren<ParticleBoost>();
        renderer = GetComponent<Renderer>();

        var scale = transform.localScale.x;
        var size = Mathf.InverseLerp(5, 200, scale);
        var size2 = size * size;

        gravityObject.GravitationalMass = Mathf.Lerp(1500, 100000, size2);
        gravityObject.Radius = (scale + 1) / 2;

        var em = particles.emission;
        var b = new ParticleSystem.Burst[em.burstCount];
        em.GetBursts(b);
        b[0].minCount = b[0].maxCount = (short)Mathf.RoundToInt(Mathf.Lerp(20, 10000, size2));
        em.SetBursts(b);

        var sh = particles.shape;
        sh.radius = (scale + 1) / 2;

        var main = particles.main;
        main.startSize = Mathf.Lerp(1, 2, size);

        boost.Boost = Mathf.Lerp(1, 18, size2);

        renderer.material.color = Color.HSVToRGB(size, 0.25f, 1);

        gravityObject.MelodyId = Mathf.FloorToInt(size * 5) % 5;
    }
    
    void Update() {
        
    }
}
