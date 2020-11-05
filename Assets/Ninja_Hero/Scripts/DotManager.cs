using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Attached to the each obstacles
/// </summary>
public class DotManager : MonoBehaviourHelper 
{
	public bool isEnable = false;

	public float position = 0;

	[SerializeField] private SpriteRenderer lineSpriteLinkToDestroySprite;
	[SerializeField] private SpriteRenderer lineSpriteLinkToAvoidSprite;

	public bool isOnCircle;

	[SerializeField] private SpriteRenderer spriteToDestroy;
	[SerializeField] private SpriteRenderer spriteToAvoid;

	bool _isBlack;
	/// <summary>
	/// is black = hazard. If write = to destroy
	/// </summary>
	public bool isBlack
	{
		get 
		{
			return _isBlack;
		}

		set
		{
			_isBlack = value;


			lineSpriteLinkToDestroySprite.gameObject.SetActive(value);
			spriteToDestroy.gameObject.SetActive(value);

			lineSpriteLinkToAvoidSprite.gameObject.SetActive(!value);
			spriteToAvoid.gameObject.SetActive(!value);

			spriteToAvoid.sortingOrder = 10;
//			lineSpriteLinkToAvoidSprite.sortingOrder = 10;

			spriteToDestroy.sortingOrder = 1;


			if (value)
			{
//				spriteToDestroy.sortingOrder = 10;
			}
			else 
			{
//				spriteToDestroy.sortingOrder = 1;
				transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 0.01f);
			}




		}
	}


	public Vector3 collisionPoint;


	void Awake()
	{
		isBlack = false;

		Reset ();
	}


	void Reset()
	{

		transform.rotation = Quaternion.identity;

		isOnCircle = true;

		transform.localScale = Vector3.one;

		transform.rotation = Quaternion.identity;

		isBlack = false;
	}

	int ratio;

	/// <summary>
	/// Display the line of each hazard
	/// </summary>
	public void ActivateLine(Vector3 target, Transform CircleBorder){


		transform.position = target;

		position = Vector2.Distance(target,CircleBorder.position);

		transform.parent = CircleBorder;
		transform.localScale = Vector3.one;

		if(lineSpriteLinkToDestroySprite!=null)
		{
			lineSpriteLinkToDestroySprite.transform.localScale = new Vector3 (position*100000/2, lineSpriteLinkToDestroySprite.transform.localScale.y, lineSpriteLinkToDestroySprite.transform.localScale.z);
			lineSpriteLinkToAvoidSprite.transform.localScale = new Vector3 (position*100000/2, lineSpriteLinkToAvoidSprite.transform.localScale.y, lineSpriteLinkToAvoidSprite.transform.localScale.z);
		}

	}
	/// <summary>
	/// If player touch a black square => game over
	/// </summary>
	void GameOverLogic(Collider2D col)
	{
		if (gameObject.name.Contains ("Square"))
		{
			if (col.CompareTag ("Player"))
			{
				if (col.gameObject.activeInHierarchy && gameObject.activeInHierarchy && !gameManager.isGameOver)
				{
					if (isBlack)
					{
						gameManager.GameOver (transform);

					}
					else
					{
						gameManager.SpawnParticleExplosionSquare (this);

//						StopAll ();
					}
				}
			}
		}
	}

//	public void StopAll()
//	{
//		if (lineSpriteLinkToDestroySprite != null)
//			lineSpriteLinkToDestroySprite.color = Color.clear;
//	}
	/// <summary>
	/// Trerred when enter an obtacle
	/// </summary>
	void OnTriggerEnter2D(Collider2D col){
		if (Application.isEditor ) {
		}
		GameOverLogic (col);

	}

}
