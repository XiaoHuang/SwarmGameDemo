using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DroneHeroBehavior : MonoBehaviour {
	public float heroSpeed = 20.0F;
	public float maxHeroSpeed = 30.0F;
	public SwarmBehavior swarm;

	void Start() {
		GetComponent<Rigidbody> ().freezeRotation = true;
		GetComponent<Renderer> ().material.color = Color.yellow;
	}

	void Update() {
		/*
		Vector3 newVelocity = GetComponent<Rigidbody>().velocity + new Vector3(speed * Input.GetAxis("Horizontal") * Time.deltaTime, 0f, speed * Input.GetAxis ("Vertical") * Time.deltaTime);
		newVelocity.y = 0f;
		GetComponent<Rigidbody> ().freezeRotation = true;
		GetComponent<Rigidbody>().velocity = Limit(newVelocity, maxSpeed);
	*/
		float moveHorizontal = heroSpeed * Input.GetAxis ("Horizontal");
		float moveVertical = heroSpeed * Input.GetAxis ("Vertical");

		Vector3 newVelocity = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		GetComponent<Rigidbody>().velocity =  Limit(newVelocity, maxHeroSpeed);

		if (swarm.state == SwarmBehavior.SwarmState.SWARM) {
			this.heroSpeed = 40f;	
			this.maxHeroSpeed = 60f;
		} else {
			this.heroSpeed = 10f;
			this.maxHeroSpeed = 30f;
		}
	}

	protected virtual Vector3 Limit(Vector3 v, float max)
	{
		if (v.magnitude > max)
		{
			return v.normalized * max;
		}
		else
		{
			return v;
		}
	}
}

