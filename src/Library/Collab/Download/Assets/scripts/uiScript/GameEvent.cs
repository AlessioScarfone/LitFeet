using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour {

	public const string DAMAGE = "DAMAGE";
	public const string KILLED = "KILLED";

    public const string RESTARTED = "RESTARTED";

    public static string UPDATE_KEY_COUNT = "UPDATE_KEY_COUNT";
    public static string RESTORE_FROM_SAVE = "RESTORE_FROM_SAVE";
}