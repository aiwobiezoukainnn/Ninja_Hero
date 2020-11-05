using UnityEngine;
using System.Collections;

public class SavePosition : MonoBehaviour 
{
	[SerializeField]
	Vector3 pos;
	void Awake () 
	{
		UpdatePos ();
	}

	void Start()
	{
		UpdatePos ();
	}

	void OnEnable ()
	{
		UpdatePos ();
	}


	public void UpdatePos()
	{
		if (pos == Vector3.zero)
		{
			pos = transform.localPosition;
		}
		else
		{
			transform.localPosition = pos;
		}
	}
}
