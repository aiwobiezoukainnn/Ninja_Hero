using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class PrefsUtil : EditorWindow
{
	[MenuItem("PlayerPrefs/Clear All")]
	public static void ShowWindow()
	{
		PlayerPrefs.DeleteAll();
	}
}

