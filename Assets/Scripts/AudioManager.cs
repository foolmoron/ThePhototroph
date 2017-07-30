using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioPack {
    [Range(0, 5)]
    public float FadeTime = 0.5f;
    [Range(0, 1)]
    public float MasterVolume = 1;
    public int Current;
    public int Previous;
    public AudioSource[] Audios;
    public float[] Volumes;
}
public class AudioManager : Manager<AudioManager> {

    public AudioPack Drums = new AudioPack {
        FadeTime = 2,
    };
    public AudioPack Synths = new AudioPack {
        FadeTime = 0.2f,
    };
    public AudioPack Melodies = new AudioPack {
        FadeTime = 0.5f,
    };

    AudioPack[] audioPacks;
    
    void Start() {
        Drums.Audios = transform.Find("Drums").GetComponentsInChildren<AudioSource>();
        Drums.Volumes = new float[Drums.Audios.Length];
        Synths.Audios = transform.Find("Synth").GetComponentsInChildren<AudioSource>();
        Synths.Volumes = new float[Synths.Audios.Length];
        Melodies.Audios = transform.Find("Melody").GetComponentsInChildren<AudioSource>();
        Melodies.Volumes = new float[Melodies.Audios.Length];

        audioPacks = new[] { Drums, Synths, Melodies };
    }
    
    void Update() {
        foreach (var pack in audioPacks) {
            if (pack.Previous != pack.Current) {
                StartCoroutine(CrossFade(pack));
            }
            for (int i = 0; i < pack.Audios.Length; i++) {
                pack.Audios[i].volume = pack.Volumes[i] * pack.MasterVolume;
            }
            pack.Previous = pack.Current;
        }

        if (Input.GetKeyDown(KeyCode.M)) {
            AudioListener.volume = AudioListener.volume == 1 ? 0 : 1;
        }
    }

    static IEnumerator CrossFade(AudioPack pack) {
        var time = 0f;
        while (time < pack.FadeTime) {
            var t = Mathf.Sqrt(Mathf.Clamp01(time / pack.FadeTime));
            for (int i = 0; i < pack.Audios.Length; i++) {
                if (i == pack.Current) {
                    pack.Audios[i].mute = false;
                    pack.Volumes[i] = t;
                } else {
                    pack.Volumes[i] = 1 - t;
                }
            }
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }
        for (int i = 0; i < pack.Audios.Length; i++) {
            pack.Audios[i].mute = i != pack.Current;
            pack.Volumes[i] = i == pack.Current ? 1 : 0;
        }
    }
}
