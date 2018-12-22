using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (ShowTips))]
public class ShowTipsHideFields : Editor {
	public override void OnInspectorGUI () {
		
		base.OnInspectorGUI ();
		// DrawDefaultInspector ();
		
		ShowTips script = (ShowTips) target;

		// draw checkbox for the bool
		script.useTimer = EditorGUILayout.Toggle ("Use Timer", script.useTimer);
		if (script.useTimer) // if bool is true, show other fields
		{
			script.timer = EditorGUILayout.FloatField ("Timer", script.timer);
		}

	}

}