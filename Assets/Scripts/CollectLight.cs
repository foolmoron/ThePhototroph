using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectLight : MonoBehaviour {

    public event Action<int> OnCollectLight = delegate { };

    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>(100);

    void Start() {
    }

    void FixedUpdate() {
    }

    void OnParticleCollision(GameObject other) {
        other.GetComponent<ParticleSystem>().GetCollisionEvents(other, collisionEvents);
        OnCollectLight(collisionEvents.Count);
    }
}
