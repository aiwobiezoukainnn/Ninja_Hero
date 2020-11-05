using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace AppAdvisory.social
{
	public static class LDEUtility 
	{
		const string menuPath = "GameObject/";

//		public static void CreateAsset<T>(string name) where T : ScriptableObject
//		{
//			var asset = ScriptableObject.CreateInstance<T>();
//			ProjectWindowUtil.CreateAsset(asset, name + ".asset");
//		}

		[MenuItem ( menuPath + "APP ADVISORY/Very Simple Leaderboard/CREATE LeaderboardInit",false,20)]
		public static void CreateLeadertboardInits()
		{
			GameObject gameObject = new GameObject("LeaderboardInit");
			LeaderboardInit a = gameObject.AddComponent<LeaderboardInit>();
			string[] guids = AssetDatabase.FindAssets("LEADERBOARD_SETTING");

			#if UNITY_5_3_OR_NEWER
			a.leaderboardIds =  AssetDatabase.LoadAssetAtPath<LEADERBOARDIDS>( AssetDatabase.GUIDToAssetPath( guids[0] ));
			a.SetLEADERBORDIDS(AssetDatabase.LoadAssetAtPath<LEADERBOARDIDS>( AssetDatabase.GUIDToAssetPath( guids[0] )));
			#else
			a.leaderboardIds =  AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[0]), typeof(LEADERBOARDIDS)) as LEADERBOARDIDS;
			a.SetLEADERBORDIDS(AssetDatabase.LoadAssetAtPath( AssetDatabase.GUIDToAssetPath( guids[0] ), typeof(LEADERBOARDIDS))  as LEADERBOARDIDS );
			#endif
			//			Autoselect();
		}

		[MenuItem("Tools/APP ADVISORY/Very Simple Leaderboard/OPEN LEADERBOARD SETTINGS", false, 0)]
		[MenuItem("Window/APP ADVISORY/Very Simple Leaderboard/OPEN LEADERBOARD SETTINGS", false, 0)]
		public static void Autoselect()
		{
			string[] guids = AssetDatabase.FindAssets("LEADERBOARD_SETTING");
			#if UNITY_5_3_OR_NEWER
			Selection.activeObject = AssetDatabase.LoadAssetAtPath<LEADERBOARDIDS>( AssetDatabase.GUIDToAssetPath( guids[0] ));
			#else
			Selection.activeObject = AssetDatabase.LoadAssetAtPath( AssetDatabase.GUIDToAssetPath( guids[0] ), typeof(LEADERBOARDIDS)) ;
			#endif
		}
	}
}