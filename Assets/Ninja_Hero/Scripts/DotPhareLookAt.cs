using UnityEngine;
using System.Collections;

public class DotPhareLookAt : MonoBehaviour {
	Transform circle;


	void OnEnable(){

		circle = GameObject.Find ("Circle").transform;

		StartCoroutine (ShadowUpdate ());
	}

	void OnDisable(){
		StopAllCoroutines ();
	}


	IEnumerator ShadowUpdate(){

		while (true) {
			float angle = Mathf.DeltaAngle (Mathf.Atan2 (transform.position.y, transform.position.x) * Mathf.Rad2Deg,
				             Mathf.Atan2 (circle.position.y, circle.position.x) * Mathf.Rad2Deg);


			transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 90 - angle));

			yield return null;
		}
//		Debug.Log ("" + angle);
	}


}
