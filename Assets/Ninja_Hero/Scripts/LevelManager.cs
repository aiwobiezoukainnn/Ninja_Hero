using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;

/// <summary>
/// Class who make the level. Click right on the LevelManager in the editor and select "execute" to generate 1200 levels.
/// </summary>
public class LevelManager : MonoBehaviourHelper
{
	void Awake()
	{
        //int level = Util.GetLastLevelPlayed();
        int level = canvasManager.lastLevel;

        if (level > Level.maxLevel)
			Util.SetLastLevelPlayed(Level.maxLevel);
	}


	public Level GetLevel(int level)
	{
		Level l = new Level (level);
		return l;
	}
}

/// <summary>
/// Level class with all the informations for to create the level in the GameManager
/// </summary>
[Serializable]
public class Level
{
	/// <summary>
	/// Don't create more than 1200 levels
	/// </summary>
	public static int maxLevel = 1200;
	/// <summary>
	/// Level number. Max = 1200
	/// </summary>
	public int levelNumber = 0;
	/// <summary>
	/// Number of obstacles in the level
	/// </summary>
	public int numberDotsOnCircle = 0;
	/// <summary>
	/// Position of obstacles 
	/// </summary>
	public float sizeRayonRation = 0.25f;
	/// <summary>
	/// Delay of one rotation
	/// </summary>
	public float rotateDelay = 8f;
	/// <summary>
	/// Ease type of the rotation
	/// </summary>
	public Ease rotateEaseType = Ease.Linear;
	/// <summary>
	/// Loop type of the rotation
	/// </summary>
	public LoopType rotateLoopType = LoopType.Incremental;
	/// <summary>
	/// Level constructor
	/// </summary>
	public Level (int level)
	{
		levelNumber = level;

		rotateEaseType = Ease.Linear;

		rotateLoopType = LoopType.Incremental;

		sizeRayonRation = 0.25f;

		rotateDelay = 20f - (level % 10);

		if (level%2 <1) 
		{
			rotateLoopType = LoopType.Incremental;
		}
		else 
		{
			rotateLoopType = LoopType.Yoyo;
		}

		int numOfEnum = (System.Enum.GetValues (typeof(Ease)).Length);

		int enumNumber = level % numOfEnum;
		rotateEaseType = (Ease)(enumNumber); 

		while(rotateEaseType.ToString ().Contains ("Elastic") || rotateEaseType.ToString ().Contains ("INTERNAL_Zero") || rotateEaseType.ToString ().Contains ("INTERNAL_Custom"))
		{
			enumNumber++;
			if (enumNumber >= numOfEnum)
				enumNumber = 0;

			rotateEaseType = (Ease)(enumNumber);
		}

		numberDotsOnCircle = (int)((10 + level % 35));
	
		if (level < 12)
			numberDotsOnCircle = 5;

		if (level == 1)
			numberDotsOnCircle = 1;

		if (level == 2)
			numberDotsOnCircle = 2;
	
		if (level > maxLevel) 
		{

			Util.SetLastLevelPlayed(Level.maxLevel);

			level = Level.maxLevel;

			Util.SetMaxLevelUnlock(Level.maxLevel);

			Application.OpenURL ("http://barouch.fr/moregames.php");

		}
	}
}
