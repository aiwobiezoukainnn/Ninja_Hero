using UnityEngine;
using System.Collections;
using DG.Tweening;

/// <summary>
/// In Charge to rotate the backgrounds elements, smoothly
/// </summary>
public class RotationBackgroundRandom : MonoBehaviour 
{

	void Start()
	{
		SequenceLogic ();
	}

	Sequence sequence;

	void SequenceLogic()
	{
		if (sequence != null)
			sequence.Kill (false);

		sequence = DOTween.Sequence ();

		Vector3 rotateVector = new Vector3 (0, 0, 1);

		if(Random.Range(0,2) == 0)
			rotateVector = new Vector3 (0, 0, -1);

		sequence.Append (transform.DORotate (rotateVector * UnityEngine.Random.Range (360, 520), Random.Range(3f,10f), RotateMode.FastBeyond360));
		sequence.SetLoops (1, LoopType.Incremental);

		sequence.OnStepComplete (() => {
			
			SequenceLogic();
		});

		sequence.Play ();
	}

    public void TogglePause()
    {
        sequence.TogglePause();
    }
}
