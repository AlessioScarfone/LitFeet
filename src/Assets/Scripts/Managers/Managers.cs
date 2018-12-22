using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (LightManager))]
[RequireComponent (typeof (PlayerManager))]
[RequireComponent (typeof (PersistenceManager))]
[RequireComponent (typeof (BalloonManager))]
[RequireComponent (typeof (AudioManager))]
public class Managers : MonoBehaviour {

	public static LightManager LightManager { get; private set; }
	public static PlayerManager PlayerManager { get; private set; }
	public static PersistenceManager PersistenceManager { get; private set; }
	public static BalloonManager BalloonManager { get; private set; }
	public static AudioManager Audio { get; private set; }
	private List<IGameManager> _startSequence;

	private void Awake () {

		_startSequence = new List<IGameManager> ();
		
		LightManager = GetComponent<LightManager> ();
		PlayerManager = GetComponent<PlayerManager>();
		PersistenceManager = GetComponent<PersistenceManager>();
		BalloonManager = GetComponent<BalloonManager>();
		Audio = GetComponent<AudioManager>();

		_startSequence.Add (LightManager);
		_startSequence.Add (PlayerManager);
		_startSequence.Add (PersistenceManager);
		_startSequence.Add (BalloonManager);
		_startSequence.Add (Audio);
		StartCoroutine (StartupMangers ());

	}

	private IEnumerator StartupMangers () {
		foreach (var m in _startSequence) {
			m.Startup ();
		}
		yield return null; //lo usiamo per aspettare che il frame finisce
	
		//track managers starting
		int numModules = _startSequence.Count;
		int numReady = 0;

		while (numReady < numModules) {
			int lastReady = numReady;
			numReady = 0;
			foreach (IGameManager manager in _startSequence) {
				if (manager.status == ManagerStatus.Started) {
					numReady++;
				}
			}
			if (numReady > lastReady)
				// Debug.Log ("Progress: " + numReady + "/" + numModules);
			yield return null;
		}
		Debug.Log ("All managers started up");

	}

}