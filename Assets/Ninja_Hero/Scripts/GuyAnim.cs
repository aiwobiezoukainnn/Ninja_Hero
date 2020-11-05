using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class GuyAnim : MonoBehaviourHelper
{
	public Transform legLeft;
	public Transform legRight;

	public Animator animator;

	public GameObject particle;

	public List<SpriteRenderer> listSpriteRenderer;

	public Collider2D m_collider;

	Color color = new Color (Color.white.r, Color.white.g, Color.white.b, 1);

	public void MakeItBlink()
	{
		transform.localRotation = Quaternion.identity;

		m_collider.enabled = false;


		StopBlink ();

		foreach(var s in listSpriteRenderer)
		{
			m_collider.enabled = false;

			s.color = Color.black;

			transform.localRotation = Quaternion.identity;



			s.DOColor (color, 0.1f)
				.SetLoops (-1,LoopType.Yoyo)
				.OnComplete (() => {
					s.color = Color.black;

					m_collider.enabled = true;

					s.color = Color.black;

					transform.localRotation = Quaternion.identity;

					m_collider.enabled = true;
				});
		}
	}

	public void StopBlink()
	{
		foreach(var s in listSpriteRenderer)
		{
			s.color = Color.black;

			m_collider.enabled = true;

			DOTween.Kill (s);

			m_collider.enabled = true;

			s.color = Color.black;
		}

		m_collider.enabled = true;
	}

	public void DoJump()
	{
		particle.SetActive (true);

		animator.Play ("Idle");
	}

	public void DoWalk()
	{
		animator.Play ("GuyAnimLegs");
		particle.SetActive (false);
	}

	public void StopAll()
	{
		animator.Play ("Idle");
	}
//		
//	void Update()
//	{
//		transform.localPosition = Vector3.up * 0.142f;
//	}
}
