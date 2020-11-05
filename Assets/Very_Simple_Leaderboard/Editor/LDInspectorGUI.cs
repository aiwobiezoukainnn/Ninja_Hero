using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;


namespace AppAdvisory.social
{


	[CustomEditor(typeof(LEADERBOARDIDS))]
	public class LDInspectorGUI : Editor
	{

		public override void OnInspectorGUI()
		{
			LEADERBOARDIDS t  = (LEADERBOARDIDS)target;

			if(!PlayerPrefs.HasKey("APP_ADVISORY_FIRST_TIME_LEADERBORD"))
			{
				Debug.Log("APP_ADVISORY_FIRST_TIME_LEADERBORD");
				PlayerPrefs.SetInt("APP_ADVISORY_FIRST_TIME_LEADERBORD",0);
				PlayerPrefs.Save();

				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, SetString(PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS)));

				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, SetString(PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android)));
			}

			var stringIos = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS);
			var stringAndroid = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);

			if(!stringIos.Contains("APPADVISORY_LEADERBOARD"))
			{
				stringIos = "APPADVISORY_LEADERBOARD" + ";" + stringIos;

				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS,stringIos);
			}

			if(!stringAndroid.Contains("APPADVISORY_LEADERBOARD"))
			{
				stringAndroid = "APPADVISORY_LEADERBOARD" + ";" + stringAndroid;

				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android,stringAndroid);
			}

			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("GET\nGoogle Play Game\nSDK",  GUILayout.Width(150), GUILayout.Height(50)))
			{
				Application.OpenURL("https://github.com/playgameservices/play-games-plugin-for-unity");
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.Space();

			EditorGUILayout.LabelField(new GUIContent("Game Center Leaderboard Id   [?]", "Find it on Itunes Connect console"));
			t.LEADERBOARDID_IOS = EditorGUILayout.TextField(t.LEADERBOARDID_IOS);

			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.Space();

			EditorGUILayout.LabelField(new GUIContent("Google Play Game Leaderboard Id   [?]", "Find it on Google Play Game console"));
			t.LEADERBOARDID_ANDROID = EditorGUILayout.TextField(t.LEADERBOARDID_ANDROID);

			if (GUI.changed)
			{
				EditorUtility.SetDirty(t); 
				PlayerPrefs.Save();
			}
		}

		string SetString(string stringIOS)
		{
			stringIOS = stringIOS.Replace("APPADVISORY_LEADERBOARD" + ";","");

			stringIOS = stringIOS.Replace("APPADVISORY_LEADERBOARD","");

			return stringIOS;
		}

	}
}