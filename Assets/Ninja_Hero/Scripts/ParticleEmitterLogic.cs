using UnityEngine;
using System.Collections;

public class ParticleEmitterLogic : MonoBehaviour {

	public ParticleEmitter p;

	void OnDespawned(){
		p.ClearParticles ();
		p.emit = false;
		p.ClearParticles ();
		p.emit = false;
	}

	void OnSpawned(){
		p.emit = true;
	}
}
