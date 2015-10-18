using UnityEngine;
using System.Collections;

public class monsterParticles : MonoBehaviour {

	public float pullStrength;
	public float coreRadius;
	public float shellRadius;

	private ParticleSystem m_system;
	private ParticleSystem.Particle[] m_particles;
	private int numParticles;

	// Use this for initialization
	void Start () {
		m_system = gameObject.GetComponent<ParticleSystem> ();
		m_particles = new ParticleSystem.Particle[m_system.maxParticles];

	}
	
	// Update is called once per frame
	void Update () {
		int numParticlesAlive = m_system.GetParticles (m_particles);
		for (int i=0; i < numParticlesAlive; i++) 
		{
			ParticleSystem.Particle curParticle = m_particles[i];


			Vector3 particleHeading = curParticle.position.normalized;
			float dist = curParticle.position.magnitude;
			curParticle.velocity -= (pullStrength/Mathf.Max(dist*dist,coreRadius*coreRadius)) *particleHeading*Time.deltaTime; 
			if (curParticle.position.magnitude > shellRadius)
			{
				curParticle.lifetime = 0;
			}

			m_particles[i]=curParticle;
		}
		m_system.SetParticles (m_particles, numParticlesAlive);
	}

}
