using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class in charge to pool all the GameObject we need during the game.
/// </summary>
public class PoolSystem : MonoBehaviour 
{
	/// <summary>
	/// Obstacle prefab
	/// </summary>
	public GameObject squarePrefab;
	/// <summary>
	/// List of obstacles pooled
	/// </summary>
	public List<DotManager> squares = new List<DotManager>();
	/// <summary>
	/// Particle prefab
	/// </summary>
	public GameObject particlePrefab;
	/// <summary>
	/// List of particles pooled
	/// </summary>
	public List<GameObject> particles = new List<GameObject>();
	/// <summary>
	/// Partciel Wave prefab
	/// </summary>
	public GameObject waveParticlePrefab;
	/// <summary>
	/// List of particle waves pooled
	/// </summary>
	public List<GameObject> waves = new List<GameObject>();


	void Awake()
	{
		PreparePools ();
	}
	/// <summary>
	/// Create the pool for each gameobject
	/// </summary>
	void PreparePools()
	{
		while (squares.Count < 50) 
		{
			squares.Add(DOInstantiate (squarePrefab).GetComponent<DotManager>());
		}

		while (particles.Count < 10) 
		{
			particles.Add(DOInstantiate (particlePrefab));
		}

		while (waves.Count < 10) 
		{
			waves.Add(DOInstantiate (waveParticlePrefab));
		}
	}
	/// <summary>
	/// Instantiate GameObject, then add it to the pooled list
	/// </summary>
	GameObject DOInstantiate(GameObject obj)
	{
		var o = Instantiate (obj) as GameObject;
		o.transform.parent = transform;
		o.SetActive (false);
		return o;
	}
	/// <summary>
	/// Despawn all pooled GameObject
	/// </summary>
	public void DespawnAll()
	{
		DespawnAllSquares ();
		DespawnAllParticles ();
		DespawnAllWaves ();
	}
	/// <summary>
	/// Despawn all pooled square GameObjects
	/// </summary>
	public void DespawnAllSquares()
	{
		var objActive = squares.FindAll (o => o.isEnable == true);
		foreach (var v in objActive) 
		{
			DespawnSquare (v);
		}
	}
	/// <summary>
	/// Despawn all pooled particle GameObjects
	/// </summary>
	public void DespawnAllParticles()
	{
		var objActive = particles.FindAll (o => o.activeInHierarchy == true);
		foreach (var v in objActive) 
		{
			DespawnParticle (v);
		}
	}
	/// <summary>
	/// Despawn all pooled particle wave GameObjects
	/// </summary>
	public void DespawnAllWaves()
	{
		var objActive = waves.FindAll (o => o.activeInHierarchy == true);
		foreach (var v in objActive) 
		{
			DespawnWave (v);
		}
	}
	/// <summary>
	/// Spawn a square
	/// </summary>
	public DotManager SpawnSquare(Vector3 pos, Quaternion angles, Transform parent)
	{
		
		if (squares.Count > 0) 
		{
			var l = squares.FindAll(o => o.isEnable == false);
			if (l == null || l.Count == 0) 
			{
				var obj = DOInstantiate (squarePrefab);
				squares.Add (obj.GetComponent<DotManager>());
				return SpawnSquare (pos, angles, parent);
			}

			l[0].transform.parent = parent;
			l[0].transform.position = pos;
			l[0].transform.rotation = angles;
			l[0].gameObject.SetActive (true);
			l[0].isEnable = true;
			return l[0];
		}
			
		var ob = DOInstantiate (squarePrefab);
		squares.Add (ob.GetComponent<DotManager>());
		return SpawnSquare (pos, angles, parent);
	}
	/// <summary>
	/// Despawn a square
	/// </summary>
	public void DespawnSquare(DotManager obj)
	{
		obj.transform.parent = transform;
		obj.isEnable = false;
		obj.gameObject.SetActive (false);
	}
	/// <summary>
	/// Spawn a particle
	/// </summary>
	public GameObject SpawnParticle(Vector3 pos, Quaternion angles)
	{
		if (particles.Count > 0) 
		{
			var l = particles.FindAll(o => o.activeInHierarchy == false);
			if (l == null || l.Count == 0) 
			{
				var obj = DOInstantiate (particlePrefab);
				particles.Add (obj);
				return SpawnParticle (pos, angles);
			}
				
			l[0].transform.position = pos;
			l[0].transform.rotation = angles;
			l[0].SetActive (true);
			return l[0];
		}

		var ob = DOInstantiate (particlePrefab);
		particles.Add (ob);
		return SpawnParticle (pos, angles);
	}
	/// <summary>
	/// Despawn a particle
	/// </summary>
	public void DespawnParticle(GameObject obj)
	{
		obj.transform.parent = transform;
		obj.SetActive (false);
	}
	/// <summary>
	/// Spawn a particle wave
	/// </summary>
	public GameObject SpawnWave(Vector3 pos, Quaternion angles)
	{
		if (waves.Count > 0) 
		{
			var l = waves.FindAll(o => o.activeInHierarchy == false);
			if (l == null || l.Count == 0) 
			{
				var obj = DOInstantiate (waveParticlePrefab);
				waves.Add (obj);
				return SpawnWave (pos, angles);
			}

			l[0].transform.position = pos;
			l[0].transform.rotation = angles;
			l[0].SetActive (true);
			return l[0];
		}

		var ob = DOInstantiate (waveParticlePrefab);
		waves.Add (ob);
		return SpawnWave (pos, angles);
	}
	/// <summary>
	/// Despawn a particle wave
	/// </summary>
	public void DespawnWave(GameObject obj)
	{
		obj.transform.parent = transform;
		obj.SetActive (false);
	}
}
