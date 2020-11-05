using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using DG.Tweening;

/// <summary>
/// Class in charge of the game logic.
/// </summary>
public class GameManager : MonoBehaviourHelper
{
	public float cameraSize = 20;

	float floorPosition = 4.19f;

	public int Level;

	bool firstStart = true;

	[NonSerialized] public bool success;

	[NonSerialized] public bool isGameOver;

	float height;

	public Transform obstaclesParent;

	public float speed = 1f;

	public float positionTouchBorder;

	DotManager lastShoot;

	public Transform rotatePlayer;

	public SpriteRenderer spriteDotGameOverZoom;

	Vector3 rotateVector;

	private float SizeRayonRatio = 1f;

	private Ease easeType = Ease.Linear;

	private LoopType loopType = LoopType.Incremental;

	private float rotateCircleDelay = 6f;

	private int numberDotsOnCircle;

	public Tweener jumpTweener;

	public float waitTime = 0.15f;

	public int segments = 10;

	Vector3 rotateDOTVector = new Vector3 (0, 0, 1);

	/// <summary>
	/// Keep a reference of the Dotween sequence use to rotate the circle and the dots linked to the circle
	/// </summary>
	public Sequence sequence;
	/// <summary>
	/// Keep a reference of the Dotween sequence use to move around the player
	/// </summary>
	public Sequence sequenceDOT;
	/// <summary>
	/// list of all the dots the player have to shoot in the level
	/// </summary>
	List<DotManager> DotsBottom;

	float positionGravity;

	float sizeDot = 0;

    public RotationBackgroundRandom[] backgrounds;

	Vector3 pos
	{
		get
		{
			return 	new Vector3 (0, -positionTouchBorder, 0);
		}
	}
	Quaternion rot
	{
		get
		{
			return Quaternion.identity;
		}
	}
	Transform parent
	{
		get 
		{
			return obstaclesParent;
		}
	}
	/// <summary>
	/// List of square to be used in the game.
	/// </summary>
	List<DotManager> LIST_SQUARE = new List<DotManager>();

	/// <summary>
	/// Do it at first. Some configurations.
	/// </summary>
	void Awake()
	{
		height = 2f * 20;

		Camera.main.transform.position = new Vector3 (0, 0, -10);


		DotsBottom = new List<DotManager>();

		AddTouchListener();

		ResetPosition ();

		Util.SetCountGameOver(0);
	}
	/// <summary>
	/// Adding the touch listener to control player.
	/// </summary>
	void AddTouchListener()
	{
		InputTouch.onTouchDown += delegate(Vector3 pos) {

            if(Util.GetIsShowGuide()) // 当前显示引导
            {
                Util.SetIsShowGuide(false); // 只显示一次
                canvasManager.GuideTip.SetActive(false);
                return;
            }
			if(pos.x < Screen.width*0.9f && pos.y < Screen.height*0.9f)
				DoJump();
		};

		InputTouch.onTouchUp += delegate(Vector3 pos) {
			if(pos.x < Screen.width*0.9f && pos.y < Screen.height*0.9f)
				DoWalk();
		};
	}

	void Start()
	{
        guyAnim.MakeItBlink();

		spriteDotGameOverZoom.transform.localScale = Vector3.zero;
	}
	/// <summary>
	/// Do the aniamtion jump of the player
	/// </summary>
	void DoJump()
	{
		if (isGameOver)
			return;

		if (jumpTweener != null)
			jumpTweener.Kill ();

		float ratio = Mathf.Abs (PLAYER.localPosition.y - positionTouchBorder) / (positionTouchBorder - floorPosition);

		guyAnim.DoJump();

		jumpTweener = PLAYER.DOLocalMoveY (positionTouchBorder, ratio*waitTime, false);
	}
	/// <summary>
	/// Do the aniamtion walk of the player
	/// </summary>
	void DoWalk()
	{
		if (isGameOver)
			return;
		
		if (jumpTweener != null)
			jumpTweener.Kill ();

		float ratio = Mathf.Abs (PLAYER.localPosition.y - floorPosition) / (positionTouchBorder - floorPosition);
		jumpTweener = PLAYER.DOLocalMoveY (floorPosition, ratio*waitTime, false)
			.OnComplete (guyAnim.DoWalk);
	}
	/// <summary>
	/// Reset all position. We have to do this at start of each level
	/// </summary>
	void ResetPosition()
	{
		rotatePlayer.transform.position = Vector3.zero;
		rotatePlayer.transform.localRotation = Quaternion.Euler (Vector3.zero);
		PLAYER.localRotation = Quaternion.Euler (Vector3.zero);
		rotateDOTVector = new Vector3 (0, 0, 1);
	}
	/// <summary>
	/// All the game level creation logic. We will create the current level.
	/// </summary>
	public void CreateGame(int level)
	{
		print("create game");
		if (sequence != null)
			sequence.Kill (false);

		if (sequenceDOT == null) 
		{
			ResetPosition ();
			SequenceDOTLogic ();
		}

	  // canvasManager.ButtonLogic();
			
		DOTween.Kill (obstaclesParent);

		DOTween.Kill (obstaclesParent);


		CancelInvoke ();

		StopAllCoroutines ();

		isGameOver = false;
		success = false;

		//Level = Util.GetLastLevelPlayed();
        Level = canvasManager.lastLevel;

		Level l = levelManager.GetLevel (Level);
		numberDotsOnCircle = l.numberDotsOnCircle;

		SizeRayonRatio = l.sizeRayonRation;
		rotateCircleDelay = l.rotateDelay;
		easeType = l.rotateEaseType;
		loopType = l.rotateLoopType;
	
		positionTouchBorder = height * SizeRayonRatio;

		//canvasManager.ButtonLogic ();

		PLAYER.localRotation = Quaternion.identity;

		Time.timeScale = 1;

		Application.targetFrameRate = 60;

		GC.Collect ();

		PLAYER.localRotation = Quaternion.identity;


		PLAYER.localScale = new Vector3(-rotateDOTVector.z,1,1);

		PLAYER.localRotation = Quaternion.identity;

		spriteDotGameOverZoom.transform.DOScale(Vector3.zero,0.5f);

		guyAnim.DoWalk();
		this.Level = level;

		Camera.main.orthographicSize = cameraSize;
		Camera.main.transform.position = new Vector3 (0, 0, -10);

		StopAllCoroutines ();

		poolSystem.DespawnAll ();

		Camera.main.transform.position = new Vector3 (0, 0, -10);

		rotateVector = new Vector3 (0, 0, 1);

		if (Level % 2 == 0)
			rotateVector = new Vector3 (0, 0, -1);
	
		obstaclesParent.gameObject.SetActive (false);

		obstaclesParent.localScale = Vector3.one;

		DOTween.Kill (PLAYER);

		PLAYER.localPosition = new Vector3 (PLAYER.localPosition.x, floorPosition, PLAYER.localPosition.z);

		guyAnim.MakeItBlink ();

		guyAnim.m_collider.enabled = false;

		CreateDotOnCircle ();

		obstaclesParent.localScale = Vector3.one*0.001f;
		obstaclesParent.gameObject.SetActive (true);

		int count = LIST_SQUARE.Count;

		guyAnim.MakeItBlink ();

		guyAnim.m_collider.enabled = false;

		for (int i = 0; i < count; i++) 
		{
			DotManager dm = LIST_SQUARE[i];

			dm.ActivateLine (dm.transform.position,dm.transform.parent);

			guyAnim.m_collider.enabled = false;
		}

		obstaclesParent.DOScale (Vector3.one, 1)
			.SetDelay(0.3f)
			.SetEase (Ease.InBack)
			.OnComplete(() => {
				//canvasManager.ButtonLogic();
				guyAnim.m_collider.enabled = false;

				Invoke("StopBlink",1);
			});
	}
		
	/// <summary>
	/// Stop the player blinking (when the player blinks, we are invicible)
	/// </summary>
	void StopBlink()
	{
		//canvasManager.ButtonLogic();

		guyAnim.StopBlink();

		guyAnim.enabled = true;

		LaunchRotateCircle ();
	}
	/// <summary>
	/// Rotate the circle and the dots linked to it
	/// </summary>
	void LaunchRotateCircle()
	{
		SequenceLogic ();
	}
	/// <summary>
	/// The method we will continuously call to move the player around
	/// </summary>
	public void SequenceDOTLogic()
	{
		PLAYER.localRotation = Quaternion.identity;

		if (sequenceDOT != null)
			sequenceDOT.Kill (false);

		if (firstStart)
			ResetPosition ();
		
		firstStart = false;

		sequenceDOT = DOTween.Sequence ();

		rotateDOTVector *= -1f;

		PLAYER.DOScaleX (-rotateDOTVector.z, 0.2f);

		LoopType loopDot = LoopType.Incremental;

		sequenceDOT.Append (rotatePlayer.DOLocalRotate(rotateDOTVector*360, 5, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetRelative(true));
		sequenceDOT.SetLoops (1, loopDot);

		sequenceDOT.OnStepComplete (() => {
			SequenceDOTLogic();
		});
	}
	/// <summary>
	/// The method we will continuously call to move the the world around
	/// </summary>
	public void SequenceLogic()
	{
		if (sequence != null)
			sequence.Kill (false);

		sequence = DOTween.Sequence ();

		if (loopType == LoopType.Incremental)
		{
			sequence.Append (obstaclesParent.DORotate (-rotateVector * UnityEngine.Random.Range (360, 520), rotateCircleDelay, RotateMode.FastBeyond360).SetEase (easeType));
			sequence.SetLoops (1, loopType);
		}
		else
		{
			sequence.Append (obstaclesParent.DORotate (-rotateVector * UnityEngine.Random.Range (360, 520), rotateCircleDelay, RotateMode.FastBeyond360).SetEase (easeType));
			sequence.SetLoops (2, loopType);
		}

		sequence.OnStepComplete (SequenceLogic);

		sequence.Play ();
	}
	/// <summary>
	/// Create the dots on the circle and activate the line to link the dots to the circle
	/// </summary>
	void CreateDotOnCircle()
	{
		LIST_SQUARE = new List<DotManager> ();

		int rand = Level % 6;



		if (rand == 0)
			CreateParralax (1, 1, 1);
		else if (rand == 1)
			CreateParralax (1, 2, 3);
		else if (rand == 2)
			CreateSpiral ();
		else if (rand == 3)
			CreateTriangle ();
		else if (rand == 4)
			CreateUpAndDown ();
		else if (rand == 5)
			CreateEscalier ();
		else if (rand == 6)
			CreateBalagan ();

		CreateBlackSquare ();
	}



	void CreateBlackSquare()
	{
		int n = LIST_SQUARE.Count;  

		for (int i = 0; i < 5; i++) 
		{
			System.Random rng = new System.Random();  
			while (n > 1) 
			{  
				n--;  
				int k = rng.Next (n + 1);  
				var value = LIST_SQUARE [k];  
				LIST_SQUARE [k] = LIST_SQUARE [n];  
				LIST_SQUARE [n] = value;  
			}  
		}

		for (int i = 0; i < LIST_SQUARE.Count; i++) 
		{
			LIST_SQUARE [i].isBlack = false;
		}

		int numBlackTotal = 1;

		float temp = 5 - this.Level%4f;
		temp = LIST_SQUARE.Count / (1 + temp + Util.GetCountGameOver());

		int iTemp = (int)(temp);

		numBlackTotal = iTemp + 1;

		if(numBlackTotal <= 0)
			numBlackTotal = 1; 


		if (LIST_SQUARE.Count == 1) 
		{
			LIST_SQUARE [0].isBlack = false;
		}
		else 
		{
			for (int i = 0; i < LIST_SQUARE.Count; i++) 
			{
				if(i < numBlackTotal)
					LIST_SQUARE [i].isBlack = true;
				else
					LIST_SQUARE [i].isBlack = false;
			}
		}
	}

	void CreateParralax(int decal,int gap, int parralaxLength)
	{
		for (int i = 0; i < numberDotsOnCircle ; i++) 
		{
			float variable = 1;

			if ( Level%5 > 3  ){
				if ( i%2 == 0 ){
					variable = 0.8f - ((Level%2)/10f);
				}else{
					variable = 1f - ((Level%2)/10f);;
				}
			}

			obstaclesParent.rotation = Quaternion.Euler( new Vector3 (0, 0, ((float)i) * 360f / numberDotsOnCircle) );

			DotManager dm = poolSystem.SpawnSquare (variable*pos*parralaxLength*decal/(parralaxLength*decal+gap*i%parralaxLength), rot, parent);

			LIST_SQUARE.Add(dm);
		}
	}

	void CreateSpiral()
	{
		for (int i = 0; i < numberDotsOnCircle ; i++) 
		{

			obstaclesParent.rotation = Quaternion.Euler( new Vector3 (0, 0, ((float)i) * 360f / numberDotsOnCircle) );

			float var = 1;

			DotManager dm = poolSystem.SpawnSquare (pos*(100f-var*i)/100f, rot, parent);

			LIST_SQUARE.Add(dm);
		}
	}

	void CreateTriangle()
	{
		int value = 1;
		int sign = 1;

		for (int i = 0; i < numberDotsOnCircle ; i++) 
		{
			obstaclesParent.rotation = Quaternion.Euler( new Vector3 (0, 0, ((float)i) * 360f / numberDotsOnCircle) );

			if (value > 3)
				sign = -1;
			
			if (value < 2)
				sign = 1;
	
			DotManager dm = poolSystem.SpawnSquare (pos*5/(5+value), rot, parent);

			value += sign;

			LIST_SQUARE.Add(dm);
		}
	}

	void CreateUpAndDown()
	{
		int i = 0;

		while(LIST_SQUARE.Count < numberDotsOnCircle)
		{

			obstaclesParent.rotation = Quaternion.Euler( new Vector3 (0, 0, ((float)i) * 360f / (numberDotsOnCircle*0.5f)) );

			var dm = poolSystem.SpawnSquare (pos, rot, parent);

			LIST_SQUARE.Add(dm);

			var dm2 = poolSystem.SpawnSquare (pos * 0.7f, rot, parent);

			LIST_SQUARE.Add(dm2);

			i++;
		}
	}

	void CreateEscalier()
	{
		int i = 0;

		int j = 0;

		float lastPos = 0f;

		float min = 0.3f;

		float decalRotate = 20;

		if (numberDotsOnCircle < 20)
			min = 0.7f;
		else if (numberDotsOnCircle < 30)
			min = 0.5f;
		else if (numberDotsOnCircle < 40) 
		{
			min = 0.4f;
			decalRotate = 20;
		}

		min = 0.7f;

		while(i < numberDotsOnCircle)
		{
			float rotation = ((float)i) * 360f;

			if (lastPos < min) 
			{
				j = 0;
				lastPos = 1f;
			}
			else 
			{
				j++;
				lastPos -= 0.15f;
			}

			obstaclesParent.rotation = Quaternion.Euler( new Vector3 (0, 0,  j*decalRotate + (rotation) / (numberDotsOnCircle*0.5f)) );

			var dm = poolSystem.SpawnSquare (pos * lastPos, rot, parent);

			LIST_SQUARE.Add(dm);

			i++;
		}
	}
		
	void CreateBalagan()
	{
		int i = 0;

		int j = 0;

		float lastPosUp = 1f;
		float lastPosDown = 0.3f;
		float signPosUp = -1;
		float signPosDown = +1;

		float decalRotate = 20;

		while(i < numberDotsOnCircle)
		{

			float rotation = ((float)i) * 360f;

			if (lastPosUp > 1) 
			{
				j = 0;
				lastPosUp = 1;
				signPosDown = -1;
			}

			if(lastPosUp < 0.4f)
			{
				j++;
				lastPosUp = 0.4f;
				signPosDown = +1;
			}

			obstaclesParent.rotation = Quaternion.Euler( new Vector3 (0, 0,  j*decalRotate + (rotation) / (numberDotsOnCircle*0.5f)) );

			var dmUp = poolSystem.SpawnSquare (pos * lastPosUp, rot, parent);

			LIST_SQUARE.Add(dmUp);

			i++;

			if (lastPosDown > 1) 
			{
				lastPosDown = 1;
				signPosDown = -1;
			}

			if(lastPosDown < 0.4f)
			{
				lastPosDown = 0.4f;
				signPosDown = +1;
			}
				
			obstaclesParent.rotation = Quaternion.Euler( new Vector3 (0, 0,  j*decalRotate + 30 + (rotation) / (numberDotsOnCircle*0.5f)) );

			var dmDown = poolSystem.SpawnSquare (pos * lastPosDown, rot, parent);

			LIST_SQUARE.Add(dmDown);

			lastPosUp += signPosUp * 0.1f;
			lastPosDown += signPosDown * 0.1f;

			i++;
		}
	}
	/// <summary>
	/// Game Over logic
	/// </summary>
	public void GameOver(Transform d)
	{
		Util.SetCountGameOver(Util.GetCountGameOver() + 1);

		CheckIfSuccess ();

        if (success)
            return;

       jumpTweener.Kill();

        sequence.Kill();

        sequenceDOT.Kill();
        sequenceDOT = null;

        guyAnim.StopAll();

        BackgroundTogglePause(); // 暂停背景旋转
        gameRevival.PromptPopup();   // 显示复活界面 
       /// gameOver.PromptPopup();
    }

    public void BackgroundTogglePause()
    {
        foreach (var tem in backgrounds)
        {
            tem.TogglePause();
        }
    }

    private void PlayGameOverAnimation(Transform d)
    {
        StopAllCoroutines();

		soundManager.PlaySoundFail();
		isGameOver = true;

		//canvasManager.ButtonLogic ();

		jumpTweener.Kill ();

		sequence.Kill ();

		sequenceDOT.Kill ();

		sequenceDOT = null;

		guyAnim.StopAll();

		Vector3 targetPosition = new Vector3(PLAYER.position.x,PLAYER.position.y,Camera.main.transform.position.z);

		canvasManager.music.DOPitch (-1, 1f)
			.OnComplete (() => {
				canvasManager.music.DOPitch (1, 1f);
			});

		Camera.main.transform.DOShakePosition (0.3f, new Vector3(1,1,0), 10, 90, false)
			.OnComplete (() => {

				DOVirtual.Float (cameraSize, 5f, 0.3f, (float size) => {
					Camera.main.orthographicSize = size;
				});

				Camera.main.transform.DOMove (targetPosition, 0.3f)
					.OnComplete (() => {

						PLAYER.DOLocalMoveY (floorPosition, 0.3f, false)
							.OnUpdate(()=> {
								Camera.main.transform.position = new Vector3(PLAYER.position.x,PLAYER.position.y,Camera.main.transform.position.z);
							})
							.OnComplete (() => {
								Camera.main.transform.position = new Vector3(PLAYER.position.x,PLAYER.position.y,Camera.main.transform.position.z);
							});

						PLAYER.DOLocalRotate (new Vector3 (0, 0, 180), 0.3f)
							.OnComplete (() => {
								DOVirtual.DelayedCall(0.3f,() => {
									guyAnim.DoWalk();

									DOVirtual.DelayedCall(0.5f,() => {
										guyAnim.StopAll();
										spriteDotGameOverZoom.transform.DOScale(Vector3.one*10,1)
											.OnComplete(() => {
												canvasManager.AnimationCameraGameOver (d.position);
											});
									});
								});
							});
					});
			});
	}
	/// <summary>
	/// Move the list of the dots to shoot when a dot is shooted.
	/// </summary>
	IEnumerator PositioningDots()
	{

		for (int i = 0; i < DotsBottom.Count; i++) 
		{
			if (DotsBottom.Count > 0) 
			{
				DotsBottom [i].transform.localScale = Vector3.one;
				DotsBottom [i].transform.DOMove (new Vector3 (0, -positionTouchBorder + (-i -2) * sizeDot), 0.001f);
			}
		}

		yield return new WaitForFixedUpdate();

		for (int i = 0; i < DotsBottom.Count; i++) 
		{
			DotsBottom [i].transform.localScale = Vector3.one;
			DotsBottom [i].transform.position = new Vector3 (DotsBottom [i].transform.position.x, -positionTouchBorder + (-i -2) * sizeDot, 0);
		}

		yield return null;
	}
	/// <summary>
	/// Display particle when we distroy a square
	/// </summary>
	public void SpawnParticleExplosionSquare(DotManager square)
	{
		poolSystem.SpawnParticle (square.transform.position,Quaternion.identity);
		poolSystem.SpawnWave (square.transform.position,Quaternion.identity);

		soundManager.PlaySoundBeep();

		if (poolSystem.gameObject.activeInHierarchy) 
		{
			poolSystem.DespawnSquare (square);
		}

		CheckIfSuccess ();
	}
	/// <summary>
	/// Check all conditions. If it's ok, launch success logic
	/// </summary>
	void CheckIfSuccess()
	{
		if (success)
			return;
		

		var dmtotal = poolSystem.squares;

		var dmFiltered = dmtotal.FindAll (dot => dot.isEnable == true && dot.isBlack == false);
	
		int numberDotsToDestroy = dmFiltered.Count;

		if ( !isGameOver && numberDotsToDestroy <= 0)
		{
			success = true;

			Util.SetCountGameOver(0);

			//canvasManager.ButtonLogic ();
		}

		if (success && !isGameOver && numberDotsToDestroy <= 0)
		{
			guyAnim.MakeItBlink();
			soundManager.PlaySoundSuccess();
			DoWalk ();
			isGameOver = true;


			obstaclesParent.DOScale (Vector3.zero, 1)
				.SetEase (Ease.InBack)
				.OnComplete (() => {
					canvasManager.AnimationCameraSuccess();
				});
		}
	}

	public void OnApplicationPause(bool pause)
	{
		if (!pause) 
		{
			Resources.UnloadUnusedAssets ();
			Time.timeScale = 1.0f;
		}
		else 
		{
			Resources.UnloadUnusedAssets ();
			Time.timeScale = 0.0f;
		}

	}  

	void OnApplicationQuit(){
		
		PlayerPrefs.Save();
	}
}
