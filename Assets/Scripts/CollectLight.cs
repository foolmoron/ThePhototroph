using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectLight : MonoBehaviour {

    public event Action<int> OnCollectLight = delegate { };

    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>(100);

    public AudioClip CollectSound;
    public int FramesBetweenSound = 5;
    int frames;

    void Start() {
    }

    void Update() {
        frames--;
    }

    void OnParticleCollision(GameObject other) {
        other.GetComponent<ParticleSystem>().GetCollisionEvents(other, collisionEvents);
        OnCollectLight(collisionEvents.Count);
        if (frames <= 0) {
            AudioSource.PlayClipAtPoint(CollectSound, Vector3.zero);
            frames = FramesBetweenSound;
        }
    }
}
