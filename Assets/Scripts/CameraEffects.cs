using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraEffects : MonoBehaviour {

    [Range(0, 1)]
    public float Intensity;
    [Range(0, 1)]
    public float RealIntensity;
    [Range(0, 5)]
    public float IntensitySpeed = 3f;


    Camera camera;
    PostProcessingBehaviour ppb;
    Shaker shaker;
    
    void Start() {
        camera = GetComponent<Camera>();
        ppb = GetComponent<PostProcessingBehaviour>();
        shaker = GetComponent<Shaker>();
    }

    void FixedUpdate() {
        RealIntensity = Mathf.MoveTowards(RealIntensity, Intensity, IntensitySpeed * Time.deltaTime);

        camera.fieldOfView = Mathf.Lerp(60, 90, RealIntensity);

        ppb.profile.vignette.enabled = RealIntensity > 0;
        var ppbSettings = ppb.profile.vignette.settings;
        ppbSettings.intensity = Mathf.Lerp(0.1f, 0.5f, RealIntensity);
        ppb.profile.vignette.settings = ppbSettings;
        
        var ppbSettings2 = ppb.profile.motionBlur.settings;
        ppbSettings2.frameBlending = Mathf.Lerp(0.17f, 0.5f, RealIntensity);
        ppb.profile.motionBlur.settings = ppbSettings2;

        shaker.Shaking = RealIntensity > 0;
        shaker.Strength = Mathf.Lerp(0, 0.2f, RealIntensity);
    }
}
