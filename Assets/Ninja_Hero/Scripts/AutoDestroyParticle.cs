using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
/// <summary>
/// In Charge to destroy particle automatically when emiter finished his work
/// </summary>
public class AutoDestroyParticle : MonoBehaviourHelper
{
	public bool isParticle;
	public bool isWave;
	public bool OnlyDeactivate;
	
	void OnEnable()
	{
		StartCoroutine("CheckIfAlive");
	}
	
	IEnumerator CheckIfAlive ()
	{
		while(true)
		{
			yield return new WaitForSeconds(0.5f);
			if(!GetComponent<ParticleSystem>().IsAlive(true))
			{
				if (OnlyDeactivate) {
					#if UNITY_3_5
						this.gameObject.SetActiveRecursively(false);
					#else
					this.gameObject.SetActive (false);
					#endif
				} else {
					if (isParticle)
						poolSystem.DespawnParticle (gameObject);

					if (isWave)
						poolSystem.DespawnWave (gameObject);
				}
				break;
			}
		}
	}
}
