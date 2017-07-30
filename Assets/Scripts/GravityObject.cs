﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour {
    public static List<GravityObject> AllInCurrentScene = new List<GravityObject>(100);

    public float GravitationalMass = 10;
    public float Radius = 1;

    public int MelodyId;

    void Awake() {
        AllInCurrentScene.Add(this);
    }

    void OnDestroy() {
        AllInCurrentScene.Remove(this);
    }
}
