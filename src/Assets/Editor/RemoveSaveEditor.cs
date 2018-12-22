using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PersistenceManager))]
public class RemoveSaveEditor : Editor {
	public override void OnInspectorGUI () {
		base.OnInspectorGUI();
		// DrawDefaultInspector ();
		if (GUILayout.Button ("Delete Save")) {
			SaveLoad.clearSavedFilesFolder();
			Debug.Log ("All Save Deleted");
		}
		if (GUILayout.Button ("Exist Save?")) {
            GameData gameData = SaveLoad.LoadFile();
            if (gameData != null){
				Debug.Log ("Exist Save?: YES");
				gameData.Dump();
			}
			else
				Debug.Log ("Exist Save?: NO");
		}

	}

}