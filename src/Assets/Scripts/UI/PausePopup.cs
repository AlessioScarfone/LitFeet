using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePopup : MonoBehaviour {

    private bool _actived = false;
    public Slider musicVolumeSlider;
    public Slider soundVolumeSlider;
    public Button postProcessingBtn;
    public AudioClip buttonPressSound;
    public Button soundsButton;
    public Button musicButton;
    public Sprite soundOffSprite;
    public Sprite soundOnSprite;
    public Sprite musicOffSprite;
    public Sprite musicOnSprite;

    public void Start () {
        musicVolumeSlider.value = Managers.Audio.musicVolume;
        soundVolumeSlider.value = Managers.Audio.soundVolume;
    }

    public void Open () {
        gameObject.SetActive (true);
        // pauseButton.SetActive(false);
        _actived = true;

        PauseGame ();

    }

    // Update is called once per frame
    public void Close () {
        gameObject.SetActive (false);
        _actived = false;
        UnPauseGame ();

    }

    public bool isActive () {
        return _actived;
    }

    public void PauseGame () {
        Time.timeScale = 0f;
    }

    public void UnPauseGame () {
        Time.timeScale = 1f;
    }

    public void OnSoundToggle () {
        Managers.Audio.soundMute = !Managers.Audio.soundMute;
        if (Managers.Audio.soundMute)
            soundsButton.GetComponent<Image> ().sprite = soundOffSprite;
        else
            soundsButton.GetComponent<Image> ().sprite = soundOnSprite;
    }

    public void OnSoundValue (float volume) {
        Managers.Audio.soundVolume = volume;
    }
    public void OnMusicToggle () {
        Managers.Audio.musicMute = !Managers.Audio.musicMute;
        if (Managers.Audio.musicMute)
            musicButton.GetComponent<Image> ().sprite = musicOffSprite;
        else
            musicButton.GetComponent<Image> ().sprite = musicOnSprite;

    }
    public void OnMusicValue (float volume) {
        Managers.Audio.musicVolume = volume;
    }

    public void OnPostProcessingToggle () {
        Managers.Audio.PlaySound (buttonPressSound);
        GameObject camera = GameObject.FindGameObjectWithTag ("MainCamera");
        if (camera != null) {
            UnityEngine.PostProcessing.PostProcessingBehaviour postProcessingBehaviour = camera.GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour> ();
            if (postProcessingBehaviour != null) {
                postProcessingBehaviour.enabled = !postProcessingBehaviour.enabled;
                if (postProcessingBehaviour.enabled)
                    postProcessingBtn.GetComponentInChildren<Text> ().text = "On";
                else
                    postProcessingBtn.GetComponentInChildren<Text> ().text = "Off";
            }
        }
    }

    public void OnPlayMusic (int selector) {
        Managers.Audio.PlaySound (buttonPressSound);
        switch (selector) {
            case 1:
                //    Managers.Audio.PlayIntroMusic();
                break;
            case 2:
                Managers.Audio.PlayLevelMusic ();
                break;
            default:
                Managers.Audio.StopMusic ();
                break;
        }
    }

    public void Quit () {
        Managers.Audio.PlaySound (buttonPressSound);
        //Application.Quit non funziona da editor!
        Application.Quit ();
    }
}