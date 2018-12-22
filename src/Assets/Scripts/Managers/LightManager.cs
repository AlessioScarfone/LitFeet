using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour, IGameManager {
	public ManagerStatus status { get; private set; }

	public List<FloorNumber> floorList;

	private Dictionary<int, GameObject> _lightDictionary;

	public void Startup () {
		// Debug.Log ("LightManger Starting ...");
		_lightDictionary = new Dictionary<int, GameObject> ();
		foreach (var item in floorList) {
			// Debug.Log ("Floor number attached: " + item.number);
			_lightDictionary.Add (item.number, item.gameObject);
		}
		status = ManagerStatus.Started;
	}

	public void TurnOnLights (int floorNumb) {
		GameObject floor = _lightDictionary[floorNumb];
		Light[] floorLights = floor.GetComponentsInChildren<Light> ();
		// Debug.Log ("Light Number on floor "+floorNumb+":"+floorLights.Length);
		foreach (Light l in floorLights) {
			l.intensity = 3f;
		}
	}

	public void TurnOnLightsUntilFloor (int floorNumb) {
		foreach (int fn in _lightDictionary.Keys) {
			if (fn <= floorNumb) {
				TurnOnLights (fn);
			}
		}

	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}