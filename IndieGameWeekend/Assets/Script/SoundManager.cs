using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;
    private AudioSource backgroundAudio;
    private AudioSource effectAudio;
    private Dictionary<string, AudioClip> backgrounds;
    private Dictionary<string, AudioClip> effects;
    private float masterVoulme;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject soundObject = new GameObject("SoundManager");
                instance = soundObject.AddComponent<SoundManager>();
                instance.backgroundAudio = soundObject.AddComponent<AudioSource>();
                instance.effectAudio = soundObject.AddComponent<AudioSource>();
                instance.LoadFile(ref instance.effects, "Effect/");
                instance.LoadFile(ref instance.backgrounds, "Background/");
                instance.masterVoulme = 1;

                DontDestroyOnLoad(soundObject);
            }
            return instance;
        }
    }
    private void LoadFile<T>(ref Dictionary<string, T> a, string path) where T : Object
    {
        a = new Dictionary<string, T>();
        T[] particleSystems = Resources.LoadAll<T>(path);
        foreach (var particle in particleSystems)
        {
            a.Add(particle.name, particle);
        }
    }

    public void PlayEffect(string name)
    {
        effectAudio.PlayOneShot(effects[name]);

    }
    public void SetMasterVolume(float scale)
    {
        masterVoulme = scale;
    }
    public float GetMasterVolume()
    {
        return masterVoulme;
    }
    public float GetEffectVolume()
    {
        return effectAudio.volume;
    }
    public void SetEffectVolume(float scale)
    {
        effectAudio.volume = scale * masterVoulme;
    }
    public void PlayBackground(string name)
    {
        backgroundAudio.Stop();
        backgroundAudio.loop = true;
        backgroundAudio.clip = backgrounds[name];
        backgroundAudio.Play();
    }
    public void StopBackground()
    {
        backgroundAudio.Stop();
    }
    public void StopeffectAudio()
    {
        effectAudio.Stop();
    }
    public float GetBackgroundVolume()
    {
        return backgroundAudio.volume;
    }
    public void SetBackgroundVolume(float scale)
    {
        backgroundAudio.volume = scale * masterVoulme;
    }

}