using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicHandler : MonoBehaviour {

	private AudioSource _musicSource;
	[SerializeField]private string music_Clip;

    public Texture2D cursorTexture;
    private  CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;

    private void Awake() {
		Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

	// Use this for initialization
	void Start () {
		_musicSource= gameObject.GetComponent<AudioSource>();
		PlayLevelMusic();
	}
	

	public void PlayLevelMusic () {

        PlayMusic ((AudioClip) Resources.Load ("Music/" + music_Clip));

    }
    private void PlayMusic (AudioClip clip) {
        _musicSource.clip = clip;
        _musicSource.Play ();
    }
    public void StopMusic () {
        _musicSource.Stop ();
    }
}
