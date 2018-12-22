using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IGameManager {

    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundLoopSource;
    [SerializeField] private string levelBGMusic;
    // [SerializeField] public AudioClip activated;

    public List<AudioClip> audioClipList;

    private Dictionary<String, AudioClip> audioDictionary;

    public ManagerStatus status { get; private set; }

    public float soundVolume {
        get { return soundSource.volume; }
        set { soundSource.volume = value; soundLoopSource.volume = value; }
    }
    public bool soundMute {
        get {
            if (soundSource != null && soundLoopSource != null) {

                return soundSource.mute && soundLoopSource.mute;
            }
            return false;
        }
        set {
            if (soundSource != null && soundLoopSource != null) {
                soundSource.mute = value;
                soundLoopSource.mute = value;

            }
        }
    }

    public float musicVolume {
        get { return musicSource.volume; }
        set { musicSource.volume = value; }
    }
    public bool musicMute {
        get {
            if (musicSource != null) {
                return musicSource.mute;
            }
            return false;
        }
        set {
            if (musicSource != null) {
                musicSource.mute = value;
            }
        }
    }

    public void Startup () {
        // Debug.Log ("Audio manager starting...");
        soundVolume = 0.2f;
        musicVolume = 0.2f;
        musicSource.ignoreListenerVolume = true;
        musicSource.ignoreListenerPause = true;

        audioDictionary = new Dictionary<String, AudioClip> ();
        foreach (var a in audioClipList) {
            audioDictionary.Add (a.name, a);
        }

        // foreach (var ak in audioDictionary.Keys)
        // {
        //     Debug.Log(ak);
        // }

        status = ManagerStatus.Started;
        PlayLevelMusic ();

    }

    public void Start () {

    }

    public AudioClip getSound (String clipName) {
        if (audioDictionary.ContainsKey (clipName))
            return audioDictionary[clipName];
        return null;
    }

    public void PlaySound (AudioClip clip) {
        if (clip != null)
            soundSource.PlayOneShot (clip);
    }

    public void PlaySound (String clipName) {
        // Debug.Log ("TRY TO PLAY:" + clipName);
        AudioClip clip = getSound (clipName);
        if (clip != null) {
            soundSource.PlayOneShot (clip);
        }
    }
    public void PlayLoopSound (AudioClip clip) {
        soundLoopSource.PlayOneShot (clip);
    }

    public void PlayLoopSound (String clipName) {
        // Debug.Log ("TRY TO PLAY:" + clipName);
        AudioClip clip = getSound (clipName);
        if (clip != null) {
            soundLoopSource.PlayOneShot (clip);
        }
    }
    public void StopSound () {
        soundSource.Stop ();
    }

    public void StopLoopSounds () {
        soundLoopSource.Stop ();

    }

    public void PlayLevelMusic () {

        PlayMusic ((AudioClip) Resources.Load ("Music/" + levelBGMusic));

    }
    private void PlayMusic (AudioClip clip) {
        musicSource.clip = clip;
        musicSource.Play ();
    }
    public void StopMusic () {
        musicSource.Stop ();
    }

    // public void PlaySoundActived () {
    //     PlaySound (activated);
    // }

}