using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChaseCameraBehaviour))]
public class AnchorCameraToPlayer : Editor {

	public override void OnInspectorGUI () {
		base.OnInspectorGUI();
		// DrawDefaultInspector ();
		if (GUILayout.Button ("Anchor to player")) {

			GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
			if(camera != null){
				Vector3 dist = new Vector3(83.3f, -61.7f, -43.6f);
				camera.GetComponent<ChaseCameraBehaviour>().SetDistance(dist);
			}
			
		}

	}

}
