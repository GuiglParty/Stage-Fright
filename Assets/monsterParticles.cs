using UnityEngine;
using System.Collections;

public class monsterParticles : MonoBehaviour {

	public float twitchSpeed;

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
			m_particles[i].velocity -= m_particles[i].position*twitchSpeed; 
		}
		m_system.SetParticles (m_particles, numParticlesAlive);
	}

}
